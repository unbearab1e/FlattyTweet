// Type: MetroTwit.ViewModel.MetroTwitStatusBase
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class MetroTwitStatusBase : ViewModelBase
  {
    private bool firstRead = false;
    private TweetListSpecial tweetListSpecial = TweetListSpecial.NotSpecial;
    private bool replyOptionsEnabled = true;
    private bool unRead;
    private bool isFavourited;
    private bool directmessageEnabled;
    private bool replyEnabled;
    private bool retweetEnabled;
    private bool deleteTweetEnabled;
    private bool isBlocked;
    private bool spamEnabled;
    private bool blockEnabled;
    private bool canUndoTweet;

      private Decimal twitterAccountID;
    public Decimal TwitterAccountID
    {
      get
      {
        return this.twitterAccountID;
      }
      set
      {
        if (Decimal.Equals(this.twitterAccountID, value))
          return;
        this.twitterAccountID = value;
        base.RaisePropertyChanged("UserImage");
        base.RaisePropertyChanged("TwitterAccountID");
      }
    }
      private EntityCollection entities;
    public EntityCollection Entities
    {
      get
      {
        return this.entities;
      }
      set
      {
        if (this.entities == value)
          return;
        this.entities = value;
        base.RaisePropertyChanged("Entities");
      }
    }
      private TweetListType tweetType;
    public TweetListType TweetType
    {
      get
      {
        return this.tweetType;
      }
      set
      {
        if (this.tweetType == value)
          return;
        this.tweetType = value;
        base.RaisePropertyChanged("TweetType");
      }
    }
      private Decimal id;
    public Decimal ID
    {
      get
      {
        return this.id;
      }
      set
      {
        if (Decimal.Equals(this.id, value))
          return;
        this.id = value;
        base.RaisePropertyChanged("Permalink");
        base.RaisePropertyChanged("ID");
      }
    }
      private decimal originalID;
    public Decimal OriginalID
    {
      get
      {
        return this.originalID;
      }
      set
      {
        if (Decimal.Equals(this.originalID, value))
          return;
        this.originalID = value;
        base.RaisePropertyChanged("OriginalID");
      }
    }
      private string rawText;
    public string RawText
    {
      get
      {
        return this.rawText;
      }
      set
      {
        if (string.Equals(this.rawText, value, StringComparison.Ordinal))
          return;
        this.rawText = value;
        base.RaisePropertyChanged("Text");
        base.RaisePropertyChanged("RawText");
      }
    }

    public string Text
    {
      get
      {
        return WebUtility.HtmlDecode(this.RawText);
      }
    }

      private MetroTwitUser user;
    [XmlIgnore]
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
        base.RaisePropertyChanged("UserImage");
        base.RaisePropertyChanged("AdImage");
        base.RaisePropertyChanged("Permalink");
        base.RaisePropertyChanged("User");
      }
    }

      private string source;
    public string Source
    {
      get
      {
        return this.source;
      }
      set
      {
        if (string.Equals(this.source, value, StringComparison.Ordinal))
          return;
        this.source = value;
        base.RaisePropertyChanged("SourceLink");
        base.RaisePropertyChanged("SourceName");
        base.RaisePropertyChanged("Source");
      }
    }

      private DateTime createdDate;
    public DateTime CreatedDate
    {
      get
      {
        return this.createdDate;
      }
      set
      {
        if (DateTime.Equals(this.createdDate, value))
          return;
        this.createdDate = value;
        base.RaisePropertyChanged("CreatedDateString");
        base.RaisePropertyChanged("CreatedDateFullString");
        base.RaisePropertyChanged("CreatedDate");
      }
    }

      private string inReplyToScreenName;
    public string InReplyToScreenName
    {
      get
      {
        return this.inReplyToScreenName;
      }
      set
      {
        if (string.Equals(this.inReplyToScreenName, value, StringComparison.Ordinal))
          return;
        this.inReplyToScreenName = value;
        base.RaisePropertyChanged("InReplyToScreenName");
      }
    }

      private Decimal inReplyTo;
    public Decimal InReplyTo
    {
      get
      {
        return this.inReplyTo;
      }
      set
      {
        if (Decimal.Equals(this.inReplyTo, value))
          return;
        this.inReplyTo = value;
        base.RaisePropertyChanged("InReplyTo");
      }
    }

      private Coordinates coordinates;
    public Coordinates Coordinates
    {
      get
      {
        return this.coordinates;
      }
      set
      {
        if (this.coordinates == value)
          return;
        this.coordinates = value;
        base.RaisePropertyChanged("Coordinates");
      }
    }

      private Place geo;
    public Place Geo
    {
      get
      {
        return this.geo;
      }
      set
      {
        if (this.geo == value)
          return;
        this.geo = value;
        base.RaisePropertyChanged("Geo");
      }
    }

      private Visibility geoVisibility;
    public Visibility GeoVisibility
    {
      get
      {
        return this.geoVisibility;
      }
      set
      {
        if (this.geoVisibility == value)
          return;
        this.geoVisibility = value;
        base.RaisePropertyChanged("GeoVisibility");
      }
    }

    public Visibility ReplyToVisibility { get; set; }

      private Visibility favouriteVisible;
    public Visibility FavouriteVisible
    {
      get
      {
        return this.favouriteVisible;
      }
      set
      {
        if (this.favouriteVisible == value)
          return;
        this.favouriteVisible = value;
        base.RaisePropertyChanged("FavouriteVisible");
      }
    }

      private Visibility retweetVisibility;
    public Visibility RetweetVisibility
    {
      get
      {
        return this.retweetVisibility;
      }
      set
      {
        if (this.retweetVisibility == value)
          return;
        this.retweetVisibility = value;
        base.RaisePropertyChanged("RetweetVisibility");
      }
    }

      private Visibility retweetCountVisibility;
    public Visibility RetweetCountVisibility
    {
      get
      {
        return this.retweetCountVisibility;
      }
      set
      {
        if (this.retweetCountVisibility == value)
          return;
        this.retweetCountVisibility= value;
        base.RaisePropertyChanged("RetweetCountVisibility");
      }
    }

      private string retweetText;
    public string RetweetText
    {
      get
      {
        return this.retweetText;
      }
      set
      {
        if (string.Equals(this.retweetText, value, StringComparison.Ordinal))
          return;
        this.retweetText = value;
        base.RaisePropertyChanged("RetweetText");
      }
    }

      private string dMText;
    public string DMText
    {
      get
      {
        return this.dMText;
      }
      set
      {
        if (string.Equals(this.dMText, value, StringComparison.Ordinal))
          return;
        this.dMText = value;
        base.RaisePropertyChanged("DMText");
      }
    }

      private ActionUrls adUrls;
    public ActionUrls AdUrls
    {
      get
      {
        return this.adUrls;
      }
      set
      {
        if (this.adUrls == value)
          return;
        this.adUrls = value;
        base.RaisePropertyChanged("AdUrls");
      }
    }

      private MetroTwitUser retweetUser;
    [XmlIgnore]
    public MetroTwitUser RetweetUser
    {
      get
      {
        return this.retweetUser;
      }
      set
      {
        if (this.retweetUser == value)
          return;
        this.retweetUser = value;
        base.RaisePropertyChanged("RetweetUser");
      }
    }

      private bool retweetPiP;
    public bool RetweetPiP
    {
      get
      {
        return this.retweetPiP;
      }
      set
      {
        if (this.retweetPiP == value)
          return;
        this.retweetPiP = value;
        base.RaisePropertyChanged("RetweetPiP");
      }
    }

      private bool isRetweet;
    public bool IsRetweet
    {
      get
      {
        return this.isRetweet;
      }
      set
      {
        if (this.isRetweet == value)
          return;
        this.isRetweet = value;
        base.RaisePropertyChanged("IsRetweet");
      }
    }

      private Visibility yourRetweetVisibility;
    public Visibility YourRetweetVisibility
    {
      get
      {
        return this.yourRetweetVisibility;
      }
      set
      {
        if (this.yourRetweetVisibility == value)
          return;
        this.yourRetweetVisibility  = value;
        base.RaisePropertyChanged("YourRetweetVisibility");
      }
    }

      private bool isSelfTweet;
    public bool IsSelfTweet
    {
      get
      {
        return this.isSelfTweet;
      }
      set
      {
        if (this.isSelfTweet == value)
          return;
        this.isSelfTweet = value;
        base.RaisePropertyChanged("IsSelfTweet");
      }
    }

    public TweetListSpecial TweetListSpecial
    {
      get
      {
        return this.tweetListSpecial;
      }
      set
      {
        if (this.tweetListSpecial == value)
          return;
        this.tweetListSpecial = value;
        base.RaisePropertyChanged("TweetListSpecial");
      }
    }

    public bool UnRead
    {
      get
      {
        return this.unRead;
      }
      set
      {
        if (this.unRead == value)
          return;
        this.unRead = value;
        base.RaisePropertyChanged("UnRead");
      }
    }

    public string FavouriteText
    {
      get
      {
        return this.IsFavourited ? "Unfavorite tweet" : "Favorite tweet";
      }
    }

    public bool IsFavourited
    {
      get
      {
        return this.isFavourited;
      }
      set
      {
        if (this.isFavourited == value)
          return;
        this.isFavourited = value;
        base.RaisePropertyChanged("IsFavourited");
        base.RaisePropertyChanged("FavouriteText");
      }
    }

    public bool DirectMessageEnabled
    {
      get
      {
        return this.directmessageEnabled;
      }
      set
      {
        if (this.directmessageEnabled == value)
          return;
        this.directmessageEnabled = value;
        base.RaisePropertyChanged("DirectMessageEnabled");
      }
    }

    public bool ReplyEnabled
    {
      get
      {
        return this.replyEnabled;
      }
      set
      {
        if (this.replyEnabled == value)
          return;
        this.replyEnabled = value;
        base.RaisePropertyChanged("ReplyEnabled");
      }
    }

    public bool RetweetEnabled
    {
      get
      {
        return this.retweetEnabled;
      }
      set
      {
        if (this.retweetEnabled == value)
          return;
        this.retweetEnabled = value;
        base.RaisePropertyChanged("RetweetEnabled");
      }
    }

    public bool DeleteTweetEnabled
    {
      get
      {
        return this.deleteTweetEnabled;
      }
      set
      {
        if (this.deleteTweetEnabled == value)
          return;
        this.deleteTweetEnabled = value;
        base.RaisePropertyChanged("DeleteTweetEnabled");
      }
    }

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

    public BitmapImage UserImage
    {
        get
        {
            object result;
            if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
            {
                result = App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
            }
            else
            {
                if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
                {
                    result = App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
                }
                else
                {
                    result = null;
                }
            }
            return (BitmapImage) result;
        }
    }

    public BitmapImage UserDefaultImage
    {
      get
      {
        return CommonCommands.DefaultUserImage();
      }
    }

    public BitmapImage AdImage
    {
      get
      {
        if (this.User != null && !string.IsNullOrEmpty(this.User.ProfileImageLocation))
        {
          byte[] buffer = CommonCommands.DownloadFile(this.User.ProfileImageLocation);
          if (buffer != null)
          {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.StreamSource = (Stream) new MemoryStream(buffer);
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
          }
        }
        return (BitmapImage) null;
      }
    }

    public string CreatedDateString
    {
      get
      {
        DateTime dateTime1;
        int num1;
        if (SettingsData.Instance.TwitterTimeDisplay != TwitterTimeDisplay.Relative)
        {
          if (SettingsData.Instance.TwitterTimeDisplay == TwitterTimeDisplay.RelativeAbsolute)
          {
            dateTime1 = this.CreatedDate;
            DateTime dateTime2 = dateTime1.ToLocalTime();
            dateTime1 = DateTime.Now;
            DateTime dateTime3 = dateTime1.AddHours(-6.0);
            if (dateTime2 > dateTime3)
              goto label_6;
          }
          if (SettingsData.Instance.TwitterTimeDisplay == TwitterTimeDisplay.AbsoluteRelative)
          {
            dateTime1 = this.CreatedDate;
            DateTime dateTime2 = dateTime1.ToLocalTime();
            dateTime1 = DateTime.Now;
            DateTime dateTime3 = dateTime1.AddHours(-6.0);
            num1 = !(dateTime2 < dateTime3) ? 1 : 0;
            goto label_7;
          }
          else
          {
            num1 = 1;
            goto label_7;
          }
        }
label_6:
        num1 = 0;
label_7:
        if (num1 == 0)
          return new DateToHumanReadableConverter().Convert((object) this.CreatedDate, typeof (string), (object) null, CultureInfo.CurrentCulture).ToString();
        DateTime now = DateTime.Now;
        dateTime1 = this.CreatedDate;
        DateTime dateTime4 = dateTime1.ToLocalTime();
        TimeSpan timeSpan = now - dateTime4;
        string str1 = string.Empty;
        string format = CultureInfo.CurrentCulture.DateTimeFormat.MonthDayPattern.Replace("MMMM", "MMM");
        int num2;
        if (timeSpan.Days <= 0)
        {
          dateTime1 = DateTime.Now - timeSpan;
          DateTime date1 = dateTime1.Date;
          dateTime1 = DateTime.Now;
          DateTime date2 = dateTime1.Date;
          num2 = !(date1 != date2) ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 == 0)
        {
          dateTime1 = this.CreatedDate;
          dateTime1 = dateTime1.ToLocalTime();
          str1 = dateTime1.ToString(format) + " ";
        }
        string str2 = str1;
        dateTime1 = this.CreatedDate;
        dateTime1 = dateTime1.ToLocalTime();
        string str3 = dateTime1.ToShortTimeString();
        return str2 + str3;
      }
    }

    public string CreatedDateFullString
    {
      get
      {
        DateTime dateTime = this.CreatedDate;
        dateTime = dateTime.ToLocalTime();
        string str1 = dateTime.ToLongDateString();
        string str2 = " ";
        dateTime = this.CreatedDate;
        dateTime = dateTime.ToLocalTime();
        string str3 = dateTime.ToLongTimeString();
        return str1 + str2 + str3;
      }
    }

    public string Permalink
    {
      get
      {
        return string.Format("http://www.twitter.com/{0}/status/{1}", (object) this.User.ScreenName, (object) this.ID);
      }
    }

    public string SourceLink
    {
      get
      {
        if (string.IsNullOrEmpty(this.Source))
          return string.Empty;
        Match match = RegularExpressions.HTML_ANCHOR_EXPRESSION.Match(this.Source);
        if (match.Success)
          return match.Groups[1].Value;
        if (this.Source.ToLower() == "web")
          return "http://www.twitter.com";
        else
          return string.Empty;
      }
    }

    public string SourceName
    {
      get
      {
        if (string.IsNullOrEmpty(this.Source))
          return string.Empty;
        Match match = RegularExpressions.HTML_ANCHOR_EXPRESSION.Match(this.Source);
        if (match.Success)
          return match.Groups[2].Value;
        if (this.Source.ToLower() == "web")
          return this.Source;
        else
          return string.Empty;
      }
    }

    public Visibility TimeVisibility
    {
      get
      {
        return SettingsData.Instance.ShowTimeofTweet ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility SourceVisibility
    {
      get
      {
        return SettingsData.Instance.ShowSourceofTweet ? Visibility.Visible : Visibility.Collapsed;
      }
    }

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
        base.RaisePropertyChanged("IsBlocked");
        base.RaisePropertyChanged("BlockedText");
      }
    }

    public string BlockedText
    {
      get
      {
        return this.isBlocked ? "Unblock user" : "Block user";
      }
    }

    public bool FirstRead
    {
      get
      {
        return this.firstRead;
      }
      set
      {
        if (this.firstRead == value)
          return;
        this.firstRead = value;
        base.RaisePropertyChanged("FirstRead");
      }
    }

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

    public bool CanUndoTweet
    {
      get
      {
        return this.canUndoTweet;
      }
      set
      {
        if (this.canUndoTweet == value)
          return;
        this.canUndoTweet = value;
        base.RaisePropertyChanged("CanUndoTweet");
      }
    }

      private RelayCommand<string> tagsCommand;
    [XmlIgnore]
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

      private RelayCommand<string>  userProfileCommand;
    [XmlIgnore]
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

      private RelayCommand<MetroTwitStatusBase> adProfileCommand;
    [XmlIgnore]
    public RelayCommand<MetroTwitStatusBase> AdProfileCommand
    {
      get
      {
        return this.adProfileCommand;
      }
      private set
      {
        if (this.adProfileCommand == value)
          return;
        this.adProfileCommand = value;
        base.RaisePropertyChanged("AdProfileCommand");
      }
    }

      private RelayCommand<object> linkCommand;
    [XmlIgnore]
    public RelayCommand<object> LinkCommand
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

      private RelayCommand retweetCommand;
    [XmlIgnore]
    public RelayCommand RetweetCommand
    {
      get
      {
        return this.retweetCommand;
      }
      private set
      {
        if (this.retweetCommand == value)
          return;
        this.retweetCommand = value;
        base.RaisePropertyChanged("RetweetCommand");
      }
    }

      private RelayCommand directMessageCommand;
    [XmlIgnore]
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

      private RelayCommand replyCommand;
    [XmlIgnore]
    public RelayCommand ReplyCommand
    {
      get
      {
        return this.replyCommand;
      }
      private set
      {
        if (this.replyCommand == value)
          return;
        this.replyCommand = value;
        base.RaisePropertyChanged("ReplyCommand");
      }
    }

      private RelayCommand replyAllCommand;
    [XmlIgnore]
    public RelayCommand ReplyAllCommand
    {
      get
      {
        return this.replyAllCommand;
      }
      private set
      {
        if (this.replyAllCommand == value)
          return;
        this.replyAllCommand = value;
        base.RaisePropertyChanged("ReplyAllCommand");
      }
    }

      private RelayCommand geoCommand;
    [XmlIgnore]
    public RelayCommand GeoCommand
    {
      get
      {
        return this.geoCommand;
      }
      private set
      {
        if (this.geoCommand == value)
          return;
        this.geoCommand = value;
        base.RaisePropertyChanged("GeoCommand");
      }
    }

    private RelayCommand conversationCommand;
    [XmlIgnore]
    public RelayCommand ConversationCommand
    {
      get
      {
        return this.conversationCommand;
      }
      private set
      {
        if (this.conversationCommand  == value)
          return;
        this.conversationCommand  = value;
        base.RaisePropertyChanged("ConversationCommand");
      }
    }

    private RelayCommand deleteTweetCommand;
    [XmlIgnore]
    public RelayCommand DeleteTweetCommand
    {
      get
      {
        return this.deleteTweetCommand ;
      }
      private set
      {
        if (this.deleteTweetCommand == value)
          return;
        this.deleteTweetCommand = value;
        base.RaisePropertyChanged("DeleteTweetCommand");
      }
    }

    private RelayCommand undoTweetCommand;
    [XmlIgnore]
    public RelayCommand UndoTweetCommand
    {
      get
      {
        return this.undoTweetCommand;
      }
      private set
      {
        if (this.undoTweetCommand == value)
          return;
        this.undoTweetCommand = value;
        base.RaisePropertyChanged("UndoTweetCommand");
      }
    }

    private RelayCommand favouriteTweetCommand;
    [XmlIgnore]
    public RelayCommand FavouriteTweetCommand
    {
      get
      {
        return this.favouriteTweetCommand;
      }
      private set
      {
        if (this.favouriteTweetCommand == value)
          return;
        this.favouriteTweetCommand = value;
        base.RaisePropertyChanged("FavouriteTweetCommand");
      }
    }

    private RelayCommand<UrlEntity> contentLinkCommand;
    [XmlIgnore]
    public RelayCommand<UrlEntity> ContentLinkCommand
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

    private RelayCommand blockCommand;
    [XmlIgnore]
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
    [XmlIgnore]
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

    private RelayCommand copyTweetCommand;
    [XmlIgnore]
    public RelayCommand CopyTweetCommand
    {
      get
      {
        return this.copyTweetCommand;
      }
      private set
      {
        if (this.copyTweetCommand == value)
          return;
        this.copyTweetCommand = value;
        base.RaisePropertyChanged("CopyTweetCommand");
      }
    }


    private RelayCommand retweetUsersCommand;
    [XmlIgnore]
    public RelayCommand RetweetUsersCommand
    {
      get
      {
        return this.retweetUsersCommand;
      }
      private set
      {
        if (this.retweetUsersCommand == value)
          return;
        this.retweetUsersCommand = value;
        base.RaisePropertyChanged("RetweetUsersCommand");
      }
    }

    private RelayCommand<UrlEntity> copyUrlCommand;
    [XmlIgnore]
    public RelayCommand<UrlEntity> CopyUrlCommand
    {
      get
      {
        return this.copyUrlCommand;
      }
      private set
      {
        if (this.copyUrlCommand == value)
          return;
        this.copyUrlCommand = value;
        base.RaisePropertyChanged("CopyUrlCommand");
      }
    }


    private RelayCommand<UrlEntity> copyExpandedUrlCommand;
    [XmlIgnore]
    public RelayCommand<UrlEntity> CopyExpandedUrlCommand
    {
      get
      {
        return this.copyExpandedUrlCommand;
      }
      private set
      {
        if (this.copyExpandedUrlCommand == value)
          return;
        this.copyExpandedUrlCommand = value;
        base.RaisePropertyChanged("CopyExpandedUrlCommand");
      }
    }

    private RelayCommand copyTweetLinkCommand;
    [XmlIgnore]
    public RelayCommand CopyTweetLinkCommand
    {
      get
      {
        return this.copyTweetLinkCommand;
      }
      private set
      {
        if (this.copyTweetLinkCommand == value)
          return;
        this.copyTweetLinkCommand = value;
        base.RaisePropertyChanged("CopyTweetLinkCommand");
      }
    }

    private RelayCommand emailTweetCommand;
    [XmlIgnore]
    public RelayCommand EmailTweetCommand
    {
      get
      {
        return this.emailTweetCommand;
      }
      private set
      {
        if (this.emailTweetCommand == value)
          return;
        this.emailTweetCommand = value;
        base.RaisePropertyChanged("EmailTweetCommand");
      }
    }

    private RelayCommand<String> filterCommand;
    [XmlIgnore]
    public RelayCommand<string> FilterCommand
    {
      get
      {
        return this.filterCommand;
      }
      private set
      {
        if (this.filterCommand == value)
          return;
        this.filterCommand = value;
        base.RaisePropertyChanged("FilterCommand");
      }
    }

    private RelayCommand followCommand;
    [XmlIgnore]
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

    private RelayCommand unfollowCommand;
    [XmlIgnore]
    public RelayCommand UnfollowCommand
    {
      get
      {
        return this.unfollowCommand;
      }
      private set
      {
        if (this.unfollowCommand== value)
          return;
        this.unfollowCommand = value;
        base.RaisePropertyChanged("UnfollowCommand");
      }
    }

    public MetroTwitStatusBase()
    {
      this.TagsCommand = new RelayCommand<string>(new Action<string>(this.ExecuteTag));
      this.UserProfileCommand = new RelayCommand<string>(new Action<string>(this.ExecuteUserProfile));
      this.LinkCommand = new RelayCommand<object>(new Action<object>(CommonCommands.ExecuteLink));
      this.ContentLinkCommand = new RelayCommand<UrlEntity>(new Action<UrlEntity>(this.ExecuteContentLink));
      this.RetweetCommand = new RelayCommand(new Action(this.Retweet));
      this.DirectMessageCommand = new RelayCommand(new Action(this.DirectMessage));
      this.ReplyCommand = new RelayCommand(new Action(this.Reply));
      this.ReplyAllCommand = new RelayCommand(new Action(this.ReplyAll));
      this.GeoCommand = new RelayCommand(new Action(this.ShowGeo));
      this.ConversationCommand = new RelayCommand(new Action(this.ShowConversation));
      this.DeleteTweetCommand = new RelayCommand(new Action(this.Delete));
      this.UndoTweetCommand = new RelayCommand(new Action(this.UndoTweet));
      this.FavouriteTweetCommand = new RelayCommand(new Action(this.FavouriteTweet));
      this.BlockCommand = new RelayCommand(new Action(this.Block));
      this.ReportSpamCommand = new RelayCommand(new Action(this.ReportSpam));
      this.CopyTweetCommand = new RelayCommand(new Action(this.CopyTweet));
      this.RetweetUsersCommand = new RelayCommand(new Action(this.RetweetUsers));
      this.CopyUrlCommand = new RelayCommand<UrlEntity>(new Action<UrlEntity>(CommonCommands.CopyUrl));
      this.CopyTweetLinkCommand = new RelayCommand(new Action(this.CopyTweetLink));
      this.EmailTweetCommand = new RelayCommand(new Action(this.EmailTweet));
      this.FilterCommand = new RelayCommand<string>(new Action<string>(this.Filter));
      this.FollowCommand = new RelayCommand(new Action(this.Follow));
      this.UnfollowCommand = new RelayCommand(new Action(this.Unfollow));
    }

    ~MetroTwitStatusBase()
    {
      if (this.Entities == null)
        return;
      this.Entities.Clear();
      this.Entities = (EntityCollection) null;
    }

    public override bool Equals(object obj)
    {
      try
      {
        object obj1 = typeof (BindingExpressionBase).GetField("DisconnectedItem", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue((object) null);
        if (obj != null && obj is MetroTwitStatusBase && (obj1 != null && obj != obj1) && obj != DependencyProperty.UnsetValue)
          return (obj as MetroTwitStatusBase).ID == this.ID;
        else
          return false;
      }
      catch
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return this.ID.GetHashCode();
    }

    private void ExecuteTag(string tag)
    {
      CommonCommands.ExecuteTag(tag, this.TwitterAccountID);
    }

    private void ExecuteUserProfile(string username)
    {
      CommonCommands.ExecuteUserProfile(username, this.TwitterAccountID);
    }

    private void ExecuteContentLink(UrlEntity link)
    {
      App.LastURLClickMousePosition = new Point?(System.Windows.Application.Current.MainWindow.PointToScreen(Mouse.GetPosition((IInputElement) System.Windows.Application.Current.MainWindow)));
      CommonCommands.ExecuteContentLink(link);
    }

    private void Retweet()
    {
      CommonCommands.Retweet(this);
    }

    private void DirectMessage()
    {
      CommonCommands.DirectMessage(this);
    }

    private void Reply()
    {
      CommonCommands.Reply(this);
    }

    private void ReplyAll()
    {
      CommonCommands.ReplyAll(this);
    }

    private void ShowGeo()
    {
      CommonCommands.ShowGeo(this);
    }

    private void ShowConversation()
    {
      CommonCommands.ShowConversation(this);
    }

    private void Delete()
    {
      if (this.TweetType == TweetListType.DirectMessages)
        CommonCommands.DeleteDirectMessage(this.ID, this.TwitterAccountID, (Action<RequestResult>) null, false);
      else
        CommonCommands.DeleteTweet(this.ID, this.TwitterAccountID, (Action<RequestResult>) null, false);
    }

    private void UndoTweet()
    {
      if (this.TweetType == TweetListType.DirectMessages)
        App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.UndoTweet(new UndoTweetState()
        {
          Id = this.ID,
          TweetType = TwitViewModel.NewTweetType.DirectMessage,
          LastTweetText = string.Format("d {0} {1}", (object) this.User.ScreenName, (object) this.Text),
          UserName = this.User.ScreenName
        }, 0 != 0);
      else
        App.AppState.Accounts[this.TwitterAccountID].TwitViewModel.UndoTweet(new UndoTweetState()
        {
          Id = this.ID,
          TweetType = this.InReplyTo > new Decimal(0) ? TwitViewModel.NewTweetType.Reply : TwitViewModel.NewTweetType.Normal,
          LastTweetText = this.Text,
          UserName = this.User.ScreenName,
          ReplyToID = this.InReplyTo
        }, 0 != 0);
    }

    private void FavouriteTweet()
    {
      if (!this.isFavourited)
        CommonCommands.Favourite(this.ID, this.TwitterAccountID);
      else
        CommonCommands.UnFavourite(this.ID, this.TwitterAccountID);
    }

    private void Block()
    {
      if (this.isBlocked)
        CommonCommands.UnBlock((long) this.User.Id, this.TwitterAccountID);
      else
        CommonCommands.Block((long) this.User.Id, this.TwitterAccountID);
    }

    private void ReportSpam()
    {
      if (this.isBlocked)
        return;
      CommonCommands.ReportSpam(this.User.Id, this.TwitterAccountID);
    }

    private void RetweetUsers()
    {
      CommonCommands.ShowRetweetUsers(this.OriginalID, this.TwitterAccountID);
    }

    private void CopyTweet()
    {
      CommonCommands.CopyTweet(this);
    }

    private void CopyTweetLink()
    {
      CommonCommands.CopyText(this.Permalink);
    }

    private void EmailTweet()
    {
      CommonCommands.EmailTweet(this);
    }

    private void Follow()
    {
      CommonCommands.Follow((string) null, this.User, this.TwitterAccountID, "following...");
    }

    private void Unfollow()
    {
      CommonCommands.Follow((string) null, this.User, this.TwitterAccountID, "unfollow");
    }

    public void UpdateTime()
    {
      base.RaisePropertyChanged("CreatedDateString");
      base.RaisePropertyChanged("CreatedDateFullString");
      if (!(this.CreatedDate < DateTime.Now.AddMinutes(-30.0)))
        return;
      this.CanUndoTweet = false;
    }

    public void Filter(string param)
    {
      List<string> list = App.AppState.Accounts[this.TwitterAccountID].Settings.Filter;
      string str = param.StartsWith("@") || param.StartsWith("#") ? param : ":" + param;
      if (list.Contains(str))
        return;
      App.AppState.Accounts[this.TwitterAccountID].Settings.Filter.Add(str);
    }
  }
}
