
using System.Net;

namespace FlattyTweet.Extensibility
{
  public interface IRestResponse
  {
    HttpStatusCode StatusCode { get; set; }

    string Content { get; set; }
  }
}
