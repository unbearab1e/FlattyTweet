
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  [DataContract]
  public class EmbedlyImage
  {
    [DataMember]
    public int? Width { get; set; }

    [DataMember]
    public int? Height { get; set; }

    [DataMember]
    public string Url { get; set; }
  }
}
