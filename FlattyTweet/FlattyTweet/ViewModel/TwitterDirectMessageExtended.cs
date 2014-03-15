
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class TwitterDirectMessageExtended : MetroTwitStatusBase
  {
    public object RecipientImage
    {
      get
      {
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
        else
          return (object) null;
      }
    }

    public new object UserImage
    {
      get
      {
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.Source.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.Source.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.Source.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.Source.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
        else
          return (object) null;
      }
    }

    public TwitterDirectMessageExtended(TwitterDirectMessage twitterDirectMessage, TweetListType TweetType, Decimal TwitterAccountID, Decimal CurrentTweetID, bool Streamed = false)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.DirectMessageEnabled = true;
      this.ReplyEnabled = false;
      this.RetweetEnabled = false;
      this.DeleteTweetEnabled = true;
      this.IsRetweet = false;
      this.YourRetweetVisibility = Visibility.Collapsed;
      if (twitterDirectMessage == null)
        return;
      this.UnRead = twitterDirectMessage.Id > CurrentTweetID && twitterDirectMessage.Sender.Id != TwitterAccountID;
      this.User = new MetroTwitUser((User) null);
      this.TweetType = TweetType;
      if (twitterDirectMessage.Text != null)
        this.RawText = twitterDirectMessage.Text;
      this.ID = twitterDirectMessage.Id;
      DateTime createdDate = twitterDirectMessage.CreatedDate;
      bool flag = 1 == 0;
      this.CreatedDate = twitterDirectMessage.CreatedDate;
      this.Source = twitterDirectMessage.Sender.ScreenName;
      if (twitterDirectMessage.Recipient.ScreenName == App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName)
      {
        this.DMText = "From";
        this.User.ScreenName = twitterDirectMessage.Sender.ScreenName;
        this.User.Name = twitterDirectMessage.Sender.Name;
        this.User.Id = twitterDirectMessage.Sender.Id;
        this.Source = twitterDirectMessage.Sender.ScreenName;
      }
      else
      {
        this.DMText = "To";
        this.User.ScreenName = twitterDirectMessage.Recipient.ScreenName;
        this.User.Name = twitterDirectMessage.Recipient.Name;
        this.User.Id = twitterDirectMessage.Recipient.Id;
        this.Source = twitterDirectMessage.Sender.ScreenName;
        this.IsSelfTweet = true;
        this.CanUndoTweet = this.CreatedDate >= DateTime.Now.AddMinutes(-30.0);
      }
      this.BlockEnabled = this.User.ScreenName.ToLower() != App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName.ToLower();
      this.SpamEnabled = this.BlockEnabled;
      this.GeoVisibility = Visibility.Collapsed;
      this.ReplyToVisibility = Visibility.Collapsed;
      this.FavouriteVisible = Visibility.Collapsed;
      this.RetweetVisibility = Visibility.Collapsed;
      this.RetweetCountVisibility = Visibility.Collapsed;
      App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseUser(twitterDirectMessage.Sender.ScreenName, twitterDirectMessage.Sender.ProfileImageSecureLocation);
      App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseUser(twitterDirectMessage.Recipient.ScreenName, twitterDirectMessage.Recipient.ProfileImageSecureLocation);
      if (Streamed || twitterDirectMessage.Entities == null)
        this.Entities = RegularExpressions.ExtractEntities(this.RawText);
      else
        this.Entities = twitterDirectMessage.Entities;
      if (this.Entities != null)
        App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseHashTags(Enumerable.Select<HashTagEntity, string>(Enumerable.OfType<HashTagEntity>((IEnumerable) this.Entities), (Func<HashTagEntity, string>) (hashTag => "#" + hashTag.Text)));
    }
  }
}
