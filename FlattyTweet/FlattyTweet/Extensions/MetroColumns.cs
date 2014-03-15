
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  [XmlRoot("MetroColumns")]
  public class MetroColumns
  {
    [XmlArrayItem("Column")]
    [XmlArray("Columns")]
    public MTColumnCollection Columns;
  }
}
