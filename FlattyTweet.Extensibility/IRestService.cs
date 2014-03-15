
using System.Collections.Generic;

namespace FlattyTweet.Extensibility
{
  public interface IRestService
  {
    IRestResponse InvokeRESTService(string url, string path, IDictionary<string, string> parameters, string UserAgent = "");

    IRestResponse InvokeRESTService(string url, string path, IDictionary<string, string> parameters, string method, string content, IDictionary<string, string> headers, string UserAgent = "", string ContentType = "");

    T DeserializeJson<T>(string jsonString);

    T DeserializeXml<T>(string xmlString);

    string SerializeJson(object objectToSerialize, bool IgnoreNullValues = false);
  }
}
