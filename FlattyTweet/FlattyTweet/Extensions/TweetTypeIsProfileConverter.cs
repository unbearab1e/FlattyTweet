
using System;
using System.Globalization;
using System.Windows.Data;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (object), typeof (bool))]
  public class TweetTypeIsProfileConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
          return (((TweetListType)value) == TweetListType.UserTimeline);
      }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
