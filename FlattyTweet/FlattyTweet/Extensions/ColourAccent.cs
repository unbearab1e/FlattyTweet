
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
    public enum ColourAccent
    {
        [XmlEnum("Blue")]
        Blue,
        [XmlEnum("Purple")]
        Purple,
        [XmlEnum("Pink")]
        Pink,
        [XmlEnum("Red")]
        Red,
        [XmlEnum("Orange")]
        Orange,
        [XmlEnum("Green")]
        Green,
        [XmlEnum("Lime")]
        Lime,
        [XmlEnum("Silver")]
        Silver,
        [XmlEnum("DarkBlue")]
        DarkBlue,
    }
}
