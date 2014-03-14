// Type: MetroTwit.Extensions.DateToHumanReadableConverter
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroTwit.Extensions
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
