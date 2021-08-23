using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Model
{
    public struct ForecastDaily
    {
        public string Day { get; set; }
        public string DayOfWeek { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int PropabilityOfRain { get; set; }
        public string Weather { get; set; }
        public int Precipitation { get; set; }
        public int MinHumidity { get; set; }
        public int MaxHumidity { get; set; }
    }
}
