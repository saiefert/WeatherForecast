using ClimaRmc.Config;
using ClimaRmc.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClimaRmc
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await RunAutomation(stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }

        public Task RunAutomation(CancellationToken stoppingToken)
        {
            var config = Settings.Conf();
            DateTime hour = Convert.ToDateTime(config.runAt.hour);
            bool execute = Convert.ToBoolean(config.runAt.active);

            if (execute && WorkerHelper.DayOfWeek(config.runAt))
            {
                var definedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour.Hour, hour.Minute, hour.Second);
                var dateHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                if (dateHour.Equals(definedDate))
                {
                    var forecastRepo = new ForecastRepository();

                    foreach (var city in Settings.MainCities())
                    {
                        var periodForecast = WorkerHelper.CreateObjectPeriodForecast(city, "today");
                        forecastRepo.WritePeriodForecast(periodForecast);

                        Thread.Sleep(3000);

                        var dailyForecast = WorkerHelper.CreateObjectDailyForecast(city);
                        forecastRepo.WriteDailyForecast(dailyForecast);
                    }

                    if (Convert.ToBoolean(config.runAt.region))
                        WorkerHelper.WriteRegion();
                }
            }

            return Task.CompletedTask;
        }
    }
}