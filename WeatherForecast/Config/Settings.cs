using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Config
{
    public static class Settings
    {
        public static dynamic Conf()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var text = File.ReadAllText("Config/settings.json");
            return JsonConvert.DeserializeObject<dynamic>(text);
        }

        public static dynamic DatabaseConnection()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var text = File.ReadAllText("Config/Connection.json");

            return JsonConvert.DeserializeObject<dynamic>(text);
        }  

        public static List<string> MainCities()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var text = File.ReadAllText("Config/MainCities.json");
            var row = JsonConvert.DeserializeObject<string[]>(text);
            var cities = new List<string>();

            foreach (var city in row)
            {
                cities.Add(city.Split(";")[0]);
            }

            return cities;
        }

        public static string AlternativeCities(string city)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var text = File.ReadAllText("Config/MainCities.json");
            var row = JsonConvert.DeserializeObject<string[]>(text);

            return row.Where(x => x.Contains(city))
                      .FirstOrDefault()
                      .Split(";")[1];            

        }
    }
}
