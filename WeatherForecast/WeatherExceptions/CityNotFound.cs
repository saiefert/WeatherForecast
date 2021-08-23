using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.ClimaExceptions
{
    public class CityNotFound : Exception
    {
        public CityNotFound(string message) : base(message)
        {
        }

        public CityNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
