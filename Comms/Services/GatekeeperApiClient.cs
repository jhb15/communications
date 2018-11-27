using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Comms.Services
{
    public class GatekeeperApiClient : IGatekeeperApiClient
    {
        private readonly HttpClient client;
        private IConfigurationSection appConfig;
        private DiscoveryCache discoveryCache;

        public GatekeeperApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            appConfig = configuration.GetSection("Comms");
            discoveryCache = new DiscoveryCache(appConfig.GetValue<string>("GatekeeperUrl"));
            client = httpClientFactory.CreateClient("gatekeeper");
        }

        private async Task<string> GetTokenAsync()
        {
            var discovery = await discoveryCache.GetAsync();
            if (discovery.IsError)
            {
                throw new GatekeeperApiException("Couldn't read discovery document.");
            }

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = appConfig.GetValue<string>("ClientId"),
                ClientSecret = appConfig.GetValue<string>("ClientSecret"),
                Scope = "gatekeeper"
            };
            var response = await client.RequestClientCredentialsTokenAsync(tokenRequest);
            if(response.IsError)
            {
                throw new GatekeeperApiException("Couldn't retrieve access token.");
            }
            return response.AccessToken;

        }

        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            client.SetBearerToken(await GetTokenAsync());
            return await client.GetAsync(path);
        }
    }

    public class GatekeeperApiException : Exception
    {
        public GatekeeperApiException(string message) : base(message)
        {
        }
    }
}
