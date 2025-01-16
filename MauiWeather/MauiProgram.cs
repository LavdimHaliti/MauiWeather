using CommunityToolkit.Maui;
using MauiWeather.Data;
using MauiWeather.Services.ApiService;
using MauiWeather.Services.DatabaseService;
using MauiWeather.Settings;
using MauiWeather.Viewmodels;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiWeather;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        

        var appSettings = LoadAppSettings();
  
        builder.Services.AddSingleton<AppSettings>(appSettings);

        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IApiService, ApiService>();

        builder.Services.AddSingleton<MainWeatherViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddSingleton<WeatherDetailsPageViewModel>();
        builder.Services.AddSingleton<WeatherDetailsPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static AppSettings LoadAppSettings()
    {
        // Load the appsettings.json file
        var assembly = typeof(MauiProgram).Assembly;
        using var stream = assembly.GetManifestResourceStream("MauiWeather.appsettings.json");
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        // Deserialize the JSON to AppSettings object
        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}
