// Type: MetroTwit.ViewModel.PopupFormSaveErrorViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace MetroTwit.ViewModel
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
