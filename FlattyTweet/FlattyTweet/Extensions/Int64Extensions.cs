
namespace FlattyTweet.Extensions
{
  public static class Int64Extensions
  {
    public static int GetLowDoubleWord(this long n)
    {
      return (int) (n & (long) uint.MaxValue);
    }

    public static int GetHighDoubleWord(this long n)
    {
      return (int) (n >> 32);
    }

    public static int GetLowWord(this long n)
    {
      return Int64Extensions.GetLowDoubleWord(n) & (int) ushort.MaxValue;
    }

    public static int GetHighWord(this long n)
    {
      return Int64Extensions.GetLowDoubleWord(n) >> 16;
    }
  }
}
