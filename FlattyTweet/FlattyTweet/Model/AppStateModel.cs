using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using FlattyTweet.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace FlattyTweet.Model
{
    [Export(typeof(ISettingsService))]
    public class AppStateModel : ViewModelBase, ISettingsService
    {
        private MetroTwitAPIStatus APIStatus = (MetroTwitAPIStatus)null;

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
                Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.ReloadTweetListView)), DispatcherPriority.Background, new object[0]);
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
                return Enumerable.FirstOrDefault<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>)App.AppState.Accounts, (Func<UserAccountViewModel, bool>)(u => u.TwitViewModel.IsActive)));
            }
        }

        public AppStateModel()
        {
            this.Accounts = new MTAccountCollection();
            this.StatusEntryFieldHeight = 90;
            this.CompactModeWidth = 65;
            this.FullModeWidth = 165;
            this.EnableMainWindowControlBox = true;
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.Exit, (Action<GenericMessage<object>>)(o =>
            {
                foreach (UserAccountViewModel item_0 in (Collection<UserAccountViewModel>)this.Accounts)
                    item_0.Settings.Save(item_0.TwitterAccountID);
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.OverlayCountRecalc, (Action<GenericMessage<object>>)(o =>
            {
                if (SettingsData.Instance.ShowTaskbarCount)
                {
                    int local_0 = 0;
                    foreach (UserAccountViewModel item_2 in (Collection<UserAccountViewModel>)this.Accounts)
                    {
                        if (item_2.TwitViewModel != null && item_2.TwitViewModel.ColumnsToShow != null)
                        {
                            foreach (TweetListViewModel item_1 in (Collection<TweetListViewModel>)item_2.TwitViewModel.ColumnsToShow)
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
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)local_0), (object)ViewModelMessages.OverlayCountUpdate);
                }
                else
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)0), (object)ViewModelMessages.OverlayCountUpdate);
            }));
        }

        ~AppStateModel()
        {

        }



        public void SaveObject<T>(Dictionary<Decimal, T> targetObjectToSave)
        {
            foreach (KeyValuePair<Decimal, T> keyValuePair in targetObjectToSave)
                this.Accounts[keyValuePair.Key].Settings.SaveObject((object)keyValuePair.Value);
        }

        public Dictionary<Decimal, T> LoadObject<T>(T type)
        {
            Dictionary<Decimal, T> dictionary = new Dictionary<Decimal, T>();
            foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>)this.Accounts)
            {
                T obj = accountViewModel.Settings.LoadObject<T>(type);
                if ((object)obj != null)
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
            foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>)App.AppState.Accounts)
            {
                if (accountViewModel.TwitterAccountID != TwitterAccountID && accountViewModel.TwitViewModel.IsActive)
                {
                    App.AppState.LastActiveAccount = accountViewModel;
                    break;
                }
            }
            foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>)App.AppState.Accounts)
                accountViewModel.TwitViewModel.IsActive = false;
            App.AppState.Accounts[TwitterAccountID].TwitViewModel.IsActive = true;
            SettingsData.Instance.ActiveAccount = TwitterAccountID;

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Messenger.Default.Send<GenericMessage<Tuple<UserAccountViewModel, bool>>>(new GenericMessage<Tuple<UserAccountViewModel, bool>>(new Tuple<UserAccountViewModel, bool>(App.AppState.Accounts[TwitterAccountID], true)), (object)ViewModelMessages.SetActiveAccount);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.ReloadTweetListView);
            }), DispatcherPriority.Background, new object[0]);
        }
    }
}
