// Type: MetroTwit.Extensions.WordMatcher
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MetroTwit.Extensions
{
  public class WordMatcher
  {
    private ConcurrentDictionary<string, List<IntellisenseItem>> wordTree = new ConcurrentDictionary<string, List<IntellisenseItem>>();
    private IntellisenseItemComparer intellisenseItemComparer;

    public bool IgnoreCase { get; set; }

    public bool ContainsEntries
    {
      get
      {
        return this.wordTree.Count > 0;
      }
    }

    public WordMatcher()
    {
      this.intellisenseItemComparer = new IntellisenseItemComparer();
      this.IgnoreCase = true;
    }

    private List<IntellisenseItem> LookupOrAdd(string key)
    {
      if (this.wordTree.ContainsKey(key))
        return this.wordTree[key];
      List<IntellisenseItem> addValue = new List<IntellisenseItem>();
      return this.wordTree.AddOrUpdate(key, addValue, (Func<string, List<IntellisenseItem>, List<IntellisenseItem>>) ((existingKey, existingValue) => existingValue));
    }

    private static IEnumerable<string> GetSubWord(string word)
    {
      int l = word.Length;
      for (int i = 0; i < l; ++i)
      {
        for (int j = 1; j <= l - i; ++j)
          yield return word.Substring(i, j);
      }
    }

    public void AddWords(IEnumerable<IntellisenseItem> words, string group)
    {
        Action action = null;
        if (words != null)
        {
            try
            {
                if (action == null)
                {
                    action = delegate
                    {
                        foreach (IntellisenseItem item in words.ToArray<IntellisenseItem>())
                        {
                            this.AddWord(item, group);
                        }
                    };
                }
                Task task = new Task(action);
                task.ContinueWith(delegate(Task t)
                {
                    if (t.Exception != null)
                    {
                    }
                });
                task.Start();
            }
            catch
            {
            }
        }
    }

    public void AddWord(IntellisenseItem word, string group)
    {
      foreach (List<IntellisenseItem> list in Enumerable.Select<string, List<IntellisenseItem>>((IEnumerable<string>) new List<string>(this.IgnoreCase ? WordMatcher.GetSubWord(word.FilterValue.ToLower()) : WordMatcher.GetSubWord(word.FilterValue))
      {
        group
      }, new Func<string, List<IntellisenseItem>>(this.LookupOrAdd)))
      {
        if (Monitor.TryEnter((object) list))
        {
          try
          {
            if (!Enumerable.Contains<IntellisenseItem>((IEnumerable<IntellisenseItem>) list, word, (IEqualityComparer<IntellisenseItem>) this.intellisenseItemComparer))
              list.Add(word);
          }
          finally
          {
            Monitor.Exit((object) list);
          }
        }
      }
    }

    public void RemoveWord(IntellisenseItem word)
    {
      foreach (List<IntellisenseItem> list in Enumerable.Select<string, List<IntellisenseItem>>(Enumerable.Where<string>(WordMatcher.GetSubWord(word.FilterValue), (Func<string, bool>) (item => this.wordTree.ContainsKey(item))), (Func<string, List<IntellisenseItem>>) (item => this.wordTree[item])))
      {
        if (Monitor.TryEnter((object) list))
        {
          try
          {
            if (list.Contains(word))
              list.Remove(word);
          }
          finally
          {
            Monitor.Exit((object) list);
          }
        }
      }
    }

    public IEnumerable<IntellisenseItem> GetMatches(string partialWord)
    {
      string key = this.IgnoreCase ? partialWord.ToLower() : partialWord;
      return this.wordTree.ContainsKey(key) ? (IEnumerable<IntellisenseItem>) Enumerable.OrderBy<IntellisenseItem, string>((IEnumerable<IntellisenseItem>) this.wordTree[key], (Func<IntellisenseItem, string>) (item => item.FilterValue)) : Enumerable.Empty<IntellisenseItem>();
    }

    public void Clear()
    {
      this.wordTree.Clear();
    }
  }
}
