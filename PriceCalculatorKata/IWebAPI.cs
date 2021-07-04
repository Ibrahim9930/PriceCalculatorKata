using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace PriceCalculatorKata
{
    public interface IWebAPI
    {
        public Task<ImmutableDictionary<string, object>> GetResponse(string endpoint,
            Dictionary<string, object> requestArguments);
    }

    class HttpClientWebAPI : IWebAPI
    {
        private HttpClient _client;

        public string BaseAddress
        {
            get { return _client.BaseAddress.ToString(); }
            set { _client.BaseAddress = new Uri(value); }
        }

        public HttpClientWebAPI()
        {
            _client = new HttpClient();
        }

        public async Task<ImmutableDictionary<string, object>> GetResponse(string endpoint,
            Dictionary<string, object> requestArguments)
        {
            string httpConcatenatedArguments = "";
            foreach (var argument in requestArguments)
            {
                httpConcatenatedArguments += $"{argument.Key}={argument.Value}&";
            }
            
            var response = await _client.GetAsync(endpoint + "?" + httpConcatenatedArguments);
            response.EnsureSuccessStatusCode();
            dynamic reponseContent = await response.Content.ReadAsAsync<ExpandoObject>();
            return ((ImmutableDictionary<string, object>) reponseContent).ToImmutableDictionary();
        }
    }
}