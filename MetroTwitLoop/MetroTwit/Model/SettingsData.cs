// Type: MetroTwit.Model.SettingsData
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace MetroTwit.Model
{
  [Export(typeof (ISettingsService))]
  [XmlRoot("Settings")]
  public class SettingsData : ISettingsService
  {
    [XmlIgnore]
    private static readonly object settingsLock = new object();
    private static bool IsSaving = false;
    public bool OOBEScreenDisplayed = false;
    public bool AutomaticallyCheckForUpdates = true;
    public bool StartMetroTwitwithWindows = false;
    public bool PlusShowAds = false;
    public Decimal ActiveAccount = new Decimal(0);
    public bool ShowNotificationToasts = true;
    public bool ShowTaskbarCount = true;
    public bool UseNotificationSound = false;
    public double NotificationTweetDisplayTime = 2.0;
    public double NotificationDisplayTime = 10.0;
    public int NotificationScreen = 0;
    public NotificationPosition NotificationPosition = NotificationPosition.BottomRight;
    public SerializableDictionary<string, string> StateBag = new SerializableDictionary<string, string>();
    public string CurrentImageUploadingService = "MetroTwit.AddIns.TwitterImageUploadService";
    public string CurrentMapService = "MetroTwit.Google.GoogleService";
    public bool KeepScrollPositionatTop = true;
    public bool UseSpellChecker = true;
    public bool UseAutoComplete = true;
    public int BacklogSeconds = 0;
    public bool APIDebug = false;
    public bool PostTweetOnEnter = true;
    public bool ShowMediaPreviews = true;
    public bool DisplayImagesInlineAutomatically = true;
    public bool MinimisetoTray = false;
    public bool RestartTriggered = false;
    public TweetListType AdDisplayColumn = TweetListType.DirectMessages;
    public bool ShowCompactAccountPane = false;
    public bool FriendsIncludeRetweets = true;
    public bool MentionsIncludeRetweets = true;
    public List<string> Filter = new List<string>();
    public bool ShowRefreshProgress = true;
    public TwitterTimeDisplay TwitterTimeDisplay = TwitterTimeDisplay.RelativeAbsolute;
    public TweetFontSizeDisplay TweetFontSizeDisplay = TweetFontSizeDisplay.Medium;
    public bool ShowTimeofTweet = true;
    public bool ShowSourceofTweet = false;
    public string Theme = "Light";
    public string Accent = "Custom";
    public ColourAccent AccentColour = ColourAccent.Blue;
    public Artwork Artwork = Artwork.None;
    public string PlusTransactionID = string.Empty;
    public string PlusEmail = string.Empty;
    public string SecuredProxyUsername = string.Empty;
    public string SecuredProxyPassword = string.Empty;
    public int? TrendLocationWOEID = new int?();
    public bool UseProxlet = false;
    public string ProxletAddress = string.Empty;
    public string CustomTwitterConsumerKey = string.Empty;
    public string CustomTwitterConsumerSecret = string.Empty;
    public MetroTwitWindow Window = new MetroTwitWindow();
    [XmlIgnore]
    public bool SettingsRecreated = false;
    [XmlIgnore]
    public bool Updating = false;
    [XmlIgnore]
    public readonly int TweetCharLimit = 140;
    [XmlIgnore]
    public bool CutIsTriggered = false;
    [XmlIgnore]
    public bool Boon = true;
    [XmlIgnore]
    public bool WindowResizing = false;
    [XmlIgnore]
    public bool QuietUpdating = false;
    [XmlIgnore]
    public int TwitterShortUrlLength = 22;
    [XmlIgnore]
    public int TwitterShortEncryptedUrlLength = 23;
    [XmlIgnore]
    public bool FirstLoad;
    [XmlIgnore]
    public bool PlayingSound;
    [XmlIgnore]
    public UIElement PopupTarget;
    [XmlIgnore]
    public FrameworkElement NewTweetEntryOptionsContainer;
    [XmlIgnore]
    public UIElement SelectedColumn;
    [XmlIgnore]
    public UIElement SearchTwitterButton;
    [XmlIgnore]
    public UIElement SearchProfileButton;
    [XmlIgnore]
    public UIElement ListsButton;
    [XmlIgnore]
    public FrameworkElement DialogActiveControl;
    [XmlIgnore]
    private static SettingsData settingsData;

    [XmlIgnore]
    public string ProxyUsername
    {
        get
        {
            try
            {
                return ((this.SecuredProxyUsername != string.Empty) ? this.SecuredProxyUsername.DecryptStringEx() : string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }
        set
        {
            this.SecuredProxyUsername = value.EncryptString();
        }
    }

    [XmlIgnore]
    public string ProxyPassword
    {
        get
        {
            try
            {
                return ((this.SecuredProxyPassword != string.Empty) ? this.SecuredProxyPassword.DecryptStringEx() : string.Empty);
            }
            catch
            {
                return string.Empty;
            }
        }
        set
        {
            this.SecuredProxyPassword = value.EncryptString();
        }
    }

    [XmlIgnore]
    public string TwitterConsumerKey
    {
      get
      {
        if (!string.IsNullOrEmpty(this.CustomTwitterConsumerKey))
          return this.CustomTwitterConsumerKey;
        else
            return "ihVSwzlmo56ulgcOeAS2w";
      }
    }

    [XmlIgnore]
    public string TwitterConsumerSecret
    {
      get
      {
        if (!string.IsNullOrEmpty(this.CustomTwitterConsumerSecret))
          return this.CustomTwitterConsumerSecret;
        else
            return "4gDixlmnRJEpbN0E8rFEGW8GZ2aQAG27PMEXgncBE";
      }
    }

    public static SettingsData Instance
    {
      get
      {
        lock (SettingsData.settingsLock)
        {
          if (SettingsData.settingsData == null)
          {
            if (!File.Exists(ApplicationPaths.ConfigFile))
              SettingsData.settingsData = new SettingsData();
            else
              SettingsData.Load(ApplicationPaths.ConfigFile);
          }
          return SettingsData.settingsData;
        }
      }
      set
      {
        SettingsData.settingsData = value;
      }
    }

    static SettingsData()
    {
    }

    private static void Load(string loadpath)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (SettingsData));
      using (TextReader textReader = (TextReader) new StreamReader(loadpath))
      {
        try
        {
          SettingsData.settingsData = (SettingsData) xmlSerializer.Deserialize(textReader);
          textReader.Close();
          if (!(loadpath == ApplicationPaths.ConfigBackup))
            return;
          SettingsData.Save();
          return;
        }
        catch
        {
        }
        textReader.Close();
        if (File.Exists(ApplicationPaths.ConfigBackup) && loadpath != ApplicationPaths.ConfigBackup)
        {
          SettingsData.Load(ApplicationPaths.ConfigBackup);
        }
        else
        {
          SettingsData.settingsData = new SettingsData();
          SettingsData.settingsData.SettingsRecreated = true;
        }
      }
    }

    public static void Save()
    {
      if (SettingsData.IsSaving)
        return;
      SettingsData.IsSaving = true;
      try
      {
        if (SettingsData.settingsData != null)
        {
          try
          {
            File.Copy(ApplicationPaths.ConfigFile, ApplicationPaths.ConfigBackup, true);
          }
          catch
          {
          }
          try
          {
            using (TextWriter textWriter = (TextWriter) new StreamWriter(ApplicationPaths.ConfigFile))
            {
              new XmlSerializer(typeof (SettingsData)).Serialize(textWriter, (object) SettingsData.settingsData);
              textWriter.Close();
            }
          }
          catch
          {
          }
        }
      }
      finally
      {
        SettingsData.IsSaving = false;
      }
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }

    public void SaveSingleObject(object targetObjectToSave)
    {
    }

    public T LoadSingleObject<T>(T type)
    {
      string key = type.GetType().ToString();
      if (this.StateBag.ContainsKey(key))
        return JsonConvert.DeserializeObject<T>(this.StateBag[key]);
      else
        return default (T);
    }

    public void SaveObject<T>(Dictionary<Decimal, T> targetObjectToSave)
    {
      throw new NotImplementedException();
    }

    public void SaveSingleObject<T>(T targetObjectToSave)
    {
      string key = targetObjectToSave.GetType().ToString();
      string str = JsonConvert.SerializeObject((object) targetObjectToSave);
      if (!this.StateBag.ContainsKey(key))
        this.StateBag.Add(key, str);
      else
        this.StateBag[key] = str;
    }

    public Dictionary<Decimal, T> LoadObject<T>(T type)
    {
      throw new NotImplementedException();
    }

    internal static void CheckUpgrade()
    {
      if (!File.Exists(ApplicationPaths.ConfigFile))
        return;
      try
      {
        File.Copy(ApplicationPaths.ConfigFile, ApplicationPaths.ConfigBackup, true);
      }
      catch
      {
      }
      XmlDocument xmlDocument1 = new XmlDocument();
      try
      {
        xmlDocument1.Load(ApplicationPaths.ConfigFile);
        XmlElement documentElement = xmlDocument1.DocumentElement;
        if (documentElement["TwitterAccountID"] != null)
        {
          Decimal TwitterAccountID = Decimal.Parse(documentElement["TwitterAccountID"].InnerText);
          if (TwitterAccountID > new Decimal(0))
          {
            App.AppState.Accounts.Add(new UserAccountViewModel(TwitterAccountID));
            UserAccountViewModel accountViewModel = Enumerable.First<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts);
            accountViewModel.Settings.TwitterAccountID = TwitterAccountID;
            if (documentElement["TwitterAccountName"] != null)
              accountViewModel.Settings.TwitterAccountName = documentElement["TwitterAccountName"].InnerText;
            if (documentElement["TwitterRealName"] != null)
              accountViewModel.Settings.TwitterRealName = documentElement["TwitterRealName"].InnerText;
            if (documentElement["TwitterUserImage"] != null)
              accountViewModel.Settings.TwitterUserImage = documentElement["TwitterUserImage"].InnerText;
            if (documentElement["TwitterUserTweetCount"] != null)
              accountViewModel.Settings.TwitterUserTweetCount = documentElement["TwitterUserTweetCount"].InnerText;
            if (documentElement["TwitterAuthSecret"] != null)
              accountViewModel.Settings.TwitterAuthSecret = documentElement["TwitterAuthSecret"].InnerText;
            if (documentElement["TwitterAuthToken"] != null)
              accountViewModel.Settings.TwitterAuthToken = documentElement["TwitterAuthToken"].InnerText;
            if (documentElement["TwitterAccountProtected"] != null)
              accountViewModel.Settings.TwitterAccountProtected = bool.Parse(documentElement["TwitterAccountProtected"].InnerText);
            if (documentElement["AllReplies"] != null)
              accountViewModel.Settings.AllReplies = bool.Parse(documentElement["AllReplies"].InnerText);
            if (documentElement["AutomaticallyShortenLinks"] != null)
              accountViewModel.Settings.AutomaticallyShortenLinks = bool.Parse(documentElement["AutomaticallyShortenLinks"].InnerText);
            if (documentElement["CurrentURLShorteningService"] != null)
              accountViewModel.Settings.CurrentURLShorteningService = documentElement["CurrentURLShorteningService"].InnerText;
            if (documentElement["StateBag"] != null)
            {
              XmlDocument xmlDocument2 = new XmlDocument();
              xmlDocument2.InsertBefore(xmlDocument2.CreateNode(XmlNodeType.Element, "dictionary", ""), (XmlNode) xmlDocument2.DocumentElement);
              xmlDocument2.DocumentElement.InnerXml = documentElement["StateBag"].InnerXml;
              using (XmlReader xmlReader = (XmlReader) new XmlNodeReader((XmlNode) xmlDocument2))
              {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof (SerializableDictionary<string, string>));
                accountViewModel.Settings.StateBag = (SerializableDictionary<string, string>) xmlSerializer.Deserialize(xmlReader);
                xmlReader.Close();
              }
            }
            if (documentElement["Columns"] != null)
            {
              XmlDocument xmlDocument2 = new XmlDocument();
              xmlDocument2.InsertBefore(xmlDocument2.CreateNode(XmlNodeType.Element, "MetroColumns", ""), (XmlNode) xmlDocument2.DocumentElement);
              xmlDocument2.DocumentElement.InnerXml = documentElement["Columns"].OuterXml;
              using (XmlReader xmlReader = (XmlReader) new XmlNodeReader((XmlNode) xmlDocument2))
              {
                MetroColumns metroColumns = (MetroColumns) new XmlSerializer(typeof (MetroColumns)).Deserialize(xmlReader);
                accountViewModel.Settings.Columns = metroColumns.Columns;
                int num = 0;
                foreach (MetroTwitColumn metroTwitColumn in (Collection<MetroTwitColumn>) accountViewModel.Settings.Columns)
                {
                  metroTwitColumn.Index = num;
                  ++num;
                }
                xmlReader.Close();
              }
            }
            SettingsData.Instance.OOBEScreenDisplayed = true;
          }
        }
      }
      catch
      {
      }
    }
  }
}
