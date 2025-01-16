using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Data;


public class ForecastWeatherDTO
{
    public string cod { get; set; }
    public int message { get; set; }
    public int cnt { get; set; }
    public ForecastWeatherList[] list { get; set; }
    public City city { get; set; }
}

public class City
{
    public int id { get; set; }
    public string name { get; set; }
    public Coordination coord { get; set; }
    public string country { get; set; }
}

public class Coordination
{
    public float lat { get; set; }
    public float lon { get; set; }
}

public class ForecastWeatherList
{
    public int dt { get; set; }
    public ForecastMain main { get; set; }
    public ForecastWeather[] weather { get; set; }
    public ForecastClouds clouds { get; set; }
    public ForecastWind wind { get; set; }
    public int visibility { get; set; }
    public float pop { get; set; }
    public Rain rain { get; set; }
    public string dt_txt { get; set; }
}

public class ForecastMain
{
    public float temp { get; set; }
    public float feels_like { get; set; }
    public float temp_min { get; set; }
    public float temp_max { get; set; }
    public int pressure { get; set; }
    public int sea_level { get; set; }
    public int grnd_level { get; set; }
    public int humidity { get; set; }
    public float temp_kf { get; set; }
}

public class ForecastClouds
{
    public int all { get; set; }
}

public class ForecastWind
{
    public float speed { get; set; }
    public int deg { get; set; }
    public float gust { get; set; }
}

public class Rain
{
    public float _3h { get; set; }
}

public class ForecastWeather
{
    public int id { get; set; }
    public string main { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
}

