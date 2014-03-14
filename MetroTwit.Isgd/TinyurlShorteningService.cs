// Type: MetroTwit.Isgd.TinyurlShorteningService
// Assembly: MetroTwit.Isgd, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01CFA794-2835-4309-B1B3-469D9F2A9961
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Isgd.dll

using MetroTwit.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows.Controls;

namespace MetroTwit.Isgd
{
  [Export(typeof (IURLShorteningService))]
  public class TinyurlShorteningService : IURLShorteningService
  {
    private ICoreServices coreService;

    public string Name
    {
      get
      {
        return "is.gd";
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
    public TinyurlShorteningService(ICoreServices coreService)
    {
      this.coreService = coreService;
    }

    public string ShortenURL(Decimal TwitterAccountID, string originalURL)
    {
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService("http://is.gd", "/api.php", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "longurl",
          originalURL
        }
      }, "");
      if (restResponse.StatusCode == HttpStatusCode.OK)
        return restResponse.Content;
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
