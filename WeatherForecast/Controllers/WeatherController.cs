using WeatherForecast.Model;
using WeatherForecast.Repository;
using WeatherForecast.WebScraping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public WeatherNow Get(string cityWithState)
        {
            //city,ESTATE
            var ta = new WeatherNowWebScraping();
            return ta.WeatherNow(cityWithState);
        }

        [HttpGet]
        [Route("PeriodWeather")]
        public ActionResult PeriodWeather(string cityWithState)
        {

            var weatherRepo = new ForecastRepository();
            var city = ClimaTempoHelper.CityState(cityWithState).Keys.FirstOrDefault();
            var obj = weatherRepo.GetPeriodForecast(city);
            var data = JsonConvert.DeserializeObject<List<WeatherPeriodJsonModel>>(obj.DataJson);
            var json = new { list = data };

            return Ok(json);
        }

        [HttpGet]
        [Route("DailyForecast")]
        public ActionResult DailyForecast(string cityWithState)
        {
            var forecastRepo = new ForecastRepository();
            var city = ClimaTempoHelper.CityState(cityWithState).Keys.FirstOrDefault();
            var obj = forecastRepo.ConsultaPrevisaoDias(city);
            var data = JsonConvert.DeserializeObject<List<ForecastDaily>>(obj.DataJson);         

            var json = new { list = data };

            return Ok(json);
        }     
    }
}

