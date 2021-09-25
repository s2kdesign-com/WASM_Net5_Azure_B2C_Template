using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace S2kDesignTemplate.ApiExtensions.Extensions.HealthChecks
{

    public static class HealthChecksExtensions
    {
        public static void AddHealthChecksExtensions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var healthCheckService = services.AddHealthChecks();

            healthCheckService.AddCheck("self", () =>
                HealthCheckResult.Healthy("Build Version: " + Assembly.GetEntryAssembly()?.GetName().Version));

            var configuration = configurationSection.Get<HealthChecksConfiguration>();
            if (configuration != null && configuration.UrlGroup.Any())
            {
                foreach (var urlGroup in configuration.UrlGroup)
                {
                    healthCheckService.AddUrlGroup(new Uri(urlGroup.Value.Url), name: urlGroup.Value.Name,
                            tags: urlGroup.Value.Tags);
                }
            }
        }
        public static void MapHealthChecksExtensions(this IEndpointRouteBuilder endpoints)
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

        }
    }
}
