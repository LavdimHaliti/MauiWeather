using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWeather.Constants;
using MauiWeather.Data;
using MauiWeather.Services.ApiService;
using MauiWeather.Services.DatabaseService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiWeather.Viewmodels;

public partial class MainWeatherViewModel : ObservableObject
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(1));
    private readonly CancellationTokenSource canncelationToken = new();

    [ObservableProperty]
    public WeatherDTO currentWeather;

    [ObservableProperty]
    string cityNameText;

    [ObservableProperty]
    string weatherUnit;

    [ObservableProperty]
    float minTemp;

    [ObservableProperty]
    float maxTemp;

    [ObservableProperty]
    float currentTemp;

    [ObservableProperty]
    string currentWeatherIcon;

    [ObservableProperty]
    bool isUnitPickerVisible;

    [ObservableProperty]
    ObservableCollection<string> unitOptions = new()
    {
        "°C (Celsius)",
        "°F (Fahrenheit)"
    };

    [ObservableProperty]
    string selectedUnit;

    [ObservableProperty]
    string weatherDescription;

    [ObservableProperty]
    float feelsLike;

    [ObservableProperty]
    ObservableCollection<ForecastWeatherList> forecastWeatherList;

    public MainWeatherViewModel(IApiService apiService, IDatabaseService databaseService)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        CityNameText = "London";
        ForecastWeatherList = new ObservableCollection<ForecastWeatherList>();
        WeatherUnit = WeatherConstants.METRIC_UNIT;
        SelectedUnit = "°C (Celsius)";
        SearchCityName();
        StartWeatherUpdateTask();
    }

    partial void OnSelectedUnitChanged(string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        switch (value)
        {
            case "°C (Celsius)":
                WeatherUnit = WeatherConstants.METRIC_UNIT;
                break;
            case "°F (Fahrenheit)":
                WeatherUnit = WeatherConstants.IMPERIAL_UNIT;
                break;
        }

        SearchCityName(); // Refresh weather data when unit changes
    }

    [RelayCommand]
    public async void SearchCityName()
    {
        HideKeyboard();
        ForecastWeatherList.Clear();

        if (string.IsNullOrEmpty(CityNameText))
            return;

        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
        {
            // Online: Fetch data from API
            var currentWeatherResponse = await _apiService.GetCurrentWeatherByCityName(CityNameText, WeatherUnit);
            var forecastWeatherResponse = await _apiService.GetForecastWeatherByCityName(CityNameText, WeatherUnit);

            if (currentWeatherResponse != null && forecastWeatherResponse != null)
            {
                CurrentWeather = currentWeatherResponse;
                CurrentTemp = currentWeatherResponse.main.temp;
                CurrentWeatherIcon = currentWeatherResponse.weather[0].icon;
                FeelsLike = currentWeatherResponse.main.feels_like;
                MinTemp = currentWeatherResponse.main.temp_min;
                MaxTemp = currentWeatherResponse.main.temp_max;
                WeatherDescription = currentWeatherResponse.weather[0].description;

                foreach (var forecast in forecastWeatherResponse.list)
                {
                    ForecastWeatherList.Add(forecast);
                }

                var forecastListJson = JsonConvert.SerializeObject(ForecastWeatherList.ToList());
                var weatherData = new WeatherData
                {
                    weatherIcon = CurrentWeatherIcon,
                    cityName = CityNameText,
                    currentTemp = CurrentTemp,
                    feelsLike = FeelsLike,
                    minTemp = MinTemp,
                    maxTemp = MaxTemp,
                    description = WeatherDescription,
                    forecastList = forecastListJson,
                    Timestamp = DateTime.Now
                };
                await _databaseService.SaveWeatherAsync(weatherData);
            }
        }
        else
        {
            // Offline: Load data from database
            await LoadWeatherFromDatabaseAsync();
        }
    }

    [RelayCommand]
    public async Task NavigatoToWeatherDetails()
    {
        var weatherJson = JsonConvert.SerializeObject(CurrentWeather);

        // Navigate to the DetailsPage and pass task item data as JSON
        await Shell.Current.GoToAsync($"{nameof(WeatherDetailsPage)}?WeatherJson={Uri.EscapeDataString(weatherJson)}");
    }

    private async void StartWeatherUpdateTask()
    {
        while (await _timer.WaitForNextTickAsync(canncelationToken.Token))
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    // Fetch the existing weather data to update
                    var weatherData = await _databaseService.GetLatestWeatherAsync(); // Example method
                    if (weatherData != null)
                    {
                        // Update the weather data
                        await _databaseService.UpdateWeatherAsync(weatherData);

                        Console.WriteLine($"Data updated: {weatherData.forecastList}");

                        // Optionally refresh UI
                        //await Application.Current.Dispatcher.DispatchAsync(() =>
                        //{
                        //    SearchCityName();
                        //});
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating weather data: {ex.Message}");
                }
            }
        }
    }

    private async Task LoadWeatherFromDatabaseAsync()
    {
        try
        {
            var weatherData = await _databaseService.GetLatestWeatherAsync();
            if (weatherData != null)
            {
                CurrentWeatherIcon = weatherData.weatherIcon;
                CityNameText = weatherData.cityName;
                CurrentTemp = weatherData.currentTemp;
                FeelsLike = weatherData.feelsLike;
                MinTemp = weatherData.minTemp;
                MaxTemp = weatherData.maxTemp;
                WeatherDescription = weatherData.description;

                var forecastList = JsonConvert.DeserializeObject<List<ForecastWeatherList>>(weatherData.forecastList);
                ForecastWeatherList = new ObservableCollection<ForecastWeatherList>(forecastList ?? new List<ForecastWeatherList>());
            }
        }
        catch (Exception ex)
        {
            await Toast.Make($"Error loading offline data: {ex.Message}", ToastDuration.Short).Show();
        }
    }
    private void HideKeyboard()
    {
    #if ANDROID
        var context = Android.App.Application.Context;
        var inputMethodManager = (Android.Views.InputMethods.InputMethodManager)context.GetSystemService(Android.Content.Context.InputMethodService);

        if (Platform.CurrentActivity?.CurrentFocus != null)
        {
            inputMethodManager.HideSoftInputFromWindow(Platform.CurrentActivity.CurrentFocus.WindowToken, Android.Views.InputMethods.HideSoftInputFlags.None);
        }
    #endif
    }


}
