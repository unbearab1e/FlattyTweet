
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Twitterizer.Models;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (MediaEntity), typeof (string))]
  public class ImagePreviewConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is MediaEntity && parameter is string)
      {
        if ((value as MediaEntity).Sizes == null)
          return (object) (value as MediaEntity).MediaUrl;
        else
          return (object) ((value as MediaEntity).MediaUrlSecure + parameter);
      }
      else if (value == null || value is string && string.IsNullOrEmpty(value.ToString()))
        return DependencyProperty.UnsetValue;
      else
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
