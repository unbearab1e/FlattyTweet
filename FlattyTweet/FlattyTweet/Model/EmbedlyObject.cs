
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  [DataContract]
  public class EmbedlyObject
  {
    [DataMember]
    public string Type { get; set; }

    [DataMember]
    public int? Width { get; set; }

    [DataMember]
    public int? Height { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Html { get; set; }
  }
}
