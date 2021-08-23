using ClimaRmc.Config;
using ClimaRmc.Model;
using ClimaRmc.Repository;
using ClimaRmc.WebScraping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClimaRmc
{
    public static class WorkerHelper
    {
        private const int _numberOfTry = 3;

        public static bool DayOfWeek(dynamic valueObject)
        {
            var date = valueObject.diaSemana;

            if (Convert.ToBoolean(date.segunda) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Monday))
                return true;
            else if (Convert.ToBoolean(date.terca) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Tuesday))
                return true;
            else if (Convert.ToBoolean(date.quarta) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Wednesday))
                return true;
            else if (Convert.ToBoolean(date.quinta) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Thursday))
                return true;
            else if (Convert.ToBoolean(date.sexta) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Friday))
                return true;
            else if (Convert.ToBoolean(date.sabado) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Saturday))
                return true;
            else if (Convert.ToBoolean(date.domingo) && (DateTime.Today.DayOfWeek == System.DayOfWeek.Sunday))
                return true;

            return false;
        }

        public static PeriodForecastModel CreateObjectPeriodForecast(string localidade, string hojeOuAmanha)
        {
            var listaPeriodo = ListPeriodForecast(localidade, hojeOuAmanha);
            var dados = JsonConvert.SerializeObject(listaPeriodo);

            var dicionario = ClimaTempoHelper.CityState(localidade).FirstOrDefault();
            var cidade = dicionario.Key;
            var estado = dicionario.Value;

            var previsaoPeriodos = new PeriodForecastModel
            {
                City = cidade,
                State = estado,
                DataJson = dados
            };

            return previsaoPeriodos;
        }

        public static List<WeatherPeriodJsonModel> ListPeriodForecast(string local, string todayOrTomorrow)
        {
            var loc = new PeriodForecastWebScraping();
            var lista = loc.Forecast(local, todayOrTomorrow);

            if (lista is null || !lista.Any())
            {
                int tentativas = 3;

                do
                {
                    lista = loc.Forecast(local, todayOrTomorrow);
                    tentativas--;

                } while (tentativas > 0 && !lista.Any());
            }

            return lista;
        }

        public static DailyForecastModel CreateObjectDailyForecast(string local)
        {
            var listaDias = ListDailyForecast(local);
            var data = JsonConvert.SerializeObject(listaDias);

            var dictionary = ClimaTempoHelper.CityState(local).FirstOrDefault();
            var city = dictionary.Key;
            var state = dictionary.Value;

            var dailyForecast = new DailyForecastModel
            {
                City = city,
                State = state,
                DataJson = data
            };

            return dailyForecast;
        }

        public static List<ForecastDaily> ListDailyForecast(string local)
        {
            var loc = new DailyForecastWebScraping();
            var list = loc.Forecast(local);

            if (list is null || !list.Any())
            {
                int numberOfTry = _numberOfTry;

                do
                {
                    list = loc.Forecast(local);
                    numberOfTry--;

                } while (numberOfTry > 0 && !list.Any());

                //verifica se tem cidade alternativa e tenta novamente
                if (numberOfTry <= 0)
                {
                    var alternative = Settings.AlternativeCities(local);
                    numberOfTry = _numberOfTry;

                    if (alternative != "none")
                    {
                        do
                        {
                            list = loc.Forecast(local);
                            numberOfTry--;

                        } while (numberOfTry > 0 && !list.Any());
                    }
                }
            }

            return list;
        }

        public static void WriteRegion()
        {
            var dictionary = RandomCities();
            var webScraping = new DailyForecastWebScraping();
            var repo = new ForecastRepository();

            foreach (var region in dictionary)
            {
                foreach (var city in region.Value)
                {
                    var local = $"{city.CityNormalized},MS";
                    var list = webScraping.Forecast(local, 1, true);

                    if (list is null || !list.Any())
                    {
                        int numberOfTry = _numberOfTry;

                        do
                        {
                            list = webScraping.Forecast(local, 1, true);
                            numberOfTry--;

                        } while (numberOfTry > 0 && !list.Any());

                        //verifica se tem cidade alternativa e tenta novamente
                        if (numberOfTry <= 0)
                        {
                            var alternative = Settings.AlternativeCities(local);
                            numberOfTry = _numberOfTry;

                            if (alternative != "none")
                            {
                                do
                                {
                                    list = webScraping.Forecast(local, 1, true);
                                    numberOfTry--;

                                } while (numberOfTry > 0 && !list.Any());
                            }
                        }
                    }

                    var tempoRegiao = new RegionsWeatherModel
                    {
                        DataJson = JsonConvert.SerializeObject(list),
                        State = "MS",
                        City = city.CityNormalized,
                        Regiao = region.Key
                    };

                    repo.GravaTempoRegiao(tempoRegiao);
                }
            }
        }

        public static Dictionary<string, List<CityModel>> RandomCities()
        {
            var dicionario = new Dictionary<string, List<CityModel>>();
            dicionario.Add("south", SortingCities("south"));
            dicionario.Add("north", SortingCities("north"));
            dicionario.Add("center", SortingCities("center"));
            dicionario.Add("east", SortingCities("east"));
            dicionario.Add("west", SortingCities("west"));

            return dicionario;
        }

        public static List<CityModel> SortingCities(string region, int number = 5)
        {
            var citiesRepo = new CitiesRepository();
            var cities = citiesRepo.City();
            var citiesRegion = cities.Where(x => x.Region == region).ToList();

            var cont = number;
            var indexList = new List<int>();
            var sortingCities = new List<CityModel>();

            do
            {
                Random random = new Random();
                var num = random.Next(citiesRegion.Count);
                cont--;

                if (!indexList.Contains(num))
                    indexList.Add(num);
                else
                    cont++;

            } while (cont > 0);

            foreach (var index in indexList)
            {
                sortingCities.Add(citiesRegion[index]);
            }

            return sortingCities.OrderBy(x => x.CityNormalized).ToList();
        }
    }
}