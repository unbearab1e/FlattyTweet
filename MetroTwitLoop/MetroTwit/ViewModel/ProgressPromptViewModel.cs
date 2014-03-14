// Type: MetroTwit.ViewModel.ProgressPromptViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;

namespace MetroTwit.ViewModel
{
  public class ProgressPromptViewModel : MultiAccountViewModelBase
  {
    private bool showAnimation;
    private string progressText;
    private string errorMessage;
    private string title;
    private bool showErrorMessage;

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

    public bool ShowErrorMessage
    {
      get
      {
        return this.showErrorMessage;
      }
      set
      {
        if (this.showErrorMessage == value)
          return;
        this.showErrorMessage = value;
        base.RaisePropertyChanged("ShowErrorMessage");
      }
    }

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

    public string ErrorMessage
    {
      get
      {
        return this.errorMessage;
      }
      set
      {
        if (string.Equals(this.errorMessage, value, StringComparison.Ordinal))
          return;
        this.errorMessage = value;
        base.RaisePropertyChanged("ErrorMessage");
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

    public ProgressPromptViewModel()
    {
      this.ShowAnimation = true;
    }
  }
}
