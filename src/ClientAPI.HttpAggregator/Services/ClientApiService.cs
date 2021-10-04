using System.Threading.Tasks;
using Grpc.Core;
using GrpcClientApi;
using Microsoft.Extensions.Logging;

namespace S2kDesignTemplate.ClientAPI.HttpAggregator.Services
{
    public class ClientApiService : ClientApi.ClientApiBase
    {
        private readonly ClientApi.ClientApiClient _clientApi;
        private readonly ILogger<ClientApiService> _logger;
        public ClientApiService(ClientApi.ClientApiClient clientApi, ILogger<ClientApiService> logger)
        {
            _clientApi = clientApi;
            _logger = logger;
        }

        public override async Task<ProfileResponse> GetProfileById(ProfileRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call ClientApiService.GetProfile for user id {0}", request.Id);

            _logger.LogDebug("grpc client created, request = {@id}", request.Id);
            var response = await _clientApi.GetProfileByIdAsync(new ProfileRequest { Id = request.Id });
            _logger.LogDebug("grpc response {@response}", response);

            return response;
        }

    }
}
