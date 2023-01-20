using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;

namespace weather_api_c
{
    public static class Cities
    {
        [FunctionName("Cities")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string city = req.Query["name"];
            log.LogInformation($"name:{ city }");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            city = city ?? data?.name;

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

                //Read Server Response
                HttpResponseMessage response = await newClient.SendAsync(newRequest);
                string resp = await response.Content.ReadAsStringAsync();
                //log.LogInformation(resp);
                var jsonResp = JsonConvert.DeserializeObject<OpenWeather>(resp);

                log.LogInformation($"temp:{jsonResp.main.temp}");
                log.LogInformation($"condition:{jsonResp.weather[0].description}");

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

    public class OpenWeather
    {
        public List<Weather> weather { get; set; }
        public main main { get; set; }
    }
    public class Weather
    {
        public string description { get; set; }
    }
    public class main
    {
        public float temp { get; set; }
    }
}
