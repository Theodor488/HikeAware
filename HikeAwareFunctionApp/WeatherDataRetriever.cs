using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HikeAwareFunctionApp
{
    public class WeatherDataRetriever
    {
        private readonly ILogger _logger;
        static readonly HttpClient httpClient = new HttpClient();

        public WeatherDataRetriever(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WeatherDataRetriever>();
        }

        [Function("WeatherDataRetriever")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Coordinates for Mt. Rainier Summit
            string latitude = "46.8523";
            string longitude = "-121.7603";
            
            // Open-Meteo API endpoint
            string apiUrl = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=snow_depth,freezing_level_height,temperature_2m,precipitation,rain,precipitation_probability&daily=snowfall_sum,precipitation_sum&current_weather=true&timezone=auto";

            try
            {
                // Call the weather API
                HttpResponseMessage httpResponse = await httpClient.GetAsync(apiUrl);
                httpResponse.EnsureSuccessStatusCode();

                // Create the response
                var response = req.CreateResponse(httpResponse.IsSuccessStatusCode ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

                // Set the content
                string responseBody = await httpResponse.Content.ReadAsStringAsync();

                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(responseBody);
                return response;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "\nAn error occurred while retrieving weather data!");
            }

            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            return badResponse;
        }
    }
}
