using Bugsense.WPF;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using FlattyTweet.HostView;
using FlattyTweet.Model;
using FlattyTweet.MVVM.Messages;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.AddIn.Hosting;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;
using System.Windows.Controls;

namespace FlattyTweet
{
    public class App : System.Windows.Application
    {
        private static readonly string SHORTENED_URL_CACHE_KEY = "ShortenedURLCache";
        private static readonly string MOST_RECENT_SEARCH_TERMS_KEY = "MostRecentSearchTerms";
        private static List<CustomAddIn> addIns;
        private static List<TweetListView> temporarilyRootedTweetListViews;
        private static Dictionary<Decimal, Dictionary<TweetListType, TweetListView>> permanentSpecialTweetListViews;
        private static ApplicationTextMetrics textMetrics;
        private CenterModalWindowHost centerModalWindowHost;
        private ModalWindowHost modalWindowHost;
        private bool currentPromptIsModal;
        private static bool isActive;

        public static List<string> MostRecentSearchTerms
        {
            get
            {
                return (List<string>)System.Windows.Application.Current.Properties[(object)App.MOST_RECENT_SEARCH_TERMS_KEY];
            }
        }

        public static Dictionary<string, Dictionary<string, string>> ShortenedURLCache
        {
            get
            {
                return (Dictionary<string, Dictionary<string, string>>)System.Windows.Application.Current.Properties[(object)App.SHORTENED_URL_CACHE_KEY];
            }
        }

        public static List<CustomAddIn> AddIns
        {
            get
            {
                return App.addIns;
            }
        }

        public static Point? LastURLClickMousePosition { get; set; }

        public static List<TweetListView> TemporarilyRootedTweetListViews
        {
            get
            {
                if (App.temporarilyRootedTweetListViews == null)
                    App.temporarilyRootedTweetListViews = new List<TweetListView>();
                return App.temporarilyRootedTweetListViews;
            }
        }

        public static Dictionary<Decimal, Dictionary<TweetListType, TweetListView>> PermanentSpecialTweetListViews
        {
            get
            {
                if (App.permanentSpecialTweetListViews == null)
                    App.permanentSpecialTweetListViews = new Dictionary<Decimal, Dictionary<TweetListType, TweetListView>>();
                return App.permanentSpecialTweetListViews;
            }
        }

        public static FrameworkElement NotificationsDisplayUITrigger { get; set; }

        public static TwitterTrendLocationCollection AvailableTrendLocations { get; set; }

        public static bool IsActive
        {
            get
            {
                return App.isActive;
            }
            set
            {
                App.isActive = value;
            }
        }

        public static bool HasExceptioned { get; set; }

        public static AppStateModel AppState { get; set; }

        public static double DpiXfactor { get; set; }

        public static double DpiYfactor { get; set; }

        public static ApplicationTextMetrics TextMetrics
        {
            get
            {
                if (App.textMetrics == null)
                {
                    App.textMetrics = new ApplicationTextMetrics();
                    App.textMetrics.SetFontSize(SettingsData.Instance.TweetFontSizeDisplay);
                }
                return App.textMetrics;
            }
        }

        static App()
        {
            App.AppState = new AppStateModel();
        }

        public App()
        {
            BugSense.Init("w8c0e738", (string)null, "http://bugsense.appspot.com/api/errors");
            this.Properties[(object)App.SHORTENED_URL_CACHE_KEY] = (object)new Dictionary<string, Dictionary<string, string>>();
            this.Properties[(object)App.MOST_RECENT_SEARCH_TERMS_KEY] = (object)new List<string>();
            this.LoadExtensions(new Decimal(0));
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(this.NetworkChange_NetworkAvailabilityChanged);
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.Application_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
            this.SessionEnding += new SessionEndingCancelEventHandler(this.App_SessionEnding);
            this.SetProxy();
            this.RegisterMessengerCallbacks();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            App.WriteExceptionLog(e.ExceptionObject as Exception);
        }

        public static void RestartApplication()
        {
            SettingsData.Instance.RestartTriggered = true;
            App app = System.Windows.Application.Current as App;
            if (app != null)
                app.OnExit((ExitEventArgs)null);
            System.Windows.Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }

        public static void LoadAddins()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                AddInStore.Rebuild(baseDirectory);
                IList<AddInToken> list = (IList<AddInToken>)AddInStore.FindAddIns(typeof(CustomAddIn), baseDirectory, new string[0]);
                App.addIns = new List<CustomAddIn>();
                foreach (AddInToken addInToken in (IEnumerable<AddInToken>)list)
                {
                    AddInProcess process = new AddInProcess(Platform.X86);
                    process.Start();
                    AddInStore.FindAddIns(typeof(CustomAddIn), PipelineStoreLocation.ApplicationBase);
                    CustomAddIn customAddIn = addInToken.Activate<CustomAddIn>(process, AddInSecurityLevel.Host);
                    App.addIns.Add(customAddIn);
                }
            }
            catch
            {
            }
        }

        public static async void RefreshSavedSearches(Decimal TwitterAccountID, Action refreshComplete = null)
        {
            TwitterResponse<TwitterSavedSearchCollection> r = await SavedSearches.SavedSearchesAsync(App.AppState.Accounts[TwitterAccountID].Tokens, MetroTwitTwitterizer.Options);
            if (r.Result == RequestResult.Success && r.ResponseObject != null && r.ResponseObject.Count > 0)
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    App.AppState.Accounts[TwitterAccountID].SavedSearches.Clear();
                    AppState.Accounts[TwitterAccountID].SavedSearches.AddRange<TwitterSavedSearch>(r.ResponseObject);
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.SavedSearchesUpdated, TwitterAccountID));
                    if (refreshComplete == null)
                        return;
                    refreshComplete();
                }), new object[0]);
        }

        public static void LogException(Exception e)
        {
            App.WriteExceptionLog(e);
            if (App.HasExceptioned)
                return;
            App.HasExceptioned = true;
            if (e.GetType() != typeof(NotSupportedException) && e.Message != "No imaging component suitable to complete this operation was found.")
            {
                try
                {
                    Exception baseException = e.GetBaseException();
                    ErrorView errorView = new ErrorView(string.Format("Exception Info: {0} {1} {2} {3}", (object)e.Message, (object)Environment.NewLine, (object)Environment.NewLine, baseException != null ? (object)baseException.StackTrace : (object)string.Empty));
                    Messenger.Default.Send<PromptMessage>(new PromptMessage()
                    {
                        IsModal = true,
                        PromptView = (FrameworkElement)errorView,
                        IsCentered = false
                    }, (object)ViewModelMessages.ShowSlidePrompt);
                }
                catch
                {
                }
            }
        }

        private static void WriteExceptionLog(Exception e)
        {
            if (e == null)
                return;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(ApplicationPaths.AppConfigPath, "TempLog.txt")))
                {
                    Exception baseException = e.GetBaseException();
                    string str = string.Format("Exception Info: {0} {1} {2} {3}", (object)e.Message, (object)Environment.NewLine, (object)Environment.NewLine, baseException != null ? (object)baseException.StackTrace : (object)string.Empty);
                    streamWriter.Write(str);
                }
            }
            catch
            {
            }
        }

        public static async void GetLists(Decimal twitterAccountID, Action<TwitterListCollection> callback)
        {
            TwitterResponse<TwitterListCollection> listsResponse = await Lists.GetListsAsync(App.AppState.Accounts[twitterAccountID].Tokens, MetroTwitTwitterizer.GetListsOptions);
            if (listsResponse.Result == RequestResult.Success)
                callback(listsResponse.ResponseObject);
            else
                callback((TwitterListCollection)null);
        }

        public static void StartupStage(StartStage StagetoRun)
        {
            switch (StagetoRun)
            {
                case StartStage.PreUI:
                    SettingsData.Instance.RestartTriggered = false;
                    CommonCommands.ChangeTheme((SharedResourceDictionary)null);
                    App.StartupStage(StartStage.MainWindow);
                    break;
                case StartStage.MainWindow:
                    new MainWindow().Show();
                    break;
                case StartStage.UIRendered:
                    if (SettingsData.Instance.SettingsRecreated)
                    {
                        Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.ConfigError);
                        SettingsData.Instance.SettingsRecreated = false;
                    }
                    App.StartupStage(StartStage.OOBE);
                    break;
                case StartStage.OOBE:
                    if (!SettingsData.Instance.OOBEScreenDisplayed && Enumerable.Count<Decimal>((IEnumerable<Decimal>)CommonCommands.GetAvailableUserAccounts()) > 0)
                    {
                        SettingsData.Instance.OOBEScreenDisplayed = true;
                        SettingsData.Save();
                    }
                    if (!SettingsData.Instance.OOBEScreenDisplayed)
                    {
                        WelcomeView content = new WelcomeView();
                        AccountManagementViewModel model = new AccountManagementViewModel(0M)
                        {
                            IsOOBE = true
                        };
                        content.DataContext = model;
                        Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(content), ViewModelMessages.ShowCenterModalWindowHost);
                    }
                    App.StartupStage(StartStage.ProfileUpdate);
                    break;
                case StartStage.ProfileUpdate:
                    foreach (Decimal num in CommonCommands.GetAvailableUserAccounts())
                    {
                        if (num > new Decimal(0))
                        {
                            UserAccountViewModel accountViewModel = new UserAccountViewModel(num);
                            if (accountViewModel.Settings.TwitterAccountID > new Decimal(0) && !string.IsNullOrEmpty(accountViewModel.Settings.TwitterAccountName))
                            {
                                App.AppState.Accounts.Add(accountViewModel);
                                (System.Windows.Application.Current as App).LoadExtensions(num);
                            }
                            else
                                CommonCommands.DeleteAccountData(num);
                        }
                        else
                        {
                            try
                            {
                                Directory.Delete(ApplicationPaths.AppConfigPath + "\\0", true);
                            }
                            catch
                            {
                            }
                        }
                    }
                    App.AppState.Accounts.Sort<int>((Func<UserAccountViewModel, int>)(x => x.Settings.Index), ListSortDirection.Ascending);
                    if (App.AppState.Accounts.Count > 0)
                        LoginView.Show(System.Windows.Application.Current.MainWindow);
                    else if (SettingsData.Instance.OOBEScreenDisplayed)
                    {
                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseCenterModalWindowHost);
                        AccountManagementViewModel managementViewModel = new AccountManagementViewModel(new Decimal(0));
                        ManageAccountView manageAccountView = new ManageAccountView();
                        manageAccountView.DataContext = (object)managementViewModel;
                        Messenger.Default.Send<GenericMessage<System.Windows.Controls.UserControl>>(new GenericMessage<System.Windows.Controls.UserControl>((System.Windows.Controls.UserControl)manageAccountView), (object)ViewModelMessages.ShowCenterModalWindowHost);
                        managementViewModel.AddUserAccountCommand.Execute((object)null);
                    }
                    foreach (UserAccountViewModel accountViewModel in (Collection<UserAccountViewModel>)App.AppState.Accounts)
                        accountViewModel.UpdateProfile(true);
                    App.StartupStage(StartStage.LoadTwitViews);
                    break;
                case StartStage.LoadTwitViews:
                    ((System.Windows.Application.Current.MainWindow as MainWindow).DataContext as MainViewModel).PostLoad();
                    break;
            }
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            this.SetProxy();
            Messenger.Default.Send<TimerMessage>(new TimerMessage(), (object)TimerMessages.RestRefresh);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.RebootTHEStream);
        }

        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            this.OnExit((ExitEventArgs)null);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.GlobalExceptionHandler(e.Exception);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SettingsData.CheckUpgrade();
            if (SingleInstance.Start() || SettingsData.Instance.RestartTriggered)
            {
                this.EnableProfileOptimisation();
                App.StartupStage(StartStage.PreUI);
            }
            else
            {
                SingleInstance.ShowFirstInstance();
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void EnableProfileOptimisation()
        {
            ProfileOptimization.SetProfileRoot(ApplicationPaths.ProfileOptimisationsPath);
            ProfileOptimization.StartProfile("FlattyTweet.Startup.Profile");
        }

        protected override void OnActivated(EventArgs e)
        {
            App.IsActive = true;
        }

        [DebuggerStepThrough]
        protected override void OnDeactivated(EventArgs e)
        {
            App.IsActive = false;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (App.addIns != null && App.addIns.Count > 0)
            {
                App.addIns[0] = (CustomAddIn)null;
                App.addIns.Clear();
            }
            SingleInstance.Stop();
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.Exit);
            OneTimer.UnregisterTimer((object)TimerMessages.SaveSettings);
            OneTimer.UnregisterTimer((object)TimerMessages.MemoryManager);
            SettingsData.Save();
        }

        private void RegisterMessengerCallbacks()
        {
            Messenger.Default.Register<GenericMessage<System.Windows.Controls.UserControl>>((object)this, (object)ViewModelMessages.ShowCenterModalWindowHost, (Action<GenericMessage<System.Windows.Controls.UserControl>>)(message =>
            {
                if (this.centerModalWindowHost != null)
                    return;
                App temp_35 = this;
                CenterModalWindowHost temp_40 = new CenterModalWindowHost()
                {
                    Owner = System.Windows.Application.Current.MainWindow
                };
                temp_35.centerModalWindowHost = temp_40;
                this.centerModalWindowHost.content.Children.Add((UIElement)message.Content);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)Visibility.Visible), (object)ViewModelMessages.OverlayVisible);
                this.centerModalWindowHost.Show();
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.CloseCenterModalWindowHost, (Action<GenericMessage<object>>)(message =>
            {
                if (this.centerModalWindowHost == null)
                    return;
                this.centerModalWindowHost.content.Children.Clear();
                this.centerModalWindowHost.Close();
                this.centerModalWindowHost = (CenterModalWindowHost)null;
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)Visibility.Collapsed), (object)ViewModelMessages.OverlayVisible);
            }));
            Messenger.Default.Register<PromptMessage>((object)this, (object)ViewModelMessages.ShowSlidePrompt, (Action<PromptMessage>)(message =>
            {
                if (this.modalWindowHost != null)
                    return;
                if (message.IsModal)
                {
                    this.currentPromptIsModal = true;
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)Visibility.Visible), (object)ViewModelMessages.OverlayVisible);
                }
                this.modalWindowHost = new ModalWindowHost();
                if (System.Windows.Application.Current.MainWindow != null)
                    this.modalWindowHost.Owner = System.Windows.Application.Current.MainWindow;
                this.modalWindowHost.content.Children.Add((UIElement)message.PromptView);
                this.modalWindowHost.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                this.modalWindowHost.Show();
            }));
            Messenger.Default.Register<GenericMessage<object>>((object)this, (object)ViewModelMessages.HideSlidePrompt, (Action<GenericMessage<object>>)(message =>
            {
                if (this.modalWindowHost == null)
                    return;
                this.modalWindowHost.content.Children.Clear();
                this.modalWindowHost.Close();
                this.modalWindowHost = (ModalWindowHost)null;
                if (this.currentPromptIsModal)
                {
                    this.currentPromptIsModal = false;
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)Visibility.Collapsed), (object)ViewModelMessages.OverlayVisible);
                }
            }));
        }

        private void PlayMainWindowStoryboard(string storyboardName)
        {
            if (this.MainWindow == null || !this.MainWindow.IsLoaded)
                return;
            this.MainWindow.BeginStoryboard((Storyboard)this.MainWindow.FindResource((object)storyboardName));
        }

        private void SetProxy()
        {
            try
            {
                WebRequest.DefaultWebProxy.Credentials = string.IsNullOrEmpty(SettingsData.Instance.ProxyUsername) || string.IsNullOrEmpty(SettingsData.Instance.ProxyPassword) ? CredentialCache.DefaultCredentials : (ICredentials)new NetworkCredential(SettingsData.Instance.ProxyUsername, SettingsData.Instance.ProxyPassword);
            }
            catch
            {
            }
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
        }

        private void LoadExtensions(Decimal TwitterAccountID)
        {
            CompositionContainer container = new CompositionContainer((ComposablePartCatalog)new DirectoryCatalog("."), new ExportProvider[0]);
            try
            {
                AttributedModelServices.ComposeParts(container, new object[1]
        {
          (object) CoreServices.Instance
        });
                if (TwitterAccountID > new Decimal(0))
                {
                    IURLShorteningService shorteningService = Enumerable.FirstOrDefault<IURLShorteningService>(Enumerable.Where<IURLShorteningService>((IEnumerable<IURLShorteningService>)CoreServices.Instance.URLShorteningServices, (Func<IURLShorteningService, bool>)(ius => ius.GetType().ToString() == App.AppState.Accounts[TwitterAccountID].Settings.CurrentURLShorteningService)));
                    App.AppState.Accounts[TwitterAccountID].Settings.URLShorteningService = shorteningService ?? Enumerable.FirstOrDefault<IURLShorteningService>((IEnumerable<IURLShorteningService>)CoreServices.Instance.URLShorteningServices);
                    if (shorteningService != null)
                        return;
                    App.AppState.Accounts[TwitterAccountID].Settings.CurrentURLShorteningService = CoreServices.Instance.CurrentURLShorteningService(TwitterAccountID) != null ? CoreServices.Instance.CurrentURLShorteningService(TwitterAccountID).GetType().ToString() : string.Empty;
                }
                else
                {
                    CoreServices.Instance.ImageUploadingServices.Add((IImageUploadService)SettingsViewModel.TwitterImageUploadService);
                    IImageUploadService imageUploadService = Enumerable.FirstOrDefault<IImageUploadService>(Enumerable.Where<IImageUploadService>((IEnumerable<IImageUploadService>)CoreServices.Instance.ImageUploadingServices, (Func<IImageUploadService, bool>)(ius => ius.GetType().ToString() == SettingsData.Instance.CurrentImageUploadingService)));
                    CoreServices.Instance.CurrentImageUploadingService = imageUploadService ?? Enumerable.FirstOrDefault<IImageUploadService>((IEnumerable<IImageUploadService>)CoreServices.Instance.ImageUploadingServices);
                    if (imageUploadService == null)
                        SettingsData.Instance.CurrentImageUploadingService = CoreServices.Instance.CurrentImageUploadingService != null ? CoreServices.Instance.CurrentImageUploadingService.GetType().ToString() : string.Empty;
                    IMapService mapService = Enumerable.FirstOrDefault<IMapService>(Enumerable.Where<IMapService>((IEnumerable<IMapService>)CoreServices.Instance.MapServices, (Func<IMapService, bool>)(ius => ius.GetType().ToString() == SettingsData.Instance.CurrentMapService)));
                    CoreServices.Instance.CurrentMapService = mapService ?? Enumerable.FirstOrDefault<IMapService>((IEnumerable<IMapService>)CoreServices.Instance.MapServices);
                    if (mapService == null)
                        SettingsData.Instance.CurrentMapService = CoreServices.Instance.CurrentMapService != null ? CoreServices.Instance.CurrentMapService.GetType().ToString() : string.Empty;
                }
            }
            catch
            {
            }
        }

        public void GlobalExceptionHandler(Exception exception)
        {
            App.LogException(exception);
        }

        [DebuggerNonUserCode]
        [STAThread]
        [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            new App().Run();
        }
    }
}
