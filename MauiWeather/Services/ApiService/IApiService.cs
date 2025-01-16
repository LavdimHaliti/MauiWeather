using MauiWeather.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Services.ApiService
{
    public interface IApiService
    {
        Task<(string lat, string lon)?> GetCoordinatesAsync(string city);
        Task<WeatherDTO?> GetCurrentWeatherByCityName(string cityName, string unit);
        Task<WeatherDTO?> GetCurrentWeather(string lat, string lon, string unit);
        Task<ForecastWeatherDTO?> GetForecastWeatherByCityName(string cityName, string unit);
        Task<ForecastWeatherDTO?> GetForecastWeather(string lat, string lon, string unit);
    }
}
