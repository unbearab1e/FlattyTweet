using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (string), typeof (string))]
  public class AppendStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is string && parameter is string)
        return (object) ((string) value + parameter);
      else
        return (object) string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
