using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using S2kDesignTemplate.ClientAPI.Model;

namespace S2kDesignTemplate.ClientAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "ClientAPI.Read" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "ClientAPI.Read" };

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Profile), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> GetProfileByIdAsync(string id)
        {
            return new Profile { DisplayName = "TestUser" };
        }
    }
}
