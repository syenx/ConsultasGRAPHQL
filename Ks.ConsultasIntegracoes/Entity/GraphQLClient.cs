
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Ks.ConsultasIntegracoes.Entity
{
    [Serializable]
    public class GraphQLClient
    {
        private RestClient _client;

        public GraphQLClient(string GraphQLApiUrl)
        {
            this._client = new RestClient(GraphQLApiUrl);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public object Execute(
          string query,
          object variables = null,
          Dictionary<string, string> additionalHeaders = null)
        {
            RestRequest restRequest = new RestRequest("/", Method.POST);
            restRequest.Timeout = 10000;
            if (additionalHeaders != null && additionalHeaders.Count > 0)
            {
                foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                    restRequest.AddHeader(additionalHeader.Key, additionalHeader.Value);
            }
            restRequest.AddJsonBody((object)new
            {
                query = query,
                variables = variables
            });
            this._client.Timeout = 10000;
            IRestResponse restResponse = this._client.Execute((IRestRequest)restRequest);
            return restResponse.StatusCode != (HttpStatusCode)0 ? (object)JObject.Parse(restResponse.Content) : (object)"";
        }
    }
}
