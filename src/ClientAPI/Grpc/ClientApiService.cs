using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClientApi
{
    public class ClientApiService : ClientApi.ClientApiBase
    {
        private readonly ILogger<ClientApiService> _logger;

        public ClientApiService( ILogger<ClientApiService> logger)
        {
            _logger = logger;
        }

        public override async Task<ProfileResponse> GetProfileById(ProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ClientApiService.GetProfile for user id {0}", request.Id);

            return new ProfileResponse{Displayname = "TestUser from GRPC - ClientAPI"};
        }
    }
}
