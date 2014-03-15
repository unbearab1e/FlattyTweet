
using FlattyTweet.Extensions;
using System;

namespace FlattyTweet.ViewModel
{
  public class TrendsViewModel : MultiAccountViewModelBase, IPopupViewModel
  {
      private TrendsGroup currentTrends;
    public TrendsGroup CurrentTrends
    {
      get
      {
        return this.currentTrends;
      }
      set
      {
        if (this.currentTrends == value)
          return;
        this.currentTrends = value;
        base.RaisePropertyChanged("CurrentTrends");
      }
    }
      private string popupTitle;
    public string PopupTitle
    {
      get
      {
        return this.popupTitle;
      }
      set
      {
        if (string.Equals(this.popupTitle, value, StringComparison.Ordinal))
          return;
        this.popupTitle = value;
        base.RaisePropertyChanged("PopupTitle");
      }
    }
      private bool allowPin;
    public bool AllowPin
    {
      get
      {
        return this.allowPin;
      }
      set
      {
        if (this.allowPin == value)
          return;
        this.allowPin = value;
        base.RaisePropertyChanged("AllowPin");
      }
    }
      private bool isTransitioningToPinned;
    public bool IsTransitioningToPinned
    {
      get
      {
        return this.isTransitioningToPinned;
      }
      set
      {
        if (this.isTransitioningToPinned == value)
          return;
        this.isTransitioningToPinned = value;
        base.RaisePropertyChanged("IsTransitioningToPinned");
      }
    }

    public TrendsViewModel()
    {
      this.CurrentTrends = new TrendsGroup(TrendsGroupType.Current);
    }
  }
}
