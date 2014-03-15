
using System;
using System.Collections.Generic;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class SharedResourceDictionary : ResourceDictionary
  {
    public static Dictionary<Uri, ResourceDictionary> _sharedDictionaries = new Dictionary<Uri, ResourceDictionary>();
    private Uri _sourceUri;

    public new Uri Source
    {
      get
      {
        return this._sourceUri;
      }
      set
      {
        this._sourceUri = value;
        if (!SharedResourceDictionary._sharedDictionaries.ContainsKey(value))
        {
          base.Source = value;
          SharedResourceDictionary._sharedDictionaries.Add(value, (ResourceDictionary) this);
        }
        else
          this.MergedDictionaries.Add(SharedResourceDictionary._sharedDictionaries[value]);
      }
    }

    static SharedResourceDictionary()
    {
    }

    public void ClearShared()
    {
      SharedResourceDictionary._sharedDictionaries.Clear();
    }
  }
}
