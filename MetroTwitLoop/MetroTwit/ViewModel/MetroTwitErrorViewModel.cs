// Type: MetroTwit.ViewModel.MetroTwitErrorViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using MetroTwit.Extensions;
using System;

namespace MetroTwit.ViewModel
{
  public class MetroTwitErrorViewModel : ViewModelBase
  {
    private TimeSpan timeToDisplay;
    private string text;
    private ErrorIcon errorIcon;

    public TimeSpan TimeToDisplay
    {
      get
      {
        return this.timeToDisplay;
      }
      set
      {
        if (TimeSpan.Equals(this.timeToDisplay, value))
          return;
        this.timeToDisplay = value;
        base.RaisePropertyChanged("TimeToDisplay");
      }
    }

    public string Text
    {
      get
      {
        return this.text;
      }
      set
      {
        if (string.Equals(this.text, value, StringComparison.Ordinal))
          return;
        this.text = value;
        base.RaisePropertyChanged("Text");
      }
    }

    public ErrorIcon ErrorIcon
    {
      get
      {
        return this.errorIcon;
      }
      set
      {
        if (this.errorIcon == value)
          return;
        this.errorIcon = value;
        base.RaisePropertyChanged("ErrorIcon");
      }
    }
  }
}
