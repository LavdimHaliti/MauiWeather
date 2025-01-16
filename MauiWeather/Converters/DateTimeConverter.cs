using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Converters;

class DateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string dateTimeString && DateTime.TryParse(dateTimeString, out var dateTime))
        {
            // Format: "DayOfWeek Hour:Minute"
            return $"{dateTime:dddd}\n{dateTime:HH:mm}";
        }

        return value; // Return original value if parsing fails
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

