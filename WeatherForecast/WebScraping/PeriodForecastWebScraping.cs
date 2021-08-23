using WeatherForecast.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WeatherForecast.Repository;
using System.Threading;

namespace WeatherForecast.WebScraping
{
    public class PeriodForecastWebScraping
    {
        public List<WeatherPeriodJsonModel> Forecast(string localization, string forecast = "today")
        {
            var cidade = localization.Split(",")[0];
            var cidadeClimaTempo = ClimaTempoHelper.NormalizeCityClimaTempo(cidade);
            var estado = localization.Split(",")[1].ToLower();

            int codigo = ClimaTempoHelper.ReturnCityCode(cidade, estado);

            HtmlDocument htmlDoc;
            if (forecast.Equals("today"))
            {
                htmlDoc = new HtmlDocument();
                var html = Selenium($"https://www.climatempo.com.br/previsao-do-tempo/cidade/{codigo}/{cidadeClimaTempo}-{estado}");
                htmlDoc.LoadHtml(html);
            }
            else
            {
                htmlDoc = new HtmlDocument();
                var html = Selenium($"https://www.climatempo.com.br/previsao-do-tempo/amanha/cidade/{codigo}/{cidadeClimaTempo}-{estado}");
                htmlDoc.LoadHtml(html);
            }

            return CreateObjects(htmlDoc);
        }


        private List<WeatherPeriodJsonModel> CreateObjects(HtmlDocument htmlDoc)
        {
            var htmlGrafico = Graphic(htmlDoc);
            var htmlWeather = ImageHourWeather(htmlDoc);
            var htmlMetricas = MainCardMetrics(htmlDoc);

            Regex regexPrecipitation = new Regex(@"\d+%");
            var rainProbability = regexPrecipitation.Match(htmlMetricas.OuterHtml).Value;
            var weathers = WeatherForecastPeriod(htmlDoc);

            var deserialized = JsonConvert.DeserializeObject<List<WeatherPeriodJsonModel>>(JsonData(htmlGrafico));
            var forecastList = new List<WeatherPeriodJsonModel>();

            foreach (var weather in htmlWeather)
            {
                var hora = Convert.ToInt32(weather.Descendants("p").SingleOrDefault().InnerText.Replace("h", ""));
                var deserializedObject = deserialized.Where(x => x.Date.Hour == hora).SingleOrDefault();

                var forecast = new WeatherPeriodJsonModel();
                forecast = deserializedObject;

                var image = weather.Descendants("img").SingleOrDefault().OuterHtml;
                var imgSource = RegexHelper.ImageSource(image);

                var weatherImage = ClimaTempoHelper.ImageWeather(imgSource);
                forecast.Weather = weatherImage;
                forecast.ProbabilityOfRain = ProbabilityOfRain(rainProbability, weatherImage);

                forecast.ConvertedDate = deserializedObject.Date.ToString("dd-MM-yyyy HH-mm");

                forecastList.Add(forecast);
            };

            return forecastList;
        }

        private int ProbabilityOfRain(string chance, string weather)
        {
            if (weather.Contains("chuv"))
                return Convert.ToInt32(chance.Replace("%", ""));

            return 0;
        }

        private Dictionary<string, string> WeatherForecastPeriod(HtmlDocument htmlDoc)
        {
            var splitDiv = htmlDoc.DocumentNode.Descendants("div")
                 .Where(x => x.GetAttributeValue("class", "")
                 .Equals("_center"))
                 .ToList();

            var dictionary = new Dictionary<string, string>();

            foreach (var div in splitDiv)
            {
                var match = RegexHelper.ImageSource(div.OuterHtml);
                var weather = ClimaTempoHelper.ImageWeather(match);
                dictionary.Add(div.InnerText, weather);
            }

            return dictionary;
        }

        private HtmlNode MainCardMetrics(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.Descendants("ul")
                      .Where(node => node.GetAttributeValue("class", "")
                      .Equals("variables-list"))
                      .FirstOrDefault();
        }


        private HtmlNode Graphic(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.Descendants("div")
                      .Where(node => node.GetAttributeValue("id", "")
                      .Equals("wrapper-chart-1"))
                      .FirstOrDefault();
        }

        private List<HtmlNode> ImageHourWeather(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("divisions"))
                .ToList();
        }

        public string Selenium(string fullUrl)
        {
            var options = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };

            options.AddArguments(new List<string>() { "headless", "disable-gpu" });
            var browser = new ChromeDriver(options);
            browser.Navigate().GoToUrl(fullUrl);


            var cards = browser.FindElementsByClassName("card");
            var html = "";

            foreach (var card in cards)
            {
                html += card.GetAttribute("outerHTML");
            };

            browser.Quit();
            return html;
        }
        public string JsonData(HtmlNode html)
        {
            var json = html.GetAttributeValue("data-infos", "");
            return json.Replace("&quot;", "\"");
        }
    }
}
