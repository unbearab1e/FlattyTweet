
using GalaSoft.MvvmLight;
using FlattyTweet.Extensions;
using System;

namespace FlattyTweet.ViewModel
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
