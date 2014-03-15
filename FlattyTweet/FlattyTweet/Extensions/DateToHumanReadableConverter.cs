
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (DateTime), typeof (string))]
  public class DateToHumanReadableConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      TimeSpan timeSpan = DateTime.Now - ((DateTime) value).ToLocalTime();
      string str = string.Empty;
      return timeSpan.Days <= 365 ? (timeSpan.Days <= 1 ? (timeSpan.Days != 1 ? (timeSpan.Hours <= 1 ? (timeSpan.Hours != 1 ? (timeSpan.Minutes <= 1 ? (timeSpan.Minutes != 1 ? (object) "just now" : (object) "1 min ago") : (object) (timeSpan.Minutes.ToString() + " min ago")) : (object) "1 hour ago") : (object) (timeSpan.Hours.ToString() + " hours ago")) : (object) "1 day ago") : (object) (timeSpan.Days.ToString() + " days ago")) : (object) "a long time ago";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
