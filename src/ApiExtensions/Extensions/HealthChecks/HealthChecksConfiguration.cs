using System.Collections.Generic;

namespace S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies
{
    public class HealthChecksConfiguration
    {
        public Dictionary<string, UrlGroup> UrlGroup { get; set; } = new ();

    }
    public class UrlGroup
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<string> Tags { get; set; } = new ();
    }
}
