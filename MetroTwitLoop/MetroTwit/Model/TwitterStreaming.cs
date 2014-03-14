// Type: MetroTwit.Model.TwitterStreaming
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.MVVM.Messages;
using MetroTwit.View;
using System;
using System.Timers;
using System.Windows;
using Twitterizer;
using Twitterizer.Models;
using Twitterizer.Streaming;

namespace MetroTwit.Model
{
  public class TwitterStreaming : MultiAccountViewModelBase, IDisposable
  {
    internal int? retrycount = new int?();
    private StopReasons? StopReason = new StopReasons?();
    internal bool Stopped = false;
    public string TwitterAccount;
    internal Stream Stream;
    private Timer retrytimer;

    public TwitterStreaming(OAuthTokens Tokens, string TwitterAccount, Decimal TwitterAccountID)
    {
      this.TwitterAccount = TwitterAccount;
      this.TwitterAccountID = TwitterAccountID;
      this.Stream = new Stream(Tokens, string.Format("MetroTwit/{0}", (object) ((object) System.Windows.Application.ResourceAssembly.GetName().Version).ToString()), (StreamOptions) new UserStreamOptions()
      {
        AllReplies = App.AppState.Accounts[this.TwitterAccountID].Settings.AllReplies
      });
      this.Stopped = true;
      this.StartStream();
    }

    private void ResetStopped()
    {
      if (!this.Stopped)
        return;
      this.Stopped = false;
      Messenger.Default.Send<TimerMessage>(new TimerMessage(), (object) TimerMessages.RestRefresh);
    }

    internal void StatusDeleted(TwitterStreamDeletedEvent status)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) status), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingStatusDeleted));
    }

    internal void StatusReceived(Status status)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) status), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingStatusReceived));
    }

    internal void FriendsReceived(TwitterIdCollection friendList)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<TwitterIdCollection>>(new GenericMessage<TwitterIdCollection>(friendList), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingFriends));
      this.retrycount = new int?();
    }

    internal void DirectMessageReceived(TwitterDirectMessage directmessage)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) directmessage), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingDirectMessage));
    }

    internal void DirectMessageDeleted(TwitterStreamDeletedEvent directmessagedeleted)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) directmessagedeleted), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingDirectMessageDeleted));
    }

    internal void EventReceived(TwitterStreamEvent events)
    {
      this.ResetStopped();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) events), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.StreamingEvent));
    }

    internal void StreamStopped(StopReasons stopreason)
    {
      this.Stopped = true;
      this.StopReason = new StopReasons?(stopreason);
      if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn && stopreason == StopReasons.Unauthorised)
      {
        System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
        {
          SimpleErrorPrompt local_0 = new SimpleErrorPrompt()
          {
            DataContext = (object) new
            {
              ErrorHeading = "service halted",
              ErrorText = "The Twitter streaming connection could not start.\r\nPlease ensure your computer time and timezone are accurate before restarting MetroTwit."
            }
          };
          Messenger.Default.Send<PromptMessage>(new PromptMessage()
          {
            IsModal = false,
            PromptView = (FrameworkElement) local_0,
            IsCentered = false
          }, (object) ViewModelMessages.ShowSlidePrompt);
        }));
        this.retrycount = new int?(0);
      }
      else
      {
        StopReasons? nullable1 = this.StopReason;
        if ((nullable1.GetValueOrDefault() != StopReasons.WebConnectionFailed ? 0 : (nullable1.HasValue ? 1 : 0)) != 0 && !this.retrycount.HasValue)
        {
          this.retrycount = new int?(0);
          this.Stream.StartUserStream(new InitUserStreamCallback(this.FriendsReceived), new StreamStoppedCallback(this.StreamStopped), new StatusCreatedCallback(this.StatusReceived), new StatusDeletedCallback(this.StatusDeleted), new DirectMessageCreatedCallback(this.DirectMessageReceived), new DirectMessageDeletedCallback(this.DirectMessageDeleted), new EventCallback(this.EventReceived), (RawJsonCallback) null);
        }
        else
        {
          if (stopreason == StopReasons.StoppedByRequest)
            return;
          if (this.retrytimer == null)
          {
            this.retrytimer = new Timer();
            this.retrytimer.Elapsed += new ElapsedEventHandler(this.retrytimer_Elapsed);
            this.retrytimer.AutoReset = false;
          }
          if (!this.retrycount.HasValue)
            this.retrycount = new int?(0);
          TwitterStreaming twitterStreaming = this;
          int? nullable2 = twitterStreaming.retrycount;
          int? nullable3 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() + 1) : new int?();
          twitterStreaming.retrycount = nullable3;
          this.retrytimer.Interval = (double) this.retrycount.Value * TimeSpan.FromSeconds(30.0).TotalMilliseconds;
          if (this.retrytimer.Interval < (stopreason == StopReasons.RateLimited ? TimeSpan.FromMinutes(15.0).TotalMilliseconds : TimeSpan.FromMinutes(5.0).TotalMilliseconds))
            this.retrytimer.Start();
          else
            System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              SimpleErrorPrompt local_0 = new SimpleErrorPrompt()
              {
                DataContext = (object) new
                {
                  ErrorHeading = "service disrupted",
                  ErrorText = "The Twitter streaming connection has been lost. We've tried reconnecting without success.\r\nTwitter restricts the number of streaming connections you can have at any one time, so please check if you have other Twitter applications running."
                }
              };
              Messenger.Default.Send<PromptMessage>(new PromptMessage()
              {
                IsModal = false,
                PromptView = (FrameworkElement) local_0,
                IsCentered = false
              }, (object) ViewModelMessages.ShowSlidePrompt);
            }));
        }
      }
    }

    private void retrytimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.Stream.StartUserStream(new InitUserStreamCallback(this.FriendsReceived), new StreamStoppedCallback(this.StreamStopped), new StatusCreatedCallback(this.StatusReceived), new StatusDeletedCallback(this.StatusDeleted), new DirectMessageCreatedCallback(this.DirectMessageReceived), new DirectMessageDeletedCallback(this.DirectMessageDeleted), new EventCallback(this.EventReceived), (RawJsonCallback) null);
    }

    public void Dispose()
    {
      if (this.Stream != null)
        this.Stream.EndStream();
      this.retrytimer.Stop();
      this.retrytimer.Elapsed -= new ElapsedEventHandler(this.retrytimer_Elapsed);
      this.retrytimer = (Timer) null;
    }

    internal void StopStream()
    {
      if (this.Stream != null)
        this.Stream.EndStream();
      this.Stopped = true;
    }

    internal void StartStream()
    {
      if (this.Stream == null)
        return;
      this.StopStream();
      this.Stream.StartUserStream(new InitUserStreamCallback(this.FriendsReceived), new StreamStoppedCallback(this.StreamStopped), new StatusCreatedCallback(this.StatusReceived), new StatusDeletedCallback(this.StatusDeleted), new DirectMessageCreatedCallback(this.DirectMessageReceived), new DirectMessageDeletedCallback(this.DirectMessageDeleted), new EventCallback(this.EventReceived), (RawJsonCallback) null);
    }
  }
}
