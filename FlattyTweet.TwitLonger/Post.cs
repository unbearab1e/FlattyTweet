
using System.Xml.Serialization;

namespace FlattyTweet.TwitLonger
{
  public class Post
  {
    public string id { get; set; }

    public string link { get; set; }

    [XmlElement("short")]
    public string shorturl { get; set; }

    public string content { get; set; }
  }
}
