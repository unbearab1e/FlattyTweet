// Type: MetroTwit.Karmacracy.KarmacracyShorteningService
// Assembly: MetroTwit.Karmacracy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D8758AA4-2444-4BFE-8204-2AC009585A92
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Karmacracy.dll

using MetroTwit.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows.Controls;

namespace MetroTwit.Karmacracy
{
  [Export(typeof (IURLShorteningService))]
  public class KarmacracyShorteningService : IURLShorteningService
  {
    private ICoreServices coreService;
    private Dictionary<Decimal, KarmacracySettings> settings;

    public string Name
    {
      get
      {
        return "karmacracy";
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
    public KarmacracyShorteningService(ICoreServices coreService)
    {
      this.coreService = coreService;
      this.LoadSettings();
    }

    private void LoadSettings()
    {
      Dictionary<Decimal, KarmacracySettings> dictionary = this.coreService.SettingService(typeof (IURLShorteningService)).LoadObject<KarmacracySettings>(new KarmacracySettings());
      if (dictionary != null)
        this.settings = dictionary;
      else
        this.settings = new Dictionary<Decimal, KarmacracySettings>();
    }

    public string ShortenURL(Decimal TwitterAccountID, string originalURL)
    {
      string url = "http://kcy.me";
      string path = "/api/";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("format", "json");
      dictionary.Add("url", originalURL);
      if (this.settings[TwitterAccountID].IsValid)
      {
        dictionary.Add("u", this.settings[TwitterAccountID].Username);
        dictionary.Add("key", this.settings[TwitterAccountID].APIKey);
      }
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService(url, path, (IDictionary<string, string>) dictionary, "");
      if (restResponse.StatusCode == HttpStatusCode.OK)
        return this.coreService.RestService.DeserializeJson<KarmacracyResponse>(restResponse.Content).data.url;
      else
        return originalURL;
    }

    public void SaveSettings()
    {
      this.coreService.SettingService(typeof (IURLShorteningService)).SaveObject<KarmacracySettings>(this.settings);
    }

    public UserControl GetUISettings(Decimal TwitterAccountID)
    {
      if (!this.settings.ContainsKey(TwitterAccountID))
        this.settings.Add(TwitterAccountID, new KarmacracySettings());
      return (UserControl) new KarmacracyUISettings(this.settings[TwitterAccountID]);
    }

    public void CancelSaveSettings()
    {
      this.LoadSettings();
    }

    public bool ValidateSettings(Decimal TwitterAccountID)
    {
      if (!this.settings.ContainsKey(TwitterAccountID))
        this.settings.Add(TwitterAccountID, new KarmacracySettings());
      return this.settings[TwitterAccountID].IsValid;
    }
  }
}
