using WeatherForecast.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WeatherForecast.WebScraping
{
    public class WeatherNowWebScraping
    {
        public WeatherNow WeatherNow(string local)
        {
            var city = local.Split(",")[0];
            var state = local.Split(",")[1].ToLower();
            var cityClimaTempo = ClimaTempoHelper.NormalizeCityClimaTempo(city);
            var codigo = ClimaTempoHelper.ReturnCityCode(city, state);

            var web = new HtmlWeb();
            var htmlDoc = web.Load($"https://www.climatempo.com.br/previsao-do-tempo/agora/cidade/{codigo}/{cityClimaTempo}-{state}");

            var html = htmlDoc.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Contains("col-lg-6"))
                        .ToList();

            return WeatherNowObject(html);     
        }
      
        private WeatherNow WeatherNowObject(List<HtmlNode> html)
        {
            var weather = new WeatherNow();

            foreach (var div in html)
            {
                if (div.InnerHtml.Contains("º"))
                {
                    Regex regex = new Regex(@"(\d)+");
                    var matches = regex.Matches(div.InnerText);

                    var temp = matches[0].Value;
                    var feelsLike = matches[1].Value;
                    var windSpeed = matches[2].Value;
                    var humidity = matches[3].Value;
                    var pressure = matches[4].Value;
                    var image = RegexHelper.ImageSource(div.OuterHtml);

                    weather = new()
                    {
                        Temperature = Convert.ToInt32(temp),
                        FellsLike = Convert.ToInt32(feelsLike),
                        WindSpeed = Convert.ToInt32(windSpeed),
                        Humidity = Convert.ToInt32(humidity),
                        Weather = ImageWeather.Weather(image),
                        Pressure = Convert.ToInt32(pressure)
                    };

                    return weather;
                }
            }

            return weather;
        }
    }
}
