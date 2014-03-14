// Type: MetroTwit.Extensions.AdDisplayColumnConverter
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroTwit.Extensions
{
  [ValueConversion(typeof (object), typeof (object))]
  public class AdDisplayColumnConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((TweetListType) value)
      {
        case TweetListType.FriendsTimeline:
          return (object) 0;
        case TweetListType.DirectMessages:
          return (object) 2;
        case TweetListType.Search:
          return (object) 3;
        case TweetListType.List:
          return (object) 4;
        case TweetListType.MentionsMyTweetsRetweeted:
          return (object) 1;
        default:
          return (object) 2;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((int) value)
      {
        case 0:
          return (object) TweetListType.FriendsTimeline;
        case 1:
          return (object) TweetListType.MentionsMyTweetsRetweeted;
        case 2:
          return (object) TweetListType.DirectMessages;
        case 3:
          return (object) TweetListType.Search;
        case 4:
          return (object) TweetListType.List;
        default:
          return (object) TweetListType.DirectMessages;
      }
    }
  }
}
