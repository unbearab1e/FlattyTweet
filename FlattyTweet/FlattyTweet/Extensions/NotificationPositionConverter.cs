
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (object), typeof (object))]
  public class NotificationPositionConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return (((int)value) == int.Parse(parameter.ToString()));
      }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (NotificationPosition) value;
    }
  }
}
