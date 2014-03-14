// Type: MetroTwit.Model._140ProofService
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Twitterizer.Models;

namespace MetroTwit.Model
{
  public static class _140ProofService
  {
    public static MetroTwitStatusBase UserAd(string UserID, double? Latitude = null, double? Longitude = null, string Language = "")
    {
      string url = "http://api.140proof.com";
      string path = "/ads/user.json";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("user_id", UserID);
      dictionary.Add("app_id", "MetroTwit_Windows");
      if (Latitude.HasValue)
        dictionary.Add("lat", Latitude.ToString());
      if (Longitude.HasValue)
        dictionary.Add("long", Longitude.ToString());
      if (!string.IsNullOrEmpty(Language))
        dictionary.Add("lang", Language);
      IRestResponse restResponse = CoreServices.Instance.RestService.InvokeRESTService(url, path, (IDictionary<string, string>) dictionary, "");
      if (restResponse.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(restResponse.Content))
        return (MetroTwitStatusBase) null;
      AdReponse adReponse = CoreServices.Instance.RestService.DeserializeJson<AdReponse>(restResponse.Content);
      if (adReponse == null)
        return (MetroTwitStatusBase) null;
      Ads ads = Enumerable.First<Ads>((IEnumerable<Ads>) adReponse.ads);
      EntityCollection entities = RegularExpressions.ExtractEntities(ads.text);
      return new MetroTwitStatusBase()
      {
        ID = !ads.status.id.HasValue ? new Decimal(0) : (Decimal) ads.status.id.Value,
        RawText = ads.text,
        Source = ads.byline,
        User = new MetroTwitUser((User) null)
        {
          Id = (Decimal) ads.user.id.Value,
          Name = ads.user.name,
          ScreenName = ads.user.screen_name,
          ProfileImageLocation = ads.user.profile_image_url
        },
        Entities = entities,
        AdUrls = ads.action_urls,
        ReplyEnabled = true,
        RetweetEnabled = true,
        DeleteTweetEnabled = false
      };
    }

    public static void HitAdUrl(string url)
    {
      Task task = new Task((Action) (() =>
      {
        try
        {
          WebRequest.Create(url).GetResponse();
        }
        catch
        {
        }
      }));
      task.ContinueWith((Action<Task>) (t => CommonCommands.CheckTaskExceptions(t)));
      task.Start();
    }
  }
}
