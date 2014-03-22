
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (int), typeof (Color))]
  public class NotificationButtonColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ((int) value == 0)
        return (object) Colors.LightGray;
      else
        return Application.Current.FindResource((object) "ModernColorFeature");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
