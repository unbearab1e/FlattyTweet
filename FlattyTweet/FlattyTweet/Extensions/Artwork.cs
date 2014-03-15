using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  public enum Artwork
  {
    [XmlEnum("None")] None,
    [XmlEnum("Flowers1")] Flowers1,
    [XmlEnum("Flowers2")] Flowers2,
    [XmlEnum("Grunge1")] Grunge1,
    [XmlEnum("Grunge2")] Grunge2,
    [XmlEnum("Grill")] Grill,
    [XmlEnum("Dots")] Dots,
    [XmlEnum("Lines")] Lines,
  }
}
