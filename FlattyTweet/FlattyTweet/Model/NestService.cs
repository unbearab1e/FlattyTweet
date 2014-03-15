
using FlattyTweet;
using FlattyTweet.Extensibility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;

namespace FlattyTweet.Model
{
  public static class NestService
  {
    private static string NestAPIKey = "80dc6af6f2634f53aba805b5a88ab067";

    static NestService()
    {
    }

    private static Dictionary<string, string> SetupHeaders(Decimal TwitterAccountID)
    {
      string format = "OAuth realm=\"{0}\", oauth_consumer_key=\"{1}\", oauth_signature_method=\"HMAC-SHA1\", oauth_token=\"{2}\", oauth_timestamp=\"{3}\", oauth_nonce=\"{4}\", oauth_version=\"1.0\", oauth_signature=\"{5}\"";
      string str1 = "http://api.twitter.com/";
      string uriString = "https://api.twitter.com/1/account/verify_credentials.json";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      OAuthBase oauthBase = new OAuthBase();
      string timeStamp = oauthBase.GenerateTimeStamp();
      string nonce = oauthBase.GenerateNonce();
      string normalizedUrl;
      string normalizedRequestParameters;
      string str2 = HttpUtility.UrlEncode(oauthBase.GenerateSignature(new Uri(uriString), SettingsData.Instance.TwitterConsumerKey, SettingsData.Instance.TwitterConsumerSecret, App.AppState.Accounts[TwitterAccountID].Settings.UserAuthToken, App.AppState.Accounts[TwitterAccountID].Settings.UserAuthSecret, "GET", timeStamp, nonce, out normalizedUrl, out normalizedRequestParameters));
      string str3 = string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, (object) str1, (object) SettingsData.Instance.TwitterConsumerKey, (object) App.AppState.Accounts[TwitterAccountID].Settings.UserAuthToken, (object) timeStamp, (object) nonce, (object) str2);
      dictionary.Add("X-App-Key", NestService.NestAPIKey);
      dictionary.Add("X-Auth-Service-Provider", uriString);
      dictionary.Add("X-Verify-Credentials-Authorization", str3);
      return dictionary;
    }

    public static void ColumnsSync(NestSync NestSync, Action<NestSync> Callback, Decimal TwitterAccountID)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (x =>
      {
        NestSync.LastSynced = App.AppState.Accounts[TwitterAccountID].Settings.SyncLastUpdate;
        IRestResponse local_0 = CoreServices.Instance.RestService.InvokeRESTService("https://api.tothenest.org/1/", "columns/sync", (IDictionary<string, string>) null, "POST", CoreServices.Instance.RestService.SerializeJson((object) NestSync, true), (IDictionary<string, string>) NestService.SetupHeaders(TwitterAccountID), "", "");
        if (local_0.StatusCode != HttpStatusCode.OK)
          return;
        Callback(CoreServices.Instance.RestService.DeserializeJson<NestSync>(local_0.Content));
      }));
    }

    public static NestSettings Settings(Decimal TwitterAccountID)
    {
      IRestResponse restResponse = CoreServices.Instance.RestService.InvokeRESTService("https://api.tothenest.org/1/", "settings", (IDictionary<string, string>) null, "GET", string.Empty, (IDictionary<string, string>) NestService.SetupHeaders(TwitterAccountID), "", "");
      if (restResponse.StatusCode == HttpStatusCode.OK)
        return CoreServices.Instance.RestService.DeserializeJson<NestSettings>(restResponse.Content);
      else
        return (NestSettings) null;
    }

    public static bool SaveSettings(Decimal TwitterAccountID)
    {
      return CoreServices.Instance.RestService.InvokeRESTService("https://api.tothenest.org/1/", "settings/save", (IDictionary<string, string>) null, "POST", string.Empty, (IDictionary<string, string>) NestService.SetupHeaders(TwitterAccountID), "", "").StatusCode == HttpStatusCode.OK;
    }

    public static void AllGone(Decimal TwitterUserID, Action<bool> Callback, Decimal TwitterAccountID)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (x => Callback(CoreServices.Instance.RestService.InvokeRESTService("https://api.tothenest.org/1/", "all/gone", (IDictionary<string, string>) null, "POST", string.Empty, (IDictionary<string, string>) NestService.SetupHeaders(TwitterAccountID), "", "").StatusCode == HttpStatusCode.OK)));
    }
  }
}
