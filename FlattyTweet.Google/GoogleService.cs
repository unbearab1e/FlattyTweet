
using FlattyTweet.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace FlattyTweet.Google
{
  [Export(typeof (IMapService))]
  public class GoogleService : IMapService
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

    [ImportingConstructor]
    public GoogleService(ICoreServices coreService)
    {
      this.coreService = coreService;
    }

    public MapReverseGeoLookupResponse ReverseGeoLookup(double Latitude, double Longitude)
    {
      IRestResponse restResponse = this.coreService.RestService.InvokeRESTService("http://maps.googleapis.com", "/maps/api/geocode/json", (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "latlng",
          Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat) + "," + Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat)
        },
        {
          "sensor",
          "false"
        }
      }, "");
      if (restResponse.StatusCode != HttpStatusCode.OK)
        return (MapReverseGeoLookupResponse) null;
      GoogleResponse googleResponse = this.coreService.RestService.DeserializeJson<GoogleResponse>(restResponse.Content != null ? restResponse.Content : string.Empty);
      return new MapReverseGeoLookupResponse()
      {
        ExpandedAddress = googleResponse.results[0].formatted_address.Replace(", ", "\n")
      };
    }

    public string StaticMapURL(double Latitude, double Longitude, int Width, int Height, string PinColour)
    {
      return string.Format("http://maps.google.com/maps/api/staticmap?size={2}x{3}&zoom=12&markers=color:{4}|{0},{1}&sensor=false", (object) Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Height.ToString(), (object) Width.ToString(), (object) PinColour);
    }

    public string LiveMapURL(double Latitude, double Longitude)
    {
      return string.Format("http://maps.google.com/?ie=UTF8&q={0},{1}&z=16", (object) Latitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat), (object) Longitude.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat));
    }
  }
}
