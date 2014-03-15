
namespace FlattyTweet.Google
{
  public class GeoResult
  {
    public string[] types { get; set; }

    public string formatted_address { get; set; }

    public Address[] address_components { get; set; }

    public Geometry geometry { get; set; }
  }
}
