// Type: MetroTwit.Extensions.Int64Extensions
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

namespace MetroTwit.Extensions
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
