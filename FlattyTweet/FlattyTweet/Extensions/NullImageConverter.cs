
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (string), typeof (object))]
  public class NullImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || value is string && string.IsNullOrEmpty(value.ToString()))
        return DependencyProperty.UnsetValue;
      else
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
