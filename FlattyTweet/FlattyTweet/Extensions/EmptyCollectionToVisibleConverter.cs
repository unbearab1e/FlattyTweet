
using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (object), typeof (Visibility))]
  public class EmptyCollectionToVisibleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && value is IList && (value as IList).Count == 0)
        return (object) Visibility.Visible;
      if (value != null && value is ICollection && (value as ICollection).Count == 0)
        return (object) Visibility.Visible;
      if (value != null && value is IDictionary && (value as IDictionary).Count == 0)
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
