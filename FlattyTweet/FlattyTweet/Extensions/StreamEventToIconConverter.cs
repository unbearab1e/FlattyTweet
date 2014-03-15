
using FlattyTweet.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Twitterizer.Streaming;

namespace FlattyTweet.Extensions
{
  [ValueConversion(typeof (TwitterStreamEventExtended), typeof (MetroTwitStatusBase))]
  public class StreamEventToIconConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      MetroTwitStatusBase metroTwitStatusBase = new MetroTwitStatusBase();
      if (value != null)
      {
        switch ((value as TwitterStreamEventExtended).EventType)
        {
          case TwitterSteamEvent.Favorite:
            return (object) (Application.Current.FindResource((object) "PathFav") as Geometry);
          case TwitterSteamEvent.Unfavorite:
            return (object) (Application.Current.FindResource((object) "PathUnfav") as Geometry);
          case TwitterSteamEvent.Follow:
            return (object) (Application.Current.FindResource((object) "PathFollowsUser") as Geometry);
          case TwitterSteamEvent.ListMemberAdded:
            return (object) (Application.Current.FindResource((object) "PathListAdd") as Geometry);
          case TwitterSteamEvent.ListMemberRemoved:
            return (object) (Application.Current.FindResource((object) "PathListRemove") as Geometry);
        }
      }
      return (object) (Application.Current.FindResource((object) "PathPin") as Geometry);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}
