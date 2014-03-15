
using System.Collections.Generic;

namespace FlattyTweet.ViewModel
{
  public class ListEqualityComparer : IEqualityComparer<TwitterListExtended>
  {
    public bool Equals(TwitterListExtended x, TwitterListExtended y)
    {
      return x.BaseListObject.Id == y.BaseListObject.Id;
    }

    public int GetHashCode(TwitterListExtended obj)
    {
      return (int) obj.BaseListObject.Id;
    }
  }
}
