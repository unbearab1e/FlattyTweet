// Type: MetroTwit.Extensions.SharedResourceDictionary
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Generic;
using System.Windows;

namespace MetroTwit.Extensions
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
