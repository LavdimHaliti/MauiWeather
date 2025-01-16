using CommunityToolkit.Mvvm.ComponentModel;
using MauiWeather.Data;
using System.Text.Json;

namespace MauiWeather.Viewmodels;

// This property will receive the JSON string from the query parameter
[QueryProperty("WeatherJson", "WeatherJson")]
public partial class WeatherDetailsPageViewModel : ObservableObject
{
    [ObservableProperty]
    public string weatherJson;

    // This property will hold the deserialized WeatherDTO object
    [ObservableProperty]
    public WeatherDTO weatherDto;

    partial void OnWeatherJsonChanged(string weatherJson)
    {
        // Deserialize the JSON string to the TaskItem object
        if (!string.IsNullOrEmpty(weatherJson))
        {
            WeatherDto = JsonSerializer.Deserialize<WeatherDTO>(Uri.UnescapeDataString(weatherJson));
        }
    }

}
