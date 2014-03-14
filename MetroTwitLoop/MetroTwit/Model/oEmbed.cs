// Type: MetroTwit.Model.oEmbed
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace MetroTwit.Model
{
  public static class oEmbed
  {
      public async static Task<OembedResponse> GetOembed(string oEmbedHost, string oEmbedUrl, int maxWidth = 500)
      {
          OembedResponse response2;
          string apiurl = oEmbedHost;
          string useragent = string.Format("Mozilla/5.0 (compatible; MetroTwit/{0}; +http://www.metrotwit.com/)", Application.ResourceAssembly.GetName().Version.ToString());
          Dictionary<string, string> parameters = new Dictionary<string, string>();
          parameters.Add("url", oEmbedUrl);
          parameters.Add("maxwidth", maxWidth.ToString());
          parameters.Add("maxheight", "500");
          parameters.Add("format", "json");
          parameters.Add("nowrap", "on");
          IRestResponse response = null;
          await Task.Run<IRestResponse>((Func<IRestResponse>)(() => (response = CoreServices.Instance.RestService.InvokeRESTService(apiurl, string.Empty, parameters, useragent))));
          if ((response != null) && (response.StatusCode == HttpStatusCode.OK))
          {
              response2 = CoreServices.Instance.RestService.DeserializeJson<OembedResponse>(response.Content);
          }
          else
          {
              response2 = null;
          }
          return response2;
      }
  }
}
