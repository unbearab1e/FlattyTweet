using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.MVVM.Messages;
using FlattyTweet.View;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;
using Twitterizer.Streaming;

namespace FlattyTweet.ViewModel
{
    public class TwitViewModel : MultiAccountViewModelBase
    {
        public static readonly Regex DM_EXPRESSION = new Regex("^[Dd] (\\w*) ");
        private bool charcountOver = false;
        private bool posttweetEnabled = false;
        private bool imageUploadButtonIsEnabled = true;
        private int postTweetEnabledCounter = 0;
        private object postTweetEnabledCounterLock = new object();
        private bool tweetEntryEnabled = true;
        private bool signedinEnabled = false;
        private int statusType = -1;
        private string charcountText = SettingsData.Instance.TweetCharLimit.ToString();
        private Visibility charCountVisibility = Visibility.Collapsed;
        private Visibility postTweetProgressVisibility = Visibility.Collapsed;
        private Visibility whatsHappeningVisibility = Visibility.Visible;
        private Visibility statusLabelVisibility = Visibility.Collapsed;
        private string actualtweetText = "";
        private TwitViewModel.NewTweetType newTweetType = TwitViewModel.NewTweetType.Normal;
        private string statusText = "";
        private bool addFriendsEnabled = false;
        private bool addRepliesEnabled = false;
        private bool addDirectEnabled = false;
        private bool addPublicEnabled = false;
        private bool addFavouriteEnabled = false;
        private bool addMyTweetsEnabled = false;
        private bool addMyReTweetsEnabled = false;
        private bool addMyListsEnabled = false;
        private const int StatusError = -2;
        private const int StatusDefault = -1;
        private const int StatusNotFollowing = 0;
        private const int StatusFollowing = 1;
        private int CharCount;
        private Decimal ReplyToID;
        private string UserName;
        private IEnumerable<TwitterList> userLists;
        private IEnumerable<TwitterList> subscribedLists;
        private bool sendingTweetInProgress;
        private List<ButtonProgress> pendingImageUploads;
        private MetroTwitStatusBase Status;
        public TwitterStreaming UserStream;
        private bool listsEnabled;
        private bool imageUploadsFailed;
        private CountdownEvent uploadImageCountDownEvent;
        private DispatcherTimer BacklogFlushTimer;
        private SearchCriteriaView currentSearchCriteriaView;
        private SearchUserView currentSearchUserView;
        private bool isActive;
        private TweetListViewModel friendsVM;
        private TweetListViewModel mentionsVM;
        private TweetListViewModel directMessagesVM;
        private UndoTweetState lastTweet;
        private SingleImageUploadRequest currentlySelectedImage;

        private bool forceReload;
        public bool ForceReload
        {
            get
            {
                return this.forceReload;
            }
            set
            {
                if (this.forceReload == value)
                    return;
                this.forceReload = value;
                base.RaisePropertyChanged("ForceReload");
            }
        }
        private FlowDocument currentTweetDocument;
        public FlowDocument CurrentTweetDocument
        {
            get
            {
                return this.currentTweetDocument;
            }
            set
            {
                if (this.currentTweetDocument == value)
                    return;
                this.currentTweetDocument = value;
                base.RaisePropertyChanged("CurrentTweetDocument");
            }
        }
        private MTColumnViewModelCollection columnsToShow;
        public MTColumnViewModelCollection ColumnsToShow
        {
            get
            {
                return this.columnsToShow;
            }
            set
            {
                if (this.columnsToShow == value)
                    return;
                this.columnsToShow = value;
                base.RaisePropertyChanged("ColumnsToShow");
            }
        }

        public string ActualTweetText
        {
            get
            {
                return this.actualtweetText;
            }
            set
            {
                if (string.Equals(this.actualtweetText, value, StringComparison.Ordinal))
                    return;
                if (this.actualtweetText != string.Empty && value == string.Empty)
                    this.ResetNewTweet(false);
                this.actualtweetText = value;
                base.RaisePropertyChanged("ImageUploadSectionIsVisible");
                base.RaisePropertyChanged("ActualTweetText");
                this.UpdateTweetTextCounter();
            }
        }

        public Visibility StatusLabelVisibility
        {
            get
            {
                return this.statusLabelVisibility;
            }
            set
            {
                if (this.statusLabelVisibility == value)
                    return;
                this.statusLabelVisibility = value;
                base.RaisePropertyChanged("StatusLabelVisibility");
            }
        }

        public Visibility WhatsHappeningVisibility
        {
            get
            {
                return this.whatsHappeningVisibility;
            }
            set
            {
                if (this.whatsHappeningVisibility == value)
                    return;
                this.whatsHappeningVisibility = value;
                base.RaisePropertyChanged("WhatsHappeningVisibility");
                base.RaisePropertyChanged("ImageUploadSectionIsVisible");
            }
        }

        public bool ImageUploadSectionIsVisible
        {
            get
            {
                return this.WhatsHappeningVisibility != Visibility.Visible || this.ActualTweetText.Length > 0 || this.WhatsHappeningVisibility == Visibility.Visible && this.ActualTweetText.Length == 0 && this.CurrentlySelectedImage != null;
            }
        }

        public Visibility PostTweetProgressVisibility
        {
            get
            {
                return this.postTweetProgressVisibility;
            }
            set
            {
                if (this.postTweetProgressVisibility == value)
                    return;
                this.postTweetProgressVisibility = value;
                base.RaisePropertyChanged("PostTweetProgressVisibility");
            }
        }

        public Visibility CharCountVisibility
        {
            get
            {
                return this.charCountVisibility;
            }
            set
            {
                if (this.charCountVisibility == value)
                    return;
                this.charCountVisibility = value;
                base.RaisePropertyChanged("CharCountVisibility");
            }
        }

        public string CharCountText
        {
            get
            {
                return this.charcountText;
            }
            set
            {
                if (string.Equals(this.charcountText, value, StringComparison.Ordinal))
                    return;
                this.charcountText = value;
                base.RaisePropertyChanged("CharCountText");
            }
        }

        public bool CharCountOver
        {
            get
            {
                return this.charcountOver;
            }
            set
            {
                if (this.charcountOver == value)
                    return;
                this.charcountOver = value;
                base.RaisePropertyChanged("CharCountOver");
            }
        }

        public bool PostTweetEnabled
        {
            get
            {
                return this.posttweetEnabled;
            }
            set
            {
                if (this.posttweetEnabled == value || this.sendingTweetInProgress && value)
                    return;
                this.posttweetEnabled = value;
                base.RaisePropertyChanged("PostTweetEnabled");
            }
        }

        public bool ImageUploadButtonIsEnabled
        {
            get
            {
                return this.imageUploadButtonIsEnabled;
            }
            set
            {
                if (this.imageUploadButtonIsEnabled == value)
                    return;
                this.imageUploadButtonIsEnabled = value;
                base.RaisePropertyChanged("ImageUploadButtonIsEnabled");
            }
        }

        public int PostTweetEnabledCounter
        {
            get
            {
                return this.postTweetEnabledCounter;
            }
            set
            {
                if (this.postTweetEnabledCounter == value)
                    return;
                this.postTweetEnabledCounter = value;
                base.RaisePropertyChanged("PostTweetEnabledCounter");
            }
        }

        public bool TweetEntryEnabled
        {
            get
            {
                return this.tweetEntryEnabled;
            }
            set
            {
                if (this.tweetEntryEnabled == value || this.sendingTweetInProgress && value)
                    return;
                this.tweetEntryEnabled = value;
                base.RaisePropertyChanged("TweetEntryEnabled");
            }
        }

        public bool SignedInEnabled
        {
            get
            {
                return this.signedinEnabled;
            }
            set
            {
                if (this.signedinEnabled == value)
                    return;
                this.signedinEnabled = value;
                base.RaisePropertyChanged("AddFriendsEnabled");
                base.RaisePropertyChanged("AddRepliesEnabled");
                base.RaisePropertyChanged("AddDirectEnabled");
                base.RaisePropertyChanged("AddFavouriteEnabled");
                base.RaisePropertyChanged("AddMyTweetsEnabled");
                base.RaisePropertyChanged("AddMyReTweetsEnabled");
                base.RaisePropertyChanged("AddMyListsEnabled");
                base.RaisePropertyChanged("SignedInEnabled");
            }
        }

        public int StatusType
        {
            get
            {
                return this.statusType;
            }
            set
            {
                if (this.statusType == value)
                    return;
                this.statusType = value;
                base.RaisePropertyChanged("StatusType");
            }
        }

        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                if (string.Equals(this.statusText, value, StringComparison.Ordinal))
                    return;
                this.statusText = value;
                base.RaisePropertyChanged("StatusText");
            }
        }

        public bool AddFriendsEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addFriendsEnabled;
            }
            set
            {
                if (this.addFriendsEnabled == value)
                    return;
                this.addFriendsEnabled = value;
                base.RaisePropertyChanged("AddFriendsEnabled");
            }
        }

        public bool AddRepliesEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addRepliesEnabled;
            }
            set
            {
                if (this.addRepliesEnabled == value)
                    return;
                this.addRepliesEnabled = value;
                base.RaisePropertyChanged("AddRepliesEnabled");
            }
        }

        public bool AddDirectEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addDirectEnabled;
            }
            set
            {
                if (this.addDirectEnabled == value)
                    return;
                this.addDirectEnabled = value;
                base.RaisePropertyChanged("AddDirectEnabled");
            }
        }

        public bool AddPublicEnabled
        {
            get
            {
                return this.addPublicEnabled;
            }
            set
            {
                if (this.addPublicEnabled == value)
                    return;
                this.addPublicEnabled = value;
                base.RaisePropertyChanged("AddPublicEnabled");
            }
        }

        public bool AddFavouriteEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addFavouriteEnabled;
            }
            set
            {
                if (this.addFavouriteEnabled == value)
                    return;
                this.addFavouriteEnabled = value;
                base.RaisePropertyChanged("AddFavouriteEnabled");
            }
        }

        public bool AddMyTweetsEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addMyTweetsEnabled;
            }
            set
            {
                if (this.addMyTweetsEnabled == value)
                    return;
                this.addMyTweetsEnabled = value;
                base.RaisePropertyChanged("AddMyTweetsEnabled");
            }
        }

        public bool AddMyReTweetsEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addMyReTweetsEnabled;
            }
            set
            {
                if (this.addMyReTweetsEnabled == value)
                    return;
                this.addMyReTweetsEnabled = value;
                base.RaisePropertyChanged("AddMyReTweetsEnabled");
            }
        }

        public bool AddMyListsEnabled
        {
            get
            {
                return this.SignedInEnabled && this.addMyListsEnabled;
            }
            set
            {
                if (this.addMyListsEnabled == value)
                    return;
                this.addMyListsEnabled = value;
                base.RaisePropertyChanged("AddMyListsEnabled");
            }
        }

        public IEnumerable<TwitterList> UserLists
        {
            get
            {
                return this.userLists;
            }
            set
            {
                if (this.userLists == value)
                    return;
                this.userLists = value;
                base.RaisePropertyChanged("UserLists");
                this.AddMyListsEnabled = Enumerable.Count<TwitterList>(this.UserLists) > 0;
            }
        }

        public IEnumerable<TwitterList> SubscribedLists
        {
            get
            {
                return this.subscribedLists;
            }
            set
            {
                if (this.subscribedLists == value)
                    return;
                this.subscribedLists = value;
                base.RaisePropertyChanged("SubscribedLists");
            }
        }

        public bool SpellCheckEnabled
        {
            get
            {
                return SettingsData.Instance.UseSpellChecker;
            }
        }

        public bool UseAutoComplete
        {
            get
            {
                return SettingsData.Instance.UseAutoComplete;
            }
        }

        public bool AutoShortenLinks
        {
            get
            {
                return App.AppState.Accounts[this.TwitterAccountID].Settings.AutomaticallyShortenLinks;
            }
        }

        public bool ListsEnabled
        {
            get
            {
                return this.listsEnabled;
            }
            set
            {
                if (this.listsEnabled == value)
                    return;
                this.listsEnabled = value;
                base.RaisePropertyChanged("ListsEnabled");
            }
        }

        public string WhatsHappeningText
        {
            get
            {
                return "What's happening, @" + App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName + "?";
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (this.isActive == value)
                    return;
                this.isActive = value;
                base.RaisePropertyChanged("IsActive");
            }
        }

        public TweetListViewModel FriendsVM
        {
            get
            {
                return this.friendsVM;
            }
            set
            {
                if (this.friendsVM == value)
                    return;
                this.friendsVM = value;
                base.RaisePropertyChanged("FriendsVM");
            }
        }

        public TweetListViewModel MentionsVM
        {
            get
            {
                return this.mentionsVM;
            }
            set
            {
                if (this.mentionsVM == value)
                    return;
                this.mentionsVM = value;
                base.RaisePropertyChanged("MentionsVM");
            }
        }

        public TweetListViewModel DirectMessagesVM
        {
            get
            {
                return this.directMessagesVM;
            }
            set
            {
                if (this.directMessagesVM == value)
                    return;
                this.directMessagesVM = value;
                base.RaisePropertyChanged("DirectMessagesVM");
            }
        }

        public SingleImageUploadRequest CurrentlySelectedImage
        {
            get
            {
                return this.currentlySelectedImage;
            }
            set
            {
                if (this.currentlySelectedImage == value)
                    return;
                this.currentlySelectedImage = value;
                base.RaisePropertyChanged("ImageUploadSectionIsVisible");
                base.RaisePropertyChanged("CurrentlySelectedImage");
                base.RaisePropertyChanged("SingleImageSelected");
                this.UpdateTweetTextCounter();
            }
        }

        public bool SingleImageSelected
        {
            get
            {
                return this.currentlySelectedImage != null;
            }
        }

        public bool TwitterImageServiceIsEnabled
        {
            get
            {
                return SettingsData.Instance.CurrentImageUploadingService == SettingsViewModel.TwitterImageUploadService.GetType().ToString() && this.newTweetType != TwitViewModel.NewTweetType.DirectMessage;
            }
        }
        private RelayCommand listsCommand;
        public RelayCommand ListsCommand
        {
            get
            {
                return this.listsCommand;
            }
            private set
            {
                if (this.listsCommand == value)
                    return;
                this.listsCommand = value;
                base.RaisePropertyChanged("ListsCommand");
            }
        }
        private RelayCommand trendsCommand;
        public RelayCommand TrendsCommand
        {
            get
            {
                return this.trendsCommand;
            }
            private set
            {
                if (this.trendsCommand == value)
                    return;
                this.trendsCommand = value;
                base.RaisePropertyChanged("TrendsCommand");
            }
        }
        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return this.refreshCommand;
            }
            private set
            {
                if (this.refreshCommand == value)
                    return;
                this.refreshCommand = value;
                base.RaisePropertyChanged("RefreshCommand");
            }
        }
        private RelayCommand postTweetCommand;
        public RelayCommand PostTweetCommand
        {
            get
            {
                return this.postTweetCommand;
            }
            private set
            {
                if (this.postTweetCommand == value)
                    return;
                this.postTweetCommand = value;
                base.RaisePropertyChanged("PostTweetCommand");
            }
        }
        private RelayCommand addSingleImageCommand;
        public RelayCommand AddSingleImageCommand
        {
            get
            {
                return this.addSingleImageCommand;
            }
            private set
            {
                if (this.addSingleImageCommand == value)
                    return;
                this.addSingleImageCommand = value;
                base.RaisePropertyChanged("AddSingleImageCommand");
            }
        }
        private RelayCommand removeSingleImageCommand;
        public RelayCommand RemoveSingleImageCommand
        {
            get
            {
                return this.removeSingleImageCommand;
            }
            private set
            {
                if (this.removeSingleImageCommand == value)
                    return;
                this.removeSingleImageCommand = value;
                base.RaisePropertyChanged("RemoveSingleImageCommand");
            }
        }
        private RelayCommand<ViewType> searchCommand;
        public RelayCommand<ViewType> SearchCommand
        {
            get
            {
                return this.searchCommand;
            }
            private set
            {
                if (this.searchCommand == value)
                    return;
                this.searchCommand = value;
                base.RaisePropertyChanged("SearchCommand");
            }
        }
        private RelayCommand<Decimal> savedSearchCommand;
        public RelayCommand<Decimal> SavedSearchCommand
        {
            get
            {
                return this.savedSearchCommand;
            }
            private set
            {
                if (this.savedSearchCommand == value)
                    return;
                this.savedSearchCommand = value;
                base.RaisePropertyChanged("SavedSearchCommand");
            }
        }
        private RelayCommand<ViewType> profileSearchCommand;
        public RelayCommand<ViewType> ProfileSearchCommand
        {
            get
            {
                return this.profileSearchCommand;
            }
            private set
            {
                if (this.profileSearchCommand == value)
                    return;
                this.profileSearchCommand = value;
                base.RaisePropertyChanged("ProfileSearchCommand");
            }
        }
        private RelayCommand<string> addColumnCommand;
        public RelayCommand<string> AddColumnCommand
        {
            get
            {
                return this.addColumnCommand;
            }
            private set
            {
                if (this.addColumnCommand == value)
                    return;
                this.addColumnCommand = value;
                base.RaisePropertyChanged("AddColumnCommand");
            }
        }
        private RelayCommand<string> listCommand;
        public RelayCommand<string> ListCommand
        {
            get
            {
                return this.listCommand;
            }
            private set
            {
                if (this.listCommand == value)
                    return;
                this.listCommand = value;
                base.RaisePropertyChanged("ListCommand");
            }
        }

        static TwitViewModel()
        {
        }

        public TwitViewModel(Decimal TwitterAccountID)
        {
            this.TwitterAccountID = TwitterAccountID;
            this.ListsCommand = new RelayCommand(new Action(this.Lists));
            this.TrendsCommand = new RelayCommand(new Action(this.Trends));
            this.RefreshCommand = new RelayCommand(new Action(this.FlushandRefresh));
            this.PostTweetCommand = new RelayCommand(new Action(this.PostTweet));
            this.SearchCommand = new RelayCommand<ViewType>(new Action<ViewType>(this.Search));
            this.SavedSearchCommand = new RelayCommand<Decimal>(new Action<Decimal>(this.SavedSearch));
            this.ProfileSearchCommand = new RelayCommand<ViewType>(new Action<ViewType>(this.ProfileSearch));
            this.AddColumnCommand = new RelayCommand<string>(new Action<string>(this.AddColumn));
            this.ListCommand = new RelayCommand<string>(new Action<string>(this.ShowList));
            this.AddSingleImageCommand = new RelayCommand(new Action(this.AddSingleImage));
            this.RemoveSingleImageCommand = new RelayCommand(new Action(this.RemoveSingleImage));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowLists), (Action<GenericMessage<object>>)(o => this.Lists()));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowList), (Action<GenericMessage<object>>)(o => this.ShowList(o.Content.ToString())));
            Messenger.Default.Register<GenericMessage<bool>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ReloadTweetViews), (Action<GenericMessage<bool>>)(o => this.LoadTweetViews(o.Content)));
            Messenger.Default.Register<GenericMessage<TweetListViewModel>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.RemoveColumn), (Action<GenericMessage<TweetListViewModel>>)(o => this.RemoveColumn(o, true)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.RebootTHEStream), new Action<GenericMessage<object>>(this.RebootTHEStream));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.Search), (Action<GenericMessage<object>>)(o =>
            {
                string local_0 = string.Empty;
                string local_1 = string.Empty;
                Decimal? local_2 = new Decimal?();
                ViewType local_3 = ViewType.Popup;
                if (o.Content is string)
                {
                    local_0 = o.Content.ToString();
                    local_1 = local_0;
                }
                else if (o.Content is Tuple<string, string, Decimal?, ViewType>)
                {
                    Tuple<string, string, Decimal?, ViewType> local_4 = o.Content as Tuple<string, string, Decimal?, ViewType>;
                    local_1 = local_4.Item1;
                    local_0 = local_4.Item2;
                    local_2 = local_4.Item3;
                    local_3 = local_4.Item4;
                }
                this.TryAddView(local_3, ViewModelType.TweetList, TweetListType.Search, local_0, local_1, local_2, (MetroTwitStatusBase)null, (TwitterList)null);
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowUserProfile), (Action<GenericMessage<object>>)(o =>
            {
                string local_0 = string.Empty;
                ViewType local_1 = ViewType.Popup;
                if (o.Content is string)
                    local_0 = o.Content.ToString();
                else if (o.Content is Tuple<string, ViewType>)
                {
                    Tuple<string, ViewType> local_2 = o.Content as Tuple<string, ViewType>;
                    local_0 = local_2.Item1;
                    local_1 = local_2.Item2;
                }
                this.TryAddView(local_1, ViewModelType.TweetList, TweetListType.UserTimeline, local_0, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null);
            }));
            Messenger.Default.Register<GenericMessage<UserControl>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowExistingViewInPopup), (Action<GenericMessage<UserControl>>)(o => this.TryAddExistingView(ViewType.Popup, o.Content)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowConversation), (Action<GenericMessage<object>>)(o => this.TryAddView(ViewType.Popup, ViewModelType.TweetList, TweetListType.Conversation, string.Empty, string.Empty, new Decimal?(), (MetroTwitStatusBase)o.Content, (TwitterList)null)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ManageList), (Action<GenericMessage<object>>)(o => this.TryAddView(ViewType.Popup, ViewModelType.ManageList, TweetListType.ManageList, string.Empty, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)o.Content)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.Retweet), new Action<GenericMessage<object>>(this.Retweet));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.Reply), new Action<GenericMessage<object>>(this.Reply));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.Mention), new Action<GenericMessage<object>>(this.Mention));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ReplyAll), new Action<GenericMessage<object>>(this.ReplyAll));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowRetweetUsers), (Action<GenericMessage<object>>)(o => this.TryAddView(ViewType.Popup, ViewModelType.TweetList, TweetListType.RetweetUsers, o.Content.ToString(), string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.DirectMessage), new Action<GenericMessage<object>>(this.DirectMessage));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.AfterSettings, (Action<GenericMessage<object>>)(o =>
            {
                base.RaisePropertyChanged("SpellCheckEnabled");
                base.RaisePropertyChanged("UseAutoComplete");
                base.RaisePropertyChanged("AutoShortenLinks");
                this.Refresh();
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.TweetRefresh, (Action<GenericMessage<object>>)(o => this.Refresh()));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.StartNestSync), (Action<GenericMessage<object>>)(o => this.StartNestTimer()));
            Messenger.Default.Register<GenericMessage<NestSync>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NestUpdate), (Action<GenericMessage<NestSync>>)(o => this.NestUpdate(o.Content)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.IncrementPostTweetCounter), (Action<GenericMessage<object>>)(o => this.IncrementPostTweetCounter()));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.DecrementPostTweetCounter), (Action<GenericMessage<object>>)(o => this.DecrementPostTweetCounter()));
            Messenger.Default.Register<GenericMessage<PinViewEventArgs>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.PinPopup), (Action<GenericMessage<PinViewEventArgs>>)(o => this.PinPopup(o.Content)));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.BacklogSeconds), new Action<GenericMessage<object>>(this.BackLogTimer));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.ImageUploadServiceChanged, (Action<GenericMessage<object>>)(o =>
            {
                if (SettingsData.Instance.CurrentImageUploadingService == SettingsViewModel.TwitterImageUploadService.GetType().ToString())
                {
                    this.pendingImageUploads.Clear();
                    this.PostTweetEnabledCounter = 0;
                }
                base.RaisePropertyChanged("SingleImageTypeEnabled");
            }));
            this.ColumnsToShow = new MTColumnViewModelCollection();
            if (App.AppState != null && App.AppState.Accounts[this.TwitterAccountID] != null)
                this.LoadTweetViews(true);
            this.BackLogTimer(new GenericMessage<object>((object)SettingsData.Instance.BacklogSeconds));
            this.pendingImageUploads = new List<ButtonProgress>();
        }

        ~TwitViewModel()
        {
            OneTimer.UnregisterTimer((object)TimerMessages.NestRefresh);
            Messenger.Default.Unregister<TimerMessage>((object)this, (object)TimerMessages.NestRefresh, new Action<TimerMessage>(this.NestTimerRefresh));
        }

        private void BackLogTimer(GenericMessage<object> o)
        {
            if ((int)o.Content > 0)
            {
                if (this.BacklogFlushTimer == null)
                {
                    this.BacklogFlushTimer = new DispatcherTimer()
                    {
                        Interval = TimeSpan.FromSeconds((double)(int)o.Content)
                    };
                    this.BacklogFlushTimer.Tick += new EventHandler(this.BacklogFlushTimer_Tick);
                    this.BacklogFlushTimer.Start();
                }
                else
                {
                    this.BacklogFlushTimer.Interval = TimeSpan.FromSeconds((double)(int)o.Content);
                    this.BacklogFlushTimer.Stop();
                    this.BacklogFlushTimer.Start();
                }
            }
            else if (this.BacklogFlushTimer != null)
            {
                this.BacklogFlushTimer.Stop();
                this.BacklogFlushTimer = (DispatcherTimer)null;
            }
        }

        private void StartNestTimer()
        {
            if (!App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns)
                return;
            if (!OneTimer.ContainsTimer(TimeSpan.FromMinutes(5.0), (object)TimerMessages.NestRefresh))
            {
                OneTimer.RegisterTimer(TimeSpan.FromMinutes(5.0), (object)TimerMessages.NestRefresh);
                Messenger.Default.Register<TimerMessage>((object)this, (object)TimerMessages.NestRefresh, new Action<TimerMessage>(this.NestTimerRefresh));
            }
            this.NestTimerRefresh(new TimerMessage());
        }

        private void NestTimerRefresh(TimerMessage o)
        {
            NestService.ColumnsSync(new NestSync(), (Action<NestSync>)(r => this.NestUpdate(r)), this.TwitterAccountID);
        }

        private void BacklogFlushTimer_Tick(object sender, EventArgs e)
        {
            this.Flush();
        }

        private void TweetTextCounter()
        {
            int count = 0;
            int num1 = 0;
            int num2 = this.actualtweetText.EndsWith(Environment.NewLine) ? Environment.NewLine.Length : 0;
            Match match1 = TwitViewModel.DM_EXPRESSION.Match(this.actualtweetText);
            if (match1.Success)
            {
                if (this.newTweetType != TwitViewModel.NewTweetType.DirectMessage)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.ResetNewTweet(false)));
                    this.newTweetType = TwitViewModel.NewTweetType.DirectMessage;
                    this.DMCheckUserFollow(this.actualtweetText.Split(new char[1]
          {
            ' '
          })[1]);
                    count = this.actualtweetText.Replace(match1.Groups[0].Value, "").TrimStart(new char[0]).Length;
                }
                else
                {
                    MatchCollection matchCollection = RegularExpressions.VALID_URL.Matches(this.actualtweetText);
                    if (matchCollection.Count > 0)
                    {
                        foreach (Match match2 in matchCollection)
                        {
                            num2 += match2.Groups[3].Length;
                            num1 += match2.Groups[4].Value == "https://" ? SettingsData.Instance.TwitterShortEncryptedUrlLength : SettingsData.Instance.TwitterShortUrlLength;
                        }
                    }
                    count = Enumerable.Count<char>((IEnumerable<char>)this.actualtweetText) - match1.Groups[0].Length - num2 + num1;
                }
            }
            else
            {
                if (this.newTweetType == TwitViewModel.NewTweetType.DirectMessage)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.ResetNewTweet(false)));
                }
                else
                {
                    MatchCollection matchCollection = RegularExpressions.VALID_URL.Matches(this.actualtweetText);
                    if (matchCollection.Count > 0)
                    {
                        foreach (Match match2 in matchCollection)
                        {
                            num2 += match2.Groups[3].Length;
                            num1 += match2.Groups[4].Value == "https://" ? SettingsData.Instance.TwitterShortEncryptedUrlLength : SettingsData.Instance.TwitterShortUrlLength;
                        }
                    }
                }
                count = Enumerable.Count<char>((IEnumerable<char>)this.actualtweetText) - num2 + num1;
            }
            if (this.CurrentlySelectedImage != null)
                count += SettingsData.Instance.TwitterShortUrlLength;
            string countText = (SettingsData.Instance.TweetCharLimit - count).ToString();
            bool postEnabled = (count <= SettingsData.Instance.TweetCharLimit || Enumerable.Count<ITweetService>(Enumerable.Where<ITweetService>((IEnumerable<ITweetService>)CoreServices.Instance.TweetServices, (Func<ITweetService, bool>)(x => x.OverrideTweetCharLimit))) != 0) && (count != 0 && this.StatusType != 0);
            Visibility countVisible = count == 0 ? Visibility.Collapsed : Visibility.Visible;
            bool countOver = count > SettingsData.Instance.TweetCharLimit;
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.CharCount = count;
                this.CharCountText = countText;
                this.PostTweetEnabled = postEnabled;
                this.CharCountVisibility = countVisible;
                this.CharCountOver = countOver;
            }));
            if (this.newTweetType != TwitViewModel.NewTweetType.Reply || (this.Status == null || this.ActualTweetText.IndexOf("@" + this.Status.User.ScreenName) >= 0))
                return;
            this.ReplyToID = new Decimal(0);
            this.newTweetType = TwitViewModel.NewTweetType.Normal;
            this.Status = (MetroTwitStatusBase)null;
        }

        private void IncrementPostTweetCounter()
        {
            lock (this.postTweetEnabledCounterLock)
                ++this.PostTweetEnabledCounter;
        }

        private void DecrementPostTweetCounter()
        {
            lock (this.postTweetEnabledCounterLock)
                --this.PostTweetEnabledCounter;
        }

        private void PinPopup(PinViewEventArgs pea)
        {
            if (pea == null)
                return;
            UserControl view = pea.View;
            if (view is BaseTweetListView)
            {
                TweetListViewModel existingColumnModel = view.DataContext as TweetListViewModel;
                if (existingColumnModel != null)
                    existingColumnModel.EnableNotifications = true;
                existingColumnModel.SettingsVisible = true;
                existingColumnModel.TitleBarVisible = true;
                App.AppState.Accounts[this.TwitterAccountID].SwitchToThisAccountCommand.Execute((object)null);
                this.TryAddColumn(true, existingColumnModel, TweetListType.UserTimeline, string.Empty, string.Empty, new Decimal?());
            }
            else if (view is ListsView)
            {
                ListsViewModel listsViewModel = view.DataContext as ListsViewModel;
            }
        }

        private void RemoveColumn(GenericMessage<TweetListViewModel> o, bool NotifyNest = true)
        {
            TweetListViewModel viewModel = o.Content;
            int index = this.ColumnsToShow.IndexOf(viewModel);
            this.SetAddButtonsEnabled(viewModel.TweetType, true);
            if (index > -1 && index < this.ColumnsToShow.Count)
            {
                this.ColumnsToShow[index] = (TweetListViewModel)null;
                this.ColumnsToShow.RemoveAt(index);
            }
            IEnumerable<MetroTwitColumn> source = Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>)App.AppState.Accounts[this.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, bool>)(x => x.ID == viewModel.UniqueTweetListID));
            if (viewModel != this.MentionsVM && viewModel != this.DirectMessagesVM && viewModel != this.FriendsVM)
            {
                if (Enumerable.Count<MetroTwitColumn>(source) > 0)
                    App.AppState.Accounts[this.TwitterAccountID].Settings.Columns.Remove(Enumerable.First<MetroTwitColumn>(source));
                if (NotifyNest && App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns)
                    NestService.ColumnsSync(new NestSync()
                    {
                        Remove = {
              (object) new NestColumn()
              {
                Id = viewModel.NestColumnID
              }
            }
                    }, (Action<NestSync>)(r => this.NestUpdate(r)), this.TwitterAccountID);
                if (viewModel.Tweets != null)
                    viewModel.Tweets.Clear();
            }
            else
                Enumerable.First<MetroTwitColumn>(source).ColumnPinned = false;
            TweetListView tweetListView = Enumerable.FirstOrDefault<TweetListView>((IEnumerable<TweetListView>)App.TemporarilyRootedTweetListViews, (Func<TweetListView, bool>)(view => view.DataContext == viewModel));
            if (tweetListView != null)
                App.TemporarilyRootedTweetListViews.Remove(tweetListView);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)index), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ColumnsToShow));
        }

        private void Retweet(GenericMessage<object> o)
        {
            if (this.sendingTweetInProgress)
                return;
            this.ResetNewTweet(true);
            this.newTweetType = TwitViewModel.NewTweetType.Retweet;
            this.Status = o.Content as MetroTwitStatusBase;
            this.ActualTweetText = "RT @{1} {2}".Replace("{1}", (o.Content as MetroTwitStatusBase).User.ScreenName).Replace("{2}", WebUtility.HtmlDecode((o.Content as MetroTwitStatusBase).Text));
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
            App.AppState.SwitchToAccount(this.TwitterAccountID);
        }

        private void Reply(GenericMessage<object> o)
        {
            if (this.sendingTweetInProgress)
                return;
            if (this.Status == null)
            {
                this.Status = o.Content as MetroTwitStatusBase;
                this.ReplyToID = this.Status.ID;
                this.UserName = this.Status.User.ScreenName;
                this.newTweetType = TwitViewModel.NewTweetType.Reply;
            }
            App.AppState.SwitchToAccount(this.TwitterAccountID);
            if (this.ActualTweetText.IndexOf("@" + this.UserName) < 0)
            {
                this.ActualTweetText = string.Format("{0}{1}@{2} ", (object)this.ActualTweetText.Trim(), this.ActualTweetText == string.Empty ? (object)"" : (object)" ", (object)this.UserName);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
            }
            else
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditFocus));
        }

        private void Mention(GenericMessage<object> o)
        {
            if (this.sendingTweetInProgress)
                return;
            App.AppState.SwitchToAccount(this.TwitterAccountID);
            string str = o.Content.ToString();
            if (this.ActualTweetText.IndexOf("@" + str) < 0)
            {
                this.ActualTweetText = string.Format("{0}{1}@{2} ", (object)this.ActualTweetText.Trim(), this.ActualTweetText == string.Empty ? (object)"" : (object)" ", (object)str);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
            }
            else
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditFocus));
        }

        private void ReplyAll(GenericMessage<object> o)
        {
            if (this.sendingTweetInProgress)
                return;
            if (this.Status == null)
            {
                this.Status = o.Content as MetroTwitStatusBase;
                this.ReplyToID = this.Status.ID;
                this.UserName = this.Status.User.ScreenName;
                this.newTweetType = TwitViewModel.NewTweetType.Reply;
            }
            string str = o.Content as MetroTwitStatusBase == this.Status ? this.SetupReplyAll(this.Status, this.UserName, this.Status.IsRetweet ? this.Status.RetweetUser.ScreenName : string.Empty) : this.SetupReplyAll(o.Content as MetroTwitStatusBase, (o.Content as MetroTwitStatusBase).User.ScreenName, (o.Content as MetroTwitStatusBase).IsRetweet ? (o.Content as MetroTwitStatusBase).RetweetUser.ScreenName : string.Empty);
            App.AppState.SwitchToAccount(this.TwitterAccountID);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)str), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SelectTweetText));
        }

        private string SetupReplyAll(MetroTwitStatusBase status, string screenname, string retweetscreenname)
        {
            string str1 = string.Empty;
            if (this.ActualTweetText.IndexOf("@" + screenname) < 0)
                this.ActualTweetText = string.Format("{0}{1}@{2} ", (object)this.ActualTweetText.Trim(), this.ActualTweetText == string.Empty ? (object)"" : (object)" ", (object)screenname);
            if (status.IsRetweet && !string.IsNullOrEmpty(retweetscreenname) && (this.ActualTweetText.IndexOf("@" + retweetscreenname) < 0 && retweetscreenname.ToLower() != App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower()))
            {
                this.ActualTweetText = string.Format("{0}{1}@{2} ", (object)this.ActualTweetText.Trim(), this.ActualTweetText == string.Empty ? (object)"" : (object)" ", (object)retweetscreenname);
                str1 = string.Format("{0}{1}@{2} ", (object)str1.Trim(), str1 == string.Empty ? (object)"" : (object)" ", (object)retweetscreenname);
            }
            IEnumerable<MentionEntity> source = status.Entities != null ? Enumerable.OfType<MentionEntity>((IEnumerable)status.Entities) : (IEnumerable<MentionEntity>)null;
            if (source != null && Enumerable.Count<MentionEntity>(source) > 0)
            {
                foreach (string str2 in Enumerable.Select<MentionEntity, string>(source, (Func<MentionEntity, string>)(x => x.ScreenName.Trim())))
                {
                    if (this.ActualTweetText.IndexOf("@" + str2) < 0 && str2.ToLower() != App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower())
                    {
                        this.ActualTweetText = string.Format("{0}{1}@{2} ", (object)this.ActualTweetText.Trim(), this.ActualTweetText == string.Empty ? (object)"" : (object)" ", (object)str2);
                        str1 = string.Format("{0}{1}@{2} ", (object)str1.Trim(), str1 == string.Empty ? (object)"" : (object)" ", (object)str2);
                    }
                }
            }
            return str1;
        }

        private void DirectMessage(GenericMessage<object> o)
        {
            if (this.sendingTweetInProgress)
                return;
            string str = string.Empty;
            string screenName = !(o.Content is string) ? (o.Content as MetroTwitStatusBase).User.ScreenName : o.Content.ToString();
            this.ResetNewTweet(true);
            this.UserName = screenName;
            this.newTweetType = TwitViewModel.NewTweetType.DirectMessage;
            this.ActualTweetText = "d " + this.UserName + " ";
            App.AppState.SwitchToAccount(this.TwitterAccountID);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
            this.DMCheckUserFollow(screenName);
            this.StatusLabelVisibility = Visibility.Visible;
            base.RaisePropertyChanged("SingleImageTypeEnabled");
        }

        private void RebootTHEStream(GenericMessage<object> o)
        {
            if (this.UserStream == null)
                return;
            this.UserStream.StopStream();
            (this.UserStream.Stream.StreamOptions as UserStreamOptions).AllReplies = App.AppState.Accounts[this.TwitterAccountID].Settings.AllReplies;
            this.UserStream.StartStream();
        }

        private void EventReceived(GenericMessage<object> o)
        {
            TwitterStreamEvent content = o.Content as TwitterStreamEvent;
            if ((((content != null) && (content.Target != null)) && (content.Target.Id == base.TwitterAccountID)) && (content.Source != null))
            {
                Action method = null;
                TwitterStreamEventExtended extendedStreamEvent;
                UserAccountViewModel account;
                switch (content.EventType)
                {
                    case TwitterSteamEvent.Block:
                        App.AppState.Accounts[base.TwitterAccountID].Cache.AddBlockedUser((long)content.Target.Id);
                        break;

                    case TwitterSteamEvent.Unblock:
                        App.AppState.Accounts[base.TwitterAccountID].Cache.RemoveBlockedUser((long)content.Target.Id);
                        break;

                    case TwitterSteamEvent.Favorite:
                    case TwitterSteamEvent.Unfavorite:
                    case TwitterSteamEvent.Follow:
                    case TwitterSteamEvent.ListMemberAdded:
                    case TwitterSteamEvent.ListMemberRemoved:
                        if (content.EventType != TwitterSteamEvent.Follow)
                        {
                            App.AppState.Accounts[base.TwitterAccountID].Cache.AddIntellisenseUser(content.Target.ScreenName, content.Target.ProfileImageSecureLocation);
                        }
                        else
                        {
                            App.AppState.Accounts[base.TwitterAccountID].Cache.AddFollowedUser(new MetroTwitUser(content.Target));
                        }
                        extendedStreamEvent = new TwitterStreamEventExtended(content)
                        {
                            TwitterAccountID = base.TwitterAccountID
                        };
                        account = App.AppState.Accounts[base.TwitterAccountID];
                        if ((from x in account.ActivitesVM.Activities.ToArray<TwitterStreamEventExtended>()
                             where ((x != null) && (x.CreatedAt == extendedStreamEvent.CreatedAt)) && (x.EventType == extendedStreamEvent.EventType)
                             select x).Count<TwitterStreamEventExtended>() == 0)
                        {
                            if (method == null)
                            {
                                method = delegate
                                {
                                    account.ActivitesVM.Activities.Add(extendedStreamEvent);
                                    account.UpdateActivtiesCount();
                                };
                            }
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(method, new object[0]);
                        }
                        if (account.Cache.Activities.Count == 0x19)
                        {
                            IEnumerable<TwitterStreamEventExtended> source = from x in account.Cache.Activities.ToArray()
                                                                             where x == null
                                                                             select x;
                            if ((source != null) && (source.Count<TwitterStreamEventExtended>() > 0))
                            {
                                foreach (TwitterStreamEventExtended extended in source)
                                {
                                    account.Cache.Activities.Remove(extended);
                                }
                            }
                            TwitterStreamEventExtended item = (from ts in account.Cache.Activities
                                                               orderby ts.CreatedAt descending
                                                               select ts).FirstOrDefault<TwitterStreamEventExtended>();
                            if (item != null)
                            {
                                account.Cache.Activities.Remove(item);
                            }
                        }
                        if ((from x in account.Cache.Activities.ToArray()
                             where ((x != null) && (x.CreatedAt == extendedStreamEvent.CreatedAt)) && (x.EventType == extendedStreamEvent.EventType)
                             select x).Count<TwitterStreamEventExtended>() == 0)
                        {
                            account.Cache.Activities.Add(extendedStreamEvent);
                            account.Cache.Save();
                        }
                        return;

                    case TwitterSteamEvent.Unfollow:
                        App.AppState.Accounts[base.TwitterAccountID].Cache.RemovedFollowedUser(new MetroTwitUser(content.Target));
                        break;
                }
            }
        }

        public void UndoLastTweet()
        {
            if (this.lastTweet != null)
                this.UndoTweet(this.lastTweet, true);
            else
                Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.NothingToUndo);
        }

        public void UndoTweet(UndoTweetState state, bool undoLastTweetTriggered = false)
        {
            ProgressPromptViewModel progressPromptViewModel = new ProgressPromptViewModel
            {
                ErrorMessage = "The DeLorean is not working! Please try again.",
                ProgressText = "back to the future!...",
                Title = "undoing the tweet"
            };
            string errorTitle = "tweet could not be undone";
            Action<RequestResult> UndoCallback = result => System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                if (result == RequestResult.Success)
                {
                    this.ActualTweetText = state.LastTweetText;
                    this.UserName = state.UserName;
                    this.newTweetType = state.TweetType;
                    this.ReplyToID = state.ReplyToID;
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(string.Empty), this.MultiAccountifyToken(ViewModelMessages.NewTweetEditUpdateText));
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.UndoTweetClose);
                }
                else
                {
                    progressPromptViewModel.Title = errorTitle;
                    progressPromptViewModel.ShowAnimation = false;
                    progressPromptViewModel.ShowErrorMessage = true;
                    if (undoLastTweetTriggered)
                    {
                        this.lastTweet = null;
                    }
                }
            });
            Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult answer)
            {
                if (answer == MessageBoxResult.Yes)
                {
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(progressPromptViewModel), DialogType.UndoTweet);
                    if ((state.TweetType == NewTweetType.Normal) || (state.TweetType == NewTweetType.Reply))
                    {
                        CommonCommands.DeleteTweet(state.Id, this.TwitterAccountID, UndoCallback, true);
                    }
                    else if (state.TweetType == NewTweetType.DirectMessage)
                    {
                        CommonCommands.DeleteDirectMessage(state.Id, this.TwitterAccountID, UndoCallback, true);
                    }
                }
            }), DialogType.UndoTweetConfirmation);
        }

        public void LoadTweetViews(bool forceReload)
        {
            this.ForceReload = forceReload;
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)0), (object)ViewModelMessages.OverlayCountUpdate);
            this.ResetAddButtonsEnabled();
            if (Enumerable.Count<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow) == 1)
                this.ColumnsToShow[0] = (TweetListViewModel)null;
            this.ColumnsToShow.Clear();
            this.ListsEnabled = App.AppState.Accounts[this.TwitterAccountID].IsSignedIn;
            if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
            {
                this.UserStream = new TwitterStreaming(App.AppState.Accounts[this.TwitterAccountID].Tokens, App.AppState.Accounts[this.TwitterAccountID].Settings.TwitterAccountName, this.TwitterAccountID);
                Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.StreamingEvent), new Action<GenericMessage<object>>(this.EventReceived));
                if (App.AppState.Accounts[this.TwitterAccountID].Settings.Columns.Count > 0)
                {
                    foreach (MetroTwitColumn column in (IEnumerable<MetroTwitColumn>)Enumerable.OrderBy<MetroTwitColumn, int>((IEnumerable<MetroTwitColumn>)App.AppState.Accounts[this.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, int>)(x => x.Index)))
                    {
                        TweetListType columnType = column.ColumnType;
                        string str = string.Empty;
                        string searchName = !string.IsNullOrEmpty(column.ColumnValue) || columnType != TweetListType.List && columnType != TweetListType.Search && columnType != TweetListType.UserTimeline ? column.ColumnValue : column.ColumnHeader;
                        Guid id = column.ID;
                        TweetListViewModel newcolumn = new TweetListViewModel(column.ID != Guid.Empty ? column.ID : Guid.Empty, columnType, this.TwitterAccountID, App.AppState.Accounts[this.TwitterAccountID].Settings.Columns[column.ID], column.ColumnHeader, !string.IsNullOrEmpty(column.ColumnValue) ? column.ColumnValue : column.ColumnHeader, searchName, column.SearchID, false, (MetroTwitStatusBase)null, column.ColumnLastUpdateID);
                        if (column.NestID > 0)
                            newcolumn.NestColumnID = column.NestID;
                        if (column.ColumnPinned)
                            this.PinColumn(column, newcolumn);
                        if (columnType == TweetListType.MentionsMyTweetsRetweeted)
                            this.MentionsVM = newcolumn;
                        else if (columnType == TweetListType.DirectMessages)
                            this.DirectMessagesVM = newcolumn;
                        else if (columnType == TweetListType.FriendsTimeline)
                            this.FriendsVM = newcolumn;
                    }
                    SettingsData.Instance.FirstLoad = true;
                }
                else
                    this.EnsureMentionsAndDirectMessagesAndFriendsVMs(true);
                this.EnsureMentionsAndDirectMessagesAndFriendsVMs(false);
                this.StartNestTimer();
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)-1), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ColumnsToShow));
                this.SignedInEnabled = true;
                App.RefreshSavedSearches(this.TwitterAccountID, (Action)null);
            }
            this.ForceReload = false;
        }

        private void PinColumn(MetroTwitColumn column, TweetListViewModel newcolumn)
        {
            this.ColumnsToShow.Add(newcolumn);
            if (column.Index != this.ColumnsToShow.Count - 1)
                column.Index = this.ColumnsToShow.Count - 1;
            this.SetAddButtonsEnabled(column.ColumnType, false);
        }

        private void EnsureMentionsAndDirectMessagesAndFriendsVMs(bool addToColumns)
        {
            if (this.FriendsVM == null)
            {
                IEnumerable<MetroTwitColumn> source = Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>)App.AppState.Accounts[this.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, bool>)(x => x.ColumnType == TweetListType.FriendsTimeline));
                MetroTwitColumn metroTwitColumn = Enumerable.Count<MetroTwitColumn>(source) > 0 ? Enumerable.FirstOrDefault<MetroTwitColumn>(source) : new MetroTwitColumn();
                TweetListViewModel newcolumn = new TweetListViewModel(Guid.Empty, TweetListType.FriendsTimeline, this.TwitterAccountID, metroTwitColumn, (string)null, (string)null, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
                this.FriendsVM = newcolumn;
                newcolumn.SoundNotification = false;
                newcolumn.TaskbarNotification = false;
                newcolumn.ToastNotification = false;
                newcolumn.ChangetoSavedColumn();
                if (addToColumns)
                    this.PinColumn(metroTwitColumn, newcolumn);
                else
                    App.AppState.Accounts[this.TwitterAccountID].Settings.Columns[newcolumn.UniqueTweetListID].ColumnPinned = false;
            }
            if (this.MentionsVM == null)
            {
                IEnumerable<MetroTwitColumn> source = Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>)App.AppState.Accounts[this.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, bool>)(x => x.ColumnType == TweetListType.MentionsMyTweetsRetweeted));
                MetroTwitColumn metroTwitColumn = Enumerable.Count<MetroTwitColumn>(source) > 0 ? Enumerable.FirstOrDefault<MetroTwitColumn>(source) : new MetroTwitColumn();
                TweetListViewModel newcolumn = new TweetListViewModel(Guid.Empty, TweetListType.MentionsMyTweetsRetweeted, this.TwitterAccountID, metroTwitColumn, (string)null, (string)null, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
                this.MentionsVM = newcolumn;
                newcolumn.ChangetoSavedColumn();
                if (addToColumns)
                    this.PinColumn(metroTwitColumn, newcolumn);
                else
                    App.AppState.Accounts[this.TwitterAccountID].Settings.Columns[newcolumn.UniqueTweetListID].ColumnPinned = false;
            }
            if (this.DirectMessagesVM != null)
                return;
            IEnumerable<MetroTwitColumn> source1 = Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>)App.AppState.Accounts[this.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, bool>)(x => x.ColumnType == TweetListType.DirectMessages));
            TweetListViewModel tweetListViewModel = new TweetListViewModel(Guid.Empty, TweetListType.DirectMessages, this.TwitterAccountID, Enumerable.Count<MetroTwitColumn>(source1) > 0 ? Enumerable.FirstOrDefault<MetroTwitColumn>(source1) : new MetroTwitColumn(), (string)null, (string)null, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
            this.DirectMessagesVM = tweetListViewModel;
            tweetListViewModel.ChangetoSavedColumn();
            App.AppState.Accounts[this.TwitterAccountID].Settings.Columns[tweetListViewModel.UniqueTweetListID].ColumnPinned = false;
        }



        private void SetAddButtonsEnabled(TweetListType columnType, bool Enable)
        {
            switch (columnType)
            {
                case TweetListType.FriendsTimeline:
                    this.AddFriendsEnabled = Enable;
                    break;
                case TweetListType.DirectMessages:
                    this.AddDirectEnabled = Enable;
                    break;
                case TweetListType.MentionsMyTweetsRetweeted:
                    this.AddRepliesEnabled = Enable;
                    break;
                case TweetListType.MyTweets:
                    this.AddMyTweetsEnabled = Enable;
                    break;
                case TweetListType.Favourites:
                    this.AddFavouriteEnabled = Enable;
                    break;
            }
        }

        private void ResetAddButtonsEnabled()
        {
            this.AddFriendsEnabled = true;
            this.AddRepliesEnabled = true;
            this.AddDirectEnabled = true;
            this.AddPublicEnabled = true;
            this.AddFavouriteEnabled = true;
            this.AddMyTweetsEnabled = true;
            this.addMyReTweetsEnabled = true;
        }

        private void TryAddExistingView(ViewType viewType, UserControl existingView)
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetNewTweetEntryOptionsContainer));
            PopupService.ShowView(existingView);
        }

        private void TryAddView(ViewType viewType, ViewModelType viewModelType, TweetListType columnType, string searchTerm, string searchName, Decimal? existingSearchID = null, MetroTwitStatusBase Tweet = null, TwitterList ListItem = null)
        {
            if (viewType == ViewType.Column)
            {
                this.TryAddColumn(false, (TweetListViewModel)null, columnType, searchTerm, searchName, existingSearchID);
            }
            else
            {
                if (viewType != ViewType.Popup)
                    return;
                if (InlinePopup.CurrentInline != null)
                    InlinePopup.CurrentInline.Close();
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetNewTweetEntryOptionsContainer));
                UserControl view = (UserControl)null;
                switch (viewModelType)
                {
                    case ViewModelType.TweetList:
                        if (columnType == TweetListType.Conversation)
                        {
                            string replyToScreenName = Tweet.InReplyToScreenName;
                            TweetListViewModel tweetListViewModel = new TweetListViewModel(Guid.Empty, columnType, this.TwitterAccountID, new MetroTwitColumn(), string.Empty, replyToScreenName, string.Empty, new Decimal?(), false, Tweet, Tweet.ID)
                            {
                                EnableNotifications = false,
                                TitleBarVisible = false,
                                SettingsVisible = false
                            };
                            TweetListView tweetListView = new TweetListView();
                            tweetListView.DataContext = (object)tweetListViewModel;
                            view = (UserControl)tweetListView;
                            break;
                        }
                        else
                        {
                            TweetListViewModel tweetListViewModel;
                            if (Tweet != null && Tweet.AdUrls != null && !string.IsNullOrEmpty(Tweet.AdUrls.friendship_url))
                                tweetListViewModel = new TweetListViewModel(Guid.Empty, columnType, this.TwitterAccountID, new MetroTwitColumn(), string.Empty, searchTerm, searchName, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0))
                                {
                                    EnableNotifications = false,
                                    AdFollowUrl = Tweet.AdUrls.friendship_url,
                                    TitleBarVisible = false,
                                    SettingsVisible = false
                                };
                            else
                                tweetListViewModel = new TweetListViewModel(Guid.Empty, columnType, this.TwitterAccountID, new MetroTwitColumn(), string.Empty, searchTerm, searchName, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0))
                                {
                                    EnableNotifications = false,
                                    TitleBarVisible = false,
                                    SettingsVisible = false
                                };
                            tweetListViewModel.AllowPin = columnType != TweetListType.RetweetUsers;
                            ProfileView profileView = new ProfileView();
                            profileView.DataContext = (object)tweetListViewModel;
                            view = (UserControl)profileView;
                            break;
                        }
                    case ViewModelType.Lists:
                        ListsView listsView = new ListsView();
                        listsView.DataContext = (object)new ListsViewModel(this.TwitterAccountID)
                        {
                            PopupTitle = "lists",
                            AllowPin = false
                        };
                        view = (UserControl)listsView;
                        break;
                    case ViewModelType.Trends:
                        TrendsView trendsView1 = new TrendsView();
                        TrendsView trendsView2 = trendsView1;
                        TrendsViewModel trendsViewModel1 = new TrendsViewModel();
                        trendsViewModel1.TwitterAccountID = this.TwitterAccountID;
                        trendsViewModel1.AllowPin = false;
                        trendsViewModel1.PopupTitle = "twitter trends";
                        TrendsViewModel trendsViewModel2 = trendsViewModel1;
                        trendsView2.DataContext = (object)trendsViewModel2;
                        view = (UserControl)trendsView1;
                        break;
                    case ViewModelType.ManageList:
                        ManageTwitterListView manageTwitterListView = new ManageTwitterListView();
                        manageTwitterListView.DataContext = (object)new ManageTwitterListViewModel(ListItem != null ? ListItem : (TwitterList)null, this.TwitterAccountID)
                        {
                            AllowPin = false,
                            PopupTitle = (ListItem != null ? string.Format("Edit {0}", (object)ListItem.Name) : "create new list")
                        };
                        view = (UserControl)manageTwitterListView;
                        break;
                }
                PopupService.ShowView(view);
            }
        }

        private void TryAddColumn(bool existing, TweetListViewModel existingColumnModel, TweetListType columnType, string searchTerm, string searchName, Decimal? existingSearchID)
        {
            bool flag = false;
            TweetListViewModel tweetListViewModel1 = existingColumnModel;
            foreach (TweetListViewModel tweetListViewModel2 in (Collection<TweetListViewModel>)this.ColumnsToShow)
            {
                if (!existing)
                    tweetListViewModel1 = new TweetListViewModel(Guid.Empty, columnType, this.TwitterAccountID, new MetroTwitColumn(), string.Empty, searchTerm, searchName, existingSearchID, true, (MetroTwitStatusBase)null, new Decimal(0));
                if (tweetListViewModel2.TweetListName == tweetListViewModel1.TweetListName && tweetListViewModel2.TweetType == tweetListViewModel1.TweetType)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                TweetListViewModel tweetListViewModel2 = existingColumnModel;
                if (!existing)
                    tweetListViewModel2 = columnType != TweetListType.DirectMessages ? (columnType != TweetListType.MentionsMyTweetsRetweeted ? (columnType != TweetListType.FriendsTimeline ? new TweetListViewModel(Guid.Empty, columnType, this.TwitterAccountID, new MetroTwitColumn(), string.Empty, searchTerm, searchName, existingSearchID, false, (MetroTwitStatusBase)null, new Decimal(0)) : App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.FriendsVM) : App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.MentionsVM) : App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.DirectMessagesVM;
                if (tweetListViewModel2.TweetType != TweetListType.DirectMessages && tweetListViewModel2.TweetType != TweetListType.MentionsMyTweetsRetweeted)
                {
                    tweetListViewModel2.ToastNotification = false;
                    tweetListViewModel2.TaskbarNotification = false;
                    tweetListViewModel2.SoundNotification = false;
                }
                this.ColumnsToShow.Add(tweetListViewModel2);
                tweetListViewModel2.ChangetoSavedColumn();
                if (App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns)
                    NestService.ColumnsSync(new NestSync()
                    {
                        Add = {
              (object) new NestColumn()
              {
                Name = tweetListViewModel2.TweetListName,
                Type = (int) tweetListViewModel2.TweetType,
                Value = tweetListViewModel2.TwitterTerm,
                Date = DateTime.UtcNow.ToUniversalTime(),
                LastReadId = 0
              }
            }
                    }, (Action<NestSync>)(r => this.NestUpdate(r)), this.TwitterAccountID);
                this.SetAddButtonsEnabled(tweetListViewModel2.TweetType, false);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)-1), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ColumnsToShow));
            }
            else
                this.UnableToAddColumn();
        }

        private void NestUpdate(NestSync sync)
        {
            if (!App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns || sync == null)
                return;
            if (sync.Add != null && sync.Add.Count > 0)
            {
                foreach (NestColumn nestColumn in (IEnumerable)sync.Add)
                {
                    NestColumn column = nestColumn;
                    IEnumerable<TweetListViewModel> source = Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, int, bool>)((x, r) => x.NestColumnID == column.Id || x.TweetListName == column.Name && x.TwitterTerm == (string.IsNullOrEmpty(column.Value) ? string.Empty : column.Value) && x.TweetType == (TweetListType)column.Type));
                    if (Enumerable.Count<TweetListViewModel>(source) == 0)
                    {
                        TweetListViewModel tweetListViewModel = new TweetListViewModel(Guid.Empty, (TweetListType)column.Type, this.TwitterAccountID, new MetroTwitColumn(), column.Name, string.Empty, column.Value, new Decimal?(), false, (MetroTwitStatusBase)null, (Decimal)column.LastReadId);
                        tweetListViewModel.ChangetoSavedColumn();
                        this.ColumnsToShow.Add(tweetListViewModel);
                        this.SetAddButtonsEnabled(tweetListViewModel.TweetType, false);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)-1), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ColumnsToShow))), new object[0]);
                    }
                    else
                        Enumerable.First<TweetListViewModel>(source).NestColumnID = column.Id;
                }
            }
            if (sync.Edit != null && sync.Edit.Count > 0)
            {
                foreach (NestColumn nestColumn in (IEnumerable)sync.Edit)
                {
                    NestColumn column = nestColumn;
                    IEnumerable<TweetListViewModel> source = Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, int, bool>)((x, r) => x.NestColumnID == column.Id));
                    if (Enumerable.Count<TweetListViewModel>(source) > 0)
                    {
                        foreach (TweetListViewModel tweetListViewModel in source)
                        {
                            TweetListViewModel MTColumn = tweetListViewModel;
                            if (!string.IsNullOrEmpty(column.Name))
                                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => MTColumn.TweetListName = column.Name), DispatcherPriority.ContextIdle, new object[0]);
                            if (!string.IsNullOrEmpty(column.Value))
                                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => MTColumn.TwitterTerm = column.Value), DispatcherPriority.ContextIdle, new object[0]);
                            if (column.LastReadId > 0L && (Decimal)column.LastReadId > MTColumn.CurrentTweetID)
                                Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    MTColumn.CurrentTweetID = (Decimal)column.LastReadId;
                                    MTColumn.UpdateReadTweets(new Decimal?());
                                }), DispatcherPriority.ContextIdle, new object[0]);
                        }
                    }
                    else
                    {
                        IEnumerable<TweetListViewModel> possiblecolumns = Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, int, bool>)((x, r) => x.TweetListName == column.Name && x.TwitterTerm == (string.IsNullOrEmpty(column.Value) ? string.Empty : column.Value) && x.TweetType == (TweetListType)column.Type));
                        if (Enumerable.Count<TweetListViewModel>(possiblecolumns) > 0)
                            Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => Enumerable.First<TweetListViewModel>(possiblecolumns).NestColumnID = column.Id), DispatcherPriority.ContextIdle, new object[0]);
                    }
                }
            }
            if (sync.Remove != null && sync.Remove.Count > 0)
            {
                foreach (NestColumn nestColumn in (IEnumerable)sync.Remove)
                {
                    NestColumn column = nestColumn;
                    foreach (TweetListViewModel tweetListViewModel in Enumerable.ToArray<TweetListViewModel>(Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, int, bool>)((x, r) => x.NestColumnID == column.Id))))
                    {
                        TweetListViewModel MTColumn = tweetListViewModel;
                        Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => this.RemoveColumn(new GenericMessage<TweetListViewModel>(MTColumn), false)), DispatcherPriority.ContextIdle, new object[0]);
                    }
                }
            }
            if (sync.Add.Count == 0 && sync.Edit.Count == 0 && sync.Edit.Count == 0 && Enumerable.Count<TweetListViewModel>(Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, bool>)(x => x.NestColumnID == 0))) == this.ColumnsToShow.Count)
            {
                NestSync NestSync = new NestSync();
                NestSync.Add = (IList)new ArrayList();
                foreach (TweetListViewModel tweetListViewModel in (Collection<TweetListViewModel>)this.ColumnsToShow)
                    NestSync.Add.Add((object)new NestColumn()
                    {
                        Name = tweetListViewModel.TweetListName,
                        Value = tweetListViewModel.TwitterTerm,
                        Type = (int)tweetListViewModel.TweetType,
                        LastReadId = (long)tweetListViewModel.CurrentTweetID,
                        Date = DateTime.UtcNow.ToUniversalTime()
                    });
                NestService.ColumnsSync(NestSync, (Action<NestSync>)(r => this.NestUpdate(r)), this.TwitterAccountID);
            }
            else
            {
                App.AppState.Accounts[this.TwitterAccountID].Settings.SyncLastUpdate = new DateTime?(DateTime.UtcNow.ToUniversalTime());
                foreach (TweetListViewModel tweetListViewModel in Enumerable.Where<TweetListViewModel>((IEnumerable<TweetListViewModel>)this.ColumnsToShow, (Func<TweetListViewModel, bool>)(x => x.NestColumnID == 0)))
                {
                    TweetListViewModel overriddencolumn = tweetListViewModel;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => this.RemoveColumn(new GenericMessage<TweetListViewModel>(overriddencolumn), false)), DispatcherPriority.ContextIdle, new object[0]);
                }
            }
        }

        private void DMCheckUserFollow(string screenName)
        {
            Friendship.ShowAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, screenName, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<TwitterRelationship>>>)(r =>
            {
                if (r.Result.Result == RequestResult.Success)
                {
                    this.StatusType = Convert.ToInt32(r.Result.ResponseObject.Relationship.Source.CanDM);
                    this.PostTweetEnabled = Convert.ToBoolean(this.StatusType);
                    this.StatusText = !Convert.ToBoolean(this.StatusType) ? "Cannot direct message, " + screenName + " is not following you" : "Direct message to " + screenName;
                }
                else
                {
                    if (r.Result.Result != RequestResult.FileNotFound)
                        return;
                    this.PostTweetEnabled = false;
                    this.StatusText = "Cannot direct message, user does not exist";
                }
            }));
            this.StatusText = "Verifying direct message to " + screenName;
            this.StatusLabelVisibility = Visibility.Visible;
        }

        public byte[] BufferFromImage(BitmapImage imageSource)
        {
            JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
            jpegBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
            byte[] numArray;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                jpegBitmapEncoder.Save((System.IO.Stream)memoryStream);
                numArray = memoryStream.ToArray();
            }
            return numArray;
        }

        private void UpdateTweetTextCounter()
        {
            new Task((Action)(() => this.TweetTextCounter())).Start();
        }

        private void Lists()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"L"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            this.TryAddView(ViewType.Popup, ViewModelType.Lists, TweetListType.List, string.Empty, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null);
        }

        private void Trends()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"T"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            this.TryAddView(ViewType.Popup, ViewModelType.Trends, TweetListType.List, string.Empty, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null);
        }

        private void Refresh()
        {
            if (this.UserStream == null)
                return;
            bool flag = this.UserStream.Stopped;
            foreach (TweetListViewModel tweetListViewModel in (Collection<TweetListViewModel>)this.ColumnsToShow)
                tweetListViewModel.RefreshCommand.Execute((object)flag);
        }

        private void Flush()
        {
            foreach (TweetListViewModel tweetListViewModel in (Collection<TweetListViewModel>)this.ColumnsToShow)
                tweetListViewModel.FlushCommand.Execute((object)false);
        }

        private void FlushandRefresh()
        {
            this.Flush();
            this.Refresh();
            if (this.UserStream == null || (!this.UserStream.Stopped || this.UserStream.retrycount.HasValue))
                return;
            this.UserStream.StartStream();
        }

        private async void PostTweet()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)false), (object)ViewModelMessages.EnableReplyOptions);
            this.sendingTweetInProgress = true;
            this.PostTweetEnabled = false;
            this.TweetEntryEnabled = false;
            this.ImageUploadButtonIsEnabled = false;
            this.PostTweetProgressVisibility = Visibility.Visible;
            bool error = false;
            int numberOfStatus = 0;
            if ((this.CharCount <= SettingsData.Instance.TweetCharLimit || Enumerable.Count<ITweetService>(Enumerable.Where<ITweetService>((IEnumerable<ITweetService>)CoreServices.Instance.TweetServices, (Func<ITweetService, bool>)(x => x.OverrideTweetCharLimit))) > 0) && this.CharCount > 0)
            {
                try
                {
                    this.TweetTextCounter();
                    if (this.CurrentlySelectedImage != null && !this.TwitterImageServiceIsEnabled)
                    {
                        this.uploadImageCountDownEvent = new CountdownEvent(1);
                        this.ProcessImageUpload(this.CurrentlySelectedImage.FilePath, (Action<string>)(s =>
                        {
                            TwitViewModel temp_978 = this;
                            string temp_982 = temp_978.ActualTweetText + " " + s;
                            temp_978.ActualTweetText = temp_982;
                            this.TweetTextCounter();
                        }));
                        await Task.Run((Action)(() => this.uploadImageCountDownEvent.Wait()));
                    }
                    //do
                    //  ;
                    //while (this.PostTweetEnabledCounter > 0 && !this.TwitterImageServiceIsEnabled);
                    if (this.imageUploadsFailed)
                    {
                        await System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            SimpleErrorPrompt local_0 = new SimpleErrorPrompt()
                            {
                                DataContext = (object)new
                                {
                                    ErrorHeading = "image upload failed",
                                    ErrorText = "Image failed to upload, please try again."
                                }
                            };
                            Messenger.Default.Send<PromptMessage>(new PromptMessage()
                            {
                                IsModal = true,
                                PromptView = (FrameworkElement)local_0,
                                IsCentered = false
                            }, (object)ViewModelMessages.ShowSlidePrompt);
                        }), new object[0]);
                        error = true;
                        this.sendingTweetInProgress = false;
                        this.PostTweetEnabled = true;
                        this.TweetEntryEnabled = true;
                        this.ImageUploadButtonIsEnabled = true;
                        this.PostTweetProgressVisibility = Visibility.Collapsed;
                        return;
                    }
                    else if (this.CharCount > SettingsData.Instance.TweetCharLimit && Enumerable.Count<ITweetService>(Enumerable.Where<ITweetService>((IEnumerable<ITweetService>)CoreServices.Instance.TweetServices, (Func<ITweetService, bool>)(x => x.OverrideTweetCharLimit))) == 0)
                    {
                        await System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.TweetLength)), new object[0]);
                        error = true;
                        return;
                    }
                    else
                    {
                        TwitterResponse<Status> updateResponse = (TwitterResponse<Status>)null;
                        TwitterResponse<TwitterDirectMessage> directMessageResponse = (TwitterResponse<TwitterDirectMessage>)null;
                        Match dmMatch = (Match)null;
                        string userName = string.Empty;
                        string tweetpoststring = this.ActualTweetText;
                        if (this.newTweetType == TwitViewModel.NewTweetType.DirectMessage)
                        {
                            dmMatch = TwitViewModel.DM_EXPRESSION.Match(tweetpoststring);
                            userName = dmMatch.Groups[1].Value;
                            tweetpoststring = tweetpoststring.Replace(dmMatch.Groups[0].Value, "").Trim();
                        }
                        else
                        {
                            foreach (ITweetService tweetService in CoreServices.Instance.TweetServices)
                            {
                                if (tweetService.WantsPreCreation)
                                {
                                    PreTweetCreationResponse creationResponse = tweetService.PreTweetCreation(tweetpoststring, this.CharCount, this.ReplyToID.ToString(), this.UserName, this.newTweetType == TwitViewModel.NewTweetType.DirectMessage, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName);
                                    if (string.IsNullOrEmpty(creationResponse.Error) && !string.IsNullOrEmpty(creationResponse.TweetContent))
                                        tweetpoststring = creationResponse.TweetContent;
                                }
                            }
                        }
                        if (this.newTweetType == TwitViewModel.NewTweetType.DirectMessage)
                        {
                            if (dmMatch.Success && userName.Length > 0)
                            {
                                directMessageResponse = await DirectMessages.SendAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, userName, tweetpoststring, (OptionalProperties)null);
                                if (directMessageResponse != null && directMessageResponse.Result == RequestResult.Success)
                                {
                                    TwitterDirectMessage responseObject = directMessageResponse.ResponseObject;
                                    this.lastTweet = new UndoTweetState()
                                    {
                                        Id = responseObject.Id,
                                        LastTweetText = this.ActualTweetText,
                                        TweetType = TwitViewModel.NewTweetType.DirectMessage,
                                        UserName = this.UserName
                                    };
                                    this.CurrentTweetDocument = (FlowDocument)null;
                                    if (responseObject != null)
                                    {
                                        numberOfStatus = responseObject.Sender.NumberOfStatuses;
                                        App.AppState.Accounts[this.TwitterAccountID].Settings.TwitterUserImage = responseObject.Sender.ProfileImageSecureLocation;
                                    }
                                }
                                else
                                {
                                    this.StatusType = -2;
                                    this.StatusText = Enumerable.First<TwitterError>(directMessageResponse.Errors).Message;
                                }
                                this.StatusLabelVisibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            switch (this.newTweetType)
                            {
                                case TwitViewModel.NewTweetType.Reply:
                                    StatusUpdateOptions statusUpdateOptons = MetroTwitTwitterizer.StatusUpdateOptions;
                                    statusUpdateOptons.InReplyToStatusId = this.ReplyToID;
                                    if (this.CurrentlySelectedImage != null && this.TwitterImageServiceIsEnabled)
                                    {
                                        updateResponse = await Tweets.UpdateWithMediaAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, this.BufferFromImage(this.CurrentlySelectedImage.Image), statusUpdateOptons);
                                        this.DecrementPostTweetCounter();
                                        break;
                                    }
                                    else
                                    {
                                        updateResponse = await Tweets.UpdateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, statusUpdateOptons);
                                        break;
                                    }
                                case TwitViewModel.NewTweetType.Retweet:
                                    if (this.CurrentlySelectedImage != null && this.TwitterImageServiceIsEnabled)
                                    {
                                        updateResponse = await Tweets.UpdateWithMediaAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, this.BufferFromImage(this.CurrentlySelectedImage.Image), MetroTwitTwitterizer.StatusUpdateOptions);
                                        this.DecrementPostTweetCounter();
                                        break;
                                    }
                                    else
                                    {
                                        updateResponse = await Tweets.UpdateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, MetroTwitTwitterizer.StatusUpdateOptions);
                                        break;
                                    }
                                default:
                                    if (this.CurrentlySelectedImage != null && this.TwitterImageServiceIsEnabled)
                                    {
                                        updateResponse = await Tweets.UpdateWithMediaAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, this.BufferFromImage(this.CurrentlySelectedImage.Image), MetroTwitTwitterizer.StatusUpdateOptions);
                                        this.DecrementPostTweetCounter();
                                        break;
                                    }
                                    else
                                    {
                                        updateResponse = await Tweets.UpdateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, tweetpoststring, MetroTwitTwitterizer.StatusUpdateOptions);
                                        break;
                                    }
                            }
                            if (updateResponse != null && updateResponse.Result == RequestResult.Success)
                            {
                                Status responseObject = updateResponse.ResponseObject;
                                this.lastTweet = new UndoTweetState()
                                {
                                    Id = responseObject.Id,
                                    LastTweetText = this.ActualTweetText,
                                    TweetType = this.newTweetType,
                                    UserName = this.UserName,
                                    ReplyToID = this.ReplyToID
                                };
                                this.CurrentTweetDocument = (FlowDocument)null;
                                if (responseObject != null)
                                {
                                    numberOfStatus = responseObject.User.NumberOfStatuses;
                                    App.AppState.Accounts[this.TwitterAccountID].Settings.TwitterUserImage = responseObject.User.ProfileImageSecureLocation;
                                    foreach (ITweetService tweetService in CoreServices.Instance.TweetServices)
                                    {
                                        if (tweetService.WantsPostCreation)
                                            tweetService.PostTweetCreation(responseObject.Text, this.CharCount, responseObject.InReplyToStatusId.ToString(), responseObject.InReplyToScreenName, false, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, responseObject.Id);
                                    }

                                }
                            }
                            else
                            {
                                this.StatusType = -2;
                                this.StatusText = Enumerable.First<TwitterError>(updateResponse.Errors).Message;
                                if (this.StatusText == "Status is a duplicate.")
                                    this.StatusText = "Tweet error: tweet is a duplicate of a last tweet posted.";
                                this.StatusLabelVisibility = Visibility.Visible;
                            }
                        }
                    }
                }
                catch
                {
                    error = true;
                }
            }
            else
                error = true;
            if (!this.imageUploadsFailed && !error)
            {
                base.RaisePropertyChanged("SingleImageTypeEnabled");
                this.CurrentlySelectedImage = (SingleImageUploadRequest)null;
                this.EnableTweetArea();
                if (numberOfStatus > 0)
                {
                    this.PostTweetEnabled = false;
                    this.ResetNewTweet(true);
                    if (App.AppState.Accounts[this.TwitterAccountID].TwitterUserTweetCount != numberOfStatus.ToString() + " tweets")
                    {
                        App.AppState.Accounts[this.TwitterAccountID].TwitterUserTweetCount = numberOfStatus.ToString() + " tweets";
                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.ProfileUpdated));
                    }
                }
            }
            else
                this.EnableTweetArea();
        }

        private void EnableTweetArea()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)true), (object)ViewModelMessages.EnableReplyOptions);
            this.sendingTweetInProgress = false;
            this.PostTweetProgressVisibility = Visibility.Collapsed;
            this.TweetEntryEnabled = true;
            this.PostTweetEnabled = true;
            this.ImageUploadButtonIsEnabled = true;
            this.imageUploadsFailed = false;
        }

        public void ResetNewTweet(bool bNotify = true)
        {
            this.imageUploadsFailed = false;
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)true), (object)ViewModelMessages.EnableReplyOptions);
            this.StatusLabelVisibility = Visibility.Collapsed;
            this.StatusText = "";
            this.ReplyToID = new Decimal(0);
            this.UserName = "";
            if (bNotify)
                this.ActualTweetText = "";
            this.newTweetType = TwitViewModel.NewTweetType.Normal;
            this.StatusType = -1;
            this.Status = (MetroTwitStatusBase)null;
            if (!bNotify)
                return;
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)string.Empty), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.NewTweetEditUpdateText));
        }

        public void Search(ViewType viewType)
        {
            this.currentSearchCriteriaView = new SearchCriteriaView();
            if (viewType == ViewType.Column)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"A"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            else if (viewType == ViewType.Popup)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"S"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            InlinePopup inlinePopup = new InlinePopup();
            inlinePopup.Style = (Style)System.Windows.Application.Current.FindResource((object)"MetroInlinePopup");
            inlinePopup.DataContext = (object)new SearchCriteriaViewModel(viewType, this.TwitterAccountID);
            inlinePopup.Content = (object)this.currentSearchCriteriaView;
            inlinePopup.Closing += new CancelEventHandler(this.searchPopup_Closing);
            inlinePopup.Closed += new EventHandler(this.searchPopup_Closed);
            inlinePopup.ShowAnimated(PlacementMode.Top, (FrameworkElement)SettingsData.Instance.PopupTarget, new Point?());
        }

        private void searchPopup_Closed(object sender, EventArgs e)
        {
            this.currentSearchCriteriaView = (SearchCriteriaView)null;
        }

        private void searchPopup_Closing(object sender, CancelEventArgs e)
        {

        }

        private void SavedSearch(Decimal searchID)
        {
            TwitterSavedSearch twitterSavedSearch = Enumerable.FirstOrDefault<TwitterSavedSearch>((IEnumerable<TwitterSavedSearch>)App.AppState.Accounts[this.TwitterAccountID].SavedSearches, (Func<TwitterSavedSearch, bool>)(s => s.Id == searchID));
            this.TryAddView(ViewType.Column, ViewModelType.TweetList, TweetListType.Search, twitterSavedSearch != null ? twitterSavedSearch.Name : string.Empty, string.Empty, new Decimal?(searchID), (MetroTwitStatusBase)null, (TwitterList)null);
        }

        private void ProfileSearch(ViewType viewType)
        {
            this.currentSearchUserView = new SearchUserView();
            if (viewType == ViewType.Column)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"A"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            else if (viewType == ViewType.Popup)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)"P"), (object)this.MultiAccountifyToken((System.Enum)ViewModelMessages.SetPopupTarget));
            InlinePopup inlinePopup = new InlinePopup();
            inlinePopup.Style = (Style)System.Windows.Application.Current.FindResource((object)"MetroInlinePopup");
            inlinePopup.DataContext = (object)new SearchUserViewModel(viewType, this.TwitterAccountID);
            inlinePopup.Content = (object)this.currentSearchUserView;
            inlinePopup.Closing += new CancelEventHandler(this.searchUserPopup_Closing);
            inlinePopup.Closed += new EventHandler(this.searchUserPopup_Closed);
            IntellisenseExtension.SetTwitterAccountID((TextBoxBase)this.currentSearchUserView.InputEdit, this.TwitterAccountID);
            IntellisenseExtension.SetIsEnabled(this.currentSearchUserView.InputEdit, SettingsData.Instance.UseAutoComplete);
            IntellisenseExtension.SetIgnorePrefix(this.currentSearchUserView.InputEdit, true);
            inlinePopup.ShowAnimated(PlacementMode.Top, (FrameworkElement)SettingsData.Instance.PopupTarget, new Point?());
        }

        private void searchUserPopup_Closed(object sender, EventArgs e)
        {
            this.currentSearchUserView = (SearchUserView)null;
        }

        private void searchUserPopup_Closing(object sender, CancelEventArgs e)
        {
            if (this.currentSearchUserView == null)
                return;
            IntellisenseExtension.SetIsEnabled(this.currentSearchUserView.InputEdit, false);
        }

        private void AddColumn(string columnType)
        {
            this.TryAddView(ViewType.Column, ViewModelType.TweetList, (TweetListType)int.Parse(columnType), string.Empty, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null);
        }

        private void ShowList(string ColumnType)
        {
            this.TryAddView(ViewType.Popup, ViewModelType.TweetList, TweetListType.List, ColumnType, string.Empty, new Decimal?(), (MetroTwitStatusBase)null, (TwitterList)null);
        }

        private void AddSingleImage()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "GIF, JPG, JPEG, PNG | *.jpg; *jpeg; *png; *.gif";
            OpenFileDialog openFileDialog2 = openFileDialog1;
            if (!openFileDialog2.ShowDialog().GetValueOrDefault(false))
                return;
            this.GenerateAndAddSingleImage(openFileDialog2.FileName);
        }

        public void GenerateAndAddSingleImage(string fileName)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(fileName);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            this.CurrentlySelectedImage = new SingleImageUploadRequest()
            {
                Image = bitmapImage,
                FilePath = fileName
            };
            this.IncrementPostTweetCounter();
        }

        private void RemoveSingleImage()
        {
            this.CurrentlySelectedImage = (SingleImageUploadRequest)null;
            this.DecrementPostTweetCounter();
        }

        private void UnableToAddColumn()
        {
            Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.ColumnExists);
        }

        private void ProcessImageUpload(string imagePath, Action<string> imageUploaded)
        {
            CoreServices.Instance.CurrentImageUploadingService.UploadImage(new ImageUploadRequest()
            {
                AccessToken = App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthToken,
                AccessTokenSecret = App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthSecret,
                ConsumerKey = SettingsData.Instance.TwitterConsumerKey,
                ConsumerSecret = SettingsData.Instance.TwitterConsumerSecret,
                FilePath = imagePath,
                CompletedCallback = (Action<string>)(url => System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    imageUploaded(url);
                    this.DecrementPostTweetCounter();
                    this.uploadImageCountDownEvent.Signal();
                }), new object[0])),
                ErrorCallback = (Action<string>)(errorMesage =>
                {
                    this.imageUploadsFailed = true;
                    this.uploadImageCountDownEvent.Signal();
                    this.DecrementPostTweetCounter();
                })
            });
        }

        public enum NewTweetType
        {
            Normal,
            Reply,
            Retweet,
            DirectMessage,
        }
    }
}
