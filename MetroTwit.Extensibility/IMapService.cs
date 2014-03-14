// Type: MetroTwit.Extensibility.IMapService
// Assembly: MetroTwit.Extensibility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BABFD559-0F69-4CCB-AC2F-DD0824A6C2D7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Extensibility.dll

namespace MetroTwit.Extensibility
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
