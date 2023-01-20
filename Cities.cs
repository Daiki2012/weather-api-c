using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace weather_api_c
{
    public static class Cities
    {
        [FunctionName("Cities")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // expecting to have a `name` query
            if(req.Query.Count == 0)
                return new BadRequestObjectResult("Not supported request");

            string city = req.Query["name"];

            if (string.IsNullOrEmpty(city))
                return new NotFoundObjectResult("City name not found");

            string apiKey = " YOUR API KEY ";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            
            try
            {
                float temperature = Single.MinValue;    
                string condition = null;

                HttpClient newClient = new HttpClient();
                HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, url);

                HttpResponseMessage response = await newClient.SendAsync(newRequest);
                string resp = await response.Content.ReadAsStringAsync();

                //log.LogInformation(resp);
                var jsonResp = JsonConvert.DeserializeObject<OpenWeather>(resp);

                // when open weather api returns error, return the error message
                if (jsonResp.cod != "200")
                    return new BadRequestObjectResult(jsonResp.message);

                temperature = jsonResp.main.temp;
                condition = jsonResp.weather[0].description;

                // just in case, if the open weather doe snot respond
                return (temperature == Single.MinValue || condition == null) ?
                    new NotFoundObjectResult("Weather info not found") :
                    new OkObjectResult($"Hello {city}! Current temperature is {temperature} degree, and weather condition is {condition}!");

            } catch(HttpRequestException ex)
            {
                return new BadRequestObjectResult($"Something went wrong: {ex.ToString()}");
            }
        }
    }
}
