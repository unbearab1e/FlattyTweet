// Type: MetroTwit.Model.MetroRestResponse`1
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;
using System.Collections.Generic;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.Model
{
  public class MetroRestResponse<T>
  {
    public T Tweets { get; set; }

    public RefreshTypes RefreshType { get; set; }

    public DateTime ResponseTimeStamp { get; set; }

    public RequestResult RequestResult { get; set; }

    public IEnumerable<TwitterError> Error { get; set; }

    public MetroRestResponse()
    {
      this.ResponseTimeStamp = DateTime.Now;
    }
  }
}
