
using System;

namespace FlattyTweet.Extensions
{
  public static class IntPtrExtensions
  {
    public static int ToLowInt32(this IntPtr ptr)
    {
      return Int64Extensions.GetLowDoubleWord(ptr.ToInt64());
    }

    public static int ToHighInt32(this IntPtr ptr)
    {
      return Int64Extensions.GetHighDoubleWord(ptr.ToInt64());
    }
  }
}
