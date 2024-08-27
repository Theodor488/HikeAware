using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
//using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace HikeAwareFunctionApp
{
    public class WeatherDataRetriever
    {
        private readonly ILogger _logger;
        private HttpClient _httpClient;

        public WeatherDataRetriever(ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            _logger = loggerFactory.CreateLogger<WeatherDataRetriever>();
            _httpClient = httpClient;
        }

        [Function("WeatherDataRetriever")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string cityName = "Seattle";
            string apiKey = "";
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}";

            // Call the weather API
            //HttpRequestMessage apiResponse = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            HttpResponseMessage apiResponse = await _httpClient.GetAsync(apiUrl);

            // Create the response
            var response = req.CreateResponse(apiResponse.IsSuccessStatusCode ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

            // Set the content
            string responseBody = await apiResponse.Content.ReadAsStringAsync();
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync(responseBody);

            return response;
        }
    }
}
