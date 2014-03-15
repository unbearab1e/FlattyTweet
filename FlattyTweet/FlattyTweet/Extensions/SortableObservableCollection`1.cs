
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FlattyTweet.Extensions
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
