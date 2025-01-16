using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Data;

public class WeatherData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string weatherIcon { get; set; }
    public string cityName { get; set; }
    public float currentTemp { get; set; }

    public float feelsLike { get; set; }
    public float minTemp { get; set; }
    public float maxTemp { get; set; }

    public string description { get; set; }

    public string forecastList { get; set; }
    public DateTime Timestamp { get; set; }
}
