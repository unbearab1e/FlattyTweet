// Type: MetroTwit.Extensions.SortableObservableCollection`1
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MetroTwit.Extensions
{
  public class SortableObservableCollection<T> : ObservableCollection<T>
  {
    public SortableObservableCollection(List<T> list)
      : base(list)
    {
    }

    public SortableObservableCollection(IEnumerable<T> collection)
      : base(collection)
    {
    }

    public void Sort<TKey>(Func<T, TKey> keySelector, ListSortDirection direction)
    {
      switch (direction)
      {
        case ListSortDirection.Ascending:
          this.ApplySort((IEnumerable<T>) Enumerable.OrderBy<T, TKey>((IEnumerable<T>) this.Items, keySelector));
          break;
        case ListSortDirection.Descending:
          this.ApplySort((IEnumerable<T>) Enumerable.OrderByDescending<T, TKey>((IEnumerable<T>) this.Items, keySelector));
          break;
      }
    }

    public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
    {
      this.ApplySort((IEnumerable<T>) Enumerable.OrderBy<T, TKey>((IEnumerable<T>) this.Items, keySelector, comparer));
    }

    private void ApplySort(IEnumerable<T> sortedItems)
    {
      List<T> list = Enumerable.ToList<T>(sortedItems);
      foreach (T obj in list)
        this.Move(this.IndexOf(obj), list.IndexOf(obj));
    }
  }
}
