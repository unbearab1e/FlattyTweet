
using System.Xml.Serialization;

namespace FlattyTweet.TwitLonger
{
  [XmlRoot(ElementName = "twitlonger")]
  public class TwitLongerPostResponse
  {
    public Post post { get; set; }
  }
}
