// Type: MetroTwit.Extensions.StreamEventToIconConverter
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Twitterizer.Streaming;

namespace MetroTwit.Extensions
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
