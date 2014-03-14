// Type: MetroTwit.Model.NestColumn
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace MetroTwit.Model
{
  [DataContract]
  public class NestColumn
  {
    [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Id { get; set; }

    [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Type { get; set; }

    [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Value { get; set; }

    [JsonProperty("lastReadId", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public long LastReadId { get; set; }

    [JsonProperty("date", DefaultValueHandling = DefaultValueHandling.Ignore)]
    [JsonConverter(typeof (IsoDateTimeConverter))]
    public DateTime Date { get; set; }
  }
}
