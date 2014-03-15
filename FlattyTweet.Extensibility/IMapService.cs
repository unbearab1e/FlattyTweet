
namespace FlattyTweet.Extensibility
{
  public interface IMapService
  {
    string Name { get; }

    string UniqueID { get; }

    MapReverseGeoLookupResponse ReverseGeoLookup(double Latitude, double Longitude);

    string StaticMapURL(double Latitude, double Longitude, int Width, int Height, string PinColour);

    string LiveMapURL(double Latitude, double Longitude);
  }
}
