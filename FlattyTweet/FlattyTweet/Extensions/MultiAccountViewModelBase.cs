
using GalaSoft.MvvmLight;
using System;

namespace FlattyTweet.Extensions
{
  public class MultiAccountViewModelBase : ViewModelBase
  {
      private Decimal twitterAccountID;
      public Decimal TwitterAccountID
    {
      get
      {
        return this.twitterAccountID;
      }
      set
      {
        if (Decimal.Equals(this.twitterAccountID, value))
          return;
        this.twitterAccountID = value;
        base.RaisePropertyChanged("TwitterAccountID");
      }
    }

    public string MultiAccountifyToken(Enum VMEnum)
    {
      return ((object) VMEnum).ToString() + this.TwitterAccountID.ToString();
    }
  }
}
