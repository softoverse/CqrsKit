using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using Scalar.AspNetCore;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class ScalarConfigurationExtension
{

    public static WebApplicationBuilder AddScalarAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
        builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<OAuth2SecuritySchemeTransformer>(); });

        builder.Services.AddOpenApi("v1", options =>
        {
            options.AddSchemaTransformer((schema, context, cancellationToken) => Task.CompletedTask);
        });

        return builder;
    }

    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.BluePlanet)
                   .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                   .WithDownloadButton(true);

            options.WithPreferredScheme("OAuth2")
                   .WithOAuth2Authentication(oauth =>
                   {
                       oauth.ClientId = "Softoverse";
                       oauth.Scopes = ["apiScope", "uiScope"];
                   });
            options.WithPreferredScheme("Bearer");
        });

        return app;
    }
}

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }] = Array.Empty<string>()
                });
            }
        }
    }
}

internal sealed class OAuth2SecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider, IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "OAuth2"))
        {
            // Define OAuth 2.0 security scheme
            var oauth2Scheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(configuration["JWT:TokenUrl"]),
                        TokenUrl = new Uri(configuration["JWT:TokenRefreshUrl"]),
                        Scopes = new Dictionary<string, string>
                        {
                            {
                                "apiScope", "Access your API"
                            },
                            {
                                "uiScope", "Access your UI"
                            }
                        }
                    }
                },
                Description = "OAuth 2.0 Authorization Code Grant"
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = document.Components.SecuritySchemes ?? new Dictionary<string, OpenApiSecurityScheme>();
            document.Components.SecuritySchemes["OAuth2"] = oauth2Scheme;

            // Apply security requirements to operations
            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "OAuth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        }] =
                        [
                            "apiScope", "uiScope"
                        ]
                });
            }
        }
    }
}