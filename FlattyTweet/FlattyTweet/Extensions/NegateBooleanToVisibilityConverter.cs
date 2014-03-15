
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (bool), typeof (Visibility))]
  public class NegateBooleanToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return new BooleanToVisibilityConverter().Convert(!((bool)value), targetType, parameter, culture);
      }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
