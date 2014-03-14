// Type: MetroTwit.Model.NestSync
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MetroTwit.Model
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
