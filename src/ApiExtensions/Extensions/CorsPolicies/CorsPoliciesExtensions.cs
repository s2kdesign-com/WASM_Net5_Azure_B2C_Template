using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies
{

    public static class CorsPoliciesExtensions
    {
        private static CorsPoliciesConfiguration _cors = new();

        public static void AddCorsExtensions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var configuration = configurationSection.Get<CorsPoliciesConfiguration>();

            if (configuration != null && configuration.CorsPolicies.Any())
            {
                _cors = configuration;

                services.AddCors(options =>
                {
                    foreach (var corsPolicy in _cors.CorsPolicies)
                    {
                        options.AddPolicy(corsPolicy.Value.PolicyName,
                            builder => builder
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                               // .SetIsOriginAllowed((host) => true)
                                .WithOrigins(corsPolicy.Value.Url)
                                .AllowCredentials());
                    }
                });
            }
        }

        public static void UseCorsExtensions(this IApplicationBuilder app)
        {
            if (_cors.CorsPolicies.Any())
            {
                foreach (var corsPolicy in _cors.CorsPolicies)
                {
                        app.UseCors(corsPolicy.Value.PolicyName);
                }
            }
        }
    }
}
