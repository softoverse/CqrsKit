using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using Scalar.AspNetCore;

using Softoverse.CqrsKit.WebApi.Controllers.Users;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class ScalarConfigurationExtension
{

    public static WebApplicationBuilder AddScalarAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
        builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<ApiKeySecuritySchemeTransformer>(); });
        // builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<OAuth2SecuritySchemeTransformer>(); });

        return builder;
    }

    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.BluePlanet)
                   .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
                   .WithDownloadButton(true)
                   .WithSearchHotKey("K");
            
            options
                .WithPreferredScheme("ApiKey") // Optional. Security scheme name from the OpenAPI document
                .WithApiKeyAuthentication(apiKey =>
                {
                    apiKey.Token = app.Configuration["ApiKey"];
                });

            options.WithPreferredScheme("Bearer")
                   .WithHttpBearerAuthentication(bearerOptions =>
                   {
                       bearerOptions.Token = GenerateApiUserToken(app.Configuration["JWT:Username"],
                                                                  app.Configuration["JWT:Key"],
                                                                  app.Configuration["JWT:Issuer"],
                                                                  app.Configuration["JWT:Audience"]);
                   });

            // options.WithPreferredScheme("OAuth2")
            //        .WithOAuth2Authentication(oauth =>
            //        {
            //            oauth.ClientId = "Softoverse";
            //            oauth.Scopes = ["apiScope", "uiScope"];
            //        });
        });

        return app;
    }

    private static string GenerateApiUserToken(string? username, string? key, string? issuer, string? audience, int accessTokenExpiresIn = Int32.MaxValue)
    {
        return AuthenticationController.GenerateJwtToken(username!, key, issuer, audience, accessTokenExpiresIn);
    }
}

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var securitySchemeRequirement = new OpenApiSecurityScheme
            {
                // for using in the operation.Value.Security
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["Bearer"] = securitySchemeRequirement
            };
            
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                var securitySchemeRef = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [securitySchemeRef] = ["apiScope", "uiScope"]
                });
            }
        }
    }
}

public class ApiKeySecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes["ApiKey"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = "X-API-Key",
            Description = "API Key Authentication"
        };

        foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
        {
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "ApiKey",
                        Type = ReferenceType.SecurityScheme
                    }
                }] = new List<string>()
            });
        }

        return Task.CompletedTask;
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
                        AuthorizationUrl = new Uri(configuration["JWT:TokenUrl"]!),
                        TokenUrl = new Uri(configuration["JWT:TokenRefreshUrl"]!),
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