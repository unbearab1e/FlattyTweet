
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class NotificationControlViewModel : ViewModelBase
  {
    private Dispatcher dispatcher = Application.Current != null ? Dispatcher.CurrentDispatcher : Application.Current.Dispatcher;
    private bool bClose = false;
    public ObservableCollection<MetroTwitStatusBase> newTweets;
    private int newCount;
    private double max;
    private string notificationName;
    private double countdownValue;
    private Timer tweetTimer;
    private MetroTwitStatusBase currentTweet;

      private int currentTweetIndex;
        public int CurrentTweetIndex
    {
      get
      {
        return this.currentTweetIndex;
      }
      set
      {
        if (this.currentTweetIndex == value)
          return;
        this.currentTweetIndex = value;
        base.RaisePropertyChanged("CurrentTweetIndex");
      }
    }

    public int NewCount
    {
      get
      {
        return this.newCount;
      }
      set
      {
        if (this.newCount == value)
          return;
        this.newCount = value;
        base.RaisePropertyChanged("NewCount");
      }
    }

    public double Max
    {
      get
      {
        return this.max;
      }
      set
      {
        if (this.max == value)
          return;
        this.max = value;
        base.RaisePropertyChanged("Max");
      }
    }

    public string NotificationName
    {
      get
      {
        return this.notificationName;
      }
      set
      {
        if (string.Equals(this.notificationName, value, StringComparison.Ordinal))
          return;
        this.notificationName = value;
        base.RaisePropertyChanged("NotificationName");
      }
    }

    public double CountdownValue
    {
      get
      {
        return this.countdownValue;
      }
      set
      {
        if (this.countdownValue == value)
          return;
        this.countdownValue = value;
        base.RaisePropertyChanged("CountdownValue");
      }
    }

    public MetroTwitStatusBase CurrentTweet
    {
      get
      {
        return this.currentTweet;
      }
      set
      {
        if (this.currentTweet == value)
          return;
        this.currentTweet = value;
        base.RaisePropertyChanged("CurrentTweet");
      }
    }
      private bool closing;
    public bool Closing
    {
      get
      {
        return this.closing;
      }
      set
      {
        if (this.closing == value)
          return;
        this.closing = value;
        base.RaisePropertyChanged("Closing");
      }
    }
      private RelayCommand closeNotification;
    public RelayCommand CloseNotification
    {
      get
      {
        return this.closeNotification;
      }
      private set
      {
        if (this.closeNotification == value)
          return;
        this.closeNotification = value;
        base.RaisePropertyChanged("CloseNotification");
      }
    }
      private RelayCommand<string> tagsCommand;
    public RelayCommand<string> TagsCommand
    {
      get
      {
        return this.tagsCommand;
      }
      private set
      {
        if (this.tagsCommand == value)
          return;
        this.tagsCommand = value;
        base.RaisePropertyChanged("TagsCommand");
      }
    }
      private RelayCommand<string> userProfileCommand;
    public RelayCommand<string> UserProfileCommand
    {
      get
      {
        return this.userProfileCommand;
      }
      private set
      {
        if (this.userProfileCommand == value)
          return;
        this.userProfileCommand = value;
        base.RaisePropertyChanged("UserProfileCommand");
      }
    }
      private RelayCommand<UrlEntity> linkCommand;
    public RelayCommand<UrlEntity> LinkCommand
    {
      get
      {
        return this.linkCommand;
      }
      private set
      {
        if (this.linkCommand == value)
          return;
        this.linkCommand = value;
        base.RaisePropertyChanged("LinkCommand");
      }
    }

    public NotificationControlViewModel(List<MetroTwitStatusBase> newTweets)
    {
      this.CloseNotification = new RelayCommand(new Action(this.Close));
      this.TagsCommand = new RelayCommand<string>(new Action<string>(this.ExecuteTag));
      this.UserProfileCommand = new RelayCommand<string>(new Action<string>(this.ExecuteUserProfile));
      this.LinkCommand = new RelayCommand<UrlEntity>(new Action<UrlEntity>(CommonCommands.ExecuteLink));
      this.newTweets = new ObservableCollection<MetroTwitStatusBase>(newTweets);
      this.CurrentTweet = this.newTweets[0];
      this.CountdownValue = SettingsData.Instance.NotificationDisplayTime;
    }

    public void tweetTimerStart()
    {
      if (this.newTweets == null || this.bClose)
        return;
      this.tweetTimer = new Timer(SettingsData.Instance.NotificationTweetDisplayTime * 1000.0 - 200.0);
      this.tweetTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => this.dispatcher.BeginInvoke((Action) (() =>
      {
        this.tweetTimer.Stop();
        if (this.CurrentTweetIndex + 1 >= this.newTweets.Count)
          return;
        base.RaisePropertyChanged("CurrentTweetIndex");
      }), DispatcherPriority.Render, new object[0]));
      this.tweetTimer.Start();
    }

    public void UpdateTweet()
    {
      ++this.CurrentTweetIndex;
      if (this.CurrentTweetIndex < this.newTweets.Count)
        this.CurrentTweet = this.newTweets[this.CurrentTweetIndex];
      this.tweetTimer.Start();
    }

    public void AddNewTweets(List<MetroTwitStatusBase> tweets)
    {
        this.newTweets.AddRange<MetroTwitStatusBase>(tweets);
        base.RaisePropertyChanged("NewCount");
    }

    private void Close()
    {
      base.RaisePropertyChanged("Closing");
    }

    private void ExecuteTag(string tag)
    {
      CommonCommands.ExecuteTag(tag, this.CurrentTweet.TwitterAccountID);
    }

    private void ExecuteUserProfile(string username)
    {
      CommonCommands.ExecuteUserProfile(username, this.CurrentTweet.TwitterAccountID);
    }
  }
}
