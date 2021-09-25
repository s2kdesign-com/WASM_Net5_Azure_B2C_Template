using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies
{
    public class SwaggerConfiguration
    {
        public string Title { get; set; }
        public OpenApiOAuthFlow OpenApiOAuthFlow { get; set; } 

    }
}
