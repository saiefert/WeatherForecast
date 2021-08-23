using WeatherForecast.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast.Repository
{
    public class ForecastRepository
    {
        public List<PeriodForecastModel> ConsultaPrevisaoPeriodos(DateTime date)
        {
            using (var db = new ApiContext())
            {
                return db.PeriodForecast
                        .Where(x => x.ConsumeHour >= date)
                        .OrderByDescending(x => x.ConsumeHour)
                        .ToList();
            }
        }

        public PeriodForecastModel GetPeriodForecast(string cidade)
        {
            using (var db = new ApiContext())
            {
                return db.PeriodForecast
                        .Where(x => x.City == cidade)
                        .OrderByDescending(x => x.ConsumeHour)
                        .FirstOrDefault();
            }
        }

        public void WritePeriodForecast(PeriodForecastModel previsao)
        {
            using (var db = new ApiContext())
            {
                previsao.ConsumeHour = DateTime.Now;
                db.PeriodForecast.Add(previsao);
                db.SaveChanges();
            }
        }

        public List<DailyForecastModel> ConsultaPrevisaoDias(DateTime date)
        {
            using (var db = new ApiContext())
            {
                return db.DailyForecast
                        .Where(x => x.ConsumeHour >= date)
                        .OrderByDescending(x => x.ConsumeHour)
                        .ToList();
            }
        }

        public DailyForecastModel ConsultaPrevisaoDias(string cidade)
        {
            using (var db = new ApiContext())
            {
                return db.DailyForecast
                        .Where(x => x.City == cidade)
                        .OrderByDescending(x => x.ConsumeHour)
                        .FirstOrDefault();
            }
        }

        public void WriteDailyForecast(DailyForecastModel previsao)
        {
            using (var db = new ApiContext())
            {
                previsao.ConsumeHour = DateTime.Now;
                db.DailyForecast.Add(previsao);
                db.SaveChanges();
            }
        }

        public List<RegionsWeatherModel> ConsultaTempoRegiao(string regiao, string estado)
        {
            using (var db = new ApiContext())
            {
                var lista = db.RegionForecast
                    .Where(x => (x.Regiao == regiao) &&
                    (x.State.Equals(estado.ToUpper())))
                    .OrderByDescending(x => x.ConsumeHour)
                    .ToList();

                if (lista.Count > 5)
                    lista.RemoveRange(5, (lista.Count() - 5));

                return lista;
            }
        }

        public List<RegionsWeatherModel> ConsultaTempoRegiao(DateTime date, string estado)
        {
            using (var db = new ApiContext())
            {
                var lista = db.RegionForecast
                    .Where(x => (x.ConsumeHour >= date) && (x.State == estado))
                    .OrderByDescending(x => x.ConsumeHour)
                    .ToList();

                if (lista.Count > 25)
                    lista.RemoveRange(25, (lista.Count() - 25));

                return lista;
            }
        }

        public void GravaTeste()
        {
            using (var db = new ApiContext())
            {
                var json = new { teste = "testando" };
                DailyForecastModel prev = new()
                {
                    City = "Campo Grande",
                    DataJson = JsonConvert.SerializeObject(json),
                    State = "MS",
                    ConsumeHour = DateTime.Now
                };

                db.DailyForecast.Add(prev);
                db.SaveChanges();
            }
        }

        public void GravaTempoRegiao(RegionsWeatherModel tempoRegiao)
        {
            using (var db = new ApiContext())
            {
                tempoRegiao.ConsumeHour = DateTime.Now;
                db.RegionForecast.Add(tempoRegiao);
                db.SaveChanges();
            }
        }
    }
}
