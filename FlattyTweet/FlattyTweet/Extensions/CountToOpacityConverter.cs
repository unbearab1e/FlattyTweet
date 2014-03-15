
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (int), typeof (double))]
  public class CountToOpacityConverter : IValueConverter
  {
    public double TargetOpacity { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int result = -1;
      if (value != null && (value is int || int.TryParse(value.ToString(), out result) || value.ToString() == "+") && (result > 0 || value.ToString() == "+"))
        return (object) 1.0;
      else
        return (object) this.TargetOpacity;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
