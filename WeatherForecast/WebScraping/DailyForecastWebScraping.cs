using ClimaRmc.ClimaExceptions;
using ClimaRmc.Model;
using ClimaRmc.Repository;
using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ClimaRmc.WebScraping
{
    public class DailyForecastWebScraping
    {
        public List<ForecastDaily> Forecast(string localization, int days = 7, bool todayForecast = false)
        {
            var city = localization.Split(",")[0];
            var cityClimaTempo = ClimaTempoHelper.NormalizeCityClimaTempo(city);
            var state = localization.Split(",")[1].ToLower();

            var code = ClimaTempoHelper.ReturnCityCode(city, state);

            var htmlDoc = new HtmlDocument();
            var html = Selenium($"https://www.climatempo.com.br/previsao-do-tempo/15-dias/cidade/{code}/{cityClimaTempo}-{state}");
            htmlDoc.LoadHtml(html);

            var list = HtmlCards(htmlDoc);
            return CreateObjects(list, days, todayForecast);
        }

        private List<ForecastDaily> CreateObjects(List<HtmlNode> list, int days, bool forecastToday)
        {
            int contDay = 0;

            var forecastList = new List<ForecastDaily>();

            foreach (var section in list)
            {
                //split de sections in divs
                var divs = section
                    .Descendants("div")
                    .ToList();

                //split the main divs
                var divOne = divs[0].InnerHtml;
                var divTwo = divs[1].InnerHtml;

                //pega o dia que está na div 1
                Regex regex = new Regex(@"\d+");
                var match = regex.Match(divOne);
                var dia = match.Value;

                //get the temperature min and max in div two 
                Regex regexMinMax = new Regex(@"\d+°", RegexOptions.Multiline);
                var matchMinMax = regexMinMax.Matches(divTwo);
                var min = matchMinMax[0].Value;
                var max = matchMinMax[1].Value;

                //acha a chance de chuva e a precipitação
                Regex regexPrecipitaion = new Regex(@"\d+mm - \d+%");
                var matchPrecipitaion = regexPrecipitaion
                    .Match(section.InnerHtml)
                    .Value.Split("-");

                var precipitation = matchPrecipitaion[0].Trim();
                var rainProbability = matchPrecipitaion[1].Trim();

                Regex regexHumidity = new Regex(@"\d+%");
                var matchHumidity = regexHumidity.Matches(section.InnerText).ToList();

                var minHumidity = matchHumidity[2].Value;
                var maxHumidity = matchHumidity[3].Value;

                var image = section.Descendants("img").
                  FirstOrDefault().OuterHtml;

                var imageSource = RegexHelper.ImageSource(image);

                var weather = ImageWeather.Weather(imageSource);

                var forecast = new ForecastDaily()
                {
                    Weather = weather,
                    Day = DateTime.Now.AddDays(contDay).ToString("dd-MM-yyyy"),
                    DayOfWeek = DateTime.Now.AddDays(contDay).DayOfWeek.ToString(),
                    Min = Convert.ToInt32(min.Replace("°", "")),
                    Max = Convert.ToInt32(max.Replace("°", "")),
                    Precipitation = Convert.ToInt32(precipitation.Replace("mm", "")),
                    PropabilityOfRain = Convert.ToInt32(rainProbability.Replace("%", "")),
                    MinHumidity = Convert.ToInt32(minHumidity.Replace("%", "")),
                    MaxHumidity = Convert.ToInt32(maxHumidity.Replace("%", ""))

                };

                forecastList.Add(forecast);
                contDay++;

                if (contDay > days)
                    break;
            }

            if (!forecastToday)
                forecastList.RemoveAt(0);

            return forecastList;
        }

        private List<HtmlNode> HtmlCards(HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.Descendants("section")
                      .Where(node => node.GetAttributeValue("class", "")
                      .Contains("-daily-infos"))
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
            Thread.Sleep(1000);
            var actions = new Actions(browser);
            var button = browser.FindElementByClassName("action-button");
            actions.Click(button).Build().Perform();
            Thread.Sleep(2000);
            var source = browser.PageSource;

            browser.Quit();
            return source;
        }     
    }
}
