
using System;
using System.Runtime.Serialization;

namespace FlattyTweet.Model
{
  [DataContract]
  public class MetroTwitAPIStatus
  {
    [DataMember]
    public string Status { get; set; }

    [DataMember]
    public DateTime Date { get; set; }

    [DataMember]
    public DateTime Expiry { get; set; }
  }
}
