using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ClimaRmc.Model
{
    public struct WeatherPeriodJsonModel
    {
        public DateTime Date { get; set; }
        public string ConvertedDate { get; set; }
        public string Weather { get; set; }
        public int ProbabilityOfRain { get; set; }
        public Wind Wind { get; set; }
        public Temperature Temperature { get; set; }
        public Rain Rain { get; set; }
        public Pressure Pressure { get; set; }
        public Humidity Humidity { get; set; }
    }

    public struct PrevisaoPeriodo
    {
        public string DataHora { get; set; }
        public string Clima { get; set; }
        public int ChanceChuva { get; set; }
        public decimal Precipitacao { get; set; }
        public int Temperatura { get; set; }
        public int Humidade { get; set; }

    }

    public struct Wind
    {
        public int Velocity { get; set; }
        public decimal Gust { get; set; }
        [JsonPropertyName("direction_degrees")]
        public decimal DirectionDegrees { get; set; }
        public string Direction { get; set; }
    }

    public struct Temperature
    {
        public int temperature { get; set; }
    }
    public struct Rain
    {
        public decimal Precipitation { get; set; }
    }

    public struct Pressure
    {
        public int pressure { get; set; }
    }
    public struct Humidity
    {
        public int RelativeHumidity { get; set; }
    }

}
