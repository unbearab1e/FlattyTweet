
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  public class OembedResponse
  {
    [DataMember]
    public string Type { get; set; }

    [DataMember]
    public string Version { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember(Name = "author_name")]
    public string AuthorName { get; set; }

    [DataMember(Name = "author_url")]
    public string AuthorUrl { get; set; }

    [DataMember(Name = "provider_name")]
    public string ProviderName { get; set; }

    [DataMember(Name = "provider_url")]
    public string ProviderUrl { get; set; }

    [DataMember(Name = "thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    [DataMember(Name = "thumbnail_width")]
    public int? ThumbnailWidth { get; set; }

    [DataMember(Name = "thumbnail_height")]
    public int? ThumbnailHeight { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public int? Width { get; set; }

    [DataMember]
    public int? Height { get; set; }

    [DataMember]
    public string Html { get; set; }
  }
}
