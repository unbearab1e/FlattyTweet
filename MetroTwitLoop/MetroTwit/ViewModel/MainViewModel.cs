// Type: MetroTwit.ViewModel.MainViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MetroTwit.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private Visibility tweetsVisible = Visibility.Visible;
    private Visibility leftPaneVisible = Visibility.Visible;
    private Visibility progressVisible = Visibility.Collapsed;
    private WindowState mainWindowState = WindowState.Normal;
    private int progressCount = 0;
    private int overlayCount = 0;
    private Visibility apidebugVisible = Visibility.Collapsed;
    private string apidebugText = "";
    private PathFigureCollection figures;
    private Timer ProgressTimeOut;
    private bool tweetOverlayIsVisible;
    private bool overlayIsVisible;
    private ObservableCollection<TwitViewModel> twitViewModels;
    private MetroTwitErrorViewModel errorMessage;

    public ObservableCollection<TwitViewModel> TwitViewModels
    {
      get
      {
        return this.twitViewModels;
      }
      set
      {
        if (this.twitViewModels == value)
          return;
        this.twitViewModels = value;
        base.RaisePropertyChanged("TwitViewModels");
      }
    }

    public int ProgressCount
    {
      get
      {
        return this.progressCount;
      }
      set
      {
        if (this.progressCount == value)
          return;
        this.progressCount = value;
        base.RaisePropertyChanged("ProgressCount");
        if (this.progressCount <= 0)
          this.ProgressVisible = Visibility.Collapsed;
        else
          this.ProgressVisible = Visibility.Visible;
      }
    }

    public Visibility TweetsVisible
    {
      get
      {
        return this.tweetsVisible;
      }
      set
      {
        if (this.tweetsVisible == value)
          return;
        this.tweetsVisible = value;
        base.RaisePropertyChanged("TweetsVisible");
      }
    }

    public Visibility LeftPaneVisible
    {
      get
      {
        return this.leftPaneVisible;
      }
      set
      {
        if (this.leftPaneVisible == value)
          return;
        this.leftPaneVisible = value;
        base.RaisePropertyChanged("LeftPaneVisible");
      }
    }

    public Visibility ProgressVisible
    {
      get
      {
        return this.progressVisible;
      }
      set
      {
        if (this.progressVisible == value)
          return;
        this.progressVisible = value;
        base.RaisePropertyChanged("ProgressVisible");
      }
    }

    public bool TweetOverlayIsVisible
    {
      get
      {
        return this.tweetOverlayIsVisible;
      }
      set
      {
        if (this.tweetOverlayIsVisible == value)
          return;
        this.tweetOverlayIsVisible = value;
        base.RaisePropertyChanged("TweetOverlayIsVisible");
      }
    }

    public bool OverlayIsVisible
    {
      get
      {
        return this.overlayIsVisible;
      }
      set
      {
        if (this.overlayIsVisible == value)
          return;
        this.overlayIsVisible = value;
        base.RaisePropertyChanged("OverlayIsVisible");
      }
    }

    public WindowState MainWindowState
    {
      get
      {
        return this.mainWindowState;
      }
      set
      {
        if (this.mainWindowState == value)
          return;
        this.mainWindowState = value;
        base.RaisePropertyChanged("MainWindowState");
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ReloadTweetListView);
      }
    }

    public int OverlayCount
    {
      get
      {
        return this.overlayCount;
      }
      set
      {
        if (this.overlayCount == value)
          return;
        this.overlayCount = value;
        base.RaisePropertyChanged("OverlayCount");
        this.UpdateFigures(this.overlayCount > 99 ? "+" : this.overlayCount.ToString());
      }
    }

    public PathFigureCollection Figures
    {
      get
      {
        return this.figures;
      }
      set
      {
        if (this.figures == value)
          return;
        this.figures = value;
        base.RaisePropertyChanged("Figures");
      }
    }

    public Visibility APIDebugVisible
    {
      get
      {
        return this.apidebugVisible;
      }
      set
      {
        if (this.apidebugVisible == value)
          return;
        this.apidebugVisible = value;
        base.RaisePropertyChanged("APIDebugVisible");
      }
    }

    public string APIDebugText
    {
      get
      {
        return this.apidebugText;
      }
      set
      {
        if (string.Equals(this.apidebugText, value, StringComparison.Ordinal))
          return;
        this.apidebugText = value;
        base.RaisePropertyChanged("APIDebugText");
      }
    }

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

    public Visibility ArtworkVisibility
    {
      get
      {
        return SettingsData.Instance.Artwork == Artwork.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }
      private RelayCommand exitCommand;
    public RelayCommand ExitCommand
    {
      get
      {
        return this.exitCommand;
      }
      private set
      {
        if (this.exitCommand == value)
          return;
        this.exitCommand = value;
        base.RaisePropertyChanged("ExitCommand");
      }
    }
      private RelayCommand minimizeCommand;
    public RelayCommand MinimiseCommand
    {
      get
      {
        return this.minimizeCommand;
      }
      private set
      {
        if (this.minimizeCommand == value)
          return;
        this.minimizeCommand = value;
        base.RaisePropertyChanged("MinimiseCommand");
      }
    }
      private RelayCommand maximizeCommand;
    public RelayCommand MaximiseCommand
    {
      get
      {
        return this.maximizeCommand;
      }
      private set
      {
        if (this.maximizeCommand == value)
          return;
        this.maximizeCommand = value;
        base.RaisePropertyChanged("MaximiseCommand");
      }
    }
      private RelayCommand newTweetShortcutCommand;
    public RelayCommand NewTweetShortcutCommand
    {
      get
      {
        return this.newTweetShortcutCommand;
      }
      private set
      {
        if (this.newTweetShortcutCommand == value)
          return;
        this.newTweetShortcutCommand = value;
        base.RaisePropertyChanged("NewTweetShortcutCommand");
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
      private RelayCommand<object> showIncomingOutgoingRequestsCommand;
    public RelayCommand<object> ShowIncomingOutgoingRequestsCommand
    {
      get
      {
        return this.showIncomingOutgoingRequestsCommand;
      }
      private set
      {
        if (this.showIncomingOutgoingRequestsCommand == value)
          return;
        this.showIncomingOutgoingRequestsCommand = value;
        base.RaisePropertyChanged("ShowIncomingOutgoingRequestsCommand");
      }
    }
      private RelayCommand addUserAccountCommand;
    public RelayCommand AddUserAccountCommand
    {
      get
      {
        return this.addUserAccountCommand;
      }
      private set
      {
        if (this.addUserAccountCommand == value)
          return;
        this.addUserAccountCommand = value;
        base.RaisePropertyChanged("AddUserAccountCommand");
      }
    }
      private RelayCommand searchCommand;
    public RelayCommand SearchCommand
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
      private RelayCommand undoTweetCommand;
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
      private RelayCommand switchAccountCommand;
    public RelayCommand SwitchAccountCommand
    {
      get
      {
        return this.switchAccountCommand;
      }
      private set
      {
        if (this.switchAccountCommand == value)
          return;
        this.switchAccountCommand = value;
        base.RaisePropertyChanged("SwitchAccountCommand");
      }
    }
      private RelayCommand switchAccountCommandBack;
    public RelayCommand SwitchAccountCommandBack
    {
      get
      {
        return this.switchAccountCommandBack;
      }
      private set
      {
        if (this.switchAccountCommandBack == value)
          return;
        this.switchAccountCommandBack = value;
        base.RaisePropertyChanged("SwitchAccountCommandBack");
      }
    }

    public MainViewModel()
    {
      this.ExitCommand = new RelayCommand(new Action(this.Exit));
      this.MinimiseCommand = new RelayCommand(new Action(this.Minimise));
      this.MaximiseCommand = new RelayCommand(new Action(this.Maximise));
      this.NewTweetShortcutCommand = new RelayCommand(new Action(this.NewTweetShortcut));
      this.RefreshCommand = new RelayCommand(new Action(this.Refresh));
      this.ShowIncomingOutgoingRequestsCommand = new RelayCommand<object>(new Action<object>(this.ShowIncomingOutgoingRequests));
      this.AddUserAccountCommand = new RelayCommand(new Action(this.AddUserAccount));
      this.SearchCommand = new RelayCommand(new Action(this.Search));
      this.UndoTweetCommand = new RelayCommand(new Action(this.UndoTweet));
      this.SwitchAccountCommand = new RelayCommand(new Action(this.SwitchAccount));
      this.SwitchAccountCommandBack = new RelayCommand(new Action(this.SwitchAccountBack));
      Messenger.Default.Register<GenericMessage<int>>((object) this, (object) ViewModelMessages.ProgressVisible, (Action<GenericMessage<int>>) (o =>
      {
        if (!SettingsData.Instance.ShowRefreshProgress)
          return;
        if (this.ProgressCount + o.Content >= 0)
          this.ProgressCount = this.ProgressCount + o.Content;
        if (this.ProgressTimeOut == null)
        {
          this.ProgressTimeOut = new Timer(10000.0);
          this.ProgressTimeOut.Elapsed += (ElapsedEventHandler) ((sender, e) =>
          {
            this.ProgressTimeOut.Stop();
            this.ProgressCount = 0;
          });
        }
        if (o.Content > 0)
        {
          this.ProgressTimeOut.Stop();
          this.ProgressTimeOut.Start();
        }
        if (this.ProgressCount == 0)
          this.ProgressTimeOut.Stop();
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.OverlayVisible, (Action<GenericMessage<object>>) (o =>
      {
        if ((Visibility) o.Content == Visibility.Visible)
          this.OverlayIsVisible = true;
        else
          this.OverlayIsVisible = false;
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.OverlayCountUpdate, (Action<GenericMessage<object>>) (o => this.OverlayCount = (int) o.Content));
      Messenger.Default.Register<GenericMessage<TwitViewModel>>((object) this, (object) ViewModelMessages.AddTwitView, (Action<GenericMessage<TwitViewModel>>) (o => Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        if (this.TwitViewModels.Contains(o.Content))
          return;
        this.TwitViewModels.Add(o.Content);
      }))));
      Messenger.Default.Register<GenericMessage<TwitViewModel>>((object) this, (object) ViewModelMessages.RemoveTwitView, (Action<GenericMessage<TwitViewModel>>) (o => Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        if (!this.TwitViewModels.Contains(o.Content))
          return;
        this.TwitViewModels.Remove(o.Content);
      }))));
      Messenger.Default.Register<GenericMessage<MetroTwitErrorViewModel>>((object) this, (object) ViewModelMessages.ErrorMessage, (Action<GenericMessage<MetroTwitErrorViewModel>>) (o => Application.Current.Dispatcher.Invoke((Action) (() => this.ErrorMessage = o.Content))));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.UpdateArtwork, (Action<GenericMessage<object>>) (o => Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        base.RaisePropertyChanged("ArtworkImage");
        base.RaisePropertyChanged("ArtworkVisibility");
      }))));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.TweetOverlayVisible, (Action<GenericMessage<object>>) (o =>
      {
        if ((Visibility) o.Content == Visibility.Visible)
          this.TweetOverlayIsVisible = true;
        else
          this.TweetOverlayIsVisible = false;
      }));
    }

    public void PostLoad()
    {
      this.TwitViewModels = new ObservableCollection<TwitViewModel>(Enumerable.Select<UserAccountViewModel, TwitViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, (Func<UserAccountViewModel, TwitViewModel>) (u => u.TwitViewModel)));
      if (!SettingsData.Instance.AutomaticallyCheckForUpdates)
        return;
      SettingsData.Instance.QuietUpdating = true;
      CommonCommands.CheckForUpdates();
    }

    public void UpdateFigures(string textToDisplay)
    {
      int num1 = 11;
      int num2 = 2;
      int num3 = 2;
      if (textToDisplay.Length < 2)
      {
        num1 = 12;
        num2 = 1;
        num3 = 5;
      }
      FormattedText formattedText = new FormattedText(textToDisplay, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Arial"), (double) num1, (Brush) Brushes.White);
      formattedText.SetFontWeight(FontWeights.Bold);
      this.Figures = formattedText.BuildGeometry(new Point((double) num3, (double) num2)).GetFlattenedPathGeometry().Figures;
    }

    private void UndoTweet()
    {
      UserAccountViewModel currentActiveAccount = App.AppState.CurrentActiveAccount;
      if (currentActiveAccount == null)
        return;
      currentActiveAccount.TwitViewModel.UndoLastTweet();
    }

    private void Search()
    {
      UserAccountViewModel currentActiveAccount = App.AppState.CurrentActiveAccount;
      if (currentActiveAccount == null)
        return;
      currentActiveAccount.TwitViewModel.Search(ViewType.Popup);
    }

    private void AddUserAccount()
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.CloseCenterModalWindowHost);
      ManageAccountView manageAccountView = new ManageAccountView();
      manageAccountView.DataContext = (object) new AccountManagementViewModel(new Decimal(0));
      Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl) manageAccountView), (object) ViewModelMessages.ShowCenterModalWindowHost);
    }

    private void Exit()
    {
      Application.Current.Shutdown();
    }

    private void Minimise()
    {
      this.MainWindowState = WindowState.Minimized;
    }

    private void Maximise()
    {
      this.MainWindowState = this.MainWindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void NewTweetShortcut()
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.NewTweetEditFocus, Enumerable.First<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, (Func<UserAccountViewModel, bool>) (x => x.TwitViewModel.IsActive))).TwitterAccountID));
    }

    private void Refresh()
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.TweetRefresh);
    }

    private void ShowIncomingOutgoingRequests(object target)
    {
    }

    private void SwitchAccount()
    {
      int num = App.AppState.Accounts.IndexOf(App.AppState.CurrentActiveAccount);
      App.AppState.SwitchToAccount(num >= App.AppState.Accounts.Count - 1 ? Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, 0).TwitterAccountID : Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, num + 1).TwitterAccountID);
    }

    private void SwitchAccountBack()
    {
      int num = App.AppState.Accounts.IndexOf(App.AppState.CurrentActiveAccount);
      App.AppState.SwitchToAccount(num <= 0 ? Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, App.AppState.Accounts.Count - 1).TwitterAccountID : Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, num - 1).TwitterAccountID);
    }
  }
}
