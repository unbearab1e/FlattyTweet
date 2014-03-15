
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class TwitterStatusExtended : MetroTwitStatusBase
  {
    private Visibility defaultGridVisible = Visibility.Visible;
    private Visibility replyGridVisible = Visibility.Collapsed;

    public object RecipientImage
    {
      get
      {
        if (this.RetweetUser != null)
        {
          if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.RetweetUser.ScreenName.ToLower()))
            return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.RetweetUser.ScreenName.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
          if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.RetweetUser.ScreenName.ToLower()))
            return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.RetweetUser.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
        }
        return (object) null;
      }
    }

    public Visibility DefaultGridVisible
    {
      get
      {
        return this.defaultGridVisible;
      }
      set
      {
        if (this.defaultGridVisible == value)
          return;
        this.defaultGridVisible = value;
        base.RaisePropertyChanged("DefaultGridVisible");
      }
    }

    public Visibility ReplyGridVisible
    {
      get
      {
        return this.replyGridVisible;
      }
      set
      {
        if (this.replyGridVisible == value)
          return;
        this.replyGridVisible = value;
        base.RaisePropertyChanged("ReplyGridVisible");
      }
    }
      private IEnumerable<MediaEntity> media;
    public IEnumerable<MediaEntity> Media
    {
      get
      {
        return this.media;
      }
      set
      {
        if (this.media == value)
          return;
        this.media = value;
        base.RaisePropertyChanged("Media");
      }
    }
      private MediaEntity mediaThumbnail;
    public MediaEntity MediaThumbnail
    {
      get
      {
        return this.mediaThumbnail;
      }
      set
      {
        if (this.mediaThumbnail == value)
          return;
        this.mediaThumbnail = value;
        base.RaisePropertyChanged("MediaThumbnail");
      }
    }
      private bool containsMedia;
    public bool ContainsMedia
    {
      get
      {
        return this.containsMedia;
      }
      set
      {
        if (this.containsMedia == value)
          return;
        this.containsMedia = value;
        base.RaisePropertyChanged("ShouldDisplayPreviewMedia");
        base.RaisePropertyChanged("ContainsMedia");
      }
    }

    public bool ShouldDisplayPreviewMedia
    {
      get
      {
        return this.ContainsMedia && SettingsData.Instance.DisplayImagesInlineAutomatically && !this.PossiblySensitive;
      }
    }
      private bool possiblySensitive;
    public bool PossiblySensitive
    {
      get
      {
        return this.possiblySensitive;
      }
      set
      {
        if (this.possiblySensitive == value)
          return;
        this.possiblySensitive = value;
        base.RaisePropertyChanged("ShouldDisplayPreviewMedia");
        base.RaisePropertyChanged("PossiblySensitive");
      }
    }

    public TwitterStatusExtended(Status twitterStatus, TweetListType TweetType, Decimal TwitterAccountID, Decimal CurrentTweetID)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.DirectMessageEnabled = true;
      this.ReplyEnabled = true;
      this.RetweetEnabled = true;
      this.DeleteTweetEnabled = false;
      if (twitterStatus == null)
        return;
      this.UnRead = twitterStatus.Id > CurrentTweetID;
      this.TweetType = TweetType;
      App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseUser(twitterStatus.User.ScreenName, twitterStatus.User.ProfileImageSecureLocation);
      DateTime createdDate = twitterStatus.CreatedDate;
      bool flag = 1 == 0;
      this.CreatedDate = twitterStatus.CreatedDate;
      this.IsFavourited = twitterStatus.IsFavorited;
      this.DeleteTweetEnabled = twitterStatus.User.ScreenName.ToLower() == App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower();
      this.IsSelfTweet = this.DeleteTweetEnabled;
      this.CanUndoTweet = this.IsSelfTweet && this.CreatedDate >= DateTime.Now.AddMinutes(-30.0) && twitterStatus.RetweetedStatus == null;
      if (twitterStatus.RetweetedStatus == null)
      {
        this.IsRetweet = false;
        this.RetweetVisibility = Visibility.Collapsed;
        this.YourRetweetVisibility = Visibility.Collapsed;
        this.RetweetUser = (MetroTwitUser) null;
        this.RetweetPiP = false;
        this.ID = twitterStatus.Id;
        this.RetweetEnabled = !twitterStatus.User.IsProtected;
      }
      else
      {
        this.IsRetweet = true;
        this.RetweetVisibility = Visibility.Visible;
        this.RetweetUser = new MetroTwitUser(twitterStatus.User);
        this.YourRetweetVisibility = this.RetweetUser.ScreenName.ToLower() == App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower() ? Visibility.Visible : Visibility.Collapsed;
        this.RetweetPiP = true;
        this.RetweetEnabled = !twitterStatus.RetweetedStatus.User.IsProtected;
        this.ID = twitterStatus.Id;
        this.OriginalID = twitterStatus.RetweetedStatus.Id;
        twitterStatus = twitterStatus.RetweetedStatus;
        this.RetweetText = this.RetweetUser.Name;
      }
      if (twitterStatus.RetweetCount >= 1)
      {
        this.RetweetVisibility = Visibility.Collapsed;
        this.RetweetCountVisibility = Visibility.Visible;
        string str = twitterStatus.RetweetCount > 1 ? " people" : " person";
        if (this.RetweetUser != null)
        {
          if (twitterStatus.RetweetCount > 1)
            this.RetweetText = string.Concat(new object[4]
            {
              (object) this.RetweetText,
              (object) " & ",
              (object) twitterStatus.RetweetCount,
              (object) str
            });
        }
        else
          this.RetweetText = twitterStatus.RetweetCount + str;
        if (this.OriginalID == new Decimal(0))
          this.OriginalID = this.ID;
      }
      else
        this.RetweetCountVisibility = Visibility.Collapsed;
      this.RawText = twitterStatus.Text;
      this.User = new MetroTwitUser(twitterStatus.User);
      this.BlockEnabled = this.User.ScreenName.ToLower() != App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower();
      this.SpamEnabled = this.BlockEnabled;
      if (twitterStatus.Source != null)
        this.Source = WebUtility.HtmlDecode(twitterStatus.Source);
      this.InReplyToScreenName = twitterStatus.InReplyToScreenName;
      this.InReplyTo = twitterStatus.InReplyToStatusId;
      this.Geo = twitterStatus.Place;
      this.Coordinates = twitterStatus.Coordinates;
      this.GeoVisibility = this.Geo != null ? Visibility.Visible : Visibility.Collapsed;
      this.ReplyToVisibility = !(this.InReplyTo > new Decimal(0)) || TweetType == TweetListType.Conversation ? Visibility.Collapsed : Visibility.Visible;
      this.FavouriteVisible = Visibility.Visible;
      App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseUser(twitterStatus.User.ScreenName, twitterStatus.User.ProfileImageSecureLocation);
      if (twitterStatus.Entities == null)
      {
        this.Entities = RegularExpressions.ExtractEntities(this.RawText);
      }
      else
      {
        App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseHashTags(Enumerable.Select<HashTagEntity, string>(Enumerable.OfType<HashTagEntity>((IEnumerable) twitterStatus.Entities), (Func<HashTagEntity, string>) (hashTag => "#" + hashTag.Text)));
        this.Entities = twitterStatus.Entities;
      }
      switch (TweetType)
      {
        case TweetListType.MentionsMyTweetsRetweeted:
          this.DefaultGridVisible = Visibility.Collapsed;
          this.ReplyGridVisible = Visibility.Visible;
          break;
        case TweetListType.MyTweets:
          this.ReplyEnabled = false;
          this.RetweetEnabled = false;
          break;
      }
      foreach (UrlEntity u in Enumerable.ToArray<UrlEntity>(Enumerable.OfType<UrlEntity>((IEnumerable) this.Entities)))
      {
        MediaEntity media = MediaManager.CheckEntityandCreateMedia(u);
        if (media != null)
        {
          this.Entities.Remove((Entity) u);
          this.Entities.Add((Entity) media);
        }
      }
      this.Media = Enumerable.OfType<MediaEntity>((IEnumerable) this.Entities);
      this.ContainsMedia = Enumerable.Count<MediaEntity>(this.Media) > 0;
      if (this.ContainsMedia)
        this.MediaThumbnail = Enumerable.FirstOrDefault<MediaEntity>(this.Media);
    }
  }
}
