using MauiWeather.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task SaveWeatherAsync(WeatherData weatherData);

        Task UpdateWeatherAsync(WeatherData weatherData);

        Task<WeatherData?> GetLatestWeatherAsync();
    }
}
