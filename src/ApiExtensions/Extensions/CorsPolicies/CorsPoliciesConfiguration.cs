using System.Collections.Generic;

namespace S2kDesignTemplate.ApiExtensions.Extensions.CorsPolicies
{
    public class CorsPoliciesConfiguration
    {
        public Dictionary<string, CorsPolicy> CorsPolicies { get; set; } = new ();

    }
    public class CorsPolicy
    {
        public string PolicyName { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
    }
}
