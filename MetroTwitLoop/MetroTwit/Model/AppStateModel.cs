// Type: MetroTwit.Model.AppStateModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace MetroTwit.Model
{
  [Export(typeof (ISettingsService))]
  public class AppStateModel : ViewModelBase, ISettingsService
  {
    private MetroTwitAPIStatus APIStatus = (MetroTwitAPIStatus) null;
    private const int adActiveRefresh = 4;
    private const int adInactiveRefresh = 20;
    private BackgroundWorker AdWorker;
    private MetroTwitStatusBase Ad;
    private bool enableMainWindowControlBox;

    public bool ShowCompactAccountPane
    {
      get
      {
        return SettingsData.Instance.ShowCompactAccountPane;
      }
      set
      {
        if (this.ShowCompactAccountPane == value)
          return;
        SettingsData.Instance.ShowCompactAccountPane = value;
        base.RaisePropertyChanged("ShowCompactAccountPane");
        Application.Current.Dispatcher.BeginInvoke((Action) (() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ReloadTweetListView)), DispatcherPriority.Background, new object[0]);
      }
    }
      private MTAccountCollection accounts;
    public MTAccountCollection Accounts
    {
      get
      {
        return this.accounts;
      }
      set
      {
        if (this.accounts == value)
          return;
        this.accounts = value;
        base.RaisePropertyChanged("CurrentActiveAccount");
        base.RaisePropertyChanged("Accounts");
      }
    }
      private UserAccountViewModel lastActiveAccount;
    public UserAccountViewModel LastActiveAccount
    {
      get
      {
        return this.lastActiveAccount;
      }
      set
      {
        if (this.lastActiveAccount == value)
          return;
        this.lastActiveAccount = value;
        base.RaisePropertyChanged("LastActiveAccount");
      }
    }
      private int statusEntryFieldHeight;
    public int StatusEntryFieldHeight
    {
      get
      {
        return this.statusEntryFieldHeight;
      }
      set
      {
        if (this.statusEntryFieldHeight == value)
          return;
        this.statusEntryFieldHeight = value;
        base.RaisePropertyChanged("StatusEntryFieldHeight");
      }
    }
      private int compactModeWidth;
    public int CompactModeWidth
    {
      get
      {
        return this.compactModeWidth;
      }
      set
      {
        if (this.compactModeWidth == value)
          return;
        this.compactModeWidth = value;
        base.RaisePropertyChanged("CompactModeWidth");
      }
    }
      private int fullModeWidth;
    public int FullModeWidth
    {
      get
      {
        return this.fullModeWidth;
      }
      set
      {
        if (this.fullModeWidth == value)
          return;
        this.fullModeWidth = value;
        base.RaisePropertyChanged("FullModeWidth");
      }
    }

    public bool EnableMainWindowControlBox
    {
      get
      {
        return this.enableMainWindowControlBox;
      }
      set
      {
        if (this.enableMainWindowControlBox == value)
          return;
        this.enableMainWindowControlBox = value;
        base.RaisePropertyChanged("EnableMainWindowControlBox");
      }
    }

    public UserAccountViewModel CurrentActiveAccount
    {
      get
      {
        return Enumerable.FirstOrDefault<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, (Func<UserAccountViewModel, bool>) (u => u.TwitViewModel.IsActive)));
      }
    }

    public AppStateModel()
    {
      this.Accounts = new MTAccountCollection();
      this.StatusEntryFieldHeight = 90;
      this.CompactModeWidth = 65;
      this.FullModeWidth = 165;
      this.EnableMainWindowControlBox = true;
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.Exit, (Action<GenericMessage<object>>) (o =>
      {
        foreach (UserAccountViewModel item_0 in (Collection<UserAccountViewModel>) this.Accounts)
          item_0.Settings.Save(item_0.TwitterAccountID);
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.OverlayCountRecalc, (Action<GenericMessage<object>>) (o =>
      {
        if (SettingsData.Instance.ShowTaskbarCount)
        {
          int local_0 = 0;
          foreach (UserAccountViewModel item_2 in (Collection<UserAccountViewModel>) this.Accounts)
          {
            if (item_2.TwitViewModel != null && item_2.TwitViewModel.ColumnsToShow != null)
            {
              foreach (TweetListViewModel item_1 in (Collection<TweetListViewModel>) item_2.TwitViewModel.ColumnsToShow)
              {
                if (item_1.Tweets != null && item_1.TaskbarNotification)
                  local_0 += item_1.UnreadCount;
              }
            }
            if (item_2.TwitViewModel != null && item_2.TwitViewModel.FriendsVM != null && (!item_2.TwitViewModel.FriendsVM.IsPinned && item_2.TwitViewModel.FriendsVM.Tweets != null) && item_2.TwitViewModel.FriendsVM.TaskbarNotification)
              local_0 += item_2.TwitViewModel.FriendsVM.UnreadCount;
            if (item_2.TwitViewModel != null && item_2.TwitViewModel.MentionsVM != null && (!item_2.TwitViewModel.MentionsVM.IsPinned && item_2.TwitViewModel.MentionsVM.Tweets != null) && item_2.TwitViewModel.MentionsVM.TaskbarNotification)
              local_0 += item_2.TwitViewModel.MentionsVM.UnreadCount;
            if (item_2.TwitViewModel != null && item_2.TwitViewModel.DirectMessagesVM != null && (!item_2.TwitViewModel.DirectMessagesVM.IsPinned && item_2.TwitViewModel.DirectMessagesVM.Tweets != null) && item_2.TwitViewModel.DirectMessagesVM.TaskbarNotification)
              local_0 += item_2.TwitViewModel.DirectMessagesVM.UnreadCount;
          }
          if (local_0 < 0)
            local_0 = 0;
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) local_0), (object) ViewModelMessages.OverlayCountUpdate);
        }
        else
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) 0), (object) ViewModelMessages.OverlayCountUpdate);
      }));
      OneTimer.RegisterTimer(TimeSpan.FromMinutes(1.0), (object) TimerMessages.APIStatusRefresh);
      Messenger.Default.Register<TimerMessage>((object) this, (object) TimerMessages.APIStatusRefresh, new Action<TimerMessage>(this.APIStatusRefresh));
      OneTimer.RegisterTimer(TimeSpan.FromMinutes(4.0), (object) TimerMessages.AdRefresh);
      Messenger.Default.Register<TimerMessage>((object) this, (object) TimerMessages.AdRefresh, (Action<TimerMessage>) (o => this.AdRefresh()));
    }

    ~AppStateModel()
    {
      OneTimer.UnregisterTimer((object) TimerMessages.APIStatusRefresh);
      Messenger.Default.Unregister<TimerMessage>((object) this, (object) TimerMessages.APIStatusRefresh, new Action<TimerMessage>(this.APIStatusRefresh));
      OneTimer.UnregisterTimer((object) TimerMessages.AdRefresh);
      Messenger.Default.Unregister<TimerMessage>((object) this, (object) TimerMessages.AdRefresh, (Action<TimerMessage>) (o => this.AdRefresh()));
    }

    private void AdRefresh()
    {
      if (OneTimer.LastInputTime().TotalMinutes > 20.0)
        return;
      if (this.AdWorker == null)
      {
        this.AdWorker = new BackgroundWorker();
        this.AdWorker.WorkerSupportsCancellation = true;
        this.AdWorker.DoWork += new DoWorkEventHandler(this.AdWorker_DoWork);
      }
      if (this.CurrentActiveAccount != null && this.CurrentActiveAccount.TwitViewModel != null && (!SettingsData.Instance.Boon || SettingsData.Instance.PlusShowAds) && !this.AdWorker.IsBusy)
        this.AdWorker.RunWorkerAsync();
    }

    private void AdWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this.Ad = _140ProofService.UserAd(this.CurrentActiveAccount.TwitterAccountID.ToString(), new double?(), new double?(), "");
      if (this.Ad == null)
        return;
      this.Ad.TwitterAccountID = this.CurrentActiveAccount.TwitterAccountID;
      if (this.Ad.AdUrls != null && !string.IsNullOrEmpty(this.Ad.AdUrls.impression_url))
        _140ProofService.HitAdUrl(this.Ad.AdUrls.impression_url);
      if (this.CurrentActiveAccount != null && this.CurrentActiveAccount.TwitViewModel != null)
        this.CurrentActiveAccount.TwitViewModel.PickColumnForAd(this.Ad, true);
    }

    public void AdColumnChanged()
    {
      if (this.CurrentActiveAccount != null && this.CurrentActiveAccount.TwitViewModel != null && this.Ad != null)
        this.CurrentActiveAccount.TwitViewModel.PickColumnForAd(this.Ad, true);
      else
        this.AdRefresh();
    }

    public void AdActiveChanged()
    {
      if (App.IsActive)
        OneTimer.AlterTimer((object) TimerMessages.AdRefresh, TimeSpan.FromMinutes(4.0), (object) TimerMessages.AdRefresh, true);
      else
        OneTimer.AlterTimer((object) TimerMessages.AdRefresh, TimeSpan.FromMinutes(20.0), (object) TimerMessages.AdRefresh, true);
    }

    private void APIStatusRefresh(TimerMessage o)
    {
      IRestResponse restResponse = CoreServices.Instance.RestService.InvokeRESTService("https://appapi.metrotwit.com/status.php", string.Empty, (IDictionary<string, string>) null, string.Format("Mozilla/5.0 (compatible; MetroTwit/{0}; +http://www.metrotwit.com/)", (object) ((object) Application.ResourceAssembly.GetName().Version).ToString()));
      if (restResponse.StatusCode == HttpStatusCode.OK)
      {
        MetroTwitAPIStatus jsonResponse = CoreServices.Instance.RestService.DeserializeJson<MetroTwitAPIStatus>(restResponse.Content);
        if ((this.APIStatus == null || this.APIStatus != null && jsonResponse != null && this.APIStatus.Expiry < jsonResponse.Expiry) && (jsonResponse != null && jsonResponse.Expiry > DateTime.Now))
          Application.Current.Dispatcher.Invoke((Action) (() => (Application.Current.MainWindow.DataContext as MainViewModel).ErrorMessage = new MetroTwitErrorViewModel()
          {
            Text = jsonResponse.Status
          }));
        this.APIStatus = jsonResponse;
      }
      else
        this.APIStatus = (MetroTwitAPIStatus) null;
    }

    public void SaveObject<T>(Dictionary<Decimal, T> targetObjectToSave)
    {
      foreach (KeyValuePair<Decimal, T> keyValuePair in targetObjectToSave)
        this.Accounts[keyValuePair.Key].Settings.SaveObject((object) keyValuePair.Value);
    }

    public Dictionary<Decimal, T> LoadObject<T>(T type)
    {
      Dictionary<Decimal, T> dictionary = new Dictionary<Decimal, T>();
      foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>) this.Accounts)
      {
        T obj = accountViewModel.Settings.LoadObject<T>(type);
        if ((object) obj != null)
          dictionary.Add(accountViewModel.TwitterAccountID, obj);
      }
      return dictionary;
    }

    public void SaveSingleObject<T>(T targetObjectToSave)
    {
      throw new NotImplementedException();
    }

    public T LoadSingleObject<T>(T type)
    {
      throw new NotImplementedException();
    }

    public void SwitchToAccount(Decimal TwitterAccountID)
    {
      if (App.AppState.Accounts[TwitterAccountID] == null || App.AppState.Accounts[TwitterAccountID].TwitViewModel.IsActive)
        return;
      foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>) App.AppState.Accounts)
      {
        if (accountViewModel.TwitterAccountID != TwitterAccountID && accountViewModel.TwitViewModel.IsActive)
        {
          App.AppState.LastActiveAccount = accountViewModel;
          break;
        }
      }
      foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>) App.AppState.Accounts)
        accountViewModel.TwitViewModel.IsActive = false;
      App.AppState.Accounts[TwitterAccountID].TwitViewModel.IsActive = true;
      SettingsData.Instance.ActiveAccount = TwitterAccountID;
      this.AdColumnChanged();
      Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<Tuple<UserAccountViewModel, bool>>>(new GenericMessage<Tuple<UserAccountViewModel, bool>>(new Tuple<UserAccountViewModel, bool>(App.AppState.Accounts[TwitterAccountID], true)), (object) ViewModelMessages.SetActiveAccount);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ReloadTweetListView);
      }), DispatcherPriority.Background, new object[0]);
    }
  }
}
