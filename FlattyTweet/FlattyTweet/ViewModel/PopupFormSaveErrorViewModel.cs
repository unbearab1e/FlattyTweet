
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace FlattyTweet.ViewModel
{
  public class PopupFormSaveErrorViewModel : ViewModelBase
  {
      private RelayCommand okCommand;
    public RelayCommand OKCommand
    {
      get
      {
        return this.okCommand;
      }
      set
      {
        if (this.okCommand == value)
          return;
        this.okCommand = value;
        base.RaisePropertyChanged("OKCommand");
      }
    }
      private string errorText;
    public string ErrorText
    {
      get
      {
        return this.errorText;
      }
      set
      {
        if (string.Equals(this.errorText, value, StringComparison.Ordinal))
          return;
        this.errorText = value;
        base.RaisePropertyChanged("ErrorText");
      }
    }

    public PopupFormSaveErrorViewModel(string errorText, Action okAction)
    {
      this.OKCommand = new RelayCommand(okAction);
      this.ErrorText = this.ErrorText;
    }
  }
}
