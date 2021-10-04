using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace S2kDesignTemplate.LandingPage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiScopes = builder.Configuration.GetSection("OpenApiOAuthFlow:Scopes").Get<Dictionary<string, string>>();

            var externalApis = builder.Configuration.GetSection("ExternalApis").Get<Dictionary<string, string>>();
            foreach (var externalApi in externalApis)
            {
                // Add Http client 
                builder.Services.AddHttpClient(externalApi.Key, client => client.BaseAddress = new Uri(externalApi.Value))
                    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: new[] { externalApi.Value },
                        scopes: apiScopes.Values));

                builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(externalApi.Key));

                // Add Grpc client 

                builder.Services.AddSingleton(services =>
                {
                    var baseUri = externalApi.Value;
                    var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

                    return GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpHandler = httpHandler });
                });
            }


            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                foreach (var apiScope in apiScopes)
                {
                    options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope.Value);
                }
            });
            await builder.Build().RunAsync();
        }
    }
}
