
using FlattyTweet.Extensibility;
using System.Net;

namespace FlattyTweet.Model
{
  public class CustomRestResponse : IRestResponse
  {
    public HttpStatusCode StatusCode { get; set; }

    public string Content { get; set; }
  }
}
