// Type: MetroTwit.ViewModel.TrendsViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;

namespace MetroTwit.ViewModel
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
