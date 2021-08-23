using ClimaRmc.Repository;
using HtmlAgilityPack;
using OpenQA.Selenium;
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
    public class CitiesCodeWebScraping
    {
        public void GetCitiesId(string state, string fullUrl = "https://www.climatempo.com.br/")
        {
            var options = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };

            //options.AddArguments(new List<string>() { "headless", "disable-gpu", "start-fullscreen" });
            options.AddArguments(new List<string>() { "start-maximized" });
            var browser = new ChromeDriver(options);
            browser.Navigate().GoToUrl(fullUrl);

            Thread.Sleep(2000);
            var search = browser.FindElement(By.Id("searchGeneral"));

            var repo = new CitiesRepository();

            dynamic cityState = repo.City();

            foreach (var city in cityState)
            {
                var actions = new Actions(browser);
                actions.DoubleClick(search);
                actions.SendKeys(Keys.Control + 'a').Build().Perform();
                actions.SendKeys(Keys.Backspace).Build().Perform();


                actions.SendKeys(search, city.apiCity);
                actions.Perform();
                Thread.Sleep(3000);
                var page = browser.PageSource;

                try
                {
                    var link = GetLink(page, city.apiCity, state);
                    var id = IdClimatempo(link);

                    repo.EditIdClimatempoCities(city.apiCity, id);

                    Thread.Sleep(2000);
                }
                catch (Exception)
                {

                }
            }

            Thread.Sleep(2000);           

            browser.Close();
        }

        private string GetLink(string html, string city, string state)
        {
            var normalizedCity = city.Replace(" ", "").ToLower();

            Regex regex = new Regex($@"\/previsao-do-tempo\/cidade\/\d+\/{normalizedCity}-{state.ToLower()}");
            return regex.Match(html).Value;

        }

        private int IdClimatempo(string link)
        {
            Regex regex = new Regex(@"\d+");
            return Convert.ToInt32(regex.Match(link).Value);
        }
    }
}
