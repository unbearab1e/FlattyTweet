
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (string), typeof (string))]
  public class EmptyStringToVisibility : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && !string.IsNullOrEmpty(value.ToString().Trim()))
        return (object) Visibility.Visible;
      else
        return (object) Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
