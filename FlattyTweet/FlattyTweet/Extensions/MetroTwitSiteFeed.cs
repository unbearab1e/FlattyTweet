
using GalaSoft.MvvmLight;
using System;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class MetroTwitSiteFeed : ViewModelBase
  {
    private Visibility releaseDateVisibility = Visibility.Visible;
    private string title;
    private DateTime releaseDate;
    private string text;

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

    public DateTime ReleaseDate
    {
      get
      {
        return this.releaseDate;
      }
      set
      {
        if (DateTime.Equals(this.releaseDate, value))
          return;
        this.releaseDate = value;
        base.RaisePropertyChanged("ReleaseDate");
      }
    }
      private string releaseDateString;
    public string ReleaseDateString
    {
      get
      {
        return this.releaseDateString;
      }
      set
      {
        if (string.Equals(this.releaseDateString, value, StringComparison.Ordinal))
          return;
        this.releaseDateString = value;
        base.RaisePropertyChanged("ReleaseDateString");
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

    public Visibility ReleaseDateVisibility
    {
      get
      {
        return this.releaseDateVisibility;
      }
      set
      {
        if (this.releaseDateVisibility == value)
          return;
        this.releaseDateVisibility = value;
        base.RaisePropertyChanged("ReleaseDateVisibility");
      }
    }
  }
}
