// Type: MetroTwit.ViewModel.UserAccountViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
    public class UserAccountViewModel : MultiAccountViewModelBase
    {
        private static readonly object settingsLock = new object();
        private static readonly object cacheLock = new object();
        private bool viewprofileEnabled = false;
        private bool viewprofiletwitterEnabled = false;
        private bool signoutEnabled = false;
        private Visibility signOutMenuItemVisibility = Visibility.Collapsed;
        private Visibility signInMenuItemVisibility = Visibility.Visible;
        private Visibility switchUserMenuItemVisibility = Visibility.Collapsed;
        private const int cMaxRequestanHour = 300;
        private bool signedIn;
        private UserControl activitiesAndRequestsView;
        private ObservableCollection<TwitterSavedSearch> savedSearches;
        private UserSettings settings;
        private CacheData cache;
        private TweetListView directMessagesView;
        private TweetListView mentionsView;
        private TweetListView friendsView;
        private string apiRateLeft;
        private string apiRateResets;
        private int nonstreamedcolumncount;

        public UserSettings Settings
        {
            get
            {
                if (this.settings == null)
                {
                    lock (UserAccountViewModel.settingsLock)
                        this.settings = UserSettings.Instance(this.TwitterAccountID);
                }
                return this.settings;
            }
        }

        public CacheData Cache
        {
            get
            {
                if (this.cache == null)
                {
                    lock (UserAccountViewModel.cacheLock)
                        this.cache = CacheData.Instance(this.TwitterAccountID);
                }
                return this.cache;
            }
        }

        public OAuthTokens Tokens
        {
            get
            {
                return new OAuthTokens()
                {
                    AccessToken = this.Settings.UserAuthToken,
                    AccessTokenSecret = this.Settings.UserAuthSecret,
                    ConsumerKey = SettingsData.Instance.TwitterConsumerKey,
                    ConsumerSecret = SettingsData.Instance.TwitterConsumerSecret
                };
            }
        }
        private RateLimitStatus rateLimits;
        public RateLimitStatus RateLimits
        {
            get
            {
                return this.rateLimits;
            }
            set
            {
                if (this.rateLimits == value)
                    return;
                this.rateLimits = value;
                base.RaisePropertyChanged("RateLimits");
            }
        }
        private TwitViewModel twitViewModel;
        public TwitViewModel TwitViewModel
        {
            get
            {
                return this.twitViewModel;
            }
            set
            {
                if (this.twitViewModel == value)
                    return;
                this.twitViewModel = value;
                base.RaisePropertyChanged("TwitViewModel");
            }
        }
        private IncomingOutgoingRequestsViewModel incomingOutgoingRequestVM;
        public IncomingOutgoingRequestsViewModel IncomingOutgoingRequestsVM
        {
            get
            {
                return this.incomingOutgoingRequestVM;
            }
            set
            {
                if (this.incomingOutgoingRequestVM == value)
                    return;
                this.incomingOutgoingRequestVM = value;
                base.RaisePropertyChanged("IncomingOutgoingRequestsVM");
            }
        }
        private ActivitiesViewModel activitiesVM;
        public ActivitiesViewModel ActivitesVM
        {
            get
            {
                return this.activitiesVM;
            }
            set
            {
                if (this.activitiesVM == value)
                    return;
                this.activitiesVM = value;
                base.RaisePropertyChanged("UnreadActivtiesCount");
                base.RaisePropertyChanged("ActivitesVM");
            }
        }

        public string UnreadActivtiesCount
        {
            get
            {
                int num = Enumerable.Count<TwitterStreamEventExtended>(Enumerable.Where<TwitterStreamEventExtended>((IEnumerable<TwitterStreamEventExtended>)this.ActivitesVM.Activities, (Func<TwitterStreamEventExtended, bool>)(a => a.UnRead)));
                string str = num > 0 ? num.ToString() : (string)null;
                if (num > 99)
                    str = "+";
                return str;
            }
        }

        public ObservableCollection<TwitterSavedSearch> SavedSearches
        {
            get
            {
                if (this.savedSearches == null)
                    this.savedSearches = new ObservableCollection<TwitterSavedSearch>();
                return this.savedSearches;
            }
        }

        public Visibility SwitchUserMenuItemVisibility
        {
            get
            {
                return this.switchUserMenuItemVisibility;
            }
            set
            {
                if (this.switchUserMenuItemVisibility == value)
                    return;
                this.switchUserMenuItemVisibility = value;
                base.RaisePropertyChanged("SwitchUserMenuItemVisibility");
            }
        }

        public Visibility SignInMenuItemVisibility
        {
            get
            {
                return this.signInMenuItemVisibility;
            }
            set
            {
                if (this.signInMenuItemVisibility == value)
                    return;
                this.signInMenuItemVisibility = value;
                base.RaisePropertyChanged("SignInMenuItemVisibility");
            }
        }

        public Visibility SignOutMenuItemVisibility
        {
            get
            {
                return this.signOutMenuItemVisibility;
            }
            set
            {
                if (this.signOutMenuItemVisibility == value)
                    return;
                this.signOutMenuItemVisibility = value;
                base.RaisePropertyChanged("SignOutMenuItemVisibility");
            }
        }

        public object TwitterUserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TwitterAccountName) && App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.TwitterAccountName.ToLower()) && App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.TwitterAccountName.ToLower()].ImageURITwitterSecure == this.Settings.TwitterUserImage)
                    return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.TwitterAccountName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
                if (!string.IsNullOrEmpty(this.Settings.TwitterUserImage))
                    return (object)CommonCommands.DownloadFile(this.Settings.TwitterUserImage.Replace("_normal", ""));
                else
                    return (object)null;
            }
        }

        public BitmapImage UserDefaultImage
        {
            get
            {
                return CommonCommands.DefaultUserImage();
            }
        }

        public string TwitterAccountName
        {
            get
            {
                return this.Settings.TwitterAccountName;
            }
            set
            {
                if (string.Equals(this.TwitterAccountName, value, StringComparison.Ordinal))
                    return;
                this.Settings.TwitterAccountName = value;
                base.RaisePropertyChanged("TwitterAccountName");
                base.RaisePropertyChanged("TwitterUserImage");
            }
        }

        public string TwitterRealName
        {
            get
            {
                return this.Settings.TwitterRealName;
            }
            set
            {
                if (string.Equals(this.TwitterRealName, value, StringComparison.Ordinal))
                    return;
                this.Settings.TwitterRealName = value;
                base.RaisePropertyChanged("TwitterRealName");
            }
        }

        public string TwitterUserTweetCount
        {
            get
            {
                return this.Settings.TwitterUserTweetCount;
            }
            set
            {
                if (string.Equals(this.TwitterUserTweetCount, value, StringComparison.Ordinal))
                    return;
                this.Settings.TwitterUserTweetCount = value;
                base.RaisePropertyChanged("TwitterUserTweetCount");
            }
        }

        public bool ViewProfileEnabled
        {
            get
            {
                return this.viewprofileEnabled;
            }
            set
            {
                if (this.viewprofileEnabled == value)
                    return;
                this.viewprofileEnabled = value;
                base.RaisePropertyChanged("ViewProfileEnabled");
            }
        }

        public bool ViewProfileTwitterEnabled
        {
            get
            {
                return this.viewprofiletwitterEnabled;
            }
            set
            {
                if (this.viewprofiletwitterEnabled == value)
                    return;
                this.viewprofiletwitterEnabled = value;
                base.RaisePropertyChanged("ViewProfileTwitterEnabled");
            }
        }

        public bool SignOutEnabled
        {
            get
            {
                return this.signoutEnabled;
            }
            set
            {
                if (this.signoutEnabled == value)
                    return;
                this.signoutEnabled = value;
                base.RaisePropertyChanged("SignOutEnabled");
            }
        }

        public bool IsSignedIn
        {
            get
            {
                return this.signedIn;
            }
            set
            {
                if (this.signedIn == value)
                    return;
                this.signedIn = value;
                base.RaisePropertyChanged("IsSignedIn");
            }
        }

        public string APIRateLeft
        {
            get
            {
                return this.apiRateLeft;
            }
            set
            {
                if (string.Equals(this.apiRateLeft, value, StringComparison.Ordinal))
                    return;
                this.apiRateLeft = value;
                base.RaisePropertyChanged("APIRateLeft");
            }
        }

        public string APIRateResets
        {
            get
            {
                return this.apiRateResets;
            }
            set
            {
                if (string.Equals(this.apiRateResets, value, StringComparison.Ordinal))
                    return;
                this.apiRateResets = value;
                base.RaisePropertyChanged("APIRateResets");
            }
        }

        public bool AutomaticallyShortenLinksFalse
        {
            get
            {
                return !this.Settings.AutomaticallyShortenLinks;
            }
            set
            {
                this.Settings.AutomaticallyShortenLinks = !value;
                base.RaisePropertyChanged("AutomaticallyShortenLinksFalse");
                base.RaisePropertyChanged("AutomaticallyShortenLinksTrue");
            }
        }

        public bool AutomaticallyShortenLinksTrue
        {
            get
            {
                return this.Settings.AutomaticallyShortenLinks;
            }
            set
            {
                if (this.AutomaticallyShortenLinksTrue == value)
                    return;
                this.Settings.AutomaticallyShortenLinks = value;
                base.RaisePropertyChanged("AutomaticallyShortenLinksFalse");
                base.RaisePropertyChanged("AutomaticallyShortenLinksTrue");
            }
        }

        public string Filter
        {
            get
            {
                return string.Join(", ", this.Settings.Filter.ToArray());
            }
            set
            {
                if (string.Equals(this.Filter, value, StringComparison.Ordinal))
                    return;
                this.Settings.Filter = Enumerable.ToList<string>(Enumerable.Where<string>((IEnumerable<string>)value.Split(new char[2]
        {
          ',',
          ' '
        }), (Func<string, bool>)(x => !string.IsNullOrEmpty(x))));
                base.RaisePropertyChanged("Filter");
            }
        }

        public bool AllReplies
        {
            get
            {
                return this.Settings.AllReplies;
            }
            set
            {
                if (this.AllReplies == value)
                    return;
                this.Settings.AllReplies = value;
                base.RaisePropertyChanged("AllReplies");
            }
        }

        public MTColumnCollection NonStreamedColumns
        {
            get
            {
                return new MTColumnCollection(Enumerable.ToList<MetroTwitColumn>(Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>)this.Settings.Columns, (Func<MetroTwitColumn, bool>)(column => column.ColumnType == TweetListType.Favourites || column.ColumnType == TweetListType.List || column.ColumnType == TweetListType.Search || column.ColumnType == TweetListType.UserTimeline))));
            }
        }
        private RelayCommand viewProfileCommand;
        public RelayCommand ViewProfileCommand
        {
            get
            {
                return this.viewProfileCommand;
            }
            private set
            {
                if (this.viewProfileCommand == value)
                    return;
                this.viewProfileCommand = value;
                base.RaisePropertyChanged("ViewProfileCommand");
            }
        }
        private RelayCommand viewProfileTwitterCommand;
        public RelayCommand ViewProfileTwitterCommand
        {
            get
            {
                return this.viewProfileTwitterCommand;
            }
            private set
            {
                if (this.viewProfileTwitterCommand == value)
                    return;
                this.viewProfileTwitterCommand = value;
                base.RaisePropertyChanged("ViewProfileTwitterCommand");
            }
        }
        private RelayCommand avatarCommand;
        public RelayCommand AvatarCommand
        {
            get
            {
                return this.avatarCommand;
            }
            private set
            {
                if (this.avatarCommand == value)
                    return;
                this.avatarCommand = value;
                base.RaisePropertyChanged("AvatarCommand");
            }
        }
        private RelayCommand twitterUserNameCommand;
        public RelayCommand TwitterUserNameCommand
        {
            get
            {
                return this.twitterUserNameCommand;
            }
            private set
            {
                if (this.twitterUserNameCommand == value)
                    return;
                this.twitterUserNameCommand = value;
                base.RaisePropertyChanged("TwitterUserNameCommand");
            }
        }
        private RelayCommand twitterUserTweetCountCommand;
        public RelayCommand TwitterUserTweetCountCommand
        {
            get
            {
                return this.twitterUserTweetCountCommand;
            }
            private set
            {
                if (this.twitterUserTweetCountCommand == value)
                    return;
                this.twitterUserTweetCountCommand = value;
                base.RaisePropertyChanged("TwitterUserTweetCountCommand");
            }
        }
        private RelayCommand switchToThisAccountCommand;
        public RelayCommand SwitchToThisAccountCommand
        {
            get
            {
                return this.switchToThisAccountCommand;
            }
            private set
            {
                if (this.switchToThisAccountCommand == value)
                    return;
                this.switchToThisAccountCommand = value;
                base.RaisePropertyChanged("SwitchToThisAccountCommand");
            }
        }
        private RelayCommand manageAccountCommand;
        public RelayCommand ManageAccountCommand
        {
            get
            {
                return this.manageAccountCommand;
            }
            private set
            {
                if (this.manageAccountCommand == value)
                    return;
                this.manageAccountCommand = value;
                base.RaisePropertyChanged("ManageAccountCommand");
            }
        }
        private RelayCommand removeAccountCommand;
        public RelayCommand RemoveAccountCommand
        {
            get
            {
                return this.removeAccountCommand;
            }
            private set
            {
                if (this.removeAccountCommand == value)
                    return;
                this.removeAccountCommand = value;
                base.RaisePropertyChanged("RemoveAccountCommand");
            }
        }
        private RelayCommand editAccountCommand;
        public RelayCommand EditAccountCommand
        {
            get
            {
                return this.editAccountCommand;
            }
            private set
            {
                if (this.editAccountCommand == value)
                    return;
                this.editAccountCommand = value;
                base.RaisePropertyChanged("EditAccountCommand");
            }
        }
        private RelayCommand activitiesCommand;
        public RelayCommand ActivitiesCommand
        {
            get
            {
                return this.activitiesCommand;
            }
            private set
            {
                if (this.activitiesCommand == value)
                    return;
                this.activitiesCommand = value;
                base.RaisePropertyChanged("ActivitiesCommand");
            }
        }
        private RelayCommand mentionsCommand;
        public RelayCommand MentionsCommand
        {
            get
            {
                return this.mentionsCommand;
            }
            private set
            {
                if (this.mentionsCommand == value)
                    return;
                this.mentionsCommand = value;
                base.RaisePropertyChanged("MentionsCommand");
            }
        }
        private RelayCommand directMessagesCommand;
        public RelayCommand DirectMessagesCommand
        {
            get
            {
                return this.directMessagesCommand;
            }
            private set
            {
                if (this.directMessagesCommand == value)
                    return;
                this.directMessagesCommand = value;
                base.RaisePropertyChanged("DirectMessagesCommand");
            }
        }
        private RelayCommand friendsCommand;
        public RelayCommand FriendsCommand
        {
            get
            {
                return this.friendsCommand;
            }
            private set
            {
                if (this.friendsCommand == value)
                    return;
                this.friendsCommand = value;
                base.RaisePropertyChanged("FriendsCommand");
            }
        }

        static UserAccountViewModel()
        {
        }

        public UserAccountViewModel(Decimal TwitterAccountID)
        {
            this.TwitViewModel = new TwitViewModel(TwitterAccountID);
            this.ActivitesVM = new ActivitiesViewModel();
            this.IncomingOutgoingRequestsVM = new IncomingOutgoingRequestsViewModel(TwitterAccountID, true, true, false);
            this.TwitterAccountID = TwitterAccountID;
            this.Settings.TwitterAccountID = TwitterAccountID;
            this.TwitterAccountName = this.Settings.TwitterAccountName;
            this.TwitterUserTweetCount = "Not signed in";
            base.RaisePropertyChanged("TwitterUserImage");
            if (this.IncomingOutgoingRequestsVM != null)
                this.IncomingOutgoingRequestsVM.CheckOverallState();
            this.ViewProfileCommand = new RelayCommand(new Action(this.ViewProfile));
            this.ViewProfileTwitterCommand = new RelayCommand(new Action(this.ViewProfileTwitter));
            this.AvatarCommand = new RelayCommand(new Action(this.AvatarAction));
            this.TwitterUserNameCommand = new RelayCommand(new Action(this.TwitterUserNameAction));
            this.TwitterUserTweetCountCommand = new RelayCommand(new Action(this.TwitterUserTweetCountAction));
            this.SwitchToThisAccountCommand = new RelayCommand(new Action(this.SwitchToThisAccount));
            this.RemoveAccountCommand = new RelayCommand(new Action(this.RemoveAccount));
            this.EditAccountCommand = new RelayCommand(new Action(this.EditAccount));
            this.MentionsCommand = new RelayCommand(new Action(this.Mentions));
            this.DirectMessagesCommand = new RelayCommand(new Action(this.DirectMessages));
            this.FriendsCommand = new RelayCommand(new Action(this.Friends));
            this.ActivitiesCommand = new RelayCommand(new Action(this.Activities));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SignedOut), (Action<GenericMessage<object>>)(o =>
            {
                this.SignInMenuItemVisibility = Visibility.Visible;
                this.SignOutMenuItemVisibility = Visibility.Collapsed;
                this.SwitchUserMenuItemVisibility = Visibility.Collapsed;
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ProfileUpdated), (Action<GenericMessage<object>>)(o => base.RaisePropertyChanged("TwitterUserImage")));
        }

        ~UserAccountViewModel()
        {
            OneTimer.UnregisterTimer((object)TimerMessages.RestRefresh);
            Messenger.Default.Unregister<TimerMessage>((object)this, (object)(((object)TimerMessages.RestRefresh).ToString() + (object)this.TwitterAccountID), new Action<TimerMessage>(this.RestRefresh));
        }

        private void RestRefresh(TimerMessage o)
        {
            this.CheckRefreshTimer();
            this.TwitViewModel.RefreshCommand.Execute((object)null);
        }

        private void CheckRefreshTimer()
        {
            if (this.nonstreamedcolumncount == this.NonStreamedColumns.Count && this.nonstreamedcolumncount != 0)
                return;
            this.nonstreamedcolumncount = this.NonStreamedColumns.Count;
            double num = TimeSpan.FromHours(1.0).TotalMilliseconds / 300.0 * (double)this.nonstreamedcolumncount;
            TimeSpan timeSpan = num > TimeSpan.FromSeconds(60.0).TotalMilliseconds ? TimeSpan.FromMilliseconds(num) : TimeSpan.FromSeconds(60.0);
            if (!OneTimer.AlterTimer((object)(((object)TimerMessages.RestRefresh).ToString() + (object)this.TwitterAccountID), timeSpan, (object)(((object)TimerMessages.RestRefresh).ToString() + (object)this.TwitterAccountID), false))
            {
                OneTimer.RegisterTimer(timeSpan, (object)(((object)TimerMessages.RestRefresh).ToString() + (object)this.TwitterAccountID));
                Messenger.Default.Register<TimerMessage>((object)this, (object)(((object)TimerMessages.RestRefresh).ToString() + (object)this.TwitterAccountID), new Action<TimerMessage>(this.RestRefresh));
            }
        }

        public void UpdateProfile(bool manualSignIn)
        {
            Action<Task<TwitterResponse<User>>> continuationAction = null;
            if ((this.Settings.UserAuthToken != "") && (this.Settings.UserAuthSecret != ""))
            {
                this.SignInMenuItemVisibility = Visibility.Collapsed;
                this.SignOutMenuItemVisibility = Visibility.Visible;
                this.SwitchUserMenuItemVisibility = Visibility.Visible;
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(Visibility.Visible), ViewModelMessages.OverlayVisible);
                try
                {
                    if (continuationAction == null)
                    {
                        continuationAction = delegate(Task<TwitterResponse<User>> r)
                        {
                            Action callback = null;
                            Action action3 = null;
                            Action action4 = null;
                            Action action5 = null;
                            Action action6 = null;
                            if (r.IsFaulted || (r.Result == null))
                            {
                                if (action6 == null)
                                {
                                    action6 = delegate
                                    {
                                        Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult o)
                                        {
                                        }), DialogType.SignInTwitterDownError);
                                        this.BacktoSignedOut();
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                    };
                                }
                                System.Windows.Application.Current.Dispatcher.Invoke(action6);
                            }
                            else
                            {
                                RequestResult result = r.Result.Result;
                                if (result == RequestResult.Unauthorized)
                                {
                                    if (callback == null)
                                    {
                                        callback = () => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult answer)
                                        {
                                            if (answer == MessageBoxResult.Yes)
                                            {
                                                this.BacktoSignedOut();
                                                this.IsSignedIn = false;
                                                Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), base.MultiAccountifyToken(ViewModelMessages.ReloadTweetViews));
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), base.MultiAccountifyToken(ViewModelMessages.SignedOut));
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                            }
                                            else
                                            {
                                                this.BacktoSignedOut();
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                            }
                                        }), DialogType.SignInUnauthorisedError);
                                    }
                                    System.Windows.Application.Current.Dispatcher.Invoke(callback);
                                }
                                else if (result == RequestResult.ProxyAuthenticationRequired)
                                {
                                    if (action3 == null)
                                    {
                                        action3 = () => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult answer)
                                        {
                                            if (answer == MessageBoxResult.Yes)
                                            {
                                                this.BacktoSignedOut();
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(Visibility.Visible), ViewModelMessages.SettingsVisible);
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                            }
                                            else
                                            {
                                                this.BacktoSignedOut();
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                            }
                                        }), DialogType.SignInProxyError);
                                    }
                                    System.Windows.Application.Current.Dispatcher.Invoke(action3);
                                }
                                else if ((r.Result.AccessLevel != AccessLevel.Read) && (r.Result.AccessLevel != AccessLevel.ReadWrite))
                                {
                                    Action action = null;
                                    User profile = r.Result.ResponseObject;
                                    if ((profile != null) && (profile.ScreenName != null))
                                    {
                                        if (action == null)
                                        {
                                            action = delegate
                                            {
                                                this.TwitterAccountName = profile.ScreenName;
                                                this.TwitterRealName = profile.Name;
                                                this.Settings.TwitterAccountLang = profile.Language;
                                                this.Settings.TwitterAccountProtected = profile.IsProtected;
                                                if (!profile.IsProtected)
                                                {
                                                    this.IncomingOutgoingRequestsVM.FetchIncoming = false;
                                                    this.IncomingOutgoingRequestsVM.FetchOutgoing = false;
                                                    this.IncomingOutgoingRequestsVM.ToggleAutoRefresh(false);
                                                }
                                                else
                                                {
                                                    this.IncomingOutgoingRequestsVM.ToggleAutoRefresh(true);
                                                }
                                                this.TwitterUserTweetCount = profile.NumberOfStatuses.ToString() + " tweets";
                                                this.Settings.TwitterUserImage = profile.ProfileImageSecureLocation;
                                                base.RaisePropertyChanged("TwitterUserImage");
                                                this.ViewProfileEnabled = true;
                                                this.ViewProfileTwitterEnabled = true;
                                                this.SignOutEnabled = true;
                                                App.AppState.Accounts[this.TwitterAccountID].Settings.Save(this.TwitterAccountID);
                                            };
                                        }
                                        System.Windows.Application.Current.Dispatcher.Invoke(action);
                                    }
                                    if (action4 == null)
                                    {
                                        action4 = delegate
                                        {
                                            switch (r.Result.Result)
                                            {
                                                case RequestResult.RateLimited:
                                                    Messenger.Default.Send<DialogMessage>(new DialogMessage(r.Result.RateLimiting.Reset.ToLocalTime().ToShortTimeString(), delegate(MessageBoxResult o)
                                                    {
                                                    }), DialogType.SignInRateLimitError);
                                                    break;

                                                case RequestResult.TwitterIsDown:
                                                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult o)
                                                    {
                                                    }), DialogType.SignInTwitterDownError);
                                                    break;

                                                case RequestResult.TwitterIsOverloaded:
                                                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult o)
                                                    {
                                                    }), DialogType.SignInOverCapacityError);
                                                    break;

                                                case RequestResult.Unknown:
                                                    {
                                                        string content = "Your internet connection appears to be down";
                                                        if (r.Result.Content != null)
                                                        {
                                                            content = r.Result.Content.Contains("<!DOCTYPE") ? RegularExpressions.ExtractHTMLTitle(r.Result.Content) : r.Result.Errors.First<TwitterError>().Message;
                                                        }
                                                        Messenger.Default.Send<DialogMessage>(new DialogMessage(content, delegate(MessageBoxResult o)
                                                        {
                                                        }), DialogType.SignInUnknownError);
                                                        break;
                                                    }
                                            }
                                            Task task = new Task(() => this.LogUserLogin());
                                            task.ContinueWith(t => CommonCommands.CheckTaskExceptions(t));
                                            task.Start();
                                            Twitterizer.Application.RateLimitStatusAsync(this.Tokens, MetroTwitTwitterizer.Options).ContinueWith(delegate(Task<TwitterResponse<RateLimitStatus>> rate)
                                            {
                                                if ((!rate.IsFaulted && (rate.Result != null)) && (rate.Result.Result == RequestResult.Success))
                                                {
                                                    this.RateLimits = rate.Result.ResponseObject;
                                                }
                                            });
                                            this.IsSignedIn = true;
                                            CommonCommands.CheckBoon(null);
                                            App.AppState.Accounts[this.TwitterAccountID].Cache.CheckCache();
                                            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), this.MultiAccountifyToken(ViewModelMessages.ReloadTweetViews));
                                            if ((SettingsData.Instance.ActiveAccount == this.TwitterAccountID) || (SettingsData.Instance.ActiveAccount == 0M))
                                            {
                                                this.SwitchToThisAccount();
                                            }
                                            this.ActivitesVM.Activities.AddRange<TwitterStreamEventExtended>(this.Cache.Activities);
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(Visibility.Collapsed), ViewModelMessages.OverlayVisible);
                                            this.CheckRefreshTimer();
                                        };
                                    }
                                    System.Windows.Application.Current.Dispatcher.Invoke(action4);
                                }
                                else
                                {
                                    if (action5 == null)
                                    {
                                        action5 = () => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult o)
                                        {
                                            this.BacktoSignedOut();
                                            this.IsSignedIn = false;
                                            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), base.MultiAccountifyToken(ViewModelMessages.ReloadTweetViews));
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), base.MultiAccountifyToken(ViewModelMessages.SignedOut));
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseLogin);
                                        }), DialogType.TwitterAccessLevelReAuth);
                                    }
                                    System.Windows.Application.Current.Dispatcher.Invoke(action5);
                                }
                            }
                            App.AppState.Accounts[base.TwitterAccountID].Settings.LoginCheckCompleted = true;
                        };
                    }
                    Account.VerifyCredentialsAsync(this.Tokens, MetroTwitTwitterizer.VerifyCredentialsOptions).ContinueWith(continuationAction);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                this.BacktoSignedOut();
            }
        }

        public void UpdateActivtiesCount()
        {
            base.RaisePropertyChanged("UnreadActivtiesCount");
        }

        private void LogUserLogin()
        {
            if (this.TwitterAccountName != string.Empty)
            {
                //((HttpWebRequest)WebRequest.Create("https://appapi.metrotwit.com/log.php?userhash=" + this.TwitterAccountName.MD5String())).GetResponse();
            }
        }

        private void BacktoSignedOut()
        {
            this.IsSignedIn = false;
            this.ViewProfileEnabled = false;
            this.ViewProfileTwitterEnabled = false;
            this.SignOutEnabled = false;
            Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ReloadTweetViews));
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SignedOut));
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseLogin);
            this.Settings.LoginCheckCompleted = true;
        }

        private bool IsNotSignedInAndNotStored()
        {
            return App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthToken == string.Empty && App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthSecret == string.Empty;
        }

        private bool IsNotSignedInAndStored()
        {
            return App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthToken != string.Empty && App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthSecret != string.Empty && !App.AppState.Accounts[this.TwitterAccountID].IsSignedIn;
        }

        private void Activities()
        {
            if (this.activitiesAndRequestsView == null)
            {
                UserAccountViewModel accountViewModel = this;
                UserControl userControl1 = new UserControl();
                userControl1.Padding = new Thickness(10.0);
                UserControl userControl2 = userControl1;
                accountViewModel.activitiesAndRequestsView = userControl2;
                TabControl tabControl = new TabControl();
                tabControl.SelectionChanged += (SelectionChangedEventHandler)((s, e) =>
                {
                    if (!(e.OriginalSource.GetType() != typeof(TabControl)))
                        return;
                    e.Handled = true;
                });
                tabControl.Loaded += (RoutedEventHandler)((o, s) => (o as TabControl).SelectedIndex = 0);
                TabItem tabItem1 = new TabItem();
                tabItem1.Header = (object)"activities";
                TabItem tabItem2 = tabItem1;
                TabItem tabItem3 = new TabItem();
                tabItem3.Header = (object)"requests";
                TabItem tabItem4 = tabItem3;
                TabItem tabItem5 = tabItem2;
                ActivitiesView activitiesView1 = new ActivitiesView();
                activitiesView1.DataContext = (object)this.ActivitesVM;
                ActivitiesView activitiesView2 = activitiesView1;
                tabItem5.Content = (object)activitiesView2;
                TabItem tabItem6 = tabItem4;
                IncomingOutgoingRequestsView outgoingRequestsView1 = new IncomingOutgoingRequestsView();
                outgoingRequestsView1.DataContext = (object)this.IncomingOutgoingRequestsVM;
                IncomingOutgoingRequestsView outgoingRequestsView2 = outgoingRequestsView1;
                tabItem6.Content = (object)outgoingRequestsView2;
                tabControl.Items.Add((object)tabItem2);
                tabControl.Items.Add((object)tabItem4);
                this.activitiesAndRequestsView.Content = (object)tabControl;
                this.activitiesAndRequestsView.DataContext = (object)new NotificationsViewModel()
                {
                    PopupTitle = "activities & requests",
                    AllowPin = false
                };
            }
            this.ActivitesVM.ShowActivityList = this.ActivitesVM.Activities.Count > 0;
            Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(this.activitiesAndRequestsView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            foreach (TwitterStreamEventExtended streamEventExtended in (Collection<TwitterStreamEventExtended>)this.ActivitesVM.Activities)
            {
                streamEventExtended.UnRead = false;
                streamEventExtended.UpdateCreatedAtTimeStamp();
            }
            this.UpdateActivtiesCount();
        }

        private void Friends()
        {
            if (!this.TwitViewModel.ColumnsToShow.Contains(this.TwitViewModel.FriendsVM))
            {
                TweetListView specialTweetListView = this.GetSpecialTweetListView(this.TwitViewModel.FriendsVM, TweetListType.FriendsTimeline, "friends");
                this.TwitViewModel.FriendsVM.AllowPin = true;
                specialTweetListView.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                specialTweetListView.ColumnTitle.Visibility = Visibility.Collapsed;
                specialTweetListView.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)specialTweetListView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
            else
            {
                if (this.friendsView == null)
                {
                    TweetListView tweetListView1 = new TweetListView();
                    tweetListView1.DataContext = (object)this.TwitViewModel.FriendsVM;
                    TweetListView tweetListView2 = tweetListView1;
                    tweetListView2.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                    tweetListView2.ColumnTitle.Visibility = Visibility.Collapsed;
                    this.TwitViewModel.FriendsVM.AllowPin = false;
                    tweetListView2.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                    this.friendsView = tweetListView2;
                }
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)this.friendsView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
        }

        private void Mentions()
        {
            if (!this.TwitViewModel.ColumnsToShow.Contains(this.TwitViewModel.MentionsVM))
            {
                TweetListView specialTweetListView = this.GetSpecialTweetListView(this.TwitViewModel.MentionsVM, TweetListType.MentionsMyTweetsRetweeted, "mentions");
                this.TwitViewModel.MentionsVM.AllowPin = true;
                specialTweetListView.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                specialTweetListView.ColumnTitle.Visibility = Visibility.Collapsed;
                specialTweetListView.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)specialTweetListView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
            else
            {
                if (this.mentionsView == null)
                {
                    TweetListView tweetListView1 = new TweetListView();
                    tweetListView1.DataContext = (object)this.TwitViewModel.MentionsVM;
                    TweetListView tweetListView2 = tweetListView1;
                    tweetListView2.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                    tweetListView2.ColumnTitle.Visibility = Visibility.Collapsed;
                    this.TwitViewModel.MentionsVM.AllowPin = false;
                    tweetListView2.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                    this.mentionsView = tweetListView2;
                }
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)this.mentionsView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
        }

        private void DirectMessages()
        {
            if (!this.TwitViewModel.ColumnsToShow.Contains(this.TwitViewModel.DirectMessagesVM))
            {
                TweetListView specialTweetListView = this.GetSpecialTweetListView(this.TwitViewModel.DirectMessagesVM, TweetListType.DirectMessages, "direct messages");
                specialTweetListView.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                specialTweetListView.ColumnTitle.Visibility = Visibility.Collapsed;
                this.TwitViewModel.DirectMessagesVM.AllowPin = true;
                specialTweetListView.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)specialTweetListView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
            else
            {
                if (this.directMessagesView == null)
                {
                    TweetListView tweetListView1 = new TweetListView();
                    tweetListView1.DataContext = (object)this.TwitViewModel.DirectMessagesVM;
                    TweetListView tweetListView2 = tweetListView1;
                    tweetListView2.ColumnTitle.ClearValue(UIElement.VisibilityProperty);
                    tweetListView2.ColumnTitle.Visibility = Visibility.Collapsed;
                    this.TwitViewModel.DirectMessagesVM.AllowPin = false;
                    tweetListView2.ColumnOptionsButton.Visibility = Visibility.Collapsed;
                    this.directMessagesView = tweetListView2;
                }
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)this.directMessagesView), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup));
            }
        }

        private TweetListView GetSpecialTweetListView(TweetListViewModel viewModel, TweetListType tweetListType, string title)
        {
            if (!App.PermanentSpecialTweetListViews.ContainsKey(this.TwitterAccountID))
                App.PermanentSpecialTweetListViews.Add(this.TwitterAccountID, new Dictionary<TweetListType, TweetListView>());
            if (!App.PermanentSpecialTweetListViews[this.TwitterAccountID].ContainsKey(tweetListType))
            {
                viewModel.AllowPin = false;
                viewModel.PopupTitle = title;
                Dictionary<TweetListType, TweetListView> dictionary = App.PermanentSpecialTweetListViews[this.TwitterAccountID];
                int num = (int)tweetListType;
                TweetListView tweetListView1 = new TweetListView();
                tweetListView1.DataContext = (object)viewModel;
                TweetListView tweetListView2 = tweetListView1;
                dictionary.Add((TweetListType)num, tweetListView2);
            }
            TweetListView tweetListView = App.PermanentSpecialTweetListViews[this.TwitterAccountID][tweetListType];
            if (tweetListView.DataContext is IPopupViewModel)
                (tweetListView.DataContext as IPopupViewModel).AllowPin = false;
            return tweetListView;
        }

        private void RemoveAccount()
        {
            if (MessageBoxView.Show(string.Format("Remove the \"{0}\" account from MetroTwit?", (object)this.TwitterAccountName), "remove account", MessageBoxButton.YesNo, MessageBoxResult.No) != MessageBoxResult.Yes)
                return;
            Messenger.Default.Send<GenericMessage<TwitViewModel>>(new GenericMessage<TwitViewModel>(this.TwitViewModel), (object)ViewModelMessages.RemoveTwitView);
            this.TwitViewModel.Cleanup();
            this.TwitViewModel = (TwitViewModel)null;
            this.Cleanup();
            App.AppState.Accounts.Remove(this);
            if (App.PermanentSpecialTweetListViews.ContainsKey(this.TwitterAccountID))
                App.PermanentSpecialTweetListViews[this.TwitterAccountID].Clear();
            CommonCommands.DeleteAccountData(this.TwitterAccountID);
            if (App.AppState.LastActiveAccount != null)
                App.AppState.SwitchToAccount(App.AppState.LastActiveAccount.TwitterAccountID);
            else if (App.AppState.Accounts.Count != 0)
                App.AppState.SwitchToAccount(Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>)App.AppState.Accounts, 0).TwitterAccountID);
        }

        private void EditAccount()
        {
        }

        private void ViewProfile()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)this.TwitterAccountName), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowUserProfile));
        }

        private void ViewProfileTwitter()
        {
            CommonCommands.OpenLink("http://www.twitter.com/" + this.TwitterAccountName);
        }

        private void SwitchToThisAccount()
        {
            App.AppState.SwitchToAccount(this.TwitterAccountID);
        }

        private void AvatarAction()
        {
        }

        private void TwitterUserNameAction()
        {
            this.ViewProfile();
        }

        private void TwitterUserTweetCountAction()
        {
            CommonCommands.OpenLink(string.Format("http://www.twitter.com/{0}", (object)this.TwitterAccountName));
        }

        internal async void UpdateAPIRates()
        {
            try
            {
                TwitterResponse<RateLimitStatus> statusReponse = await Twitterizer.Application.RateLimitStatusAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, MetroTwitTwitterizer.Options);
                if (statusReponse.Result != RequestResult.Success)
                    ;
            }
            catch
            {
            }
        }

        internal int RateLimitsLeft(TweetListType TweetType)
        {
            switch (TweetType)
            {
                case TweetListType.FriendsTimeline:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/home_timeline") && this.RateLimits.Resources["statuses"]["/statuses/home_timeline"] != null) && this.RateLimits.Resources["statuses"]["/statuses/home_timeline"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/statuses/home_timeline"].Remaining;
                    else
                        break;
                case TweetListType.DirectMessages:
                    if (this.RateLimits.Resources.ContainsKey("direct_messages") && this.RateLimits.Resources["direct_messages"] != null && (this.RateLimits.Resources["direct_messages"].ContainsKey("/direct_messages") && this.RateLimits.Resources["direct_messages"]["/direct_messages"] != null) && (this.RateLimits.Resources["direct_messages"]["/direct_messages"].Reset > DateTime.UtcNow && this.RateLimits.Resources["direct_messages"].ContainsKey("/direct_messages/sent") && this.RateLimits.Resources["direct_messages"]["/direct_messages/sent"] != null) && this.RateLimits.Resources["direct_messages"]["/direct_messages/sent"].Reset > DateTime.UtcNow)
                        return Math.Min(this.RateLimits.Resources["direct_messages"]["/direct_messages"].Remaining, this.RateLimits.Resources["direct_messages"]["/direct_messages/sent"].Remaining);
                    else
                        break;
                case TweetListType.Search:
                    if (this.RateLimits.Resources.ContainsKey("search") && this.RateLimits.Resources["search"] != null && (this.RateLimits.Resources["search"].ContainsKey("/search/tweets") && this.RateLimits.Resources["search"]["/search/tweets"] != null) && this.RateLimits.Resources["search"]["/search/tweets"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["search"]["/search/tweets"].Remaining;
                    else
                        break;
                case TweetListType.UserTimeline:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/user_timeline") && this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] != null) && this.RateLimits.Resources["statuses"]["/statuses/user_timeline"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/statuses/user_timeline"].Remaining;
                    else
                        break;
                case TweetListType.List:
                    if (this.RateLimits.Resources.ContainsKey("lists") && this.RateLimits.Resources["lists"] != null && (this.RateLimits.Resources["lists"].ContainsKey("/lists/statuses") && this.RateLimits.Resources["lists"]["/lists/statuses"] != null) && this.RateLimits.Resources["lists"]["/lists/statuses"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["lists"]["/lists/statuses"].Remaining;
                    else
                        break;
                case TweetListType.MentionsMyTweetsRetweeted:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/mentions_timeline") && this.RateLimits.Resources["statuses"]["/statuses/mentions_timeline"] != null) && (this.RateLimits.Resources["statuses"]["/statuses/mentions_timeline"].Reset > DateTime.UtcNow && this.RateLimits.Resources["statuses"].ContainsKey("/statuses/retweets_of_me") && this.RateLimits.Resources["statuses"]["/statuses/retweets_of_me"] != null) && this.RateLimits.Resources["statuses"]["/statuses/retweets_of_me"].Reset > DateTime.UtcNow)
                        return Math.Min(this.RateLimits.Resources["statuses"]["/statuses/mentions_timeline"].Remaining, this.RateLimits.Resources["statuses"]["/statuses/retweets_of_me"].Remaining);
                    else
                        break;
                case TweetListType.MyTweets:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/user_timeline") && this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] != null) && this.RateLimits.Resources["statuses"]["/statuses/user_timeline"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/statuses/user_timeline"].Remaining;
                    else
                        break;
                case TweetListType.Favourites:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/favorites/list") && this.RateLimits.Resources["statuses"]["/favorites/list"] != null) && this.RateLimits.Resources["statuses"]["/favorites/list"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/favorites/list"].Remaining;
                    else
                        break;
                case TweetListType.Conversation:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/show/:id") && this.RateLimits.Resources["statuses"]["/statuses/show/:id"] != null) && this.RateLimits.Resources["statuses"]["/statuses/show/:id"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/statuses/show/:id"].Remaining;
                    else
                        break;
                case TweetListType.RetweetUsers:
                    if (this.RateLimits.Resources.ContainsKey("statuses") && this.RateLimits.Resources["statuses"] != null && (this.RateLimits.Resources["statuses"].ContainsKey("/statuses/retweets/:id") && this.RateLimits.Resources["statuses"]["/statuses/retweets/:id"] != null) && this.RateLimits.Resources["statuses"]["/statuses/retweets/:id"].Reset > DateTime.UtcNow)
                        return this.RateLimits.Resources["statuses"]["/statuses/retweets/:id"].Remaining;
                    else
                        break;
                default:
                    return 1;
            }
            return 1;
        }

        internal void UpdateRateLimits(TweetListType TweetType, RateLimitResource rateLimiting, string additionalInfo = "")
        {
            try
            {
                if (this.RateLimits == null)
                {
                    this.RateLimits = new RateLimitStatus();
                    this.RateLimits.Resources = new Dictionary<string, Dictionary<string, RateLimitResource>>();
                }
                switch (TweetType)
                {
                    case TweetListType.FriendsTimeline:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/home_timeline"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/home_timeline", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/home_timeline"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/statuses/home_timeline"] = rateLimiting;
                        break;
                    case TweetListType.DirectMessages:
                        if (!this.RateLimits.Resources.ContainsKey("direct_messages"))
                            this.RateLimits.Resources.Add("direct_messages", new Dictionary<string, RateLimitResource>());
                        if (additionalInfo == "R")
                        {
                            if (this.RateLimits.Resources["direct_messages"] != null && !this.RateLimits.Resources["direct_messages"].ContainsKey("/direct_messages"))
                                this.RateLimits.Resources["direct_messages"].Add("/direct_messages", new RateLimitResource());
                            if (this.RateLimits.Resources["direct_messages"]["/direct_messages"] != null && rateLimiting.Limit > 0)
                                this.RateLimits.Resources["direct_messages"]["/direct_messages"] = rateLimiting;
                        }
                        if (!(additionalInfo == "S"))
                            break;
                        if (this.RateLimits.Resources["direct_messages"] != null && !this.RateLimits.Resources["direct_messages"].ContainsKey("/direct_messages/sent"))
                            this.RateLimits.Resources["direct_messages"].Add("/direct_messages/sent", new RateLimitResource());
                        if (this.RateLimits.Resources["direct_messages"]["/direct_messages/sent"] != null && rateLimiting.Limit > 0)
                            this.RateLimits.Resources["direct_messages"]["/direct_messages/sent"] = rateLimiting;
                        break;
                    case TweetListType.Search:
                        if (!this.RateLimits.Resources.ContainsKey("search"))
                            this.RateLimits.Resources.Add("search", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["search"] != null && !this.RateLimits.Resources["search"].ContainsKey("/search/tweets"))
                            this.RateLimits.Resources["search"].Add("/search/tweets", new RateLimitResource());
                        if (this.RateLimits.Resources["search"]["/search/tweets"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["search"]["/search/tweets"] = rateLimiting;
                        break;
                    case TweetListType.UserTimeline:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/user_timeline"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/user_timeline", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] = rateLimiting;
                        break;
                    case TweetListType.List:
                        if (!this.RateLimits.Resources.ContainsKey("lists"))
                            this.RateLimits.Resources.Add("lists", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["lists"] != null && !this.RateLimits.Resources["lists"].ContainsKey("/lists/statuses"))
                            this.RateLimits.Resources["lists"].Add("/lists/statuses", new RateLimitResource());
                        if (this.RateLimits.Resources["lists"]["/lists/statuses"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["lists"]["/lists/statuses"] = rateLimiting;
                        break;
                    case TweetListType.MentionsMyTweetsRetweeted:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (additionalInfo == "M")
                        {
                            if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/mentions_timeline"))
                                this.RateLimits.Resources["statuses"].Add("/statuses/mentions_timeline", new RateLimitResource());
                            if (this.RateLimits.Resources["statuses"]["/statuses/mentions_timeline"] != null && rateLimiting.Limit > 0)
                                this.RateLimits.Resources["statuses"]["/statuses/mentions_timeline"] = rateLimiting;
                        }
                        if (!(additionalInfo == "R"))
                            break;
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/retweets_of_me"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/retweets_of_me", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/retweets_of_me"] != null && rateLimiting.Limit > 0)
                            this.RateLimits.Resources["statuses"]["/statuses/retweets_of_me"] = rateLimiting;
                        break;
                    case TweetListType.MyTweets:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/user_timeline"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/user_timeline", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/statuses/user_timeline"] = rateLimiting;
                        break;
                    case TweetListType.Favourites:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/favorites/list"))
                            this.RateLimits.Resources["statuses"].Add("/favorites/list", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/favorites/list"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/favorites/list"] = rateLimiting;
                        break;
                    case TweetListType.Conversation:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/show/:id"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/show/:id", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/show/:id"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/statuses/show/:id"] = rateLimiting;
                        break;
                    case TweetListType.RetweetUsers:
                        if (!this.RateLimits.Resources.ContainsKey("statuses"))
                            this.RateLimits.Resources.Add("statuses", new Dictionary<string, RateLimitResource>());
                        if (this.RateLimits.Resources["statuses"] != null && !this.RateLimits.Resources["statuses"].ContainsKey("/statuses/retweets/:id"))
                            this.RateLimits.Resources["statuses"].Add("/statuses/retweets/:id", new RateLimitResource());
                        if (this.RateLimits.Resources["statuses"]["/statuses/retweets/:id"] == null || rateLimiting.Limit <= 0)
                            break;
                        this.RateLimits.Resources["statuses"]["/statuses/retweets/:id"] = rateLimiting;
                        break;
                }
            }
            catch
            {
            }
        }
    }
}
