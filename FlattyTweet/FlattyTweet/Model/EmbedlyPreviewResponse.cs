
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  [DataContract]
  public class EmbedlyPreviewResponse
  {
    [DataMember(Name = "original_url")]
    public string OriginalUrl { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Type { get; set; }

    [DataMember(Name = "cache_age")]
    public string CacheAge { get; set; }

    [DataMember]
    public bool Safe { get; set; }

    [DataMember(Name = "safe_type")]
    public string SafeType { get; set; }

    [DataMember(Name = "safe_message")]
    public string SafeMessage { get; set; }

    [DataMember(Name = "provider_name")]
    public string ProviderName { get; set; }

    [DataMember(Name = "provider_url")]
    public string ProviderUrl { get; set; }

    [DataMember(Name = "provider_display")]
    public string ProviderDisplay { get; set; }

    [DataMember(Name = "favicon_url")]
    public string FavIconUrl { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember(Name = "author_name")]
    public string AuthorName { get; set; }

    [DataMember(Name = "author_url")]
    public string AuthorUrl { get; set; }

    [DataMember]
    public EmbedlyObject Object { get; set; }

    [DataMember]
    public List<EmbedlyImage> Images { get; set; }

    [DataMember]
    public string Content { get; set; }

    [DataMember]
    public EmbedlyPlace Place { get; set; }
  }
}
