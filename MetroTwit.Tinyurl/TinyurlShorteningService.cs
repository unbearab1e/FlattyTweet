// Type: MetroTwit.Tinyurl.TinyurlShorteningService
// Assembly: MetroTwit.Tinyurl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 524CFDFA-376C-4F1E-9A6F-644EB6142939
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Tinyurl.dll

using MetroTwit.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows.Controls;

namespace MetroTwit.Tinyurl
{
  [Export(typeof (IURLShorteningService))]
  public class TinyurlShorteningService : IURLShorteningService
  {
    private ICoreServices coreService;

    public string Name
    {
      get
      {
        return "tinyurl";
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
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService("http://tinyurl.com", "/api-create.php", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "url",
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
