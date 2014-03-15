
namespace FlattyTweet.Extensions
{
  public static class Int32Extensions
  {
    public static int GetLowWord(this int n)
    {
      return n & (int) ushort.MaxValue;
    }

    public static int GetHighWord(this int n)
    {
      return n >> 16;
    }
  }
}
