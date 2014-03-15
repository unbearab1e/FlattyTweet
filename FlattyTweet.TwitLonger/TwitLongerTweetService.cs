
using FlattyTweet.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Net;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Controls;

namespace FlattyTweet.TwitLonger
{
  [Export(typeof (ITweetService))]
  public class TwitLongerTweetService : ITweetService
  {
    private MessageBoxResult CurrentTweetInvoke = MessageBoxResult.None;
    private string CurrentTwitLongerID = string.Empty;
    private ICoreServices coreService;
    private TwitLongerSettings settings;

    public string Name
    {
      get
      {
        return "TwitLonger";
      }
    }

    public string UniqueID
    {
      get
      {
        return ((object) Assembly.GetExecutingAssembly()).GetType().ToString();
      }
    }

    public bool WantsPreCreation
    {
      get
      {
        return true;
      }
    }

    public bool WantsPostCreation
    {
      get
      {
        return true;
      }
    }

    public bool OverrideTweetCharLimit
    {
      get
      {
        return this.settings.UseTwitLonger;
      }
    }

    public bool HasSettings
    {
      get
      {
        return true;
      }
    }

    public UserControl GetUISettings
    {
      get
      {
        return (UserControl) new TwitLongerUISettings(this.settings);
      }
    }

    [ImportingConstructor]
    public TwitLongerTweetService(ICoreServices coreService)
    {
      this.coreService = coreService;
      this.LoadSettings();
    }

    private void LoadSettings()
    {
      TwitLongerSettings twitLongerSettings = this.coreService.SettingService(typeof (ITweetService)).LoadSingleObject<TwitLongerSettings>(new TwitLongerSettings());
      if (twitLongerSettings != null)
        this.settings = twitLongerSettings;
      else
        this.settings = new TwitLongerSettings();
    }

    public PreTweetCreationResponse PreTweetCreation(string TweetContent, int CharCount, string InReplyToID, string InReplyToScreenName, bool DirectMessage, string TwitterAccountName)
    {
      if (CharCount <= 140 || !this.settings.UseTwitLonger)
        return new PreTweetCreationResponse();
      this.CurrentTweetInvoke = this.coreService.MessageDialogService.InvokeShow("Your tweet is over 140 characters, would you like to use TwitLonger?", "use TwitLonger", MessageBoxButton.YesNo, MessageBoxResult.Yes);
      if (this.CurrentTweetInvoke == MessageBoxResult.Yes)
      {
        string url = "http://www.twitlonger.com";
        string path = "/api_post";
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("application", "metrotwit");
        dictionary.Add("api_key", "t66E26k53QO4z3m3");
        dictionary.Add("username", TwitterAccountName);
        if (!string.IsNullOrEmpty(InReplyToID) && InReplyToID != "0")
        {
          dictionary.Add("in_reply", InReplyToID);
          dictionary.Add("in_reply_user", InReplyToScreenName);
        }
        if (DirectMessage)
          dictionary.Add("direct_message", Convert.ToInt16(DirectMessage).ToString());
        dictionary.Add("message", HttpUtility.UrlEncode(TweetContent));
        string content = string.Empty;
        foreach (string index in dictionary.Keys)
        {
          if (!string.IsNullOrEmpty(content))
            content = content + "&";
          content = content + index + "=" + dictionary[index];
        }
        string ContentType = "application/x-www-form-urlencoded";
        IRestResponse restResponse = this.coreService.RestService.InvokeRESTService(url, path, (IDictionary<string, string>) null, "POST", content, (IDictionary<string, string>) null, (string) null, ContentType);
        if (restResponse.StatusCode == HttpStatusCode.OK)
        {
          TwitLongerPostResponse longerPostResponse = this.coreService.RestService.DeserializeXml<TwitLongerPostResponse>(restResponse.Content);
          this.CurrentTwitLongerID = longerPostResponse.post.id;
          return new PreTweetCreationResponse()
          {
            TweetContent = longerPostResponse.post.content
          };
        }
      }
      return new PreTweetCreationResponse();
    }

    public PostTweetCreationResponse PostTweetCreation(string TweetContent, int CharCount, string InReplyToID, string InReplyToScreenName, bool DirectMessage, string TwitterAccountName, Decimal TweetID)
    {
      if (this.CurrentTweetInvoke != MessageBoxResult.Yes)
        return new PostTweetCreationResponse();
      string url = "http://www.twitlonger.com";
      string path = "/api_set_id";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("application", "metrotwit");
      dictionary.Add("api_key", "t66E26k53QO4z3m3");
      dictionary.Add("message_id", this.CurrentTwitLongerID);
      dictionary.Add("twitter_id", TweetID.ToString());
      string content = string.Empty;
      foreach (string index in dictionary.Keys)
      {
        if (!string.IsNullOrEmpty(content))
          content = content + "&";
        content = content + index + "=" + dictionary[index];
      }
      string ContentType = "application/x-www-form-urlencoded";
      if (this.coreService.RestService.InvokeRESTService(url, path, (IDictionary<string, string>) null, "POST", content, (IDictionary<string, string>) null, (string) null, ContentType).StatusCode == HttpStatusCode.OK)
      {
        this.CurrentTwitLongerID = string.Empty;
        this.CurrentTweetInvoke = MessageBoxResult.None;
        return new PostTweetCreationResponse();
      }
      else
      {
        this.CurrentTwitLongerID = string.Empty;
        this.CurrentTweetInvoke = MessageBoxResult.None;
        return new PostTweetCreationResponse();
      }
    }

    public void SaveSettings()
    {
      this.coreService.SettingService(typeof (ITweetService)).SaveSingleObject<TwitLongerSettings>(this.settings);
    }

    public void CancelSaveSettings()
    {
      this.LoadSettings();
    }

    public bool ValidateSettings()
    {
      return true;
    }
  }
}
