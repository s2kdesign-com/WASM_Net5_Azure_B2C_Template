using System.Collections.Generic;
using System.Linq;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ServerAPI
{
    public class Startup
    {
        private static Dictionary<string, string> _corsPolicies = new();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var buildVersionDescription = "Build Version: " + GetType().Assembly.GetName().Version;
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(buildVersionDescription));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));

            #region AzureB2C Configuration

            var azureAdB2CConfiguration = new MicrosoftIdentityOptions();
            Configuration.GetSection("AzureAdB2C").Bind(azureAdB2CConfiguration);
            
            var openApiOAuthFlow = new OpenApiOAuthFlow();
            Configuration.GetSection(nameof(OpenApiOAuthFlow)).Bind(openApiOAuthFlow);

            // TODO: Can not add url to Dictionary Key - result: key="https:", value=null
            var orderedScopes = Configuration.GetSection(nameof(OpenApiOAuthFlow) + ":Scopes").GetChildren()
                .ToDictionary(x => x.Value, x => x.Key);
            openApiOAuthFlow.Scopes = orderedScopes;

            #endregion
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ServerAPI", 
                    Version = "v1",
                    Description = buildVersionDescription
                }); 
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,  
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = openApiOAuthFlow
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

            _corsPolicies = Configuration.GetSection("CorsPolicies").Get<Dictionary<string, string>>();
            services.AddCors(options =>
            {
                foreach (var corsPolicies in _corsPolicies)
                {
                    options.AddPolicy(corsPolicies.Key,
                        builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed((host) => true)
                            .WithOrigins(corsPolicies.Value)
                            .AllowCredentials());
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerAPI v1");

                    c.OAuthClientId(Configuration.GetValue<string>(nameof(SwaggerUIOptions) + ":OAuthClientId"));
                    c.OAuthAppName(Configuration.GetValue<string>(nameof(SwaggerUIOptions) + ":OAuthAppName"));
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            foreach (var corsPolicy in _corsPolicies)
            {
                app.UseCors(corsPolicy.Key);
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
                endpoints.MapControllers();
            });
        }
    }
}
