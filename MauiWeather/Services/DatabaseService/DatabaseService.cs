using MauiWeather.Data;
using SQLite;

namespace MauiWeather.Services.DatabaseService;

public class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "weatherDatabase.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<WeatherData>().Wait();
    }

    public Task SaveWeatherAsync(WeatherData weatherData)
    {
        return _database.InsertAsync(weatherData);
    }

    public Task UpdateWeatherAsync(WeatherData weatherData)
    {
        return _database.UpdateAsync(weatherData);
    }

    public async Task<WeatherData?> GetLatestWeatherAsync()
    {
        try
        {
            // Fetch the most recent weather data based on the timestamp
            var latestWeather = await _database.Table<WeatherData>()
                                               .OrderByDescending(w => w.Timestamp)
                                               .FirstOrDefaultAsync();
            return latestWeather;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching latest weather data: {ex.Message}");
            return null;
        }
    }
}
