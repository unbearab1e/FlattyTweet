// Type: MetroTwit.ViewModel.SettingsViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.AddIns;
using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using MetroTwit.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MetroTwit.ViewModel
{
  public class SettingsViewModel : ViewModelBase
  {
    private Visibility settingsVisible = Visibility.Visible;
    private int minimisetoTrayInt;
    private static TwitterImageUploadService twitterImageUploadService;
    private DispatcherTimer changeThemeTimer;

    public bool AutomaticallyCheckForUpdates
    {
      get
      {
        return SettingsData.Instance.AutomaticallyCheckForUpdates;
      }
      set
      {
        if (this.AutomaticallyCheckForUpdates == value)
          return;
        SettingsData.Instance.AutomaticallyCheckForUpdates = value;
        base.RaisePropertyChanged("AutomaticallyCheckForUpdates");
      }
    }

    public bool StartMetroTwitwithWindows
    {
      get
      {
        return SettingsData.Instance.StartMetroTwitwithWindows;
      }
      set
      {
        if (this.StartMetroTwitwithWindows == value)
          return;
        SettingsData.Instance.StartMetroTwitwithWindows = value;
        base.RaisePropertyChanged("StartMetroTwitwithWindows");
      }
    }

    public bool ShowNotificationToasts
    {
      get
      {
        return SettingsData.Instance.ShowNotificationToasts;
      }
      set
      {
        if (this.ShowNotificationToasts == value)
          return;
        SettingsData.Instance.ShowNotificationToasts = value;
        base.RaisePropertyChanged("ShowNotificationToasts");
      }
    }

    public bool ShowTaskbarCount
    {
      get
      {
        return SettingsData.Instance.ShowTaskbarCount;
      }
      set
      {
        if (this.ShowTaskbarCount == value)
          return;
        SettingsData.Instance.ShowTaskbarCount = value;
        base.RaisePropertyChanged("ShowTaskbarCount");
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.OverlayCountRecalc);
      }
    }

    public bool UseNotificationSound
    {
      get
      {
        return SettingsData.Instance.UseNotificationSound;
      }
      set
      {
        if (this.UseNotificationSound == value)
          return;
        SettingsData.Instance.UseNotificationSound = value;
        base.RaisePropertyChanged("UseNotificationSound");
      }
    }

    public double NotificationTweetDisplayTime
    {
      get
      {
        return SettingsData.Instance.NotificationTweetDisplayTime;
      }
      set
      {
        if (this.NotificationTweetDisplayTime == value)
          return;
        SettingsData.Instance.NotificationTweetDisplayTime = value;
        base.RaisePropertyChanged("NotificationTweetDisplayTime");
      }
    }

    public double NotificationDisplayTime
    {
      get
      {
        return SettingsData.Instance.NotificationDisplayTime;
      }
      set
      {
        if (this.NotificationDisplayTime == value)
          return;
        SettingsData.Instance.NotificationDisplayTime = value;
        base.RaisePropertyChanged("NotificationDisplayTime");
      }
    }

      private ObservableCollection<MetroTwitScreen> screens;
    public ObservableCollection<MetroTwitScreen> Screens
    {
      get
      {
        return this.screens;
      }
      set
      {
        if (this.screens == value)
          return;
        this.screens = value;
        base.RaisePropertyChanged("Screens");
      }
    }

    public int NotificationScreen
    {
      get
      {
        return SettingsData.Instance.NotificationScreen;
      }
      set
      {
        if (this.NotificationScreen == value)
          return;
        SettingsData.Instance.NotificationScreen = value;
        base.RaisePropertyChanged("NotificationScreen");
      }
    }

    public int NotificationPosition
    {
      get
      {
        return (int) SettingsData.Instance.NotificationPosition;
      }
      set
      {
        if (this.NotificationPosition == value)
          return;
        SettingsData.Instance.NotificationPosition = (NotificationPosition) System.Enum.Parse(typeof (NotificationPosition), value.ToString());
        base.RaisePropertyChanged("NotificationPosition");
      }
    }

    public bool KeepScrollPositionatTop
    {
      get
      {
        return SettingsData.Instance.KeepScrollPositionatTop;
      }
      set
      {
        if (this.KeepScrollPositionatTop == value)
          return;
        SettingsData.Instance.KeepScrollPositionatTop = value;
        base.RaisePropertyChanged("KeepScrollPositionatTop");
      }
    }

    public bool UseSpellChecker
    {
      get
      {
        return SettingsData.Instance.UseSpellChecker;
      }
      set
      {
        if (this.UseSpellChecker == value)
          return;
        SettingsData.Instance.UseSpellChecker = value;
        base.RaisePropertyChanged("UseSpellChecker");
      }
    }

    public bool UseAutoComplete
    {
      get
      {
        return SettingsData.Instance.UseAutoComplete;
      }
      set
      {
        if (this.UseAutoComplete == value)
          return;
        SettingsData.Instance.UseAutoComplete = value;
        base.RaisePropertyChanged("UseAutoComplete");
      }
    }

    public bool PostTweetOnEnter
    {
      get
      {
        return SettingsData.Instance.PostTweetOnEnter;
      }
      set
      {
        if (this.PostTweetOnEnter == value)
          return;
        SettingsData.Instance.PostTweetOnEnter = value;
        base.RaisePropertyChanged("PostTweetOnEnter");
      }
    }

    public int BacklogSeconds
    {
      get
      {
        return Math.Abs(SettingsData.Instance.BacklogSeconds - 120);
      }
      set
      {
        if (this.BacklogSeconds == value)
          return;
        SettingsData.Instance.BacklogSeconds = Math.Abs(value - 120);
        base.RaisePropertyChanged("BacklogSeconds");
        base.RaisePropertyChanged("BacklogSecondsText");
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) SettingsData.Instance.BacklogSeconds), (object) ViewModelMessages.BacklogSeconds);
      }
    }

    public string BacklogSecondsText
    {
      get
      {
        if (SettingsData.Instance.BacklogSeconds == 0)
          return "never (display in real time)";
        else
          return SettingsData.Instance.BacklogSeconds.ToString() + " seconds";
      }
    }

    public bool ShowRefreshProgress
    {
      get
      {
        return SettingsData.Instance.ShowRefreshProgress;
      }
      set
      {
        if (this.ShowRefreshProgress == value)
          return;
        SettingsData.Instance.ShowRefreshProgress = value;
        base.RaisePropertyChanged("ShowRefreshProgress");
      }
    }

    public bool MinimisetoTray
    {
      get
      {
        return SettingsData.Instance.MinimisetoTray;
      }
      set
      {
        if (this.MinimisetoTray == value)
          return;
        SettingsData.Instance.MinimisetoTray = value;
        base.RaisePropertyChanged("MinimisetoTray");
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.TrayIconVisible);
      }
    }

    public int MinimisetoTrayInt
    {
      get
      {
        return this.minimisetoTrayInt;
      }
      set
      {
        if (this.minimisetoTrayInt == value)
          return;
        this.minimisetoTrayInt = value;
        this.MinimisetoTray = this.MinimisetoTrayInt == 1;
        base.RaisePropertyChanged("MinimisetoTrayInt");
      }
    }

    public string Theme
    {
      get
      {
        return SettingsData.Instance.Theme;
      }
      set
      {
        if (!(value != this.Theme))
          return;
        SettingsData.Instance.Theme = value;
        base.RaisePropertyChanged("Theme");
        if (this.Accent != "Aero")
          CommonCommands.ChangeTheme((SharedResourceDictionary) null);
        else
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ChangeThemeToAero);
      }
    }

    public string Accent
    {
      get
      {
        return SettingsData.Instance.Accent;
      }
      set
      {
        if (!(value != this.Accent))
          return;
        SettingsData.Instance.Accent = value;
        base.RaisePropertyChanged("Accent");
        if (this.Accent != "Aero")
          CommonCommands.ChangeTheme((SharedResourceDictionary) null);
        else
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ChangeThemeToAero);
      }
    }

    public ColourAccent AccentColour
    {
      get
      {
        return SettingsData.Instance.AccentColour;
      }
      set
      {
        if (this.AccentColour == value || value == this.AccentColour)
          return;
        SettingsData.Instance.AccentColour = value;
        base.RaisePropertyChanged("AccentColour");
        if (this.changeThemeTimer == null)
          this.changeThemeTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(500.0), DispatcherPriority.Render, (EventHandler) ((o, s) =>
          {
            CommonCommands.ChangeTheme((SharedResourceDictionary) null);
            this.changeThemeTimer.Stop();
          }), System.Windows.Application.Current.Dispatcher);
        else
          this.changeThemeTimer.Start();
      }
    }

    public Artwork Artwork
    {
      get
      {
        return SettingsData.Instance.Artwork;
      }
      set
      {
        if (value == this.Artwork)
          return;
        SettingsData.Instance.Artwork = value;
        base.RaisePropertyChanged("Artwork");
        base.RaisePropertyChanged("ArtworkImage");
        base.RaisePropertyChanged("ArtworkVisibility");
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.UpdateArtwork);
      }
    }

    public Visibility ArtworkVisibility
    {
      get
      {
        return SettingsData.Instance.Artwork == Artwork.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public BitmapImage ArtworkImage
    {
      get
      {
        switch (SettingsData.Instance.Artwork)
        {
          case Artwork.None:
            return (BitmapImage) null;
          case Artwork.Flowers1:
            return CommonCommands.GetImage("Resources/sidebarbg-flower-1.jpg");
          case Artwork.Flowers2:
            return CommonCommands.GetImage("Resources/sidebarbg-flower-2.jpg");
          case Artwork.Grunge1:
            return CommonCommands.GetImage("Resources/sidebarbg-grunge-1.jpg");
          case Artwork.Grunge2:
            return CommonCommands.GetImage("Resources/sidebarbg-grunge-2.jpg");
          case Artwork.Grill:
            return CommonCommands.GetImage("Resources/sidebarbg-grill.jpg");
          case Artwork.Dots:
            return CommonCommands.GetImage("Resources/sidebarbg-dots.jpg");
          case Artwork.Lines:
            return CommonCommands.GetImage("Resources/sidebarbg-lines.jpg");
          default:
            return (BitmapImage) null;
        }
      }
    }

    public IImageUploadService ImageUploadingService
    {
      get
      {
        if (!CoreServices.Instance.ImageUploadingServices.Contains((IImageUploadService) SettingsViewModel.TwitterImageUploadService))
          CoreServices.Instance.ImageUploadingServices.Add((IImageUploadService) SettingsViewModel.TwitterImageUploadService);
        return Enumerable.FirstOrDefault<IImageUploadService>(Enumerable.Where<IImageUploadService>((IEnumerable<IImageUploadService>) CoreServices.Instance.ImageUploadingServices, (Func<IImageUploadService, bool>) (ius => ius.GetType().ToString() == SettingsData.Instance.CurrentImageUploadingService)));
      }
      set
      {
        if (this.ImageUploadingService == value)
          return;
        SettingsData.Instance.CurrentImageUploadingService = value.GetType().ToString();
        CoreServices.Instance.CurrentImageUploadingService = Enumerable.FirstOrDefault<IImageUploadService>(Enumerable.Where<IImageUploadService>((IEnumerable<IImageUploadService>) CoreServices.Instance.ImageUploadingServices, (Func<IImageUploadService, bool>) (ius => ius.GetType().ToString() == SettingsData.Instance.CurrentImageUploadingService)));
        base.RaisePropertyChanged("ImageUploadingService");
      }
    }

    public MTAccountCollection Accounts
    {
      get
      {
        return App.AppState.Accounts;
      }
    }

    public IMapService MapService
    {
      get
      {
        return Enumerable.FirstOrDefault<IMapService>(Enumerable.Where<IMapService>((IEnumerable<IMapService>) CoreServices.Instance.MapServices, (Func<IMapService, bool>) (ius => ius.GetType().ToString() == SettingsData.Instance.CurrentMapService)));
      }
      set
      {
        if (this.MapService == value)
          return;
        SettingsData.Instance.CurrentMapService = value.GetType().ToString();
        CoreServices.Instance.CurrentMapService = Enumerable.FirstOrDefault<IMapService>(Enumerable.Where<IMapService>((IEnumerable<IMapService>) CoreServices.Instance.MapServices, (Func<IMapService, bool>) (ius => ius.GetType().ToString() == SettingsData.Instance.CurrentMapService)));
        base.RaisePropertyChanged("MapService");
      }
    }

    public Visibility SettingsVisible
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

    public string VersionandBuildDate
    {
      get
      {
        return "Version " + ((object) System.Windows.Application.ResourceAssembly.GetName().Version).ToString() + " " + ((AssemblyDescriptionAttribute) System.Windows.Application.ResourceAssembly.GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false)[0]).Description;
      }
    }

    public string Copyright
    {
      get
      {
        return ((AssemblyCopyrightAttribute) System.Windows.Application.ResourceAssembly.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false)[0]).Copyright;
      }
    }

    public TwitterTimeDisplay TwitterTimeDisplay
    {
      get
      {
        return SettingsData.Instance.TwitterTimeDisplay;
      }
      set
      {
        if (this.TwitterTimeDisplay == value)
          return;
        SettingsData.Instance.TwitterTimeDisplay = value;
        base.RaisePropertyChanged("TwitterTimeDisplay");
      }
    }

    public TweetFontSizeDisplay TweetFontSizePreference
    {
      get
      {
        return SettingsData.Instance.TweetFontSizeDisplay;
      }
      set
      {
        if (this.TweetFontSizePreference == value)
          return;
        SettingsData.Instance.TweetFontSizeDisplay = value;
        App.TextMetrics.SetFontSize(value);
        base.RaisePropertyChanged("TweetFontSizePreference");
      }
    }

    public bool ShowTimeofTweet
    {
      get
      {
        return SettingsData.Instance.ShowTimeofTweet;
      }
      set
      {
        if (this.ShowTimeofTweet == value)
          return;
        SettingsData.Instance.ShowTimeofTweet = value;
        base.RaisePropertyChanged("ShowTimeofTweet");
      }
    }

    public bool ShowSourceofTweet
    {
      get
      {
        return SettingsData.Instance.ShowSourceofTweet;
      }
      set
      {
        if (this.ShowSourceofTweet == value)
          return;
        SettingsData.Instance.ShowSourceofTweet = value;
        base.RaisePropertyChanged("ShowSourceofTweet");
      }
    }

    public bool UseProxlet
    {
      get
      {
        return SettingsData.Instance.UseProxlet;
      }
      set
      {
        if (this.UseProxlet == value)
          return;
        SettingsData.Instance.UseProxlet = value;
        base.RaisePropertyChanged("UseProxlet");
      }
    }

    public string ProxletAddress
    {
      get
      {
        return SettingsData.Instance.ProxletAddress;
      }
      set
      {
        if (string.Equals(this.ProxletAddress, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.ProxletAddress = value.LastIndexOf('/') >= value.Length - 1 ? value : value + "/";
        base.RaisePropertyChanged("ProxletAddress");
      }
    }

    public bool ShowMediaPreviews
    {
      get
      {
        return SettingsData.Instance.ShowMediaPreviews;
      }
      set
      {
        if (this.ShowMediaPreviews == value)
          return;
        SettingsData.Instance.ShowMediaPreviews = value;
        base.RaisePropertyChanged("ShowMediaPreviews");
      }
    }

    public TweetListType AdDisplayColumn
    {
      get
      {
        return SettingsData.Instance.AdDisplayColumn;
      }
      set
      {
        if (this.AdDisplayColumn == value)
          return;
        SettingsData.Instance.AdDisplayColumn = value;
        base.RaisePropertyChanged("AdDisplayColumn");
        App.AppState.AdColumnChanged();
      }
    }

    public bool PlusShowAds
    {
      get
      {
        return SettingsData.Instance.PlusShowAds;
      }
      set
      {
        if (this.PlusShowAds == value)
          return;
        SettingsData.Instance.PlusShowAds = value;
        base.RaisePropertyChanged("PlusShowAds");
        App.AppState.AdColumnChanged();
      }
    }

    public string PlusTransactionID
    {
      get
      {
        return SettingsData.Instance.PlusTransactionID;
      }
      set
      {
        if (string.Equals(this.PlusTransactionID, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.PlusTransactionID = value;
        base.RaisePropertyChanged("PlusTransactionID");
      }
    }

    public string PlusEmail
    {
      get
      {
        return SettingsData.Instance.PlusEmail;
      }
      set
      {
        if (string.Equals(this.PlusEmail, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.PlusEmail = value;
        base.RaisePropertyChanged("PlusEmail");
      }
    }

    public bool PlusActive
    {
      get
      {
        return SettingsData.Instance.Boon;
      }
    }

    public bool PlusInActive
    {
      get
      {
        return !SettingsData.Instance.Boon;
      }
    }

    public string ProxyUsername
    {
      get
      {
        return SettingsData.Instance.ProxyUsername;
      }
      set
      {
        if (string.Equals(this.ProxyUsername, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.ProxyUsername = value;
        base.RaisePropertyChanged("ProxyUsername");
        this.UpdateProxyDetails();
      }
    }

    public string ProxyPassword
    {
      get
      {
        return SettingsData.Instance.ProxyPassword;
      }
      set
      {
        if (string.Equals(this.ProxyPassword, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.ProxyPassword = value;
        base.RaisePropertyChanged("ProxyPassword");
        this.UpdateProxyDetails();
      }
    }

    public string Filter
    {
      get
      {
        return string.Join(", ", SettingsData.Instance.Filter.ToArray());
      }
      set
      {
        if (string.Equals(this.Filter, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.Filter = Enumerable.ToList<string>(Enumerable.Where<string>(Enumerable.Select<string, string>((IEnumerable<string>) value.Split(new char[1]
        {
          ','
        }), (Func<string, string>) (y => y.Trim())), (Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
        base.RaisePropertyChanged("Filter");
      }
    }

    public bool DisplayImagesInlineAutomatically
    {
      get
      {
        return SettingsData.Instance.DisplayImagesInlineAutomatically;
      }
      set
      {
        if (this.DisplayImagesInlineAutomatically == value)
          return;
        SettingsData.Instance.DisplayImagesInlineAutomatically = value;
        base.RaisePropertyChanged("DisplayImagesInlineAutomatically");
      }
    }

    public static TwitterImageUploadService TwitterImageUploadService
    {
      get
      {
        if (SettingsViewModel.twitterImageUploadService == null)
          SettingsViewModel.twitterImageUploadService = new TwitterImageUploadService();
        return SettingsViewModel.twitterImageUploadService;
      }
    }

    public string CustomTwitterConsumerKey
    {
      get
      {
        return SettingsData.Instance.CustomTwitterConsumerKey;
      }
      set
      {
        if (string.Equals(this.CustomTwitterConsumerKey, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.CustomTwitterConsumerKey = value;
        base.RaisePropertyChanged("CustomTwitterConsumerKey");
      }
    }

    public string CustomTwitterConsumerSecret
    {
      get
      {
        return SettingsData.Instance.CustomTwitterConsumerSecret;
      }
      set
      {
        if (string.Equals(this.CustomTwitterConsumerSecret, value, StringComparison.Ordinal))
          return;
        SettingsData.Instance.CustomTwitterConsumerSecret = value;
        base.RaisePropertyChanged("CustomTwitterConsumerSecret");
      }
    }
      private RelayCommand backCommand;
    public RelayCommand BackCommand
    {
      get
      {
        return this.backCommand;
      }
      private set
      {
        if (this.backCommand == value)
          return;
        this.backCommand = value;
        base.RaisePropertyChanged("BackCommand");
      }
    }
      private RelayCommand checkForUpdatesCommand;
    public RelayCommand CheckForUpdatesCommand
    {
      get
      {
        return this.checkForUpdatesCommand;
      }
      private set
      {
        if (this.checkForUpdatesCommand == value)
          return;
        this.checkForUpdatesCommand = value;
        base.RaisePropertyChanged("CheckForUpdatesCommand");
      }
    }
      private RelayCommand <object> notificationPositionCommand;
    public RelayCommand<object> NotificationPositionCommand
    {
      get
      {
        return this.notificationPositionCommand;
      }
      private set
      {
        if (this.notificationPositionCommand == value)
          return;
        this.notificationPositionCommand = value;
        base.RaisePropertyChanged("NotificationPositionCommand");
      }
    }
      private RelayCommand clearNestCommand;
    public RelayCommand ClearNestCommand
    {
      get
      {
        return this.clearNestCommand;
      }
      private set
      {
        if (this.clearNestCommand == value)
          return;
        this.clearNestCommand = value;
        base.RaisePropertyChanged("ClearNestCommand");
      }
    }
      private RelayCommand getPlusCommand;
    public RelayCommand GetPlusCommand
    {
      get
      {
        return this.getPlusCommand;
      }
      private set
      {
        if (this.getPlusCommand == value)
          return;
        this.getPlusCommand = value;
        base.RaisePropertyChanged("GetPlusCommand");
      }
    }
      private RelayCommand verifyPlusCommand;
    public RelayCommand VerifyPlusCommand
    {
      get
      {
        return this.verifyPlusCommand;
      }
      private set
      {
        if (this.verifyPlusCommand == value)
          return;
        this.verifyPlusCommand = value;
        base.RaisePropertyChanged("VerifyPlusCommand");
      }
    }
      private RelayCommand getRowiCommand;
    public RelayCommand GetRowiCommand
    {
      get
      {
        return this.getRowiCommand;
      }
      private set
      {
        if (this.getRowiCommand == value)
          return;
        this.getRowiCommand = value;
        base.RaisePropertyChanged("GetRowiCommand");
      }
    }

    public SettingsViewModel()
    {
      this.BackCommand = new RelayCommand(new Action(this.Back));
      this.ClearNestCommand = new RelayCommand(new Action(this.ClearNest));
      this.CheckForUpdatesCommand = new RelayCommand(new Action(CommonCommands.CheckForUpdates));
      this.NotificationPositionCommand = new RelayCommand<object>(new Action<object>(this.NotifyPosition));
      this.GetPlusCommand = new RelayCommand(new Action(this.GetPlus));
      this.VerifyPlusCommand = new RelayCommand(new Action(this.VerifyPlus));
      this.GetRowiCommand = new RelayCommand(new Action(this.GetRowi));
      new Action(this.Initialize).BeginInvoke((AsyncCallback) null, (object) null);
    }

    private void Initialize()
    {
      foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>) App.AppState.Accounts)
        accountViewModel.UpdateAPIRates();
      this.Screens = (ObservableCollection<MetroTwitScreen>) null;
      this.MinimisetoTrayInt = this.MinimisetoTray ? 1 : 0;
      this.Screens = new ObservableCollection<MetroTwitScreen>(Enumerable.Select<Screen, MetroTwitScreen>(Enumerable.Cast<Screen>((IEnumerable) Enumerable.OrderBy<Screen, int>((IEnumerable<Screen>) Screen.AllScreens, (Func<Screen, int>) (x => x.Bounds.Left))), (Func<Screen, MetroTwitScreen>) (screen =>
      {
        MetroTwitScreen local_0 = new MetroTwitScreen();
        local_0.BoundsHeight = (double) (screen.Bounds.Height / 10);
        local_0.BoundsMargin = new Thickness(0.0);
        local_0.BoundsWidth = (double) (screen.Bounds.Width / 10);
        local_0.WorkingAreaHeight = (double) (screen.WorkingArea.Height / 10);
        MetroTwitScreen temp_91 = local_0;
        Rectangle local_2_3 = screen.WorkingArea;
        int temp_95 = local_2_3.Left;
        local_2_3 = screen.Bounds;
        int temp_99 = local_2_3.Left;
        double temp_103 = (double) ((temp_95 - temp_99) / 10);
        local_2_3 = screen.WorkingArea;
        int temp_107 = local_2_3.Top;
        local_2_3 = screen.Bounds;
        int temp_111 = local_2_3.Top;
        double temp_115 = (double) ((temp_107 - temp_111) / 10);
        local_2_3 = screen.Bounds;
        int temp_119 = local_2_3.Right;
        local_2_3 = screen.WorkingArea;
        int temp_123 = local_2_3.Right;
        double temp_127 = (double) ((temp_119 - temp_123) / 10);
        local_2_3 = screen.Bounds;
        int temp_131 = local_2_3.Bottom;
        local_2_3 = screen.WorkingArea;
        int temp_135 = local_2_3.Bottom;
        double temp_139 = (double) ((temp_131 - temp_135) / 10);
        Thickness temp_140 = new Thickness(temp_103, temp_115, temp_127, temp_139);
        temp_91.WorkingAreaMargin = temp_140;
        local_0.WorkingAreaWidth = (double) (screen.WorkingArea.Width / 10);
        return local_0;
      })));
      int num = 0;
      foreach (MetroTwitScreen metroTwitScreen in (Collection<MetroTwitScreen>) this.Screens)
      {
        ++num;
        metroTwitScreen.ScreenNumber = num;
      }
      base.RaisePropertyChanged("Screens");
    }

    private void UpdateProxyDetails()
    {
      if (string.IsNullOrEmpty(this.ProxyUsername) || string.IsNullOrEmpty(this.ProxyPassword))
        return;
      WebRequest.DefaultWebProxy.Credentials = (ICredentials) new NetworkCredential(this.ProxyUsername, this.ProxyPassword);
    }

    private bool ValidateSelectedURLShorteningService()
    {
      foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>) this.Accounts)
      {
        if (accountViewModel.Settings.URLShorteningService != null && (accountViewModel.Settings.URLShorteningService.HasSettings && !accountViewModel.Settings.URLShorteningService.ValidateSettings(accountViewModel.TwitterAccountID)))
        {
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ExpandSettingsServices);
          return false;
        }
      }
      return true;
    }

    private bool ValidateTweetServices()
    {
      foreach (ITweetService tweetService in CoreServices.Instance.TweetServices)
      {
        if (tweetService.HasSettings && !tweetService.ValidateSettings())
        {
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ExpandSettingsServices);
          return false;
        }
      }
      return true;
    }

    private void NotifyServicesToSaveOrCancel()
    {
      foreach (IURLShorteningService shorteningService in CoreServices.Instance.URLShorteningServices)
      {
        if (shorteningService.HasSettings)
          shorteningService.SaveSettings();
      }
      foreach (ITweetService tweetService in CoreServices.Instance.TweetServices)
      {
        if (tweetService.HasSettings)
          tweetService.SaveSettings();
      }
    }

    private void Back()
    {
      if (this.VerifyPlusCommand != null)
        this.VerifyPlusCommand.Execute((object) null);
      if (!this.ValidateSelectedURLShorteningService() || !this.ValidateTweetServices())
        return;
      this.NotifyServicesToSaveOrCancel();
      this.SettingsVisible = Visibility.Collapsed;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) this.SettingsVisible), (object) ViewModelMessages.SettingsVisible);
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ImageUploadServiceChanged);
    }

    private void NotifyPosition(object Parameter)
    {
      this.NotificationPosition = int.Parse(Parameter.ToString());
    }

    private void ClearNest()
    {
      Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate (MessageBoxResult r) {
                if (r == MessageBoxResult.Yes)
                {
                }
            }), DialogType.ClearNest);
    }

    internal void GetPlus()
    {
      CommonCommands.OpenLink("http://www.metrotwit.com/plus");
    }

    internal void GetRowi()
    {
      CommonCommands.OpenLink("http://hiddenpineapple.com/rowi");
    }

    internal void VerifyPlus()
    {
      CommonCommands.CheckBoon(new Action(this.BoonUpdated));
    }

    internal void BoonUpdated()
    {
      base.RaisePropertyChanged("PlusActive");
      base.RaisePropertyChanged("PlusInActive");
      App.AppState.AdColumnChanged();
    }
  }
}
