
using FlattyTweet.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlattyTweet.Extensions
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
