// Type: MetroTwit.Bing.BingService
// Assembly: MetroTwit.Bing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D3D736DA-69B5-45EA-AE15-6F850EDBB636
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bing.dll

using MetroTwit.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace MetroTwit.Bing
{
  [Export(typeof (IMapService))]
  public class BingService : IMapService
  {
    private ICoreServices coreService;

    public string Name
    {
      get
      {
        return "Bing";
      }
    }

    public string UniqueID
    {
      get
      {
        return ((object) Assembly.GetExecutingAssembly()).GetType().ToString();
      }
    }

    [ImportingConstructor]
    public BingService(ICoreServices coreService)
    {
      this.coreService = coreService;
    }

    public MapReverseGeoLookupResponse ReverseGeoLookup(double Latitude, double Longitude)
    {
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService("http://dev.virtualearth.net", string.Format("/REST/v1/Locations/{0},{1}", (object) Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat)), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "includeEntityTypes",
          "Address"
        },
        {
          "key",
          "ArOU1VJUUCETySFxHal9memVOPOtKjfaAWqb3lKJiX3NQ59UVMkY_DVA2LRTH2aK"
        }
      }, "");
      if (restResponse.StatusCode != HttpStatusCode.OK)
        return (MapReverseGeoLookupResponse) null;
      string jsonString = restResponse.Content != null ? restResponse.Content : string.Empty;
      try
      {
        BingResponse bingResponse = this.coreService.RestService.DeserializeJson<BingResponse>(jsonString);
        if (bingResponse.resourceSets[0].estimatedTotal > 0 && bingResponse.resourceSets[0].resources[0].address != null)
          return new MapReverseGeoLookupResponse()
          {
            ExpandedAddress = bingResponse.resourceSets[0].resources[0].address.formattedAddress.Replace(", ", "\n")
          };
        else
          return new MapReverseGeoLookupResponse()
          {
            ExpandedAddress = string.Empty
          };
      }
      catch
      {
        return new MapReverseGeoLookupResponse()
        {
          ExpandedAddress = string.Empty
        };
      }
    }

    public string StaticMapURL(double Latitude, double Longitude, int Width, int Height, string PinColour)
    {
      return string.Format("http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{0},{1}/12?mapSize={2},{3}&pushpin={0},{1};35&mapVersion=1&key=ArOU1VJUUCETySFxHal9memVOPOtKjfaAWqb3lKJiX3NQ59UVMkY_DVA2LRTH2aK", (object) Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Height.ToString(), (object) Width.ToString(), (object) PinColour);
    }

    public string LiveMapURL(double Latitude, double Longitude)
    {
      return string.Format("http://www.bing.com/maps/?&v=2&cp={0}~{1}&lvl=16&dir=0&sty=r&where1={0},{1}&q={0},{1}", (object) Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
    }
  }
}
