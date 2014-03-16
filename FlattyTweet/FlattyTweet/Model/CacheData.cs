
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.Model
{
  [XmlRoot("Cache")]
  public class CacheData
  {
    public List<TwitterStreamEventExtended> Activities = new List<TwitterStreamEventExtended>();
    public SerializableDictionary<string, CacheUser> CachedUsers = new SerializableDictionary<string, CacheUser>();
    public SerializableDictionary<string, CacheHashTag> CachedHashTags = new SerializableDictionary<string, CacheHashTag>();
    public TwitterIdCollection BlockIds = new TwitterIdCollection();
    public TwitterIdCollection NoRetweetIds = new TwitterIdCollection();
    public DateTime LastBlocksUpdate = DateTime.Now.AddHours(-12.0);
    public DateTime LastNoRetweetsUpdate = DateTime.Now.AddHours(-12.0);
    [XmlIgnore]
    public SerializableDictionary<string, CacheUser> NonCachedUsers = new SerializableDictionary<string, CacheUser>();
    private Decimal TwitterAccountID;

    public void CacheSetup()
    {
      if (!OneTimer.ContainsTimer(TimeSpan.FromMinutes(90.0), (object) TimerMessages.CacheRefresh))
        OneTimer.RegisterTimer(TimeSpan.FromMinutes(90.0), (object) TimerMessages.CacheRefresh);
      Messenger.Default.Register<TimerMessage>((object) this, (object) TimerMessages.CacheRefresh, (Action<TimerMessage>) (o => this.CheckCache()));
      Messenger.Default.Register<GenericMessage<TwitterIdCollection>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.StreamingFriends, this.TwitterAccountID), new Action<GenericMessage<TwitterIdCollection>>(this.StreamingFriends));
    }

    public void UpdateIntellisense()
    {
      Task task = new Task((Action) (() =>
      {
        Thread.CurrentThread.Name = "Intellisense Update";
        this.UpdateMentionsIntellisense();
        this.UpdateTagsIntellisense();
      }));
      task.ContinueWith((Action<Task>) (t => CommonCommands.CheckTaskExceptions(t)));
      task.Start();
    }

    private async void StreamingFriends(GenericMessage<TwitterIdCollection> o)
    {
      List<long> userstoadd = new List<long>();
      if (o.Content == null || o.Content.Count <= 0)
        return;
      foreach (KeyValuePair<string, CacheUser> keyValuePair in Enumerable.ToArray<KeyValuePair<string, CacheUser>>((IEnumerable<KeyValuePair<string, CacheUser>>) this.CachedUsers))
      {
        KeyValuePair<string, CacheUser> cacheuser = keyValuePair;
        if (Enumerable.Count<long>(Enumerable.Where<long>((IEnumerable<long>) o.Content, (Func<long, int, bool>) ((y, z) => (Decimal) y == cacheuser.Value.TwitterID))) == 0)
          this.CachedUsers.Remove(cacheuser.Key);
      }
      foreach (long num in (Collection<long>) o.Content)
      {
        Decimal userid = (Decimal) num;
        if (Enumerable.Count<KeyValuePair<string, CacheUser>>(Enumerable.Where<KeyValuePair<string, CacheUser>>((IEnumerable<KeyValuePair<string, CacheUser>>) this.CachedUsers, (Func<KeyValuePair<string, CacheUser>, int, bool>) ((x, r) => x.Value.TwitterID == userid))) == 0)
          userstoadd.Add((long) userid);
      }
      if (userstoadd.Count > 0)
        await this.LoadUsersfromTwitter(userstoadd);
    }

    public void UpdateMentionsIntellisense()
    {
      try
      {
        if (this.CachedUsers == null || this.CachedUsers.Values == null)
          return;
        IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.Clear();
        IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.AddWords(Enumerable.Select<CacheUser, IntellisenseItem>(Enumerable.Where<CacheUser>(Enumerable.Cast<CacheUser>((IEnumerable) this.CachedUsers.Values), (Func<CacheUser, bool>) (user => user != null)), (Func<CacheUser, IntellisenseItem>) (user => new IntellisenseItem()
        {
          DisplayValue = "@" + user.ScreenName,
          FilterValue = user.ScreenName,
          TwitterAccountID = this.TwitterAccountID,
          ExtendedMetaText = user.FullName
        })), "@");
      }
      catch
      {
      }
    }

    public void UpdateTagsIntellisense()
    {
      try
      {
        if (this.CachedHashTags == null || this.CachedHashTags.Values == null)
          return;
        IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.Clear();
        IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.AddWords(Enumerable.Select<CacheHashTag, IntellisenseItem>(Enumerable.Where<CacheHashTag>(Enumerable.Cast<CacheHashTag>((IEnumerable) this.CachedHashTags.Values), (Func<CacheHashTag, bool>) (hashtag => hashtag != null)), (Func<CacheHashTag, IntellisenseItem>) (hashtag => new IntellisenseItem()
        {
          DisplayValue = hashtag.HashTag,
          FilterValue = hashtag.HashTag.Replace("#", ""),
          TwitterAccountID = this.TwitterAccountID
        })), "#");
      }
      catch
      {
      }
    }

    public void AddIntellisenseUser(IEnumerable<string> ScreenNames)
    {
      try
      {
        if (ScreenNames == null)
          return;
        foreach (string ScreenName in ScreenNames)
          this.AddIntellisenseUser(ScreenName, "Resources/defaultavatar.png");
      }
      catch
      {
      }
    }

    public void AddIntellisenseUser(string ScreenName, string ProfileImageSecure)
    {
      try
      {
        string key = this.CacheUserName(ScreenName);
        if (this.CachedUsers == null)
          this.CachedUsers = new SerializableDictionary<string, CacheUser>();
        if (!this.CachedUsers.ContainsKey(key) && !this.NonCachedUsers.ContainsKey(key))
        {
          this.NonCachedUsers.Add(key, new CacheUser()
          {
            ScreenName = ScreenName,
            ImageURITwitterSecure = ProfileImageSecure
          });
        }
        else
        {
          if (this.CachedUsers.ContainsKey(key) && (this.CachedUsers[key].ImageURITwitterSecure != ProfileImageSecure && !ProfileImageSecure.Equals("Resources/defaultavatar.png")))
          {
            this.CachedUsers[key].ImageURITwitterSecure = ProfileImageSecure;
            this.CachedUsers[key].ImageURILocal = this.UserImageURI(this.CachedUsers[key].ImageURITwitterSecure, this.CachedUsers[key].ScreenName);
            this.CachedUsers[key].CloseImages();
          }
          if (this.NonCachedUsers.ContainsKey(key) && this.NonCachedUsers[key].ImageURITwitterSecure != ProfileImageSecure)
          {
            this.NonCachedUsers[key].ImageURITwitterSecure = ProfileImageSecure;
            this.NonCachedUsers[key].CloseImages();
          }
        }
      }
      catch
      {
      }
    }

    public void AddIntellisenseHashTags(IEnumerable<string> HashTags)
    {
      try
      {
        if (HashTags == null)
          return;
        foreach (string hashtag in HashTags)
          this.AddIntellisenseHashTags(hashtag);
      }
      catch
      {
      }
    }

    public void AddIntellisenseHashTags(string hashtag)
    {
      try
      {
        if (this.CachedHashTags == null)
          this.CachedHashTags = new SerializableDictionary<string, CacheHashTag>();
        if (this.CachedHashTags.ContainsKey(hashtag.ToLower()))
          return;
        this.CachedHashTags.Add(hashtag.ToLower(), new CacheHashTag()
        {
          Expiry = DateTime.Now.AddMinutes(30.0),
          HashTag = hashtag
        });
        IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.AddWord(new IntellisenseItem()
        {
          DisplayValue = hashtag,
          FilterValue = hashtag.Replace("#", ""),
          TwitterAccountID = this.TwitterAccountID
        }, "#");
      }
      catch
      {
      }
    }

    public void RemoveCachedUser(Decimal id)
    {
      IEnumerable<KeyValuePair<string, CacheUser>> source = Enumerable.Where<KeyValuePair<string, CacheUser>>((IEnumerable<KeyValuePair<string, CacheUser>>) this.CachedUsers, (Func<KeyValuePair<string, CacheUser>, int, bool>) ((x, r) => x.Value.TwitterID == id));
      if (Enumerable.Count<KeyValuePair<string, CacheUser>>(source) <= 0)
        return;
      foreach (KeyValuePair<string, CacheUser> keyValuePair in source)
        this.CachedUsers.Remove(keyValuePair.Key);
    }

    private string CacheUserName(string username)
    {
      return "@" + username.ToLower();
    }

    public static CacheData Instance(Decimal TwitterAccountID)
    {
      if (!File.Exists(ApplicationPaths.UserSettings(TwitterAccountID, FileType.Cache)))
      {
        CacheData cacheData = new CacheData()
        {
          TwitterAccountID = TwitterAccountID
        };
        cacheData.CacheSetup();
        return cacheData;
      }
      else
      {
        try
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (CacheData));
          using (TextReader textReader = (TextReader) new StreamReader(ApplicationPaths.UserSettings(TwitterAccountID, FileType.Cache)))
          {
            try
            {
              CacheData cacheData = (CacheData) xmlSerializer.Deserialize(textReader);
              if (cacheData == null)
                cacheData = new CacheData()
                {
                  TwitterAccountID = TwitterAccountID
                };
              else
                cacheData.TwitterAccountID = TwitterAccountID;
              cacheData.CacheSetup();
              return cacheData;
            }
            catch
            {
              CacheData cacheData = new CacheData()
              {
                TwitterAccountID = TwitterAccountID
              };
              cacheData.CacheSetup();
              return cacheData;
            }
            finally
            {
              textReader.Close();
            }
          }
        }
        catch
        {
          return new CacheData();
        }
      }
    }

    public void CheckCache()
    {
      if (App.AppState.Accounts[this.TwitterAccountID] == null || !App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
        return;
      this.CheckTwitterUsers().ContinueWith((Action<Task<bool>>) (t =>
      {
        CommonCommands.CheckTaskExceptions((Task) t);
        this.UpdateMentionsIntellisense();
      }));
      this.CheckTwitterHashTags().ContinueWith((Action<Task<bool>>) (t =>
      {
        CommonCommands.CheckTaskExceptions((Task) t);
        this.UpdateTagsIntellisense();
      }));
      this.CheckTwitterBlocks().ContinueWith((Action<Task<bool>>) (t => CommonCommands.CheckTaskExceptions((Task) t)));
      this.CheckTwitterNoRetweets().ContinueWith((Action<Task<bool>>) (t => CommonCommands.CheckTaskExceptions((Task) t)));
    }

    private async Task<bool> CheckTwitterUsers()
        {
            if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
            {
                foreach (KeyValuePair<string, CacheUser> pair in this.CachedUsers.ToArray<KeyValuePair<string, CacheUser>>())
                {
                    if ((pair.Value == null) || (pair.Value.TwitterID == 0M))
                    {
                        this.CachedUsers.Remove(pair.Key);
                    }
                }
                UsersIdsOptions options = MetroTwitTwitterizer.UsersIdsOptions;
                options.Cursor = -1L;
                options.ScreenName = App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName;
                TwitterResponse<UserIdCollection> response = await Friendship.FriendsIdsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, options);
                TwitterResponse<UserIdCollection> asyncVariable0 = response;
                List<long> userstoadd = new List<long>();
                if ((asyncVariable0 != null) && (asyncVariable0.Result == RequestResult.Success))
                {
                    do
                    {
                        if (asyncVariable0.Result == RequestResult.Success)
                        {
                            UserIdCollection responseObject = asyncVariable0.ResponseObject;
                            if (responseObject != null)
                            {
                                using (IEnumerator<long> enumerator = responseObject.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Func<CacheUser, int, bool> predicate = null;
                                        long friendid = enumerator.Current;
                                        if (predicate == null)
                                        {
                                            predicate = (x, r) => x.TwitterID == friendid;
                                        }
                                        if (this.CachedUsers.Values.Where<CacheUser>(predicate).Count<CacheUser>() == 0)
                                        {
                                            userstoadd.Add(friendid);
                                        }
                                    }
                                }
                            }
                        }
                        if (asyncVariable0.ResponseObject != null)
                        {
                            options.Cursor = asyncVariable0.ResponseObject.NextCursor;
                            if (asyncVariable0.ResponseObject.NextCursor != 0L)
                            {
                                response = await Friendship.FriendsIdsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, options);
                                asyncVariable0 = response;
                            }
                        }
                        else
                        {
                            options.Cursor = 0L;
                        }
                    }
                    while (options.Cursor != 0L);
                    foreach (CacheUser user in this.CachedUsers.Values.Where<CacheUser>(((Func<CacheUser, int, bool>) ((s, r) => (s.Expiry <= DateTime.Now)))).ToArray<CacheUser>())
                    {
                        userstoadd.Add((long) user.TwitterID);
                        user.DeleteImage();
                        this.CachedUsers.Remove(this.CacheUserName(user.ScreenName));
                    }
                    await this.LoadUsersfromTwitter(userstoadd);
                }
            }
            return false;
        }

    private async Task<bool> LoadUsersfromTwitter(List<long> userstoadd)
        {
            foreach (IEnumerable<long> asyncVariable0 in userstoadd.Partition<long>(100))
            {
                LookupUsersOptions lookupUsersOptions = MetroTwitTwitterizer.LookupUsersOptions;
                lookupUsersOptions.UserIds = new TwitterIdCollection(asyncVariable0.ToList<long>());
                if ((App.AppState.Accounts[this.TwitterAccountID] != null) && (lookupUsersOptions.UserIds.Count > 1))
                {
                    TwitterResponse<TwitterUserCollection> lookup = await Users.LookupAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, lookupUsersOptions);
                    if (lookup.Result == RequestResult.Success)
                    {
                        using (IEnumerator<User> enumerator = lookup.ResponseObject.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                User current = enumerator.Current;
                                string key = this.CacheUserName(current.ScreenName);
                                if (!this.CachedUsers.ContainsKey(key))
                                {
                                    CacheUser user2 = new CacheUser {
                                        TwitterID = current.Id,
                                        ScreenName = current.ScreenName,
                                        FullName = current.Name,
                                        ImageURITwitterSecure = current.ProfileImageSecureLocation,
                                        Expiry = DateTime.Now.AddHours(6.0),
                                        ImageURILocal = this.UserImageURI(current.ProfileImageLocation, current.ScreenName)
                                    };
                                    this.CachedUsers.Add(key, user2);
                                    if (this.NonCachedUsers.ContainsKey(key))
                                    {
                                        this.NonCachedUsers.Remove(key);
                                    }
                                }
                            }
                        }
                        this.Save();
                    }
                }
            }
            this.UpdateMentionsIntellisense();
            return false;
        }

    private async Task<bool> CheckTwitterHashTags()
        {
            if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn)
            {
                foreach (KeyValuePair<string, CacheHashTag> pair in this.CachedHashTags.Where<KeyValuePair<string, CacheHashTag>>(((Func<KeyValuePair<string, CacheHashTag>, int, bool>) ((s, r) => ((s.Value == null) || (s.Value.Expiry <= DateTime.Now))))).ToArray<KeyValuePair<string, CacheHashTag>>())
                {
                    this.CachedHashTags.Remove(pair.Key);
                }
                TrendsOptions trendsOptions = MetroTwitTwitterizer.TrendsOptions;
                trendsOptions.ExcludeHashTags = false;
                TwitterResponse<TwitterTrendCollection> asyncVariable0 = await Trends.TrendsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, 1, trendsOptions);
                if (asyncVariable0.Result == RequestResult.Success)
                {
                    using (IEnumerator<TwitterTrend> enumerator = asyncVariable0.ResponseObject.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            TwitterTrend current = enumerator.Current;
                            if (!(this.CachedHashTags.ContainsKey(current.Name.ToLower()) || (current.Name.IndexOf("#") <= -1)))
                            {
                                CacheHashTag tag = new CacheHashTag {
                                    Expiry = DateTime.Now.AddHours(2.0),
                                    HashTag = current.Name
                                };
                                this.CachedHashTags.Add(current.Name.ToLower(), tag);
                            }
                        }
                    }
                }
                this.Save();
            }
            return false;
        }

    private async Task<bool> CheckTwitterBlocks()
        {
            if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn && (this.LastBlocksUpdate <= DateTime.Now.AddHours(-6.0)))
            {
                TwitterResponse<UserIdCollection> asyncVariable0 = await Block.BlockingIdsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, MetroTwitTwitterizer.BlockingIdOptions);
                if ((asyncVariable0.Result == RequestResult.Success) && (asyncVariable0.ResponseObject != null))
                {
                    this.BlockIds = asyncVariable0.ResponseObject;
                    this.LastBlocksUpdate = DateTime.Now;
                    this.Save();
                }
            }
            return false;
        }

    private async Task<bool> CheckTwitterNoRetweets()
    {
        if (App.AppState.Accounts[this.TwitterAccountID].IsSignedIn && (this.LastNoRetweetsUpdate <= DateTime.Now.AddHours(-6.0)))
        {
            TwitterResponse<UserIdCollection> asyncVariable0 = await Friendship.NoRetweetIDsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, MetroTwitTwitterizer.Options);
            if ((asyncVariable0.Result == RequestResult.Success) && (asyncVariable0.ResponseObject != null))
            {
                this.NoRetweetIds = asyncVariable0.ResponseObject;
                this.LastNoRetweetsUpdate = DateTime.Now;
                this.Save();
            }
        }
        return false;
    }

    public string UserImageURI(string UserImage, string UserName)
    {
      string[] strArray = UserImage.Split(new char[1]
      {
        '/'
      });
      return strArray[Enumerable.Count<string>((IEnumerable<string>) strArray) - 2] + "_" + UserName.Replace("@", "") + Path.GetExtension(strArray[Enumerable.Count<string>((IEnumerable<string>) strArray) - 1]);
    }

    public void Save()
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (CacheData));
        TextWriter textWriter = (TextWriter) new StreamWriter(ApplicationPaths.UserSettings(this.TwitterAccountID, FileType.Cache));
        xmlSerializer.Serialize(textWriter, (object) this);
        textWriter.Close();
      }
      catch
      {
      }
    }

    internal void RemoveBlockedUser(long id)
    {
      if (!this.BlockIds.Contains(id) || id <= 0L)
        return;
      this.BlockIds.Remove(id);
    }

    internal void AddBlockedUser(long id)
    {
      if (this.BlockIds.Contains(id) || id <= 0L)
        return;
      this.BlockIds.Add(id);
    }

    internal void RemoveNoRetweetsUser(long id)
    {
      if (!this.NoRetweetIds.Contains(id) || id <= 0L)
        return;
      this.NoRetweetIds.Remove(id);
    }

    internal void AddNoRetweetsUser(long id)
    {
      if (this.NoRetweetIds.Contains(id) || id <= 0L)
        return;
      this.NoRetweetIds.Add(id);
    }

    internal void RemovedFollowedUser(MetroTwitUser User)
    {
      if (User == null || !this.CachedUsers.ContainsKey(this.CacheUserName(User.ScreenName)))
        return;
      this.CachedUsers.Remove(this.CacheUserName(User.ScreenName));
    }

    internal void AddFollowedUser(MetroTwitUser User)
    {
      if (User == null)
        return;
      string key = this.CacheUserName(User.ScreenName);
      if (!this.CachedUsers.ContainsKey(key))
      {
        this.CachedUsers.Add(key, new CacheUser()
        {
          TwitterID = User.Id,
          ScreenName = User.ScreenName,
          FullName = User.Name,
          ImageURITwitterSecure = User.ProfileImageSecureLocation,
          Expiry = DateTime.Now.AddHours(6.0),
          ImageURILocal = this.UserImageURI(User.ProfileImageLocation, User.ScreenName)
        });
        if (this.NonCachedUsers.ContainsKey(key))
          this.NonCachedUsers.Remove(key);
      }
    }
  }
}
