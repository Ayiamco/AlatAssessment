using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using AlatAssessment.DTOs;
using AlatAssessment.Helpers;
using AlatAssessment.Services.Interfaces;

using Newtonsoft.Json;

namespace AlatAssessment.Services.Implementation
{
    public class WemaInternalHttpProxyHttpProxy : IWemaInternalHttpProxy
    {
        private readonly IHttpClientFactory _clientFactory;

        public WemaInternalHttpProxyHttpProxy(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<BankDto>> GetBanks()
        {
           var httpClient= _clientFactory.CreateClient(ClientProxy.WemaInternal);
           var response = await httpClient.GetAsync(AppSettings.WemaInternalUrl);

           var resp = await response.Content.ReadAsStringAsync();
           var jsonResp = JsonConvert.DeserializeObject<BankDtoFull>(resp);
           return jsonResp.result;
        }

        public class ClientProxy
        {
            public const string WemaInternal = "WemaInternal";
        }
    }
}
