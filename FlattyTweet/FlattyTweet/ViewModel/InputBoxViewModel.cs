
using GalaSoft.MvvmLight;
using System;

namespace FlattyTweet.ViewModel
{
  public class InputBoxViewModel : ViewModelBase
  {
      private string title;
    public string Title
    {
      get
      {
        return this.title;
      }
      set
      {
        if (string.Equals(this.title, value, StringComparison.Ordinal))
          return;
        this.title = value;
        base.RaisePropertyChanged("Title");
      }
    }
      private string message;
    public string Message
    {
      get
      {
        return this.message;
      }
      set
      {
        if (string.Equals(this.message, value, StringComparison.Ordinal))
          return;
        this.message = value;
        base.RaisePropertyChanged("Message");
      }
    }
  }
}
