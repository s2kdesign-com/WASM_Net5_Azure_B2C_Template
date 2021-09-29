using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using S2kDesignTemplate.ApiExtensions.Extensions.HealthChecks;
using S2kDesignTemplate.ApiExtensions.Extensions.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ClientAPI
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


            // Defined in S2kDesignTemplate.Extensions
            app.UseSwaggerExtensions(Configuration.GetSection(nameof(SwaggerUIOptions)));

            //app.UseHttpsRedirection();

            app.UseRouting();

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
