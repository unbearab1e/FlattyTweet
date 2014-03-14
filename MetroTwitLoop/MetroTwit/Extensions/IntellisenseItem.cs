// Type: MetroTwit.Extensions.IntellisenseItem
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using System;
using System.Windows.Media.Imaging;

namespace MetroTwit.Extensions
{
  public class IntellisenseItem
  {
    public object ImageURI
    {
      get
      {
        if (App.AppState.Accounts[this.TwitterAccountID] != null)
        {
          if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey(this.DisplayValue.ToLower()))
            return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers[this.DisplayValue.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
          if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey(this.DisplayValue.ToLower()))
            return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers[this.DisplayValue.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
        }
        return (object) null;
      }
    }

    public BitmapImage UserDefaultImage
    {
      get
      {
        return CommonCommands.DefaultUserImage();
      }
    }

    public string DisplayValue { get; set; }

    public string FilterValue { get; set; }

    public string ExtendedMetaText { get; set; }

    public string DisplayWithExtendedMetaText
    {
      get
      {
        if (this.ExtendedMetaText != null)
          return string.Format("{0} ({1})", (object) this.DisplayValue, (object) this.ExtendedMetaText);
        else
          return string.Format("{0}", (object) this.DisplayValue);
      }
    }

    public Decimal TwitterAccountID { get; set; }

    public IntellisenseItem()
    {
      this.ExtendedMetaText = (string) null;
    }

    public override string ToString()
    {
      return this.DisplayValue;
    }
  }
}
