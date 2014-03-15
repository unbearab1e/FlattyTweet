using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (bool), typeof (Visibility))]
  public class BooleanToVisibilityHiddenConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is bool))
        return (object) null;
      else
        return (object) (Visibility) ((bool) value ? 0 : 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
