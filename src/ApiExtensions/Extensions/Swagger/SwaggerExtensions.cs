using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ApiExtensions.Extensions.Swagger
{

    public static class SwaggerExtensions
    {
        public static void AddSwaggerExtensions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var configuration = configurationSection.Get<SwaggerConfiguration>();

            // TODO: Can not add url to Dictionary Key - result: key="https:", value=null
            var orderedScopes = configuration.OpenApiOAuthFlow.Scopes
                .ToDictionary(x => x.Value, x => x.Key);
            configuration.OpenApiOAuthFlow.Scopes = orderedScopes;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = configuration.Title,
                    Version = "v1",
                    Description = "Build number: " + Assembly.GetEntryAssembly()?.GetName().Version
                });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = configuration.OpenApiOAuthFlow
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "oauth2",
                            In = ParameterLocation.Header
                        },
                        new List < string > ()
                    }
                });
            });
        }

        public static void UseSwaggerExtensions(this IApplicationBuilder app, IConfigurationSection configurationSection)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerAPI v1");

                configurationSection.Bind(c);

                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }
    }
}
