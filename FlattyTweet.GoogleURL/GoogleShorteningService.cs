
using FlattyTweet.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows.Controls;

namespace FlattyTweet.GoogleURL
{
  [Export(typeof (IURLShorteningService))]
  public class GoogleShorteningService : IURLShorteningService
  {
    private ICoreServices coreService;

    public string Name
    {
      get
      {
        return "Google";
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
        return false;
      }
    }

    [ImportingConstructor]
    public GoogleShorteningService(ICoreServices coreService)
    {
      this.coreService = coreService;
    }

    public string ShortenURL(Decimal TwitterAccountID, string originalURL)
    {
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService("https://www.googleapis.com", "/urlshortener/v1/url", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "key",
          "AIzaSyDSKtqudm6qtJ9CnbFIB15msEICQ_DN2TM"
        }
      }, "POST", "{\"longUrl\": \"" + originalURL + "\"}", (IDictionary<string, string>) null, (string) null, "application/json");
      if (restResponse.StatusCode == HttpStatusCode.OK)
        return this.coreService.RestService.DeserializeJson<GoogleResponse>(restResponse.Content).id;
      else
        return originalURL;
    }

    public void SaveSettings()
    {
      throw new NotImplementedException();
    }

    public void CancelSaveSettings()
    {
      throw new NotImplementedException();
    }

    public UserControl GetUISettings(Decimal TwitterAccountID)
    {
      throw new NotImplementedException();
    }

    public bool ValidateSettings(Decimal TwitterAccountID)
    {
      throw new NotImplementedException();
    }
  }
}
