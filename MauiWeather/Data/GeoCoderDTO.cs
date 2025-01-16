using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Data;


public class GeoCoderDTO
{
    public string name { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public string country { get; set; }
    public string state { get; set; }
}
