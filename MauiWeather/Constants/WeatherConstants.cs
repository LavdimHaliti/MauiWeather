using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Constants;

public class WeatherConstants
{
    //Unit constants
    public const string METRIC_UNIT = "metric";
    public const string IMPERIAL_UNIT = "imperial";

    //Endpoint constants
    public const string CURRENT_WEATHER_ENPOINT = "data/2.5/weather";
    public const string FORECAST_WEATHER_ENPOINT = "data/2.5/forecast";
    public const string GEO_ENDPOINT = "geo/1.0/direct";
}
