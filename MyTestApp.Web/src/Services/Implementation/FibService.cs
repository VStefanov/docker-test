using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MyTestApp.Services.Implementation
{
    public class FibService : IFibService
    {

        private readonly IConfiguration configuration;
        private readonly RestClient restClient;
        public FibService(IConfiguration configuration)
        {
            this.configuration = configuration;
            string apiAddress =this.configuration.GetSection("ApiSettings").GetSection("Address").Value;
            restClient = new RestClient(apiAddress);
        }

        public void AddIndexToStorage(string number)
        {
            var request = new RestRequest("api/calculations/AddRedisKey", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { Index= number });
            restClient.Execute(request);

            request = new RestRequest("api/calculations/AddPostgreKey", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { Index = number });
            restClient.Execute(request);
        }

        public List<string> GetAvailableIndexes()
        {
            var request = new RestRequest("api/calculations/GetPostgreKeys", Method.GET);
            IRestResponse<List<string>> response = restClient.Execute<List<string>>(request);

            return response.Data;
        }

        public Dictionary<string, string> GetAvailableIndexesValues()
        {
            var request = new RestRequest("api/calculations/GetKeyValuePairFromRedis", Method.GET);
            IRestResponse<Dictionary<string, string>> response = restClient.Execute<Dictionary<string,string>>(request);

            return response.Data;
        }
    }
}
