
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace FlattyTweet.ViewModel
{
  public class GenericProgressAndMessagePopupViewModel : ViewModelBase
  {
    private bool showAnimation;
    private bool showMessage;
    private string progressText;
    private string message;

      private RelayCommand oKCommand;
    public RelayCommand OKCommand
    {
      get
      {
        return this.oKCommand;
      }
      private set
      {
        if (this.oKCommand == value)
          return;
        this.oKCommand = value;
        base.RaisePropertyChanged("OKCommand");
      }
    }

    public bool ShowAnimation
    {
      get
      {
        return this.showAnimation;
      }
      set
      {
        if (this.showAnimation == value)
          return;
        this.showAnimation = value;
        base.RaisePropertyChanged("ShowAnimation");
      }
    }

    public bool ShowMessage
    {
      get
      {
        return this.showMessage;
      }
      set
      {
        if (this.showMessage == value)
          return;
        this.showMessage = value;
        base.RaisePropertyChanged("ShowMessage");
      }
    }

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

    public string ProgressText
    {
      get
      {
        return this.progressText;
      }
      set
      {
        if (string.Equals(this.progressText, value, StringComparison.Ordinal))
          return;
        this.progressText = value;
        base.RaisePropertyChanged("ProgressText");
      }
    }

    public GenericProgressAndMessagePopupViewModel(Action dismissCallback)
    {
      this.OKCommand = new RelayCommand(dismissCallback);
    }
  }
}
