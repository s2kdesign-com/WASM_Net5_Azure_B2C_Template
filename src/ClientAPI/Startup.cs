using GrpcClientApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using S2kDesignTemplate.ApiExtensions.Extensions.HealthChecks;
using S2kDesignTemplate.ApiExtensions.Extensions.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ClientAPI
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
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });
            
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
                // TODO: Add cors policy support
                endpoints.MapGrpcService<ClientApiService>() ;

                endpoints.MapControllers();
                // Defined in S2kDesignTemplate.Extensions
                endpoints.MapHealthChecksExtensions();
            });
        }
    }
}
