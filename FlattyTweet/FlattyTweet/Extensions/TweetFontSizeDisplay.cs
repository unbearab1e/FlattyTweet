
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  public enum TweetFontSizeDisplay
  {
    [XmlEnum("Small")] Small,
    [XmlEnum("Medium")] Medium,
    [XmlEnum("Large")] Large,
  }
}
