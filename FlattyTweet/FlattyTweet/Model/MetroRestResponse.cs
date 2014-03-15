using FlattyTweet.Extensions;
using System;
using System.Collections.Generic;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.Model
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
