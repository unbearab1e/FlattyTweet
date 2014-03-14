// Type: MetroTwit.ViewModel.InputBoxViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using System;

namespace MetroTwit.ViewModel
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
