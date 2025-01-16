using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Converters;

public class SpeedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string windSpeed)
        {
            // Map weather codes to icon file names
            return windSpeed switch
            {
                "01d" or "01n" => "sunny.png",
                "02d" or "02n" => "partly_cloudy.png",
                "03d" or "03n" => "cloudy.png",
                "04d" or "04n" => "cloudy.png",
                "09d" or "09n" => "rainy.png",
                "10d" or "10n" => "rainy.png",
                "11d" or "11n" => "thunderstorm.png",
                "13d" or "13n" => "snowy.png",
                "50d" or "50n" => "mist.png",
                _ => "sunny.png",
            };
        }
        return "sunny.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
