// Type: MetroTwit.ViewModel.TwitterStreamEventExtended
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using System;
using System.Linq.Expressions;
using Twitterizer.Models;
using Twitterizer.Streaming;

namespace MetroTwit.ViewModel
{
  public class TwitterStreamEventExtended : MetroTwitStatusBase
  {
    private string userScreenname;
    private string fullname;
     private TwitterSteamEvent eventType;
    public TwitterSteamEvent EventType
    {
      get
      {
        return this.eventType;
      }
      set
      {
        if (this.eventType == value)
          return;
        this.eventType = value;
        base.RaisePropertyChanged("EventType");
      }
    }
      private DateTime createdAt;
    public DateTime CreatedAt
    {
      get
      {
        return this.createdAt;
      }
      set
      {
        if (DateTime.Equals(this.createdAt, value))
          return;
        this.createdAt = value;
        base.RaisePropertyChanged("CreatedAt");
      }
    }
      private string userImageLocation;
    public string UserImageLocation
    {
      get
      {
        return this.userImageLocation;
      }
      set
      {
        if (string.Equals(this.userImageLocation, value, StringComparison.Ordinal))
          return;
        this.userImageLocation = value;
        base.RaisePropertyChanged("UserImageLocation");
      }
    }
      private string userImageSecureLocation;
    public string UserImageSecureLocation
    {
      get
      {
        return this.userImageSecureLocation;
      }
      set
      {
        if (string.Equals(this.userImageSecureLocation, value, StringComparison.Ordinal))
          return;
        this.userImageSecureLocation = value;
        base.RaisePropertyChanged("UserImage");
        base.RaisePropertyChanged("UserImageSecureLocation");
      }
    }

    public string UserScreenName
    {
      get
      {
        return this.userScreenname;
      }
      set
      {
        if (string.Equals(this.userScreenname, value, StringComparison.Ordinal))
          return;
        this.userScreenname = value;
        base.RaisePropertyChanged("UserImage");
        base.RaisePropertyChanged("UserScreenName");
        this.TryCreateUser();
        this.User.ScreenName = value;
      }
    }

    public string FullName
    {
      get
      {
        return this.fullname;
      }
      set
      {
        if (string.Equals(this.fullname, value, StringComparison.Ordinal))
          return;
        this.fullname = value;
        base.RaisePropertyChanged("FullName");
        this.TryCreateUser();
        this.User.Name = value;
      }
    }

    public new object UserImage
    {
      get
      {
        if (!App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.UserScreenName.ToLower()) && !App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.UserScreenName.ToLower()))
          App.AppState.Accounts[this.TwitterAccountID].Cache.AddIntellisenseUser(this.UserScreenName, this.UserImageSecureLocation);
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
        if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.User.ScreenName.ToLower()))
          return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.User.ScreenName.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
        else
          return (object) null;
      }
    }

    public TwitterStreamEventExtended()
    {
      this.TryCreateUser();
    }

    public TwitterStreamEventExtended(TwitterStreamEvent baseEvent)
    {
      this.UserScreenName = baseEvent.Source.ScreenName;
      this.FullName = baseEvent.Source.Name;
      this.UserImageLocation = baseEvent.Source.ProfileImageLocation;
      this.UserImageSecureLocation = baseEvent.Source.ProfileImageSecureLocation;
      string str = string.Empty;
      this.CreatedAt = baseEvent.CreatedAt;
      this.EventType = baseEvent.EventType;
      this.UnRead = true;
      switch (baseEvent.EventType)
      {
        case TwitterSteamEvent.Favorite:
          str = string.Format("favorited tweet \"{0}\"", (object[]) new string[1]
          {
            ((object) (baseEvent.TargetObject as Status).Text).ToString()
          });
          break;
        case TwitterSteamEvent.Unfavorite:
          str = string.Format("unfavorited tweet \"{0}\"", (object[]) new string[1]
          {
            ((object) (baseEvent.TargetObject as Status).Text).ToString()
          });
          break;
        case TwitterSteamEvent.Follow:
          str = "followed you";
          break;
        case TwitterSteamEvent.ListMemberAdded:
          str = string.Format("added you to the list \"{0}\"", (object[]) new string[1]
          {
            ((object) (baseEvent.TargetObject as TwitterList).Name).ToString()
          });
          break;
        case TwitterSteamEvent.ListMemberRemoved:
          str = string.Format("removed you from list \"{0}\"", (object[]) new string[1]
          {
            ((object) (baseEvent.TargetObject as TwitterList).Name).ToString()
          });
          break;
      }
      this.RawText = str;
    }

    private void TryCreateUser()
    {
      if (this.User != null)
        return;
      this.User = new MetroTwitUser((User) null);
    }

    public void UpdateCreatedAtTimeStamp()
    {
      base.RaisePropertyChanged<DateTime>((Expression<Func<DateTime>>) (() => this.CreatedAt));
    }
  }
}
