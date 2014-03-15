
using FlattyTweet;
using System;
using System.Windows.Media.Imaging;

namespace FlattyTweet.Extensions
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
