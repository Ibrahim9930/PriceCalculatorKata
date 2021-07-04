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
        public string BaseAddress { get; set; }

        public Task<Dictionary<string, object>> GetResponse(string endpoint,
            Dictionary<string, object> requestArguments);
    }

    public class HttpClientWebAPI : IWebAPI
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

        public async Task<Dictionary<string, object>> GetResponse(string endpoint,
            Dictionary<string, object> requestArguments)
        {
            string httpConcatenatedArguments = "";
            foreach (var argument in requestArguments)
            {
                httpConcatenatedArguments += $"{argument.Key}={argument.Value}&";
            }

            string requestURI = endpoint + "?" + httpConcatenatedArguments;
            var response =  _client.GetAsync(requestURI).Result;
            response.EnsureSuccessStatusCode();
            dynamic reponseContent = response.Content.ReadAsAsync<ExpandoObject>().Result;
            var dic = new Dictionary<string, object>(reponseContent);
            return dic;
        }
    }
}