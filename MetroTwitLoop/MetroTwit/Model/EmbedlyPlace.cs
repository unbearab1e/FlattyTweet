// Type: MetroTwit.Model.EmbedlyPlace
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Runtime.Serialization;

namespace MetroTwit.Model
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
