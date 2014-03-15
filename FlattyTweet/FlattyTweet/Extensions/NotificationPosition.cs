
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  public enum NotificationPosition
  {
    [XmlEnum("TopLeft")] TopLeft = 1,
    [XmlEnum("TopRight")] TopRight = 2,
    [XmlEnum("BottomLeft")] BottomLeft = 3,
    [XmlEnum("BottomRight")] BottomRight = 4,
  }
}
