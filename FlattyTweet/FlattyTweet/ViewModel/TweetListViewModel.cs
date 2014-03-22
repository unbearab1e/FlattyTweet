
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;
using Twitterizer.Streaming;

namespace FlattyTweet.ViewModel
{
    public class TweetListViewModel : MultiAccountViewModelBase, IPopupViewModel
    {
        private static string[] loadingstrings = new string[8]
    {
      "Loading tweets...",
      "Fetching tweets...",
      "Downloading tweets...",
      "Getting tweets...",
      "Talking with Twitter...",
      "Making sense of Twitter...",
      "Organizing a date with Twitter...",
      "Secretly plotting world domination..."
    };
        private bool ForeverScrolling = false;
        private bool StreamPreviouslyDown = false;
        private object _tweetsLock = new object();
        private List<MetroTwitStatusBase> backlog;
        private MetroTwitUser user;
        private MetroTwitColumn Settings;
        private string userimageUrl;
        private bool searchColumnIsInSaveMode;
        private bool InitialForeverScroll;
        private bool errorStateIntialLoad;
        private object hideLoadLock;
        private int taskCount;

        private bool enableNotification;
        public bool EnableNotifications
        {
            get
            {
                return this.enableNotification;
            }
            set
            {
                if (this.enableNotification == value)
                    return;
                this.enableNotification = value;
                base.RaisePropertyChanged("EnableNotifications");
            }
        }
        private RefreshTypes lastCollectionState;
        public RefreshTypes LastCollectionState
        {
            get
            {
                return this.lastCollectionState;
            }
            set
            {
                if (this.lastCollectionState == value)
                    return;
                this.lastCollectionState = value;
                base.RaisePropertyChanged("LastCollectionState");
            }
        }
        private bool noTweets;
        public bool NoTweets
        {
            get
            {
                return this.noTweets;
            }
            set
            {
                if (this.noTweets == value)
                    return;
                this.noTweets = value;
                base.RaisePropertyChanged("NoTweets");
            }
        }
        private bool scrollNearTop;
        public bool ScrollNearTop
        {
            get
            {
                return this.scrollNearTop;
            }
            set
            {
                if (this.scrollNearTop == value)
                    return;
                this.scrollNearTop = value;
                base.RaisePropertyChanged("ScrollNearTop");
            }
        }
        private string adFollowUrl;
        public string AdFollowUrl
        {
            get
            {
                return this.adFollowUrl;
            }
            set
            {
                if (string.Equals(this.adFollowUrl, value, StringComparison.Ordinal))
                    return;
                this.adFollowUrl = value;
                base.RaisePropertyChanged("AdFollowUrl");
            }
        }
        private MetroTwitStatusBase ad;
        public MetroTwitStatusBase Ad
        {
            get
            {
                return this.ad;
            }
            set
            {
                if (this.ad == value)
                    return;
                this.ad = value;
                base.RaisePropertyChanged("Ad");
            }
        }
        private MetroTwitStatusBase userRenderedDescription;
        public MetroTwitStatusBase UserRenderedDescription
        {
            get
            {
                return this.userRenderedDescription;
            }
            set
            {
                if (this.userRenderedDescription == value)
                    return;
                this.userRenderedDescription = value;
                base.RaisePropertyChanged("UserRenderedDescription");
            }
        }
        private bool showContentVisibility;
        public bool ShowContentVisibility
        {
            get
            {
                return this.showContentVisibility;
            }
            set
            {
                if (this.showContentVisibility == value)
                    return;
                this.showContentVisibility = value;
                base.RaisePropertyChanged("ShowContentVisibility");
            }
        }
        private bool showProfileLoading;
        public bool ShowProfileLoading
        {
            get
            {
                return this.showProfileLoading;
            }
            set
            {
                if (this.showProfileLoading == value)
                    return;
                this.showProfileLoading = value;
                base.RaisePropertyChanged("ShowProfileLoading");
            }
        }
        private bool showLoading;
        public bool ShowLoading
        {
            get
            {
                return this.showLoading;
            }
            set
            {
                if (this.showLoading == value)
                    return;
                this.showLoading = value;
                base.RaisePropertyChanged("ShowLoading");
            }
        }
        private bool showingAd;
        public bool ShowingAd
        {
            get
            {
                return this.showingAd;
            }
            set
            {
                if (this.showingAd == value)
                    return;
                this.showingAd = value;
                base.RaisePropertyChanged("ShowingAd");
            }
        }
        private SortableObservableCollection<MetroTwitStatusBase> tweets;
        public SortableObservableCollection<MetroTwitStatusBase> Tweets
        {
            get
            {
                return this.tweets;
            }
            set
            {
                if (this.tweets == value)
                    return;
                this.tweets = value;
                base.RaisePropertyChanged("Tweets");
            }
        }
        private TweetListViewModel followers;
        public TweetListViewModel Followers
        {
            get
            {
                return this.followers;
            }
            set
            {
                if (this.followers == value)
                    return;
                this.followers = value;
                base.RaisePropertyChanged("Followers");
            }
        }
        private TweetListViewModel following;
        public TweetListViewModel Following
        {
            get
            {
                return this.following;
            }
            set
            {
                if (this.following == value)
                    return;
                this.following = value;
                base.RaisePropertyChanged("Following");
            }
        }
        private TweetListViewModel listedIn;
        public TweetListViewModel ListedIn
        {
            get
            {
                return this.listedIn;
            }
            set
            {
                if (this.listedIn == value)
                    return;
                this.listedIn = value;
                base.RaisePropertyChanged("ListedIn");
            }
        }
        private bool profileDetailsVisible;
        public bool ProfileDetailsVisible
        {
            get
            {
                return this.profileDetailsVisible;
            }
            set
            {
                if (this.profileDetailsVisible == value)
                    return;
                this.profileDetailsVisible = value;
                base.RaisePropertyChanged("ProfileDetailsVisible");
            }
        }
        private bool searchDetailsVisible;
        public bool SearchDetailsVisible
        {
            get
            {
                return this.searchDetailsVisible;
            }
            set
            {
                if (this.searchDetailsVisible == value)
                    return;
                this.searchDetailsVisible = value;
                base.RaisePropertyChanged("SearchDetailsVisible");
            }
        }
        private bool settingsVisible;
        public bool SettingsVisible
        {
            get
            {
                return this.settingsVisible;
            }
            set
            {
                if (this.settingsVisible == value)
                    return;
                this.settingsVisible = value;
                base.RaisePropertyChanged("SettingsVisible");
            }
        }
        private bool columnBackVisible;
        public bool ColumnBackVisible
        {
            get
            {
                return this.columnBackVisible;
            }
            set
            {
                if (this.columnBackVisible == value)
                    return;
                this.columnBackVisible = value;
                base.RaisePropertyChanged("ColumnBackVisible");
            }
        }
        private bool userRetweets;
        public bool UserRetweets
        {
            get
            {
                return this.userRetweets;
            }
            set
            {
                if (this.userRetweets == value)
                    return;
                this.userRetweets = value;
                base.RaisePropertyChanged("UserRetweets");
            }
        }
        private bool nameVisible;
        public bool NameVisible
        {
            get
            {
                return this.nameVisible;
            }
            set
            {
                if (this.nameVisible == value)
                    return;
                this.nameVisible = value;
                base.RaisePropertyChanged("NameVisible");
            }
        }
        private bool webSiteVisible;
        public bool WebSiteVisible
        {
            get
            {
                return this.webSiteVisible;
            }
            set
            {
                if (this.webSiteVisible == value)
                    return;
                this.webSiteVisible = value;
                base.RaisePropertyChanged("WebSiteVisible");
            }
        }
        private string followButtonText;
        public string FollowButtonText
        {
            get
            {
                return this.followButtonText;
            }
            set
            {
                if (string.Equals(this.followButtonText, value, StringComparison.Ordinal))
                    return;
                this.followButtonText = value;
                base.RaisePropertyChanged("FollowButtonText");
            }
        }
        private bool followButtonIsEnabled;
        public bool FollowButtonIsEnabled
        {
            get
            {
                return this.followButtonIsEnabled;
            }
            set
            {
                if (this.followButtonIsEnabled == value)
                    return;
                this.followButtonIsEnabled = value;
                base.RaisePropertyChanged("FollowButtonIsEnabled");
            }
        }
        private string profileListCountText;
        public string ProfileListCountText
        {
            get
            {
                return this.profileListCountText;
            }
            set
            {
                if (string.Equals(this.profileListCountText, value, StringComparison.Ordinal))
                    return;
                this.profileListCountText = value;
                base.RaisePropertyChanged("ProfileListCountText");
            }
        }
        private bool locationVisible;
        public bool LocationVisible
        {
            get
            {
                return this.locationVisible;
            }
            set
            {
                if (this.locationVisible == value)
                    return;
                this.locationVisible = value;
                base.RaisePropertyChanged("LocationVisible");
            }
        }
        private bool bioVisible;
        public bool BioVisible
        {
            get
            {
                return this.bioVisible;
            }
            set
            {
                if (this.bioVisible == value)
                    return;
                this.bioVisible = value;
                base.RaisePropertyChanged("BioVisible");
            }
        }
        private bool followButtonVisible;
        public bool FollowButtonVisible
        {
            get
            {
                return this.followButtonVisible;
            }
            set
            {
                if (this.followButtonVisible == value)
                    return;
                this.followButtonVisible = value;
                base.RaisePropertyChanged("FollowButtonVisible");
            }
        }
        private bool userVisible;
        public bool UserVisible
        {
            get
            {
                return this.userVisible;
            }
            set
            {
                if (this.userVisible == value)
                    return;
                this.userVisible = value;
                base.RaisePropertyChanged("UserVisible");
            }
        }
        private bool verifiedVisible;
        public bool VerifiedVisible
        {
            get
            {
                return this.verifiedVisible;
            }
            set
            {
                if (this.verifiedVisible == value)
                    return;
                this.verifiedVisible = value;
                base.RaisePropertyChanged("VerifiedVisible");
            }
        }
        private bool followsYouVisible;
        public bool FollowsYouVisible
        {
            get
            {
                return this.followsYouVisible;
            }
            set
            {
                if (this.followsYouVisible == value)
                    return;
                this.followsYouVisible = value;
                base.RaisePropertyChanged("FollowsYouVisible");
            }
        }
        private bool protectedVisible;
        public bool ProtectedVisible
        {
            get
            {
                return this.protectedVisible;
            }
            set
            {
                if (this.protectedVisible == value)
                    return;
                this.protectedVisible = value;
                base.RaisePropertyChanged("ProtectedVisible");
            }
        }
        private bool protectedPanelVisible;
        public bool ProtectedPanelVisible
        {
            get
            {
                return this.protectedPanelVisible;
            }
            set
            {
                if (this.protectedPanelVisible == value)
                    return;
                this.protectedPanelVisible = value;
                base.RaisePropertyChanged("ProtectedPanelVisible");
            }
        }
        private bool titleBarVisible;
        public bool TitleBarVisible
        {
            get
            {
                return this.titleBarVisible;
            }
            set
            {
                if (this.titleBarVisible == value)
                    return;
                this.titleBarVisible = value;
                base.RaisePropertyChanged("TitleBarVisible");
            }
        }
        private bool blockEnabled;
        public bool BlockEnabled
        {
            get
            {
                return this.blockEnabled;
            }
            set
            {
                if (this.blockEnabled == value)
                    return;
                this.blockEnabled = value;
                base.RaisePropertyChanged("BlockEnabled");
            }
        }
        private bool spamEnabled;
        public bool SpamEnabled
        {
            get
            {
                return this.spamEnabled;
            }
            set
            {
                if (this.spamEnabled == value)
                    return;
                this.spamEnabled = value;
                base.RaisePropertyChanged("SpamEnabled");
            }
        }
        private bool saveRemoveSearchIsEnabled;
        public bool SaveRemoveSearchIsEnabled
        {
            get
            {
                return this.saveRemoveSearchIsEnabled;
            }
            set
            {
                if (this.saveRemoveSearchIsEnabled == value)
                    return;
                this.saveRemoveSearchIsEnabled = value;
                base.RaisePropertyChanged("SaveRemoveSearchIsEnabled");
            }
        }
        private string searchName;
        public string SearchName
        {
            get
            {
                return this.searchName;
            }
            set
            {
                if (string.Equals(this.searchName, value, StringComparison.Ordinal))
                    return;
                this.searchName = value;
                base.RaisePropertyChanged("SearchName");
            }
        }
        private bool replyOptionsEnabled;
        public bool ReplyOptionsEnabled
        {
            get
            {
                return this.replyOptionsEnabled;
            }
            set
            {
                if (this.replyOptionsEnabled == value)
                    return;
                this.replyOptionsEnabled = value;
                base.RaisePropertyChanged("ReplyOptionsEnabled");
            }
        }
        private MetroTwitErrorViewModel errorMessage;
        public MetroTwitErrorViewModel ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                if (this.errorMessage == value)
                    return;
                this.errorMessage = value;
                base.RaisePropertyChanged("ErrorMessage");
            }
        }
        private bool tweetPanelVisible;
        public bool TweetPanelVisible
        {
            get
            {
                return this.tweetPanelVisible;
            }
            set
            {
                if (this.tweetPanelVisible == value)
                    return;
                this.tweetPanelVisible = value;
                base.RaisePropertyChanged("TweetPanelVisible");
            }
        }
        private string saveRemoveSearchText;
        public string SaveRemoveSearchText
        {
            get
            {
                return this.saveRemoveSearchText;
            }
            set
            {
                if (string.Equals(this.saveRemoveSearchText, value, StringComparison.Ordinal))
                    return;
                this.saveRemoveSearchText = value;
                base.RaisePropertyChanged("SaveRemoveSearchText");
            }
        }
        private bool resetWidthVisibility;
        public bool ResetWidthVisibility
        {
            get
            {
                return this.resetWidthVisibility;
            }
            set
            {
                if (this.resetWidthVisibility == value)
                    return;
                this.resetWidthVisibility = value;
                base.RaisePropertyChanged("ResetWidthVisibility");
            }
        }
        private bool allowPin;
        public bool AllowPin
        {
            get
            {
                return this.allowPin;
            }
            set
            {
                if (this.allowPin == value)
                    return;
                this.allowPin = value;
                base.RaisePropertyChanged("AllowPin");
            }
        }
        private bool isTransitioningToPinned;
        public bool IsTransitioningToPinned
        {
            get
            {
                return this.isTransitioningToPinned;
            }
            set
            {
                if (this.isTransitioningToPinned == value)
                    return;
                this.isTransitioningToPinned = value;
                base.RaisePropertyChanged("IsTransitioningToPinned");
            }
        }
        private DisplayType displayType;
        public DisplayType DisplayType
        {
            get
            {
                return this.displayType;
            }
            set
            {
                if (this.displayType == value)
                    return;
                this.displayType = value;
                base.RaisePropertyChanged("DisplayTypeInt");
                base.RaisePropertyChanged("DisplayType");
            }
        }
        private bool isPaused;
        public bool IsPaused
        {
            get
            {
                return this.isPaused;
            }
            set
            {
                if (this.isPaused == value)
                    return;
                this.isPaused = value;
                base.RaisePropertyChanged("ColumnPausedVisibility");
                base.RaisePropertyChanged("PausedText");
                base.RaisePropertyChanged("IsPaused");
            }
        }
        private bool isBlocked;
        public bool IsBlocked
        {
            get
            {
                return this.isBlocked;
            }
            set
            {
                if (this.isBlocked == value)
                    return;
                this.isBlocked = value;
                base.RaisePropertyChanged("BlockedText");
                base.RaisePropertyChanged("IsBlocked");
            }
        }

        public Decimal CurrentTweetID
        {
            get
            {
                return this.Settings.ColumnLastUpdateID;
            }
            set
            {
                if (Decimal.Equals(this.CurrentTweetID, value))
                    return;
                this.Settings.ColumnLastUpdateID = value;
                base.RaisePropertyChanged("CurrentTweetID");
                if (!App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns)
                    return;
                NestService.ColumnsSync(new NestSync()
                {
                    Edit = {
            (object) new NestColumn()
            {
              Id = this.NestColumnID,
              LastReadId = (long) this.CurrentTweetID,
              Date = DateTime.UtcNow.ToUniversalTime()
            }
          }
                }, (Action<NestSync>)(r => Messenger.Default.Send<GenericMessage<NestSync>>(new GenericMessage<NestSync>(r), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.NestUpdate))), this.TwitterAccountID);
            }
        }

        public TweetListType TweetType
        {
            get
            {
                return this.Settings.ColumnType;
            }
            set
            {
                if (this.TweetType == value)
                    return;
                this.Settings.ColumnType = value;
                base.RaisePropertyChanged("TweetType");
                base.RaisePropertyChanged("ItemName");
                base.RaisePropertyChanged("ListRetweetsVisibility");
            }
        }

        public string TweetListName
        {
            get
            {
                return this.Settings.ColumnHeader;
            }
            set
            {
                if (string.Equals(this.TweetListName, value, StringComparison.Ordinal))
                    return;
                if (this.ColumnBackVisible && App.AppState.Accounts[this.TwitterAccountID].Settings.SyncColumns)
                {
                    NestSync NestSync = new NestSync();
                    NestSync.Edit = (IList)new ArrayList();
                    NestSync.Edit.Add((object)new NestColumn()
                    {
                        Id = this.NestColumnID,
                        Name = value,
                        Date = DateTime.UtcNow.ToUniversalTime()
                    });
                    NestService.ColumnsSync(NestSync, (Action<NestSync>)(r => Messenger.Default.Send<GenericMessage<NestSync>>(new GenericMessage<NestSync>(r), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.NestUpdate))), this.TwitterAccountID);
                }
                this.Settings.ColumnHeader = value;
                base.RaisePropertyChanged("TweetListName");
                base.RaisePropertyChanged("PopupTitle");
            }
        }

        public string ItemName
        {
            get
            {
                switch (this.TweetType)
                {
                    case TweetListType.DirectMessages:
                        return "direct messages";
                    case TweetListType.Favourites:
                        return "favorites";
                    default:
                        return "tweets";
                }
            }
        }

        public string TwitterTerm
        {
            get
            {
                return this.Settings.ColumnValue;
            }
            set
            {
                if (this.Settings.ColumnValue != value)
                    this.Settings.ColumnValue = value;
                base.RaisePropertyChanged("TwitterTerm");
            }
        }

        public Decimal? SearchID
        {
            get
            {
                return this.Settings.SearchID;
            }
            set
            {
                if (Nullable.Equals<Decimal>(this.SearchID, value))
                    return;
                this.Settings.SearchID = value;
                base.RaisePropertyChanged("SearchID");
            }
        }

        public Guid UniqueTweetListID
        {
            get
            {
                return this.Settings.ID;
            }
            set
            {
                if (this.UniqueTweetListID == value)
                    return;
                this.Settings.ID = value;
                base.RaisePropertyChanged("UniqueTweetListID");
            }
        }

        public int NestColumnID
        {
            get
            {
                return this.Settings.NestID;
            }
            set
            {
                if (this.NestColumnID == value)
                    return;
                this.Settings.NestID = value;
                base.RaisePropertyChanged("NestColumnID");
            }
        }

        public bool TaskbarNotification
        {
            get
            {
                return this.Settings.TaskbarNotification;
            }
            set
            {
                if (this.TaskbarNotification == value)
                    return;
                this.Settings.TaskbarNotification = value;
                base.RaisePropertyChanged("TaskbarNotification");
            }
        }

        public bool IsPinned
        {
            get
            {
                return this.Settings.ColumnPinned;
            }
        }

        public bool SoundNotification
        {
            get
            {
                return this.Settings.SoundNotification;
            }
            set
            {
                if (this.SoundNotification == value)
                    return;
                this.Settings.SoundNotification = value;
                base.RaisePropertyChanged("SoundNotification");
            }
        }

        public bool ToastNotification
        {
            get
            {
                return this.Settings.ToastNotification;
            }
            set
            {
                if (this.ToastNotification == value)
                    return;
                this.Settings.ToastNotification = value;
                base.RaisePropertyChanged("ToastNotification");
            }
        }

        public bool ListRetweets
        {
            get
            {
                return this.Settings.ListRetweets;
            }
            set
            {
                if (this.ListRetweets == value)
                    return;
                this.Settings.ListRetweets = value;
                base.RaisePropertyChanged("ListRetweets");
            }
        }

        public Visibility ListRetweetsVisibility
        {
            get
            {
                return this.TweetType == TweetListType.List ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int UnreadCount
        {
            get
            {
                try
                {
                    if (this.Tweets == null)
                        return 0;
                    lock (this._tweetsLock)
                    {
                        if (this.Tweets.Count > 0)
                        {
                            MetroTwitStatusBase local_0 = Enumerable.FirstOrDefault<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets, (Func<MetroTwitStatusBase, bool>)(x => x != null && x.FirstRead));
                            if (local_0 != null)
                                local_0.FirstRead = false;
                        }
                        IEnumerable<MetroTwitStatusBase> local_1 = Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets, (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.UnRead));
                        if (local_1 == null)
                            return 0;
                        if (Enumerable.Count<MetroTwitStatusBase>(local_1) > 0 && this.TitleBarVisible && this.TweetType != TweetListType.Conversation)
                        {
                            MetroTwitStatusBase local_2 = Enumerable.FirstOrDefault<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets, (Func<MetroTwitStatusBase, bool>)(x => x != null && !x.UnRead));
                            if (local_2 != null)
                                local_2.FirstRead = true;
                        }
                        return Enumerable.Count<MetroTwitStatusBase>(local_1);
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string TweetCount
        {
            get
            {
                return this.UnreadCount > 99 ? "+" : (this.UnreadCount == 0 ? (string)null : this.UnreadCount.ToString());
            }
        }

        public string BacklogCount
        {
            get
            {
                return this.backlog != null ? this.backlog.Count.ToString() : "0";
            }
        }

        public MetroTwitUser User
        {
            get
            {
                return this.user;
            }
            set
            {
                if (this.user == value)
                    return;
                this.user = value;
                base.RaisePropertyChanged("UserImageUrl");
                base.RaisePropertyChanged("NoRetweetsText");
                if (this.user != null)
                {
                    if (string.IsNullOrEmpty(this.user.Name))
                        this.NameVisible = false;
                    if (!string.IsNullOrEmpty(this.user.Website))
                        this.WebSiteVisible = true;
                    if (!string.IsNullOrEmpty(this.user.Description))
                        this.BioVisible = true;
                    if (!string.IsNullOrEmpty(this.user.Location))
                        this.LocationVisible = true;
                    if (this.user.Verified)
                        this.VerifiedVisible = true;
                }
                base.RaisePropertyChanged("User");
            }
        }

        public object UserImageUrl
        {
            get
            {
                if (this.User != null && App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()) && App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.User.ScreenName.ToLower()].ImageURITwitterSecure == this.userimageUrl)
                    return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
                if (!string.IsNullOrEmpty(this.userimageUrl))
                    return (object)CommonCommands.DownloadFile(this.userimageUrl.Replace("_normal", ""));
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

        public string PausedText
        {
            get
            {
                return this.IsPaused ? "Resume column updates" : "Pause column updates";
            }
        }

        public string BlockedText
        {
            get
            {
                return this.IsBlocked ? "Unblock user" : "Block user";
            }
        }

        public bool ColumnPausedVisibility
        {
            get
            {
                return this.IsPaused;
            }
        }

        public string NoRetweetsText
        {
            get
            {
                return this.User != null && App.AppState.Accounts[this.TwitterAccountID].Cache.NoRetweetIds.Contains((long)this.User.Id) ? "Show retweets from user" : "Hide retweets from user";
            }
        }

        public string LoadingText
        {
            get
            {
                Random random = new Random();
                return TweetListViewModel.loadingstrings[random.Next(0, TweetListViewModel.loadingstrings.Length)];
            }
        }

        public int Index
        {
            get
            {
                return this.Settings.Index;
            }
            set
            {
                if (this.Index == value)
                    return;
                this.Settings.Index = value;
                base.RaisePropertyChanged("Index");
            }
        }

        public bool ShowNotificationToasts
        {
            get
            {
                return SettingsData.Instance.ShowNotificationToasts;
            }
        }

        public bool ShowTaskbarCount
        {
            get
            {
                return SettingsData.Instance.ShowTaskbarCount;
            }
        }

        public bool UseNotificationSound
        {
            get
            {
                return SettingsData.Instance.UseNotificationSound;
            }
        }

        public string PopupTitle
        {
            get
            {
                return this.TweetListName;
            }
            set
            {
                if (string.Equals(this.PopupTitle, value, StringComparison.Ordinal))
                    return;
                base.RaisePropertyChanged("PopupTitle");
            }
        }

        public int DisplayTypeInt
        {
            get
            {
                return (int)this.DisplayType;
            }
            set
            {
                if (this.DisplayTypeInt == value)
                    return;
                this.DisplayType = (DisplayType)value;
                this.LoadDataIfRequired();
                base.RaisePropertyChanged("DisplayTypeInt");
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
        private RelayCommand removeColumnCommand;
        public RelayCommand RemoveColumnCommand
        {
            get
            {
                return this.removeColumnCommand;
            }
            private set
            {
                if (this.removeColumnCommand == value)
                    return;
                this.removeColumnCommand = value;
                base.RaisePropertyChanged("RemoveColumnCommand");
            }
        }
        private RelayCommand<string> contentLinkCommand;
        public RelayCommand<string> ContentLinkCommand
        {
            get
            {
                return this.contentLinkCommand;
            }
            private set
            {
                if (this.contentLinkCommand == value)
                    return;
                this.contentLinkCommand = value;
                base.RaisePropertyChanged("ContentLinkCommand");
            }
        }
        private RelayCommand linkCommand;
        public RelayCommand LinkCommand
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
        private RelayCommand profileLinkCommand;
        public RelayCommand ProfileLinkCommand
        {
            get
            {
                return this.profileLinkCommand;
            }
            private set
            {
                if (this.profileLinkCommand == value)
                    return;
                this.profileLinkCommand = value;
                base.RaisePropertyChanged("ProfileLinkCommand");
            }
        }
        private RelayCommand markasReadCommand;
        public RelayCommand MarkasReadCommand
        {
            get
            {
                return this.markasReadCommand;
            }
            private set
            {
                if (this.markasReadCommand == value)
                    return;
                this.markasReadCommand = value;
                base.RaisePropertyChanged("MarkasReadCommand");
            }
        }
        private RelayCommand followCommand;
        public RelayCommand FollowCommand
        {
            get
            {
                return this.followCommand;
            }
            private set
            {
                if (this.followCommand == value)
                    return;
                this.followCommand = value;
                base.RaisePropertyChanged("FollowCommand");
            }
        }
        private RelayCommand blockCommand;
        public RelayCommand BlockCommand
        {
            get
            {
                return this.blockCommand;
            }
            private set
            {
                if (this.blockCommand == value)
                    return;
                this.blockCommand = value;
                base.RaisePropertyChanged("BlockCommand");
            }
        }
        private RelayCommand reportSpamCommand;
        public RelayCommand ReportSpamCommand
        {
            get
            {
                return this.reportSpamCommand;
            }
            private set
            {
                if (this.reportSpamCommand == value)
                    return;
                this.reportSpamCommand = value;
                base.RaisePropertyChanged("ReportSpamCommand");
            }
        }
        private RelayCommand mentionCommand;
        public RelayCommand MentionCommand
        {
            get
            {
                return this.mentionCommand;
            }
            private set
            {
                if (this.mentionCommand == value)
                    return;
                this.mentionCommand = value;
                base.RaisePropertyChanged("MentionCommand");
            }
        }
        private RelayCommand directMessageCommand;
        public RelayCommand DirectMessageCommand
        {
            get
            {
                return this.directMessageCommand;
            }
            private set
            {
                if (this.directMessageCommand == value)
                    return;
                this.directMessageCommand = value;
                base.RaisePropertyChanged("DirectMessageCommand");
            }
        }
        private RelayCommand closeAdCommand;
        public RelayCommand CloseAdCommand
        {
            get
            {
                return this.closeAdCommand;
            }
            private set
            {
                if (this.closeAdCommand == value)
                    return;
                this.closeAdCommand = value;
                base.RaisePropertyChanged("CloseAdCommand");
            }
        }
        private RelayCommand columnSettingsCommand;
        public RelayCommand ColumnSettingsCommand
        {
            get
            {
                return this.columnSettingsCommand;
            }
            private set
            {
                if (this.columnSettingsCommand == value)
                    return;
                this.columnSettingsCommand = value;
                base.RaisePropertyChanged("ColumnSettingsCommand");
            }
        }
        private RelayCommand<bool> flushCommand;
        public RelayCommand<bool> FlushCommand
        {
            get
            {
                return this.flushCommand;
            }
            private set
            {
                if (this.flushCommand == value)
                    return;
                this.flushCommand = value;
                base.RaisePropertyChanged("FlushCommand");
            }
        }
        private RelayCommand unpauseColumnCommand;
        public RelayCommand UnpauseColumnCommand
        {
            get
            {
                return this.unpauseColumnCommand;
            }
            private set
            {
                if (this.unpauseColumnCommand == value)
                    return;
                this.unpauseColumnCommand = value;
                base.RaisePropertyChanged("UnpauseColumnCommand");
            }
        }
        private RelayCommand saveRemoveSearchCommand;
        public RelayCommand SaveRemoveSearchCommand
        {
            get
            {
                return this.saveRemoveSearchCommand;
            }
            private set
            {
                if (this.saveRemoveSearchCommand == value)
                    return;
                this.saveRemoveSearchCommand = value;
                base.RaisePropertyChanged("SaveRemoveSearchCommand");
            }
        }
        private RelayCommand noRetweetsCommand;
        public RelayCommand NoRetweetsCommand
        {
            get
            {
                return this.noRetweetsCommand;
            }
            private set
            {
                if (this.noRetweetsCommand == value)
                    return;
                this.noRetweetsCommand = value;
                base.RaisePropertyChanged("NoRetweetsCommand");
            }
        }
        private RelayCommand addToListCommand;
        public RelayCommand AddToListCommand
        {
            get
            {
                return this.addToListCommand;
            }
            private set
            {
                if (this.addToListCommand == value)
                    return;
                this.addToListCommand = value;
                base.RaisePropertyChanged("AddToListCommand");
            }
        }
        private RelayCommand resetColumnWidthCommand;
        public RelayCommand ResetColumnWidthCommand
        {
            get
            {
                return this.resetColumnWidthCommand;
            }
            private set
            {
                if (this.resetColumnWidthCommand == value)
                    return;
                this.resetColumnWidthCommand = value;
                base.RaisePropertyChanged("ResetColumnWidthCommand");
            }
        }

        public event EventHandler DataContextInitialised;

        static TweetListViewModel()
        {
        }

        public TweetListViewModel(Guid UniqueID, TweetListType tweetListType, Decimal TwitterAccountID, MetroTwitColumn Settings, string columntitle = null, string searchterm = null, string searchName = null, Decimal? existingSearchID = null, bool dontloadtweets = false, MetroTwitStatusBase originaltweet = null, Decimal InitialLastID = 0M)
        {
            TweetListViewModel tweetListViewModel = this;
            this.Settings = Settings;
            this.UniqueTweetListID = UniqueID != Guid.Empty ? UniqueID : Guid.NewGuid();
            this.RefreshCommand = new RelayCommand(new Action(this.RefreshExecute));
            this.FlushCommand = new RelayCommand<bool>(new Action<bool>(this.Flush));
            this.RemoveColumnCommand = new RelayCommand(new Action(this.RemoveColumn));
            this.ContentLinkCommand = new RelayCommand<string>(new Action<string>(this.ExecuteLink));
            this.LinkCommand = new RelayCommand(new Action(this.OpenLink));
            this.ProfileLinkCommand = new RelayCommand(new Action(this.ProfileLink));
            this.MarkasReadCommand = new RelayCommand(new Action(this.MarkasRead));
            this.FollowCommand = new RelayCommand(new Action(this.Follow));
            this.BlockCommand = new RelayCommand(new Action(this.Block));
            this.ReportSpamCommand = new RelayCommand(new Action(this.ReportSpam));
            this.MentionCommand = new RelayCommand(new Action(this.Mention));
            this.DirectMessageCommand = new RelayCommand(new Action(this.DirectMessage));
            this.CloseAdCommand = new RelayCommand(new Action(this.CloseAd));
            this.ColumnSettingsCommand = new RelayCommand(new Action(this.ColumnSettings));
            this.UnpauseColumnCommand = new RelayCommand(new Action(this.UnpauseColumn));
            this.SaveRemoveSearchCommand = new RelayCommand(new Action(this.SaveRemoveSearch));
            this.NoRetweetsCommand = new RelayCommand(new Action(this.NoRetweets));
            this.AddToListCommand = new RelayCommand(new Action(this.AddToList));
            this.ResetColumnWidthCommand = new RelayCommand(new Action(this.ResetColumnWidth));
            this.EnableNotifications = true;
            this.TweetType = tweetListType;
            this.TwitterAccountID = TwitterAccountID;
            this.IsPaused = false;
            this.ShowLoading = true;
            this.NameVisible = true;
            this.TweetPanelVisible = true;
            this.TitleBarVisible = true;
            this.FollowButtonIsEnabled = true;
            this.DisplayType = DisplayType.Tweets;
            this.ResetWidthVisibility = this.Settings.ColumnIsSetWidth;
            this.hideLoadLock = new object();
            this.Tweets = new SortableObservableCollection<MetroTwitStatusBase>(new List<MetroTwitStatusBase>());
            BindingOperations.EnableCollectionSynchronization((IEnumerable)this.Tweets, this._tweetsLock);
            switch (tweetListType)
            {
                case TweetListType.FriendsTimeline:
                    this.TweetListName = "friends";
                    break;
                case TweetListType.DirectMessages:
                    this.TweetListName = "direct messages";
                    break;
                case TweetListType.Search:
                    this.SearchID = existingSearchID;
                    if (searchterm == null)
                        return;
                    this.SearchName = searchName;
                    this.TwitterTerm = searchterm.Trim().Replace("\n", "");
                    this.TweetListName = !string.IsNullOrEmpty(searchName) ? searchName : this.TwitterTerm;
                    this.SearchDetailsVisible = true;
                    this.SetSaveRemoveSearchButton();
                    break;
                case TweetListType.UserTimeline:
                    if (searchterm == null)
                        return;
                    this.TwitterTerm = searchterm.Trim().Replace("\n", "").Replace("@", "");
                    this.TweetListName = "@" + this.TwitterTerm;
                    if (!dontloadtweets)
                    {
                        this.ShowProfileLoading = true;
                        this.ProfileDetailsVisible = true;
                        if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
                            Users.ShowAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, this.TwitterTerm, MetroTwitTwitterizer.Options).ContinueWith(new Action<Task<TwitterResponse<User>>>(this.UserDetails));
                        break;
                    }
                    else
                        break;
                case TweetListType.List:
                    if (searchterm == null)
                        return;
                    this.TwitterTerm = searchterm.Trim();
                    this.TweetListName = this.TwitterTerm;
                    break;
                case TweetListType.MentionsMyTweetsRetweeted:
                    SettingsData settingsData = SettingsData.Instance;
                    this.TweetListName = "mentions";
                    settingsData = (SettingsData)null;
                    break;
                case TweetListType.MyTweets:
                    this.TweetListName = "my tweets";
                    break;
                case TweetListType.Favourites:
                    this.TweetListName = "favourites";
                    break;
                case TweetListType.Conversation:
                    this.TweetListName = "tweet conversation";
                    if (originaltweet != null)
                    {
                        this.TwitterTerm = originaltweet.InReplyToScreenName;
                        originaltweet.TwitterAccountID = this.TwitterAccountID;
                        originaltweet.TweetType = TweetListType.Conversation;
                        originaltweet.ReplyToVisibility = Visibility.Collapsed;
                        this.AddTweet(originaltweet);
                        ++this.taskCount;
                        this.ShowContentPanel();
                        break;
                    }
                    else
                        break;
                case TweetListType.RetweetUsers:
                    this.TweetListName = "retweeted by";
                    this.TwitterTerm = searchterm;
                    break;
            }
            if (!string.IsNullOrEmpty(columntitle))
                this.TweetListName = columntitle;
            if (!dontloadtweets)
            {
                Messenger.Default.Register<GenericMessage<object>>((object)this, (object)(((object)ViewModelMessages.RestUpdate).ToString() + this.UniqueTweetListID.ToString()), new Action<GenericMessage<object>>(this.RestUpdate));
                ++this.taskCount;
                Task.Run((Action)(() => TwitterREST.TwitterRest(tweetListViewModel.UniqueTweetListID, tweetListType, tweetListViewModel.TwitterAccountID, tweetListViewModel.TweetType != TweetListType.Conversation ? RefreshTypes.Normal : RefreshTypes.ForeverScroll, new Action(tweetListViewModel.ShowContentPanel), tweetListViewModel.TwitterTerm, originaltweet != null ? originaltweet.InReplyTo : new Decimal(0), InitialLastID, new Decimal(0), tweetListViewModel.ListRetweets)));
                Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.DeleteTweet), new Action<GenericMessage<object>>(this.DeleteTweet));
                Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.DeleteUserTweets), new Action<GenericMessage<object>>(this.DeleteUserTweets));
                Messenger.Default.Register<GenericMessage<KeyValuePair<Decimal, bool>>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.Blocked), new Action<GenericMessage<KeyValuePair<Decimal, bool>>>(this.Blocked));
                Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.EnableReplyOptions, new Action<GenericMessage<object>>(this.EnableReplyOptions));
                if (tweetListType != TweetListType.DirectMessages)
                {
                    if (tweetListType == TweetListType.FriendsTimeline || tweetListType == TweetListType.MyTweets || tweetListType == TweetListType.MentionsMyTweetsRetweeted || tweetListType == TweetListType.Conversation)
                    {
                        Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingStatusReceived), new Action<GenericMessage<object>>(this.StreamingUpdate));
                        Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingStatusDeleted), new Action<GenericMessage<object>>(this.StreamingDelete));
                    }
                    Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.RemoveFavouriteTweet), new Action<GenericMessage<object>>(this.RemoveFavouriteTweet));
                    Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.FavouriteTweet), new Action<GenericMessage<object>>(this.FavouriteTweet));
                }
                else
                {
                    Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingDirectMessage), new Action<GenericMessage<object>>(this.StreamingDMUpdate));
                    Messenger.Default.Register<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingDirectMessageDeleted), new Action<GenericMessage<object>>(this.StreamingDMDelete));
                }
            }
            if (this.DataContextInitialised != null)
                this.DataContextInitialised((object)this, EventArgs.Empty);
        }

        ~TweetListViewModel()
        {
        }

        private void AddTweet(MetroTwitStatusBase tweet)
        {
            lock (this._tweetsLock)
                this.Tweets.Add(tweet);
        }

        private void RemoveTweet(MetroTwitStatusBase tweet)
        {
            lock (this._tweetsLock)
                this.Tweets.Remove(tweet);
        }

        private void SetSaveRemoveSearchButton()
        {
            Task task = new Task((Action)(() =>
            {
                this.SaveRemoveSearchIsEnabled = true;
                if (this.SearchID.HasValue)
                {
                    this.searchColumnIsInSaveMode = false;
                    this.SaveRemoveSearchText = "_remove saved search";
                }
                else
                {
                    this.searchColumnIsInSaveMode = true;
                    this.SaveRemoveSearchText = "_save search";
                }
            }));
            task.ContinueWith((Action<Task>)(t => CommonCommands.CheckTaskExceptions(t)));
            task.Start();
        }

        private void EnableReplyOptions(GenericMessage<object> o)
        {
            if (this.Tweets == null || this.Tweets.Count <= 0)
                return;
            foreach (MetroTwitStatusBase metroTwitStatusBase in (Collection<MetroTwitStatusBase>)this.Tweets)
                this.ReplyOptionsEnabled = (bool)o.Content;
        }

        private void SaveRemoveSearch()
        {
            if (this.searchColumnIsInSaveMode)
            {
                this.SaveRemoveSearchIsEnabled = false;
                this.SaveRemoveSearchText = "saving...";
                SavedSearches.CreateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, this.TwitterTerm, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<TwitterSavedSearch>>>)(response =>
                {
                    if (response.Result.Result == RequestResult.Success)
                    {
                        this.SearchID = new Decimal?(response.Result.ResponseObject.Id);
                        this.SetSaveRemoveSearchButton();
                        App.RefreshSavedSearches(this.TwitterAccountID, (Action)null);
                    }
                    else
                        App.RefreshSavedSearches(this.TwitterAccountID, (Action)(() =>
                        {
                            TwitterSavedSearch local_0 = Enumerable.FirstOrDefault<TwitterSavedSearch>((IEnumerable<TwitterSavedSearch>)App.AppState.Accounts[this.TwitterAccountID].SavedSearches, (Func<TwitterSavedSearch, bool>)(s => s.Query == this.TwitterTerm));
                            this.SearchID = local_0 == null ? new Decimal?() : new Decimal?(local_0.Id);
                            this.SetSaveRemoveSearchButton();
                        }));
                }));
            }
            else
            {
                this.SaveRemoveSearchIsEnabled = false;
                this.SaveRemoveSearchText = "removing...";
                SavedSearches.DeleteAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, this.SearchID.GetValueOrDefault(new Decimal(-1)), MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<TwitterSavedSearch>>>)(response =>
                {
                    if (response.Result.Result != RequestResult.Success && response.Result.Result != RequestResult.FileNotFound)
                        return;
                    this.SearchID = new Decimal?();
                    this.SetSaveRemoveSearchButton();
                    App.RefreshSavedSearches(this.TwitterAccountID, (Action)null);
                }));
            }
        }

        private void ShowContentPanel()
        {
            lock (this.hideLoadLock)
            {
                --this.taskCount;
                if (this.taskCount != 0)
                    return;
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.ShowLoading = false;
                    this.ShowContentVisibility = true;
                }), new object[0]);
            }
        }

        private void UserDetails(Task<TwitterResponse<User>> r)
        {
            if (r.Result.Result == RequestResult.Success)
            {
                this.ShowProfileLoading = false;
                this.User = new MetroTwitUser(r.Result.ResponseObject);
                this.userimageUrl = this.User.ProfileImageSecureLocation.Replace("_normal", "");
                base.RaisePropertyChanged("UserImageUrl");
                this.UserVisible = true;
                this.BlockEnabled = this.User.ScreenName.ToLower() != App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower();
                this.SpamEnabled = this.BlockEnabled;
                this.VerifiedVisible = this.User.Verified;
                this.ProtectedVisible = this.User.IsProtected;
                if (!string.IsNullOrEmpty(this.user.Description))
                {
                    var b = RegularExpressions.AT_SIGNS;
                    EntityCollection tempEntities = RegularExpressions.ExtractEntities(this.User.Description);
                    this.UserRenderedDescription = new MetroTwitStatusBase
                    {
                        TwitterAccountID = base.TwitterAccountID,
                        RawText = this.User.Description,
                        Entities = tempEntities,
                        User = this.User
                    };
                }
            }
            if (!App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
                return;
            if (App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower() != this.TwitterTerm.ToLower())
            {
                Friendship.ShowAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, this.TwitterTerm, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<TwitterRelationship>>>)(r2 =>
                {
                    if (r2.Result.Result != RequestResult.Success)
                        return;
                    TwitterRelationship local_0 = r2.Result.ResponseObject;
                    this.FollowButtonText = local_0.Relationship.Source.Following ? "unfollow" : "follow";
                    this.FollowButtonVisible = true;
                    this.FollowsYouVisible = local_0.Relationship.Target.Following;
                    if (this.ProtectedVisible)
                        this.ProtectedPanelVisible = !local_0.Relationship.Source.Following;
                    if (this.ProtectedPanelVisible)
                    {
                        this.TweetPanelVisible = false;
                        this.Tweets = (SortableObservableCollection<MetroTwitStatusBase>)null;
                    }
                    this.IsBlocked = local_0.Relationship.Source.Blocking.Value;
                    base.RaisePropertyChanged("BlockedText");
                }));
            }
            else
            {
                this.FollowButtonText = "it's you";
                this.FollowButtonVisible = true;
            }
        }

        private void StreamingDMDelete(GenericMessage<object> r)
        {
            if (this.Tweets == null || r.Content == null)
                return;
            foreach (MetroTwitStatusBase tweet in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r2) => x != null && x.ID == (r.Content as TwitterStreamDeletedEvent).Id && x.User.Id == (r.Content as TwitterStreamDeletedEvent).UserId)))
            {
                this.RemoveTweet(tweet);
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    base.RaisePropertyChanged("TweetCount");
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
                }), DispatcherPriority.Background, new object[0]);
            }
        }

        private void StreamingDMUpdate(GenericMessage<object> r)
        {
            if (this.Tweets == null)
                this.Tweets = new SortableObservableCollection<MetroTwitStatusBase>(new List<MetroTwitStatusBase>());
            if (r.Content == null)
                return;
            this.TweetsUpdated((MetroTwitStatusBase)new TwitterDirectMessageExtended(r.Content as TwitterDirectMessage, this.TweetType, this.TwitterAccountID, this.CurrentTweetID, true), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
        }

        private void StreamingDelete(GenericMessage<object> r)
        {
            if (this.Tweets == null || r.Content == null)
                return;
            foreach (MetroTwitStatusBase tweet in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToArray<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r2) => x.ID == (r.Content as TwitterStreamDeletedEvent).Id && x.User.Id == (r.Content as TwitterStreamDeletedEvent).UserId)))
            {
                this.RemoveTweet(tweet);
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    base.RaisePropertyChanged("TweetCount");
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
                }), DispatcherPriority.Background, new object[0]);
            }
        }

        private void StreamingUpdate(GenericMessage<object> r)
        {
            if (this.Tweets == null)
                this.Tweets = new SortableObservableCollection<MetroTwitStatusBase>(new List<MetroTwitStatusBase>());
            try
            {
                if (r.Content != null && this.Tweets != null)
                {
                    Status twitterStatus = r.Content as Status;
                    if (twitterStatus != null && (!App.AppState.Accounts[this.TwitterAccountID].Cache.BlockIds.Contains((long)twitterStatus.User.Id) && (twitterStatus.RetweetedStatus == null || twitterStatus.RetweetedStatus != null && !App.AppState.Accounts[this.TwitterAccountID].Cache.BlockIds.Contains((long)twitterStatus.RetweetedStatus.User.Id))))
                    {
                        int num;
                        if (this.TweetType == TweetListType.FriendsTimeline)
                            num = App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsValue(new CacheUser()
                            {
                                TwitterID = twitterStatus.User.Id
                            }) ? 0 : (!(twitterStatus.User.Id == this.TwitterAccountID) ? 1 : 0);
                        else
                            num = 1;
                        if (num == 0)
                            this.TweetsUpdated((MetroTwitStatusBase)new TwitterStatusExtended(twitterStatus, this.TweetType, this.TwitterAccountID, this.CurrentTweetID), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
                        if (this.TweetType == TweetListType.MentionsMyTweetsRetweeted && twitterStatus.Entities != null && twitterStatus.Entities.Count > 0 && Enumerable.Count<MentionEntity>(Enumerable.Where<MentionEntity>(Enumerable.OfType<MentionEntity>((IEnumerable)twitterStatus.Entities), (Func<MentionEntity, int, bool>)((x, r2) => (Decimal)x.UserId == this.TwitterAccountID))) > 0)
                            this.TweetsUpdated((MetroTwitStatusBase)new TwitterStatusExtended(twitterStatus, this.TweetType, this.TwitterAccountID, this.CurrentTweetID), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
                        if (this.TweetType == TweetListType.MyTweets && twitterStatus.User.Id == this.TwitterAccountID)
                            this.TweetsUpdated((MetroTwitStatusBase)new TwitterStatusExtended(twitterStatus, this.TweetType, this.TwitterAccountID, this.CurrentTweetID), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
                        if (this.TweetType == TweetListType.Conversation && this.Tweets.Count > 0 && twitterStatus.InReplyToStatusId == Enumerable.First<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID)
                            this.TweetsUpdated((MetroTwitStatusBase)new TwitterStatusExtended(twitterStatus, this.TweetType, this.TwitterAccountID, this.CurrentTweetID), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
                        if (this.TweetType == TweetListType.RetweetUsers && (twitterStatus.RetweetCount > 0 && twitterStatus.Id == Decimal.Parse(this.TwitterTerm) || twitterStatus.RetweetedStatus != null && twitterStatus.RetweetedStatus.Id == Decimal.Parse(this.TwitterTerm)))
                            this.TweetsUpdated((MetroTwitStatusBase)new TwitterStatusExtended(twitterStatus, TweetListType.RetweetUsers, this.TwitterAccountID, this.CurrentTweetID), (List<MetroTwitStatusBase>)null, RefreshTypes.Refresh, false);
                    }
                }
            }
            catch
            {
            }
            if (this.Tweets == null)
                return;
            this.NoTweets = this.Tweets.Count == 0;
        }

        private void RestUpdate(GenericMessage<object> o)
        {
            if (this.Tweets == null)
                this.Tweets = new SortableObservableCollection<MetroTwitStatusBase>(new List<MetroTwitStatusBase>());
            if (o.Content != null)
            {
                if (o.Content is MetroRestResponse<TwitterStatusCollection>)
                {
                    MetroRestResponse<TwitterStatusCollection> metroRestResponse = o.Content as MetroRestResponse<TwitterStatusCollection>;
                    if (metroRestResponse.Tweets != null)
                    {
                        List<MetroTwitStatusBase> newtweets = Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.Select<Status, TwitterStatusExtended>(Enumerable.Where<Status>(Enumerable.Cast<Status>((IEnumerable)metroRestResponse.Tweets), (Func<Status, bool>)(stat => stat != null && !App.AppState.Accounts[this.TwitterAccountID].Cache.BlockIds.Contains((long)stat.User.Id))), (Func<Status, TwitterStatusExtended>)(stat => new TwitterStatusExtended(stat, this.TweetType, this.TwitterAccountID, this.CurrentTweetID))));
                        if (newtweets != null)
                            this.TweetsUpdated((MetroTwitStatusBase)null, newtweets, metroRestResponse.RefreshType, false);
                    }
                    if (metroRestResponse.RequestResult != RequestResult.Success && metroRestResponse.Error != null)
                    {
                        this.ErrorMessage = new MetroTwitErrorViewModel()
                        {
                            Text = Enumerable.First<TwitterError>(metroRestResponse.Error).Message
                        };
                        this.errorStateIntialLoad = metroRestResponse.RefreshType == RefreshTypes.Normal;
                    }
                }
                if (o.Content is MetroRestResponse<IEnumerable<TwitterDirectMessage>>)
                {
                    MetroRestResponse<IEnumerable<TwitterDirectMessage>> metroRestResponse = o.Content as MetroRestResponse<IEnumerable<TwitterDirectMessage>>;
                    if (metroRestResponse.Tweets != null)
                    {
                        List<MetroTwitStatusBase> newtweets = Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.Select<TwitterDirectMessage, TwitterDirectMessageExtended>(Enumerable.Cast<TwitterDirectMessage>((IEnumerable)metroRestResponse.Tweets), (Func<TwitterDirectMessage, TwitterDirectMessageExtended>)(stat => new TwitterDirectMessageExtended(stat, this.TweetType, this.TwitterAccountID, this.CurrentTweetID, false))));
                        if (newtweets != null)
                            this.TweetsUpdated((MetroTwitStatusBase)null, newtweets, metroRestResponse.RefreshType, false);
                    }
                    if (metroRestResponse.RequestResult != RequestResult.Success && metroRestResponse.Error != null)
                    {
                        this.ErrorMessage = new MetroTwitErrorViewModel()
                        {
                            Text = Enumerable.First<TwitterError>(metroRestResponse.Error).Message
                        };
                        this.errorStateIntialLoad = metroRestResponse.RefreshType == RefreshTypes.Normal;
                    }
                }
            }
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (this.Tweets == null)
                    return;
                this.NoTweets = this.Tweets.Count == 0;
            }), DispatcherPriority.Background, new object[0]);
        }

        private void DeleteTweet(GenericMessage<object> o)
        {
            if (this.Tweets == null)
                return;
            foreach (MetroTwitStatusBase tweet in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.ID == (Decimal)o.Content)))
            {
                this.RemoveTweet(tweet);
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    base.RaisePropertyChanged("TweetCount");
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
                }), DispatcherPriority.Background, new object[0]);
            }
        }

        private void DeleteUserTweets(GenericMessage<object> o)
        {
            if (this.Tweets == null || this.TweetType == TweetListType.UserTimeline)
                return;
            Decimal user = Decimal.Parse(o.Content.ToString());
            foreach (MetroTwitStatusBase tweet in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.User.Id == user)))
            {
                this.RemoveTweet(tweet);
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    base.RaisePropertyChanged("TweetCount");
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
                }), DispatcherPriority.Background, new object[0]);
            }
        }

        private void Blocked(GenericMessage<KeyValuePair<Decimal, bool>> o)
        {
            try
            {
                KeyValuePair<Decimal, bool> content;
                int num1;
                if (this.TweetType == TweetListType.UserTimeline && this.User != null)
                {
                    Decimal id = this.User.Id;
                    content = o.Content;
                    Decimal key = content.Key;
                    num1 = !(id == key) ? 1 : 0;
                }
                else
                    num1 = 1;
                if (num1 == 0)
                {
                    TweetListViewModel tweetListViewModel = this;
                    content = o.Content;
                    int num2 = content.Value ? 1 : 0;
                    tweetListViewModel.IsBlocked = num2 != 0;
                }
                else if (this.Tweets != null)
                {
                    foreach (MetroTwitStatusBase metroTwitStatusBase in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, bool>)(x => x.User.Id == o.Content.Key)))
                    {
                        content = o.Content;
                        int num2 = content.Value ? 1 : 0;
                        metroTwitStatusBase.IsBlocked = num2 != 0;
                    }
                }
            }
            catch
            {
            }
        }

        private void RemoveFavouriteTweet(GenericMessage<object> o)
        {
            if (this.Tweets == null)
                return;
            foreach (MetroTwitStatusBase metroTwitStatusBase in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.ID == (Decimal)o.Content)))
            {
                MetroTwitStatusBase tweet = metroTwitStatusBase;
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (this.TweetType != TweetListType.Favourites)
                        tweet.IsFavourited = false;
                    else
                        this.RemoveTweet(tweet);
                    base.RaisePropertyChanged("TweetCount");
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
                }), DispatcherPriority.Background, new object[0]);
            }
        }

        private void FavouriteTweet(GenericMessage<object> o)
        {
            if (this.Tweets == null)
                return;
            foreach (MetroTwitStatusBase metroTwitStatusBase in Enumerable.ToArray<MetroTwitStatusBase>(Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.ID == (Decimal)o.Content))))
            {
                MetroTwitStatusBase tweet = metroTwitStatusBase;
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => tweet.IsFavourited = true), DispatcherPriority.Background, new object[0]);
            }
        }

        private void TweetsUpdated(MetroTwitStatusBase tweet, List<MetroTwitStatusBase> newtweets, RefreshTypes RefreshType, bool flushingbacklog = false)
        {
            Action method = null;
            Action action2 = null;
            Action action3 = null;
            try
            {
                if ((tweet != null) || (newtweets != null))
                {
                    if (this.Tweets == null)
                    {
                        this.Tweets = new SortableObservableCollection<MetroTwitStatusBase>(new List<MetroTwitStatusBase>());
                    }
                    if (tweet != null)
                    {
                        tweet = this.FilterTweets(tweet);
                    }
                    if (newtweets != null)
                    {
                        newtweets = this.FilterTweets(newtweets);
                    }
                    if ((((SettingsData.Instance.BacklogSeconds > 0) || this.IsPaused) && (!flushingbacklog && (RefreshType != RefreshTypes.ForeverScroll))) && (RefreshType != RefreshTypes.Normal))
                    {
                        if (this.backlog == null)
                        {
                            this.backlog = new List<MetroTwitStatusBase>();
                        }
                        if (tweet != null)
                        {
                            this.backlog.Insert(0, tweet);
                        }
                        else if ((newtweets != null) && (newtweets.Count > 0))
                        {
                            this.backlog.InsertRange<MetroTwitStatusBase>(newtweets);
                        }
                        if (this.backlog.Count > 0)
                        {
                            if (this.Tweets[0].TweetListSpecial != TweetListSpecial.Backlog)
                            {
                                MetroTwitStatusBase item = new MetroTwitStatusBase
                                {
                                    TweetListSpecial = TweetListSpecial.Backlog,
                                    CreatedDate = DateTime.MaxValue
                                };
                                this.Tweets.Insert(0, item);
                            }
                            else
                            {
                                if (method == null)
                                {
                                    method = () => this.RaisePropertyChanged("BacklogCount");
                                }
                                System.Windows.Application.Current.Dispatcher.BeginInvoke(method, DispatcherPriority.Background, new object[0]);
                            }
                        }
                    }
                    else
                    {
                        if (RefreshType != RefreshTypes.ForeverScroll)
                        {
                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(false), ViewModelMessages.TweetsUpdated.ToString() + this.UniqueTweetListID.ToString());
                        }
                        bool flag = false;
                        if ((tweet != null) || ((newtweets != null) && (newtweets.Count > 0)))
                        {
                            if ((newtweets != null) && (newtweets.Count > 0))
                            {
                                newtweets = newtweets.Distinct<MetroTwitStatusBase>().ToList<MetroTwitStatusBase>();
                                MetroTwitStatusBase[] baseArray = newtweets.ToArray();
                                for (int i = 0; i < baseArray.Length; i++)
                                {
                                    Func<MetroTwitStatusBase, int, bool> predicate = null;
                                    MetroTwitStatusBase t = baseArray[i];
                                    if (predicate == null)
                                    {
                                        predicate = (x, r) => (x.ID == t.ID) || (x.ID == t.OriginalID);
                                    }
                                    IEnumerable<MetroTwitStatusBase> source = this.Tweets.ToArray<MetroTwitStatusBase>().Where<MetroTwitStatusBase>(predicate);
                                    if (RefreshType != RefreshTypes.ForeverScroll)
                                    {
                                        foreach (MetroTwitStatusBase base3 in source)
                                        {
                                            if (base3.UnRead)
                                            {
                                                flag = true;
                                            }
                                            this.RemoveTweet(base3);
                                        }
                                    }
                                    else if (source.Count<MetroTwitStatusBase>() > 0)
                                    {
                                        newtweets.Remove(t);
                                    }
                                }
                                if (newtweets.Count == 0)
                                {
                                    return;
                                }
                            }
                            if (flag)
                            {
                                if (action2 == null)
                                {
                                    action2 = delegate
                                    {
                                        base.RaisePropertyChanged("TweetCount");
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.OverlayCountRecalc);
                                    };
                                }
                                System.Windows.Application.Current.Dispatcher.BeginInvoke(action2, DispatcherPriority.Background, new object[0]);
                            }
                        }
                        if (newtweets != null)
                        {
                            foreach (MetroTwitStatusBase base4 in newtweets.Where<MetroTwitStatusBase>(((Func<MetroTwitStatusBase, int, bool>)((x, r) => (x.RetweetUser != null)))).ToArray<MetroTwitStatusBase>())
                            {
                                if (App.AppState.Accounts[base.TwitterAccountID].Cache.NoRetweetIds.Contains((long)base4.RetweetUser.Id))
                                {
                                    newtweets.Remove(base4);
                                }
                            }
                        }
                        if (((tweet != null) && (tweet.RetweetUser != null)) && App.AppState.Accounts[base.TwitterAccountID].Cache.NoRetweetIds.Contains((long)tweet.RetweetUser.Id))
                        {
                            tweet = null;
                        }
                        else if ((tweet != null) && this.Tweets.Contains(tweet))
                        {
                            tweet = null;
                        }
                        else
                        {
                            this.LastCollectionState = this.InitialForeverScroll ? RefreshTypes.InitialLoadForeverScroll : RefreshType;
                            if (RefreshType != RefreshTypes.ForeverScroll)
                            {
                                if (tweet != null)
                                {
                                    this.Tweets.Insert((this.TweetType != TweetListType.RetweetUsers) ? 0 : 1, tweet);
                                }
                                else
                                {
                                    this.Tweets.InsertRange<MetroTwitStatusBase>(newtweets);
                                }
                            }
                            else if (tweet != null)
                            {
                                this.AddTweet(tweet);
                            }
                            else
                            {
                                lock (this._tweetsLock)
                                {
                                    this.Tweets.AddRange<MetroTwitStatusBase>(newtweets);
                                }
                            }
                            if (action3 == null)
                            {
                                action3 = delegate
                                {
                                    base.RaisePropertyChanged("TweetCount");
                                    if ((tweet != null) && tweet.UnRead)
                                    {
                                        this.MakeSomeNoise(tweet, this.TweetListName, RefreshType);
                                    }
                                    if ((newtweets != null) && (newtweets.Count > 0))
                                    {
                                        this.MakeSomeNoise(newtweets.Where<MetroTwitStatusBase>((Func<MetroTwitStatusBase, int, bool>)((x, r) => x.UnRead)), this.TweetListName, RefreshType);
                                    }
                                    if (RefreshType != RefreshTypes.ForeverScroll)
                                    {
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(true), ViewModelMessages.TweetsUpdated.ToString() + this.UniqueTweetListID.ToString());
                                    }
                                };
                            }
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(action3, DispatcherPriority.Background, new object[0]);
                            if (((RefreshType == RefreshTypes.ForeverScroll) && (this.TweetType != TweetListType.Conversation)) && (this.TweetType != TweetListType.RetweetUsers))
                            {
                                this.ForeverScrolling = false;
                                this.InitialForeverScroll = false;
                            }
                            if ((RefreshType == RefreshTypes.Normal) && (this.Tweets.Count<MetroTwitStatusBase>() < 50))
                            {
                                this.InitialForeverScroll = true;
                                this.ForeverScroll();
                            }
                            if (this.TweetType != TweetListType.RetweetUsers)
                            {
                                this.Tweets.Sort<decimal>(x => x.ID, ListSortDirection.Descending);
                                this.UpdateReadTweets(null);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }


        private MetroTwitStatusBase FilterTweets(MetroTwitStatusBase tweet)
        {
            if (this.FilteringRemoveTweet(tweet, SettingsData.Instance.Filter) || this.FilteringRemoveTweet(tweet, App.AppState.Accounts[this.TwitterAccountID].Settings.Filter))
                return (MetroTwitStatusBase)null;
            else
                return tweet;
        }

        private bool FilteringRemoveTweet(MetroTwitStatusBase tweet, List<string> filter)
        {
            using (List<string>.Enumerator enumerator = filter.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Func<UrlEntity, bool> predicate = null;
                    string f = enumerator.Current;
                    if (f.StartsWith(":") && tweet.Source.Contains(f.Replace(":", ""), StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    if (f.StartsWith("@"))
                    {
                        string str = string.Empty;
                        string str2 = string.Empty;
                        if (f.Contains(":"))
                        {
                            string[] strArray = f.Split(new char[] { ':' });
                            str = strArray[0];
                            str2 = strArray[1];
                        }
                        else
                        {
                            str = f;
                        }
                        if ((tweet.User.ScreenName.ToLower() == str.Replace("@", "").ToLower()) || ((tweet.RetweetUser != null) && (tweet.RetweetUser.ScreenName.ToLower() == str.Replace("@", "").ToLower())))
                        {
                            if (str2 == string.Empty)
                            {
                                return true;
                            }
                            if (tweet.Source.Contains(str2.Replace(":", ""), StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                    if (!f.StartsWith("@") && !f.StartsWith(":"))
                    {
                        if (tweet.Text.Contains(f, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                        if (tweet.Entities != null)
                        {
                            if (predicate == null)
                            {
                                predicate = en => (!string.IsNullOrEmpty(en.DisplayUrl) && en.DisplayUrl.Contains(f)) || (string.IsNullOrEmpty(en.DisplayUrl) && en.Url.Contains(f));
                            }
                            if (tweet.Entities.OfType<UrlEntity>().Cast<UrlEntity>().Where<UrlEntity>(predicate).Count<UrlEntity>() > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private List<MetroTwitStatusBase> FilterTweets(List<MetroTwitStatusBase> newtweets)
        {
            List<MetroTwitStatusBase> list = newtweets;
            foreach (MetroTwitStatusBase tweet in newtweets.ToArray())
            {
                if (this.FilteringRemoveTweet(tweet, SettingsData.Instance.Filter) || this.FilteringRemoveTweet(tweet, App.AppState.Accounts[this.TwitterAccountID].Settings.Filter))
                    list.Remove(tweet);
            }
            return list;
        }

        private void MakeSomeNoise(MetroTwitStatusBase Tweet, string Name, RefreshTypes RefreshType)
        {
            this.MakeSomeNoise((IEnumerable<MetroTwitStatusBase>)new Collection<MetroTwitStatusBase>()
      {
        Tweet
      }, Name, RefreshType);
        }

        private void MakeSomeNoise(IEnumerable<MetroTwitStatusBase> NewTweets, string Name, RefreshTypes RefreshType)
        {
            if (!this.EnableNotifications)
                return;
            bool showtoasts = RefreshType != RefreshTypes.ForeverScroll && SettingsData.Instance.ShowNotificationToasts && this.ToastNotification;
            bool playsound = RefreshType != RefreshTypes.ForeverScroll && SettingsData.Instance.UseNotificationSound && this.SoundNotification;
            NotificationView.Notify(Enumerable.ToList<MetroTwitStatusBase>(NewTweets), Name, showtoasts, playsound);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
        }

        public void ForeverScroll()
        {
            if (this.ForeverScrolling || this.TweetType == TweetListType.Conversation || this.TweetType == TweetListType.RetweetUsers)
                return;
            this.ForeverScrolling = true;
            switch (this.DisplayType)
            {
                case DisplayType.Tweets:
                    Task.Run((Action)(() => TwitterREST.TwitterRest(this.UniqueTweetListID, this.TweetType, this.TwitterAccountID, RefreshTypes.ForeverScroll, (Action)null, this.TwitterTerm, new Decimal(0), new Decimal(0), this.Tweets == null || this.Tweets.Count <= 0 ? this.CurrentTweetID : Enumerable.Last<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID, this.ListRetweets)));
                    break;
                case DisplayType.Followers:
                    this.Followers.ForeverScroll();
                    break;
                case DisplayType.Following:
                    this.Following.ForeverScroll();
                    break;
                case DisplayType.ListedIn:
                    this.ListedIn.ForeverScroll();
                    break;
            }
        }

        public void RemoveOldTweets()
        {
            if (this.Tweets == null || this.Tweets.Count <= 0)
                return;
            IOrderedEnumerable<MetroTwitStatusBase> orderedEnumerable = Enumerable.OrderBy<MetroTwitStatusBase, DateTime>(Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToArray<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.CreatedDate < DateTime.Now.AddMinutes(-10.0))), (Func<MetroTwitStatusBase, DateTime>)(z => z.CreatedDate));
            if (orderedEnumerable != null)
            {
                foreach (MetroTwitStatusBase tweet in (IEnumerable<MetroTwitStatusBase>)orderedEnumerable)
                {
                    if (this.Tweets.Count > 50 && tweet != null)
                        this.RemoveTweet(tweet);
                    else
                        break;
                }
            }
        }

        public void UpdateTweetDetails()
        {
            if (this.Tweets == null || this.Tweets.Count <= 0)
                return;
            foreach (MetroTwitStatusBase metroTwitStatusBase in (Collection<MetroTwitStatusBase>)this.Tweets)
                metroTwitStatusBase.UpdateTime();
        }

        public void UpdateReadTweets(Decimal? OverrideID = null)
        {
            Decimal ReadID = !OverrideID.HasValue ? this.CurrentTweetID : OverrideID.Value;
            if (this.Tweets == null || this.Tweets.Count <= 0)
                return;
            foreach (MetroTwitStatusBase metroTwitStatusBase in Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToList<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, int, bool>)((x, r) => x != null && x.ID <= ReadID && x.UnRead)))
                metroTwitStatusBase.UnRead = false;
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                base.RaisePropertyChanged("TweetCount");
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.OverlayCountRecalc);
            }), DispatcherPriority.Background, new object[0]);
        }

        public void ShowAd(MetroTwitStatusBase Ad)
        {
            this.Ad = (MetroTwitStatusBase)null;
            this.ShowingAd = true;
            this.Ad = Ad;
        }

        internal void CloseAd()
        {
            this.ShowingAd = false;
            this.Ad = (MetroTwitStatusBase)null;
        }

        private void AddToList()
        {
            SelectTwitterListView selectTwitterListView = new SelectTwitterListView();
            selectTwitterListView.DataContext = (object)new SelectTwitterListViewModel(this.TwitterAccountID, this.User);
            Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)selectTwitterListView), (object)ViewModelMessages.ShowCenterModalWindowHost);
        }

        private void RefreshExecute()
        {
            this.Refresh(App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.UserStream.Stopped);
        }

        private void Refresh(bool streamdown)
        {
            if (!this.IsPaused && this.Tweets != null && App.AppState.Accounts[this.TwitterAccountID].RateLimitsLeft(this.TweetType) > 0 && (this.TweetType == TweetListType.List || this.TweetType == TweetListType.UserTimeline || (this.TweetType == TweetListType.Search || this.TweetType == TweetListType.Favourites) || (streamdown || this.StreamPreviouslyDown)))
            {
                Decimal currentid = this.Tweets.Count > 0 ? Enumerable.First<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID : new Decimal(0);
                Decimal lastid = this.Tweets.Count > 0 ? Enumerable.Last<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID : new Decimal(0);
                RefreshTypes rType = this.errorStateIntialLoad ? RefreshTypes.Normal : RefreshTypes.Refresh;
                Task.Run((Action)(() => TwitterREST.TwitterRest(this.UniqueTweetListID, this.TweetType, this.TwitterAccountID, rType, (Action)(() =>
                {
                    if (this.ShowLoading)
                        return;
                    this.ShowContentPanel();
                }), this.TwitterTerm, new Decimal(0), currentid, lastid, this.ListRetweets)));
            }
            this.StreamPreviouslyDown = streamdown;
            if (this.ScrollNearTop)
                this.RemoveOldTweets();
            this.UpdateTweetDetails();
        }

        private void DoNothing()
        {
        }

        private void Flush(bool Override = false)
        {
            if ((this.backlog == null || this.IsPaused) && !Override)
                return;
            try
            {
                if (this.Tweets != null && this.Tweets.Count > 0)
                {
                    IEnumerable<MetroTwitStatusBase> enumerable = Enumerable.Where<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)Enumerable.ToArray<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets), (Func<MetroTwitStatusBase, bool>)(x => x.TweetListSpecial == TweetListSpecial.Backlog));
                    if (enumerable != null)
                    {
                        foreach (MetroTwitStatusBase tweet in enumerable)
                        {
                            if (tweet != null)
                                this.RemoveTweet(tweet);
                        }
                    }
                }
            }
            catch
            {
            }
            this.TweetsUpdated((MetroTwitStatusBase)null, this.backlog, RefreshTypes.Refresh, true);
            if (this.backlog != null && this.backlog.Count > 0)
                this.backlog.Clear();
            base.RaisePropertyChanged("BacklogCount");
        }

        private void RemoveColumn()
        {
            Messenger.Default.Send<GenericMessage<TweetListViewModel>>(new GenericMessage<TweetListViewModel>(this), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.RemoveColumn));
        }

        private void ExecuteLink(string link)
        {
            App.LastURLClickMousePosition = new Point?(System.Windows.Application.Current.MainWindow.PointToScreen(Mouse.GetPosition((IInputElement)System.Windows.Application.Current.MainWindow)));
            string str = link.Replace("_normal", "");
            CommonCommands.ExecuteContentLink(new UrlEntity()
            {
                Url = str,
                ExpandedUrl = str
            });
        }

        private void OpenLink()
        {
            CommonCommands.OpenLink(this.User.Website);
        }

        private void ProfileLink()
        {
            CommonCommands.OpenLink(string.Format("http://www.twitter.com/{0}", (object)this.User.ScreenName));
        }

        private void MarkasRead()
        {
            if (this.Tweets == null || this.Tweets.Count <= 0)
                return;
            this.UpdateReadTweets(new Decimal?(Enumerable.First<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID));
            this.CurrentTweetID = Enumerable.First<MetroTwitStatusBase>((IEnumerable<MetroTwitStatusBase>)this.Tweets).ID;
        }

        private async void Follow()
        {
            if (this.FollowButtonText == "follow")
            {
                this.FollowButtonIsEnabled = false;
                this.FollowButtonText = "following...";
            }
            else if (this.FollowButtonText == "unfollow")
            {
                this.FollowButtonIsEnabled = false;
                this.FollowButtonText = "unfollowing...";
            }
            bool? success = await CommonCommands.Follow(this.AdFollowUrl, this.User, this.TwitterAccountID, this.FollowButtonText);
            if (success.HasValue)
            {
                if (this.FollowButtonText == "following...")
                {
                    if (success.Value)
                    {
                        this.FollowButtonText = "unfollow";
                        this.FollowButtonIsEnabled = true;
                        this.Refresh(App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.UserStream.Stopped);
                    }
                    else
                    {
                        this.FollowButtonText = "follow";
                        this.FollowButtonIsEnabled = true;
                    }
                }
                else if (this.FollowButtonText == "unfollowing...")
                {
                    if (success.Value)
                    {
                        this.FollowButtonText = "follow";
                        this.FollowButtonIsEnabled = true;
                        this.Refresh(App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.UserStream.Stopped);
                    }
                    else
                    {
                        this.FollowButtonText = "unfollow";
                        this.FollowButtonIsEnabled = true;
                    }
                }
            }
        }

        private void DirectMessage()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)this.User.ScreenName), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.DirectMessage));
        }

        private void Mention()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)this.User.ScreenName), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.Mention));
        }

        private void Block()
        {
            if (this.IsBlocked)
                CommonCommands.UnBlock((long)this.User.Id, this.TwitterAccountID);
            else
                CommonCommands.Block((long)this.User.Id, this.TwitterAccountID);
        }

        private void ReportSpam()
        {
            if (this.IsBlocked)
                return;
            CommonCommands.ReportSpam(this.User.Id, this.TwitterAccountID);
        }

        private void NoRetweets()
        {
            if (this.User == null)
                return;
            bool showretweet = App.AppState.Accounts[this.TwitterAccountID].Cache.NoRetweetIds.Contains((long)this.User.Id);
            Friendship.UpdateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, this.User.Id, new UpdateFriendshipOptions()
            {
                ShowRetweets = new bool?(showretweet)
            }).ContinueWith((Action<Task<TwitterResponse<TwitterRelationship>>>)(r =>
            {
                if (r.Result.Result != RequestResult.Success)
                    return;
                if (showretweet)
                    App.AppState.Accounts[this.TwitterAccountID].Cache.RemoveNoRetweetsUser((long)this.User.Id);
                else
                    App.AppState.Accounts[this.TwitterAccountID].Cache.AddNoRetweetsUser((long)this.User.Id);
                base.RaisePropertyChanged("NoRetweetsText");
            }));
        }

        private void ColumnSettings()
        {
            this.ColumnBackVisible = !this.ColumnBackVisible;
            base.RaisePropertyChanged("UseNotificationSound");
            base.RaisePropertyChanged("ShowNotificationToasts");
            base.RaisePropertyChanged("ShowTaskbarCount");
        }

        private void UnpauseColumn()
        {
            this.IsPaused = false;
            this.Flush(true);
            base.RaisePropertyChanged("ColumnPausedVisibility");
        }

        public override void Cleanup()
        {
            base.Cleanup();
            if (this.ShowingAd)
                this.CloseAd();
            Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)(((object)ViewModelMessages.RestUpdate).ToString() + this.UniqueTweetListID.ToString()), new Action<GenericMessage<object>>(this.RestUpdate));
            Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.DeleteTweet), new Action<GenericMessage<object>>(this.DeleteTweet));
            Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.DeleteUserTweets), new Action<GenericMessage<object>>(this.DeleteUserTweets));
            Messenger.Default.Unregister<GenericMessage<KeyValuePair<Decimal, bool>>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.Blocked), new Action<GenericMessage<KeyValuePair<Decimal, bool>>>(this.Blocked));
            if (this.TweetType != TweetListType.DirectMessages)
            {
                if (this.TweetType == TweetListType.FriendsTimeline || this.TweetType == TweetListType.MyTweets || this.TweetType == TweetListType.MentionsMyTweetsRetweeted)
                {
                    Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingStatusReceived), new Action<GenericMessage<object>>(this.StreamingUpdate));
                    Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingStatusDeleted), new Action<GenericMessage<object>>(this.StreamingDelete));
                }
                Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.RemoveFavouriteTweet), new Action<GenericMessage<object>>(this.RemoveFavouriteTweet));
                Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.FavouriteTweet), new Action<GenericMessage<object>>(this.FavouriteTweet));
            }
            else
            {
                Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingDirectMessage), new Action<GenericMessage<object>>(this.StreamingDMUpdate));
                Messenger.Default.Unregister<GenericMessage<object>>((object)this, (object)this.MultiAccountifyToken((Enum)ViewModelMessages.StreamingDirectMessageDeleted), new Action<GenericMessage<object>>(this.StreamingDMDelete));
            }
            if (this.Tweets != null)
                this.Tweets.Clear();
            this.User = (MetroTwitUser)null;
            this.Tweets = (SortableObservableCollection<MetroTwitStatusBase>)null;
        }

        private void ResetColumnWidth()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)this), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.ResetColumnWidth));
            this.ResetWidthVisibility = false;
        }

        internal void ChangetoSavedColumn()
        {
            this.Settings.ColumnPinned = true;
            if (!App.AppState.Accounts[this.TwitterAccountID].Settings.Columns.Contains(this.Settings))
                App.AppState.Accounts[this.TwitterAccountID].Settings.Columns.Add(this.Settings);
            if (!this.Settings.ColumnIsSetWidth)
                return;
            this.ResetColumnWidth();
        }

        internal void LoadDataIfRequired()
        {
            switch (this.DisplayType)
            {
                case DisplayType.Followers:
                    if (this.Followers != null)
                        break;
                    this.Followers = new TweetListViewModel(Guid.NewGuid(), TweetListType.Followers, this.TwitterAccountID, new MetroTwitColumn(), "", this.TwitterTerm, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
                    break;
                case DisplayType.Following:
                    if (this.Following != null)
                        break;
                    this.Following = new TweetListViewModel(Guid.NewGuid(), TweetListType.Following, this.TwitterAccountID, new MetroTwitColumn(), "", this.TwitterTerm, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
                    break;
                case DisplayType.ListedIn:
                    if (this.ListedIn != null)
                        break;
                    this.ListedIn = new TweetListViewModel(Guid.NewGuid(), TweetListType.ListedIn, this.TwitterAccountID, new MetroTwitColumn(), "", this.TwitterTerm, (string)null, new Decimal?(), false, (MetroTwitStatusBase)null, new Decimal(0));
                    break;
            }
        }
    }
}
