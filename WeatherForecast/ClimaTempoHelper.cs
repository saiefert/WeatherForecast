using ClimaRmc.Repository;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimaRmc
{
    public static class ClimaTempoHelper
    {
        public static string ImageWeather(string image)
        {
            var arrayNames = image.Split(@"/");
            var name = arrayNames[arrayNames.Count() - 1].Replace(".svg", "");

            switch (name)
            {
                case "1":
                case "1n":
                    return "clear";
                case "2":
                case "2n":
                    return "few clouds";
                case "2r":
                case "2rn":
                case "3tm":
                    return "cloudy";
                case "3":
                case "3n":
                    return "light rain";
                case "4":
                case "4n":
                    return "sun with rain";
                case "4r":
                case "4rn":
                    return "rain showers";
                case "4t":
                case "4tn":
                    return "sun rain with thunderstorms";
                case "5":
                case "5n":
                    return "rainy";
                case "6":
                case "6n":
                    return "thunderstorm rain";
                case "7":
                case "7n":
                    return "frost";
                default:
                    return image;
            }
        }

        public static int DateForEpoch(DateTime date)
        {
            TimeSpan t = DateTime.UtcNow - date;
            int secondsSinceEpoch = (int)t.TotalSeconds;

            return secondsSinceEpoch;
        }

        public static Dictionary<string, string> CityState(string local)
        {
            var city = local.Split(",")[0];
            var state = local.Split(",")[1].ToLower();
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(city, state);

            return dictionary;
        }

        public static int ReturnCityCode(string city, string state)
        {
            var repo = new CitiesRepository();         
            
            return repo.CidadesMs(city).IdClimaTempo;
        }

        //This method normalize name of few city of State of Mato Grosso do sul
        public static string NormalizeCityClimaTempo(string city)
        {
            if (city.Contains("Paraiso das"))
                return "paraiso";
            if (city.Contains("Bataypora"))
                return "bataipora";

            return city.ToLower().Replace(" ", "").Replace("'", "");
        }


        public static string Screenshot(ChromeDriver browser)
        {
            var ss = ((ITakesScreenshot)browser).GetScreenshot();
            string screenshot = ss.AsBase64EncodedString;
            byte[] screenshotAsByteArray = ss.AsByteArray;
            //ss.SaveAsFile(@"C:\QRCode\filename");
            //ss.ToString();

            return screenshot;
        }
    }
}
