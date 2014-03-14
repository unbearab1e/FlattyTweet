// Type: MetroTwit.Model.RestService
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensibility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MetroTwit.Model
{
  public class RestService : IRestService
  {
    public IRestResponse InvokeRESTService(string url, string path, IDictionary<string, string> parameters, string UserAgent = "")
    {
      return this.InvokeRESTService(url, path, parameters, "GET", string.Empty, (IDictionary<string, string>) null, UserAgent, "");
    }

    public IRestResponse InvokeRESTService(string url, string path, IDictionary<string, string> parameters, string method, string content, IDictionary<string, string> headers, string UserAgent = "", string ContentType = "")
    {
      string requestUriString = url + path;
      if (parameters != null && parameters.Count > 0)
      {
        string str = string.Empty;
        foreach (string index in (IEnumerable<string>) parameters.Keys)
        {
          str = !string.IsNullOrEmpty(str) ? str + "&" : str + "?";
          str = str + index + "=" + parameters[index];
        }
        if (!string.IsNullOrEmpty(str))
          requestUriString = url + path + str;
      }
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = method;
      httpWebRequest.ContentLength = 0L;
      httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      if (headers != null)
      {
        foreach (string name in (IEnumerable<string>) headers.Keys)
          ((NameValueCollection) httpWebRequest.Headers).Add(name, headers[name]);
      }
      if (!string.IsNullOrEmpty(ContentType))
        httpWebRequest.ContentType = ContentType;
      if (method == "POST" && !string.IsNullOrEmpty(content))
      {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        httpWebRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = ((WebRequest) httpWebRequest).GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
      }
      if (!string.IsNullOrEmpty(UserAgent))
        httpWebRequest.UserAgent = UserAgent;
      HttpWebResponse httpWebResponse;
      try
      {
        httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
      }
      catch (WebException ex)
      {
        httpWebResponse = (HttpWebResponse) ex.Response;
      }
      string str1 = string.Empty;
      if (httpWebResponse == null)
        return (IRestResponse) new CustomRestResponse();
      string str2 = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd().Replace("\n", "");
      httpWebResponse.Close();
      return (IRestResponse) new CustomRestResponse()
      {
        StatusCode = httpWebResponse.StatusCode,
        Content = str2
      };
    }

    public T DeserializeJson<T>(string jsonString)
    {
      return new JsonSerializer().Deserialize<T>((JsonReader) new JsonTextReader((TextReader) new StringReader(jsonString)));
    }

    public T DeserializeXml<T>(string xmlString)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      StringReader stringReader = new StringReader(xmlString);
      XmlTextReader xmlTextReader = new XmlTextReader((TextReader) stringReader);
      T obj = (T) xmlSerializer.Deserialize((XmlReader) xmlTextReader);
      xmlTextReader.Close();
      stringReader.Close();
      return obj;
    }

    public string SerializeJson(object objectToSerialize, bool IgnoreNullValues = false)
    {
      if (!IgnoreNullValues)
        return JsonConvert.SerializeObject(objectToSerialize);
      return JsonConvert.SerializeObject(objectToSerialize, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore
      });
    }
  }
}
