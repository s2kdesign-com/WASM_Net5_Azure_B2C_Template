using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace S2kDesignTemplate.WebStatus.Pages
{
    public class ConfigModel : PageModel
    {
        private IConfiguration _configuration;
        public ConfigModel(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public Dictionary<string, string> Configs;

        public void OnGet()
        {
            var configurationValues = _configuration.GetSection("HealthChecksUI:HealthChecks")
                .GetChildren()
                .SelectMany(cs => cs.GetChildren())
                .Union(_configuration.GetSection("HealthChecks-UI:HealthChecks")
                    .GetChildren()
                    .SelectMany(cs => cs.GetChildren()))
                .ToDictionary(v => v.Path, v => v.Value);
            Configs = configurationValues;
        }
    }
}
