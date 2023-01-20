using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_api_c
{
    public class OpenWeather
    {
        public List<Weather> weather { get; set; }
        public main main { get; set; }
        public string cod { get; set; }
        public string message { get; set; }
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
