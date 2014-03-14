// Type: MetroTwit.Extensions.MTColumnViewModelCollection
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MetroTwit.Extensions
{
  public class MTColumnViewModelCollection : ObservableCollection<TweetListViewModel>
  {
    public TweetListViewModel this[Guid ID]
    {
      get
      {
        IEnumerable<TweetListViewModel> source = Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>) this, (Func<TweetListViewModel, bool>) (x => x.UniqueTweetListID == ID));
        if (source != null && Enumerable.Count<TweetListViewModel>(source) > 0)
          return Enumerable.FirstOrDefault<TweetListViewModel>(source);
        else
          return (TweetListViewModel) null;
      }
    }
  }
}
