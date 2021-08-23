using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Model
{
    public class RegionsWeatherModel
    {
        public int Id { get; set; }
        public string Regiao { get; set; }
        public string State { get; set; }
        public string DataJson { get; set; }
        public DateTime ConsumeHour { get; set; }
        public string City { get; set; }
    }
    public class PeriodForecastModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DataJson { get; set; }
        public DateTime ConsumeHour { get; set; }
    }

    public class DailyForecastModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DataJson { get; set; }
        public DateTime ConsumeHour { get; set; }
    }

    public class CityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityNormalized { get; set; }
        public float Latitide { get; set; }
        public float Longitude { get; set; }
        public string Region { get; set; }
        public int IdClimaTempo { get; set; }
    }

    public class CidadesMtModel
    {
        public int Id { get; set; }
        public string Cidade { get; set; }
        public string CidadeApi { get; set; }
        public float Latitide { get; set; }
        public float Longitude { get; set; }
        public string Regiao { get; set; }
        public int IdClimaTempo { get; set; }
    }
}
