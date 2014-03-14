// Type: MetroTwit.Extensions.Int32Extensions
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

namespace MetroTwit.Extensions
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
