using System.Net;
using System.Threading.Tasks;
using GrpcClientApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using S2kDesignTemplate.ClientAPI.HttpAggregator.Services;

namespace S2kDesignTemplate.ClientAPI.HttpAggregator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ClientApi.ClientApiClient _clientApi;
        private readonly ILogger<ProfileController> _logger;


        // The Web API will only accept tokens 1) for users, and 2) having the "ClientAPI.Gateway.Read" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "ClientAPI.HttpAggregator.Read" };

        public ProfileController(ClientApi.ClientApiClient clientApiService, ILogger<ProfileController> logger)
        {
            _clientApi = clientApiService;
            _logger = logger;
        }

        [Route("{profileId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProfileResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProfileResponse>> Get(string profileId)
        {

            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            if (string.IsNullOrEmpty(profileId))
            {
                return BadRequest("Need a valid profileId");
            }

            return await _clientApi.GetProfileByIdAsync(new ProfileRequest {  Id = profileId});
        }
    }
}
