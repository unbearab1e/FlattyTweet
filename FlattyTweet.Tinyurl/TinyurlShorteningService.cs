﻿
using FlattyTweet.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Windows.Controls;

namespace FlattyTweet.Tinyurl
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
