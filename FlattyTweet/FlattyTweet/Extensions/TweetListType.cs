
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
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
