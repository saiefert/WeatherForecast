using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Model
{
    public struct WeatherNow
    {
        public int Temperature { get; set; }
        public int FellsLike { get; set; }
        public string Weather { get; set; }
        public int WindSpeed { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
    }
}
