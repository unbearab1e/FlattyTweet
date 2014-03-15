
using System.Collections.Generic;

namespace FlattyTweet.Extensions
{
  public class IntellisenseItemComparer : IEqualityComparer<IntellisenseItem>
  {
    public bool Equals(IntellisenseItem x, IntellisenseItem y)
    {
      return x.FilterValue == y.FilterValue;
    }

    public int GetHashCode(IntellisenseItem obj)
    {
      return obj.GetHashCode();
    }
  }
}
