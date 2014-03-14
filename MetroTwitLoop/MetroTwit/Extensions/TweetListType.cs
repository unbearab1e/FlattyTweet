// Type: MetroTwit.Extensions.TweetListType
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Xml.Serialization;

namespace MetroTwit.Extensions
{
  public enum TweetListType
  {
    [XmlEnum(Name = "2")] FriendsTimeline = 0,
    [XmlEnum(Name = "4")] DirectMessages = 2,
    [XmlEnum(Name = "8")] Search = 3,
    [XmlEnum(Name = "9")] UserTimeline = 4,
    [XmlEnum(Name = "1")] PublicTimeline = 5,
    [XmlEnum(Name = "10")] List = 6,
    [XmlEnum(Name = "7")] MentionsMyTweetsRetweeted = 8,
    [XmlEnum(Name = "3")] MyTweets = 9,
    [XmlEnum(Name = "5")] Favourites = 10,
    [XmlEnum(Name = "6")] RetweetsByMe = 11,
    [XmlEnum(Name = "Conversation")] Conversation = 100,
    [XmlEnum(Name = "RetweetUsers")] RetweetUsers = 101,
    [XmlEnum(Name = "ManageList")] ManageList = 102,
    Followers = 103,
    Following = 104,
    ListedIn = 105,
  }
}
