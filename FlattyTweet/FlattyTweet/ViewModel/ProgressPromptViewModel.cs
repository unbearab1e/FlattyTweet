
using FlattyTweet.Extensions;
using System;

namespace FlattyTweet.ViewModel
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
