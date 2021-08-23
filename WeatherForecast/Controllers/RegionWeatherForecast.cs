using WeatherForecast.Model;
using WeatherForecast.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionWeatherForecast : ControllerBase
    {
        
        [Route("North/{refer}")]
        [HttpGet]
        public ActionResult North(string refer)
        {
            var repo = new ForecastRepository();
            var region = repo.ConsultaTempoRegiao(refer, "ms");
            var json = new { lista = RegionWeather(region, refer) };
            
            return Ok(json);
        }


        [Route("South/{refer}")]
        [HttpGet]
        public ActionResult South(string refer)
        {
            var repo = new ForecastRepository();
            var region = repo.ConsultaTempoRegiao(refer, "ms");

            var json = new { lista = RegionWeather(region, refer) };
            return Ok(json);
        }

        [Route("East/{refer}")]
        [HttpGet]
        public ActionResult East(string refer)
        {
            var repo = new ForecastRepository();
            var region = repo.ConsultaTempoRegiao(refer, "ms");
            var json = new { lista = RegionWeather(region, refer) };

            return Ok(json);
        }

        [Route("West/{refer}")]
        [HttpGet]
        public ActionResult West(string refer)
        {
            var repo = new ForecastRepository();
            var region = repo.ConsultaTempoRegiao(refer, "ms");
            var json = new { lista = RegionWeather(region, refer) };

            return Ok(json);
        }

        [Route("Center/{refer}")]
        [HttpGet]
        public ActionResult Center(string refer)
        {
            var repo = new ForecastRepository();
            var region = repo.ConsultaTempoRegiao(refer, "ms");
            var json = new { lista = RegionWeather(region, refer) };

            return Ok(json);
        }

        private List<Cidade> RegionWeather(List<RegionsWeatherModel> region, string referencia)
        {
            List<Cidade> list = new();

            foreach (var city in region)
            {
                var data = JsonConvert.DeserializeObject<List<ForecastDaily>>(city.DataJson);

                foreach (var item in data)
                {
                    var date = Convert.ToDateTime(item.Day);

                    Cidade previsao = new()
                    {
                        Name = city.City,
                        Weather = item.Weather,
                        Day = item.Day,
                        DayOfWeek = item.DayOfWeek,
                        Min = item.Min,
                        Max = item.Max,
                        Precipitaion = item.Precipitation,
                        ProbabilityOfRain = item.PropabilityOfRain,
                        Date = date,
                        Ref = date.Date > DateTime.Now.Date ? "Tomorrow" : date.Date < DateTime.Now.Date ? "Yesterday" : "Today"
                    };

                    list.Add(previsao);
                }
            }

            return list.Where(x => x.Ref.Equals(referencia)).OrderBy(x => x.Name).ToList();
        }

        public record Cidade
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public string Day { get; set; }
            public string Ref { get; set; }
            public string DayOfWeek { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }
            public int ProbabilityOfRain { get; set; }
            public string Weather { get; set; }
            public int Precipitaion { get; set; }
        }
    }
}
