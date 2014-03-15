using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (string), typeof (string))]
  public class AppendSpaceToNonEmptyString : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null && value.ToString().Trim().Length > 0)
        return (object) (value.ToString() + " ");
      else
        return (object) string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
