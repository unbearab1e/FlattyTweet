// Type: MetroTwit.Extensions.MTAccountCollection
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetroTwit.Extensions
{
  public class MTAccountCollection : SortableObservableCollection<UserAccountViewModel>
  {
    public UserAccountViewModel this[Decimal TwitterAccountID]
    {
      get
      {
        IEnumerable<UserAccountViewModel> source = Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) this, (Func<UserAccountViewModel, bool>) (x => x.TwitterAccountID == TwitterAccountID));
        if (source != null && Enumerable.Count<UserAccountViewModel>(source) > 0)
          return Enumerable.FirstOrDefault<UserAccountViewModel>(source);
        else
          return (UserAccountViewModel) null;
      }
    }

    public MTAccountCollection()
      : base(new List<UserAccountViewModel>())
    {
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
      if (newIndex <= -1 || oldIndex <= -1)
        return;
      if (newIndex < oldIndex)
      {
        for (int index = newIndex; index <= oldIndex; ++index)
          Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) this, index).Settings.Index = index + 1;
      }
      if (newIndex > oldIndex)
      {
        for (int index = oldIndex; index <= newIndex; ++index)
          Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) this, index).Settings.Index = index - 1;
      }
      base.MoveItem(oldIndex, newIndex);
      Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) this, newIndex).Settings.Index = newIndex;
    }
  }
}
