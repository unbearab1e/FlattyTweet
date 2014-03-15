
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace FlattyTweet.Model
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
