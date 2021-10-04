using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using S2kDesignTemplate.ApiExtensions.Extensions.HealthChecks;
using S2kDesignTemplate.ApiExtensions.Extensions.Swagger;
using S2kDesignTemplate.ClientAPI.HttpAggregator.Infrastructure;
using S2kDesignTemplate.ClientAPI.HttpAggregator.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ClientAPI.HttpAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddHealthChecksExtensions(Configuration.GetSection(nameof(HealthChecksConfiguration)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));

            services.AddControllers();

            services.AddSwaggerExtensions(Configuration.GetSection(nameof(SwaggerConfiguration)));


            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
                        .AllowCredentials());
            });
            services.AddGrpcServices(Configuration.GetSection("GrpcClients"));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerExtensions(Configuration.GetSection(nameof(SwaggerUIOptions)));

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseGrpcWeb();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // TODO: Add cors policy support
                endpoints.MapGrpcService<ClientApiService>().EnableGrpcWeb().RequireCors("CorsPolicy");

                endpoints.MapControllers();
                endpoints.MapHealthChecksExtensions();
            });
        }

    }


    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfigurationSection configurationSection)
        {

            var configuration = configurationSection.Get<Dictionary<string, string>>();
            if (configuration != null && configuration.Any())
            {

                services.AddTransient<GrpcExceptionInterceptor>();
                
                foreach (var client in configuration)
                {
                    if (client.Key == "ClientApi")
                    {
                        services.AddGrpcClient<GrpcClientApi.ClientApi.ClientApiClient>((services, options) =>
                        {
                            options.Address = new Uri(client.Value);
                        }).AddInterceptor<GrpcExceptionInterceptor>();
                    }
                }

            }
            return services;
        }
    }
}
