
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace FlattyTweet.Model
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
