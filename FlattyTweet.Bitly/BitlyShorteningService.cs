// Type: MetroTwit.Bitly.BitlyShorteningService
// Assembly: MetroTwit.Bitly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B2DBF1B-8845-4660-8620-D7CA34A41F2D
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bitly.dll

using MetroTwit.Extensibility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Web;
using System.Windows.Controls;

namespace MetroTwit.Bitly
{
  [Export(typeof (IURLShorteningService))]
  public class BitlyShorteningService : IURLShorteningService
  {
    private ICoreServices coreService;
    private Dictionary<Decimal, BitlySettings> settings;

    public string Name
    {
      get
      {
        return "bit.ly or j.mp";
      }
    }

    public string UniqueID
    {
      get
      {
        return ((object) Assembly.GetExecutingAssembly()).GetType().ToString();
      }
    }

    public bool HasSettings
    {
      get
      {
        return true;
      }
    }

    [ImportingConstructor]
    public BitlyShorteningService(ICoreServices coreService)
    {
      this.coreService = coreService;
      this.LoadSettings();
    }

    private void LoadSettings()
    {
      Dictionary<Decimal, BitlySettings> dictionary = this.coreService.SettingService(typeof (IURLShorteningService)).LoadObject<BitlySettings>(new BitlySettings());
      if (dictionary != null)
        this.settings = dictionary;
      else
        this.settings = new Dictionary<Decimal, BitlySettings>();
    }

    public string ShortenURL(Decimal TwitterAccountID, string originalURL)
    {
      string url = "http://api.bit.ly";
      string path = "/v3/shorten";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("format", "json");
      dictionary.Add("longUrl", HttpUtility.UrlEncode(originalURL));
      if (this.settings[TwitterAccountID].IsValid && this.settings[TwitterAccountID].UsePersonalLogin)
      {
        dictionary.Add("login", this.settings[TwitterAccountID].Username);
        dictionary.Add("apiKey", this.settings[TwitterAccountID].APIKey);
      }
      else
      {
        dictionary.Add("login", "metrotwit");
        dictionary.Add("apiKey", "R_151df03d901641a072655e7b91e972d6");
      }
      if (this.settings[TwitterAccountID].Usejmp)
        dictionary.Add("domain", "j.mp");
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService(url, path, (IDictionary<string, string>) dictionary, "");
      if (restResponse.StatusCode != HttpStatusCode.OK)
        return originalURL;
      JObject jobject = JObject.Parse(restResponse.Content);
      if (!(((object) jobject["status_txt"]).ToString() != "\"INVALID_LOGIN\""))
        throw new ArgumentException(((object) jobject["status_txt"]).ToString());
      if (((object) jobject["status_code"]).ToString() == "200")
        return this.coreService.RestService.DeserializeJson<BitLyResponse>(restResponse.Content).data.url;
      else
        return originalURL;
    }

    public void SaveSettings()
    {
      this.coreService.SettingService(typeof (IURLShorteningService)).SaveObject<BitlySettings>(this.settings);
    }

    public UserControl GetUISettings(Decimal TwitterAccountID)
    {
      if (!this.settings.ContainsKey(TwitterAccountID))
        this.settings.Add(TwitterAccountID, new BitlySettings());
      return (UserControl) new BitlyUISettings(this.settings[TwitterAccountID]);
    }

    public void CancelSaveSettings()
    {
      this.LoadSettings();
    }

    public bool ValidateSettings(Decimal TwitterAccountID)
    {
      if (!this.settings.ContainsKey(TwitterAccountID))
        this.settings.Add(TwitterAccountID, new BitlySettings());
      return this.settings[TwitterAccountID].IsValid;
    }
  }
}
