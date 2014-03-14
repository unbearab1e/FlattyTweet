// Type: MetroTwit.Model.Embedly
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensibility;
using System.Collections.Generic;
using System.Net;
using System.Windows;

namespace MetroTwit.Model
{
  public static class Embedly
  {
    public static EmbedlyPreviewResponse GetEmbedlyPreview(string url)
    {
      string url1 = " http://api.embed.ly";
      string path = "/1/preview";
      string UserAgent = string.Format("Mozilla/5.0 (compatible; MetroTwit/{0}; +http://www.metrotwit.com/)", (object) ((object) Application.ResourceAssembly.GetName().Version).ToString());
      IRestResponse restResponse = CoreServices.Instance.RestService.InvokeRESTService(url1, path, (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "url",
          url
        },
        {
          "maxwidth",
          "500"
        },
        {
          "maxheight",
          "500"
        },
        {
          "format",
          "json"
        },
        {
          "wmode",
          "window"
        },
        {
          "key",
          "abbd7e06256411e1b1074040d3dc5c07"
        }
      }, UserAgent);
      if (restResponse.StatusCode == HttpStatusCode.OK)
        return CoreServices.Instance.RestService.DeserializeJson<EmbedlyPreviewResponse>(restResponse.Content);
      else
        return (EmbedlyPreviewResponse) null;
    }
  }
}
