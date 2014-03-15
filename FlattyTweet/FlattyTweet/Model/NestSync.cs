
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FlattyTweet.Model
{
  [JsonObject(MemberSerialization.OptIn)]
  public class NestSync
  {
    [JsonProperty("mergeClients", NullValueHandling = NullValueHandling.Ignore)]
    public bool? MergeClients { get; set; }

    [JsonProperty("lastSynced", NullValueHandling = NullValueHandling.Ignore)]
    [JsonConverter(typeof (IsoDateTimeConverter))]
    public DateTime? LastSynced { get; set; }

    [JsonProperty("add", NullValueHandling = NullValueHandling.Ignore)]
    public IList Add { get; set; }

    [JsonProperty("edit", NullValueHandling = NullValueHandling.Ignore)]
    public IList Edit { get; set; }

    [JsonProperty("remove", NullValueHandling = NullValueHandling.Ignore)]
    public IList Remove { get; set; }

    public NestSync()
    {
      this.Add = (IList) new List<NestColumn>(0);
      this.Edit = (IList) new List<NestColumn>(0);
      this.Remove = (IList) new List<NestColumn>(0);
    }
  }
}
