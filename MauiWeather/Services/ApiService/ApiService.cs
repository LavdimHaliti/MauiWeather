using MauiWeather.Constants;
using MauiWeather.Data;
using MauiWeather.Settings;
using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiWeather.Services.ApiService;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public ApiService(AppSettings appSettings)
    {
        _apiKey = appSettings.ApiKey;
        _baseUrl = appSettings.BaseUrl;
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };

        // Add API key to the HTTP client headers. Not needed for this project
        //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<(string lat, string lon)?> GetCoordinatesAsync(string city)
    {
        try
        {
            var endpoint = $"{WeatherConstants.GEO_ENDPOINT}?q={city}&limit=1&appid={_apiKey}";

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<GeoCoderDTO>>(json);

                if (result != null && result.Any())
                {
                    var coordinates = result.First();
                    return (coordinates.lat.ToString(), coordinates.lon.ToString());
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return null;
    }

    public async Task<WeatherDTO?> GetCurrentWeatherByCityName(string cityName, string unit)
    {
        var coordinates = await GetCoordinatesAsync(cityName);
        if (coordinates == null)
        {
            Console.WriteLine("Failed to fetch coordinates.");
            return null;
        }

        var (lat, lon) = coordinates.Value;
        return await GetCurrentWeather(lat, lon, unit);
    }

    public async Task<WeatherDTO?> GetCurrentWeather(string lat, string lon, string unit)
    {
        try
        {
            var endpoint = $"{WeatherConstants.CURRENT_WEATHER_ENPOINT}?lat={lat}&lon={lon}&exclude=minutely,hourly,daily&units={unit}&appid={_apiKey}";
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherDTO>(json);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return default;
    }

    public async Task<ForecastWeatherDTO?> GetForecastWeatherByCityName(string cityName, string unit)
    {
        var coordinates = await GetCoordinatesAsync(cityName);
        if (coordinates == null)
        {
            Console.WriteLine("Failed to fetch coordinates.");
            return null;
        }

        var (lat, lon) = coordinates.Value;
        return await GetForecastWeather(lat, lon, unit);
    }

    public async Task<ForecastWeatherDTO?> GetForecastWeather(string lat, string lon, string unit)
    {
        try
        {
            var endpoint = $"{WeatherConstants.FORECAST_WEATHER_ENPOINT}?lat={lat}&lon={lon}&exclude=minutely,hourly,daily&units={unit}&appid={_apiKey}";
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ForecastWeatherDTO>(json);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return default;
    }
}
