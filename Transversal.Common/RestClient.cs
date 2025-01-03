using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Common
{
    public class RestClient: IRestClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAppLogger _Logger;
        public RestClient(IAppLogger logger, IHttpClientFactory clientFactory)
        {
            _Logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<RestEntity> PostAsync(string uri, dynamic request)
        {
            RestEntity restEntity = new RestEntity();
            try
            {
                var client = _clientFactory.CreateClient();
                var jsonParameters = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonParameters, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(uri, content);
                var statusCode = result.StatusCode;
                restEntity.StatusCode = Convert.ToInt32(statusCode);
                restEntity.ResultContent = result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"RestClient|PostAsync - URI: {uri}");
                restEntity.ResultContent = e.Message;
            }
            return restEntity;
        }
        public async Task<RestEntity> GetAsync(string uri)
        {
            RestEntity restEntity = new RestEntity();
            try
            {
                var client = _clientFactory.CreateClient();
                var result = await client.GetAsync(string.Concat(uri));
                var statusCode = result.StatusCode;
                restEntity.StatusCode = Convert.ToInt32(statusCode);
                restEntity.ResultContent = result.Content.ReadAsStringAsync().Result;

                
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"RestClient|GetAsync - URI: {uri}");
                restEntity.ResultContent = e.Message;
            }
            return restEntity;
        }
        public async Task<RestEntity> GetAsyncWithHeaders(string uri, string token, List<HeaderRest> headers)
        {
            RestEntity restEntity = new RestEntity();
            try
            {
                var client = _clientFactory.CreateClient();
                var header = new AuthenticationHeaderValue("Bearer", token.Replace(Constants.Bearer, string.Empty));
                client.DefaultRequestHeaders.Authorization = header;

                headers?.ForEach(x => client.DefaultRequestHeaders.Add(x.Name, x.Value));

                var result = await client.GetAsync(string.Concat(uri));
                var statusCode = result.StatusCode;
                restEntity.StatusCode = Convert.ToInt32(statusCode);
                restEntity.ResultContent = result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"|RestClient|GetAsyncWithHeaders - uri: {uri}");
                restEntity.ResultContent = e.Message;
            }
            return restEntity;
        }
        public async Task<RestEntity> PostAsyncWithHeaders(string uri, List<HeaderRest> headers, dynamic request)
        {
            RestEntity restEntity = new RestEntity();
            try
            {
                var client = _clientFactory.CreateClient();
                var jsonParameters = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonParameters, Encoding.UTF8, "application/json");

                headers?.ForEach(x => client.DefaultRequestHeaders.Add(x.Name, x.Value));

                var result = await client.PostAsync(uri, content);
                var statusCode = result.StatusCode;
                restEntity.StatusCode = Convert.ToInt32(statusCode);
                restEntity.ResultContent = result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"RestClient|PostAsyncWithHeaders - URI: {uri}");
                restEntity.ResultContent = e.Message;
            }
            return restEntity;
        }
        public async Task<RestEntity> PostAsyncWithTokenAndHeaders(string uri, dynamic request, string token, List<HeaderRest> headers)
        {
            RestEntity restEntity = new RestEntity();
            string transactionID = string.Empty;
            string transactionIDHeader = headers?.Find(x => x.Name.Equals(Constants.TransactionId))?.Value ?? string.Empty;

            try
            {
                var client = _clientFactory.CreateClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                var jsonParameters = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonParameters, Encoding.UTF8, Constants.MediaType);
                var header = new AuthenticationHeaderValue("Bearer", token.Replace(Constants.Bearer, string.Empty));
                client.DefaultRequestHeaders.Authorization = header;

                headers?.ForEach(x => client.DefaultRequestHeaders.Add(x.Name, x.Value));

                var result = await client.PostAsync(uri, content);

                HttpHeaders headerResponse = result.Headers;
                if (headerResponse.TryGetValues(Constants.TransactionId, out IEnumerable<string> values))
                {
                    transactionID = values.First();
                    restEntity.TransactionId = transactionID;
                }

                if (result.Headers.TryGetValues("Signature", out IEnumerable<string> signatureValues))
                {
                    restEntity.Signature = signatureValues.FirstOrDefault();
                }

                if (result.Headers.TryGetValues("Token", out IEnumerable<string> tokenValue))
                {
                    restEntity.Token = tokenValue.FirstOrDefault();
                }

                var statusCode = result.StatusCode;
                restEntity.StatusCode = Convert.ToInt32(statusCode);
                restEntity.ResultContent = result.Content.ReadAsStringAsync().Result;

                _Logger.LogInfo($"{transactionIDHeader}|RestClient|URI: {uri}");
                _Logger.LogInfo($"{transactionIDHeader}|RestClient|StatusCode: {restEntity.StatusCode}");
                _Logger.LogInfo($"{transactionIDHeader}|RestClient|ResultContent: {restEntity.ResultContent}");
            }
            catch (Exception e)
            {
                _Logger.LogError(e, $"RestClient|PostAsyncWithTokenAndHeaders - URI: {uri}");
                restEntity.ResultContent = e.Message;
            }
            return restEntity;
        }
        public class RestEntity
        {
            public int StatusCode { get; set; }
            public string ResultContent { get; set; }
            public string TransactionId { get; set; }
            public string Signature { get; set; }

            [JsonIgnore]
            public string Token { get; set; }
        }
        public class HeaderRest
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}