
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  [DataContract]
  public class EmbedlyPlace
  {
    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Provider { get; set; }

    [DataMember(Name = "street_address")]
    public string StreetAddress { get; set; }

    [DataMember]
    public string Locality { get; set; }

    [DataMember]
    public string Region { get; set; }

    [DataMember(Name = "postal_code")]
    public string PostalCode { get; set; }

    [DataMember(Name = "country_code")]
    public string CountryCode { get; set; }

    [DataMember]
    public double Latitude { get; set; }

    [DataMember]
    public double Longitude { get; set; }
  }
}
