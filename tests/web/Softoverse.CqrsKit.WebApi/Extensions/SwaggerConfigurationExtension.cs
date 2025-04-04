﻿using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace Softoverse.CqrsKit.WebApi.Extensions;

public static class SwaggerConfigurationExtension
{
    public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            // Configure Swagger to use OAuth2 with Resource Owner Password Credentials (ROPC) flow
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{builder.Configuration["JWT:TokenUrl"]}"),
                        RefreshUrl = new Uri($"{builder.Configuration["JWT:TokenRefreshUrl"]}"),
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
                }
            });

            // Add security requirement to operations
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    ["apiScope", "uiScope"]
                }
            });

            // Add API Key Authentication to Swagger
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key Authentication",
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKey"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    []
                }
            });
        });

        return builder;
    }

    public static WebApplication UseSwaggerUi(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var supportedVersionsRaw = app.Configuration["ApiVersion:Supported"];
            var versions = supportedVersionsRaw?.Split(",").Select(Convert.ToDouble).ToArray();

            for (int i = 0; i < versions?.Length; i++)
            {
                var version = versions[i];
                c.SwaggerEndpoint($"v{version}/swagger.json", $"API V{version}");
            }

            c.OAuthClientId(app.Configuration["JWT:ClientId"]);
            c.OAuthClientSecret(app.Configuration["JWT:ClientSecret"]);
            c.OAuthAppName("Softoverse.CqrsKit");
            c.OAuthUseBasicAuthenticationWithAccessCodeGrant();

            c.EnablePersistAuthorization();
            c.EnableFilter();

            c.DisplayRequestDuration();
            c.DefaultModelRendering(ModelRendering.Model);
            c.DocExpansion(DocExpansion.List);
            c.EnableValidator();
            c.EnableTryItOutByDefault();
        });

        app.MapSwagger("{documentName}/swagger.json");

        return app;
    }
}