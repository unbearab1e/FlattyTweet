﻿// Type: MetroTwit.Model.EmbedlyObject
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Runtime.Serialization;

namespace MetroTwit.Model
{
  [DataContract]
  public class EmbedlyObject
  {
    [DataMember]
    public string Type { get; set; }

    [DataMember]
    public int? Width { get; set; }

    [DataMember]
    public int? Height { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Html { get; set; }
  }
}
