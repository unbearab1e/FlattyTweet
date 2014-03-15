
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  public enum TwitterTimeDisplay
  {
    [XmlEnum("Relative")] Relative,
    [XmlEnum("Absolute")] Absolute,
    [XmlEnum("RelativeAbsolute")] RelativeAbsolute,
    [XmlEnum("AbsoluteRelative")] AbsoluteRelative,
  }
}
