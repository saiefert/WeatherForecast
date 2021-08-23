using Microsoft.EntityFrameworkCore;
using WeatherForecast.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecast.Config;

namespace WeatherForecast
{
    public class ApiContext : DbContext
    {
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<PeriodForecastModel> PeriodForecast { get; set; }
        public DbSet<DailyForecastModel> DailyForecast { get; set; }
        public DbSet<RegionsWeatherModel> RegionForecast { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conn = Settings.DatabaseConnection();

            optionsBuilder.UseNpgsql($"Host={conn.server};Database={conn.database};Username={conn.user};Password={conn.password}");
        }

        private void ModelRegionWeather(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionsWeatherModel>(entity =>
            {
                entity.ToTable("region_weather");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName("id_region_weather").ValueGeneratedOnAdd();
                entity.Property(x => x.Regiao).HasColumnName("region");
                entity.Property(x => x.City).HasColumnName("city");
                entity.Property(x => x.State).HasColumnName("state");
                entity.Property(x => x.DataJson).HasColumnName("data_json").HasColumnType("json");
                entity.Property(x => x.ConsumeHour).HasColumnName("consuming_hour");
            });
        }
        private void ModeloPrevisaoPeriodos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeriodForecastModel>(entity =>
            {
                entity.ToTable("period_forecast");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName("id_period_forecast").ValueGeneratedOnAdd();
                entity.Property(x => x.City).HasColumnName("city");
                entity.Property(x => x.State).HasColumnName("state");
                entity.Property(x => x.DataJson).HasColumnName("data_json").HasColumnType("json");
                entity.Property(x => x.ConsumeHour).HasColumnName("consuming_hour");
            });
        }
        private void ModelDailyForecast(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyForecastModel>(entity =>
            {
                entity.ToTable("daily_forecast");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName("id_daily_forecast").ValueGeneratedOnAdd();
                entity.Property(x => x.City).HasColumnName("city");
                entity.Property(x => x.State).HasColumnName("state");
                entity.Property(x => x.DataJson).HasColumnName("data_json").HasColumnType("json");
                entity.Property(x => x.ConsumeHour).HasColumnName("consuming_hour");
            });
        }

        private void ModelCityState(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityModel>(entity =>
            {
                entity.ToTable("state_city");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName("ibge_code");
                entity.Property(x => x.CityName).HasColumnName("city_name");
                entity.Property(x => x.CityNormalized).HasColumnName("city_without_characters");
                entity.Property(x => x.Latitide).HasColumnName("latitude");
                entity.Property(x => x.Longitude).HasColumnName("longitude");
                entity.Property(x => x.Region).HasColumnName("regiao");
                entity.Property(x => x.IdClimaTempo).HasColumnName("id_climatempo");

            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            ModelRegionWeather(modelBuilder);
            ModeloPrevisaoPeriodos(modelBuilder);
            ModelDailyForecast(modelBuilder);
            ModelCityState(modelBuilder);
        }
    }
}
