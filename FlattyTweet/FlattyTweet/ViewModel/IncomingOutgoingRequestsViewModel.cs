
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class IncomingOutgoingRequestsViewModel : MultiAccountViewModelBase
  {
      private ObservableCollection<User> incomingRequests;
        public ObservableCollection<User> IncomingRequests
    {
      get
      {
        return this.incomingRequests;
      }
      set
      {
        if (this.incomingRequests == value)
          return;
        this.incomingRequests = value;
        base.RaisePropertyChanged("IncomingRequestEmpty");
        base.RaisePropertyChanged("IncomingRequestsCount");
        base.RaisePropertyChanged("TotalRequestsCount");
        base.RaisePropertyChanged("IncomingRequests");
      }
    }
      private ObservableCollection<User> outgoingRequests;
    public ObservableCollection<User> OutgoingRequests
    {
      get
      {
        return this.outgoingRequests;
      }
      set
      {
        if (this.outgoingRequests == value)
          return;
        this.outgoingRequests = value;
        base.RaisePropertyChanged("OutgoingRequestEmpty");
        base.RaisePropertyChanged("OutgoingRequestsCount");
        base.RaisePropertyChanged("TotalRequestsCount");
        base.RaisePropertyChanged("OutgoingRequests");
      }
    }
      private bool fetchIncoming;
    public bool FetchIncoming
    {
      get
      {
        return this.fetchIncoming;
      }
      set
      {
        if (this.fetchIncoming == value)
          return;
        this.fetchIncoming = value;
        base.RaisePropertyChanged("FetchIncoming");
      }
    }
      private bool fetchOutgoing;
    public bool FetchOutgoing
    {
      get
      {
        return this.fetchOutgoing;
      }
      set
      {
        if (this.fetchOutgoing == value)
          return;
        this.fetchOutgoing = value;
        base.RaisePropertyChanged("FetchOutgoing");
      }
    }

    public bool IncomingRequestEmpty
    {
      get
      {
        return this.IncomingRequests.Count == 0;
      }
    }

    public bool OutgoingRequestEmpty
    {
      get
      {
        return this.OutgoingRequests.Count == 0;
      }
    }

    public int IncomingRequestsCount
    {
      get
      {
        return this.IncomingRequests.Count;
      }
    }

    public int OutgoingRequestsCount
    {
      get
      {
        return this.OutgoingRequests.Count;
      }
    }

    public int TotalRequestsCount
    {
      get
      {
        return this.OutgoingRequests.Count + this.IncomingRequests.Count;
      }
    }
      private RelayCommand manageRequestsCommand;
    public RelayCommand ManageRequestsCommand
    {
      get
      {
        return this.manageRequestsCommand;
      }
      set
      {
        if (this.manageRequestsCommand == value)
          return;
        this.manageRequestsCommand = value;
        base.RaisePropertyChanged("ManageRequestsCommand");
      }
    }
      private RelayCommand <User> profileLinkCommand;
    public RelayCommand<User> ProfileLinkCommand
    {
      get
      {
        return this.profileLinkCommand;
      }
      set
      {
        if (this.profileLinkCommand == value)
          return;
        this.profileLinkCommand = value;
        base.RaisePropertyChanged("ProfileLinkCommand");
      }
    }

    public IncomingOutgoingRequestsViewModel(Decimal TwitterAccoundID, bool fetchIncoming = true, bool fetchOutgoing = true, bool fetchRelationships = false)
    {
      this.TwitterAccountID = TwitterAccoundID;
      this.FetchIncoming = fetchIncoming;
      this.FetchOutgoing = fetchOutgoing;
      this.ManageRequestsCommand = new RelayCommand(new Action(this.ManageRequests));
      this.ProfileLinkCommand = new RelayCommand<User>(new Action<User>(this.ProfileLink));
      this.IncomingRequests = new ObservableCollection<User>();
      this.OutgoingRequests = new ObservableCollection<User>();
    }

    public void ToggleAutoRefresh(bool enable)
    {
      if (enable)
      {
        OneTimer.RegisterTimer(TimeSpan.FromMinutes(2.0), (object) TimerMessages.RequestsRefresh);
        Messenger.Default.Register<TimerMessage>((object) this, (object) TimerMessages.RequestsRefresh, new Action<TimerMessage>(this.RequestRefresh));
        this.Refresh();
      }
      else
      {
        OneTimer.UnregisterTimer((object) TimerMessages.RequestsRefresh);
        Messenger.Default.Unregister<TimerMessage>((object) this, (object) TimerMessages.RequestsRefresh, new Action<TimerMessage>(this.RequestRefresh));
      }
    }

    private void RequestRefresh(TimerMessage o)
    {
      this.Refresh();
    }

    private void ManageRequests()
    {
      CommonCommands.OpenLink(string.Format("http://twitter.com/#!/{0}/follower_requests", (object) App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName));
    }

    private void ProfileLink(User user)
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) user.ScreenName), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ShowUserProfile));
    }

    public async void Refresh()
    {
        if (this.FetchIncoming)
        {
            TwitterResponse<TwitterCursorPagedIdCollection> response = await Friendship.IncomingRequestsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, MetroTwitTwitterizer.IncomingFriendshipsOptions);
            TwitterResponse<TwitterCursorPagedIdCollection> incomingRequests = response;
            LookupUsersOptions lookupUsersOptions = MetroTwitTwitterizer.LookupUsersOptions;
            if (incomingRequests.ResponseObject != null)
            {
                lookupUsersOptions.UserIds = new TwitterIdCollection(incomingRequests.ResponseObject.ToList<long>());
                if (lookupUsersOptions.UserIds.Count > 0)
                {
                    Action callback = null;
                    this.IncomingRequests.Clear();
                    TwitterResponse<TwitterUserCollection> incomingUsers = await Users.LookupAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, lookupUsersOptions);
                    if (incomingUsers.Result == RequestResult.Success)
                    {
                        if (callback == null)
                        {
                            callback = delegate
                            {
                                this.IncomingRequests.AddRange<User>(incomingUsers.ResponseObject.ToList<User>());
                                this.RaisePropertyChanged("IncomingRequestsCount");
                                this.RaisePropertyChanged("IncomingRequestEmpty");
                                this.RaisePropertyChanged("TotalRequestsCount");
                            };
                        }
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(callback, DispatcherPriority.Background);
                    }
                }
            }
        }
        if (this.FetchOutgoing)
        {

            TwitterResponse<TwitterCursorPagedIdCollection> outgoingRequests = await Friendship.OutgoingRequestsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, MetroTwitTwitterizer.OutgoingFriendshipsOptions);
            LookupUsersOptions options = MetroTwitTwitterizer.LookupUsersOptions;
            if (outgoingRequests.ResponseObject != null)
            {
                options.UserIds = new TwitterIdCollection(outgoingRequests.ResponseObject.ToList<long>());
                if (options.UserIds.Count > 0)
                {
                    Action asyncVariable2 = null;
                    this.OutgoingRequests.Clear();
                    TwitterResponse<TwitterUserCollection> outgoingUsers = await Users.LookupAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, options);
                    if (outgoingUsers.Result == RequestResult.Success)
                    {
                        if (asyncVariable2 == null)
                        {
                            asyncVariable2 = delegate
                            {
                                this.OutgoingRequests.AddRange<User>(outgoingUsers.ResponseObject.ToList<User>());
                                this.RaisePropertyChanged("OutgoingRequestsCount");
                                this.RaisePropertyChanged("OutgoingRequestEmpty");
                                this.RaisePropertyChanged("TotalRequestsCount");
                            };
                        }
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(asyncVariable2, DispatcherPriority.Background);
                    }
                }
            }
        }
        this.CheckOverallState();
    }

    public void CheckOverallState()
    {
      if (App.NotificationsDisplayUITrigger == null)
        return;
      if (this.TotalRequestsCount > 0)
        VisualStateManager.GoToState(App.NotificationsDisplayUITrigger, "notificationsButtonActive", false);
      else
        VisualStateManager.GoToState(App.NotificationsDisplayUITrigger, "notificationsButtonInactive", false);
    }
  }
}
