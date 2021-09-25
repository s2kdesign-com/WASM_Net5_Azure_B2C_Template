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
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using S2kDesignTemplate.ApiExtensions.Extensions.HealthChecks;
using S2kDesignTemplate.ApiExtensions.Extensions.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ServerAPI
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
            services.AddHealthChecksExtensions(Configuration.GetSection(nameof(HealthChecksConfiguration)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
            
            services.AddControllers();

            services.AddSwaggerExtensions(Configuration.GetSection(nameof(SwaggerConfiguration)));
            
            // Defined in S2kDesignTemplate.Extensions
            services.AddCorsExtensions(Configuration.GetSection(nameof(CorsPoliciesConfiguration)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            // Defined in S2kDesignTemplate.Extensions
            app.UseSwaggerExtensions(Configuration.GetSection(nameof(SwaggerUIOptions)));

            app.UseRouting();

            // Defined in S2kDesignTemplate.Extensions
            app.UseCorsExtensions();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Defined in S2kDesignTemplate.Extensions
                endpoints.MapHealthChecksExtensions();
            });
        }
    }
}
