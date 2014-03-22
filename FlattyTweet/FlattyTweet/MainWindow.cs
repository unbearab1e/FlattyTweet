using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FlattyTweet
{
  public partial class MainWindow : Window
  {
    private double previousHeight = 0.0;
    private NotifyIcon TaskbarIcon = new NotifyIcon();
    private System.Timers.Timer resizeTimer = new System.Timers.Timer(100.0)
    {
      Enabled = false
    };
    private const int defaultwidth = 1024;
    private const int defaultheight = 700;
    private SettingsView settingsView;
    private double prevMousePointY;
    private HwndSource hwndSource;
   

    public MainWindow()
    {
      this.InitializeComponent();
      MainViewModel mainViewModel = new MainViewModel();
      mainViewModel.PropertyChanged += new PropertyChangedEventHandler(this.MainWindow_PropertyChanged);
      this.DataContext = (object) mainViewModel;
      this.SourceInitialized += new EventHandler(this.InitializeWindowSource);
      this.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
      this.ContentRendered += new EventHandler(this.MainWindow_ContentRendered);
      this.resizeTimer.Elapsed += new ElapsedEventHandler(this.ResizingDone);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    private void InitializeDPI()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      App.DpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      App.DpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            this.hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        }
        catch
        {
        }
        this.InitializeDPI();
        double minWidth = System.Windows.Application.Current.MainWindow.MinWidth;
        double minHeight = System.Windows.Application.Current.MainWindow.MinHeight;
        if (SettingsData.Instance.Window.Height == 0.0)
        {
            SettingsData.Save();
        }
        base.Width = SettingsData.Instance.Window.Width;
        base.Height = SettingsData.Instance.Window.Height;
        base.Top = SettingsData.Instance.Window.Top;
        base.Left = SettingsData.Instance.Window.Left;
        CommonCommands.GetAvailableUserAccounts();
        Screen screen = CommonCommands.CurrentScreen();
        if ((((SettingsData.Instance.Window.Left == 0.0) && (SettingsData.Instance.Window.Top == 0.0)) && (SettingsData.Instance.Window.Height == 0.0)) && (SettingsData.Instance.Window.Width == 0.0))
        {
            base.Width = 1024.0;
            base.Height = 700.0;
            base.Top = (screen.WorkingArea.Height - (base.Height * App.DpiXfactor)) / 2.0;
            base.Left = (screen.WorkingArea.Width - (base.Width * App.DpiXfactor)) / 2.0;
        }
        else
        {
            base.Width = ((base.Width * App.DpiXfactor) > screen.WorkingArea.Width) ? (((double)screen.WorkingArea.Width) / App.DpiXfactor) : ((base.Width < minWidth) ? minWidth : base.Width);
            base.Height = ((base.Height * App.DpiYfactor) > screen.WorkingArea.Height) ? (((double)screen.WorkingArea.Height) / App.DpiYfactor) : ((base.Height < minHeight) ? minHeight : base.Height);
            base.Top = ((base.Top >= (screen.WorkingArea.Top + screen.WorkingArea.Height)) || (base.Top < screen.WorkingArea.Top)) ? (screen.WorkingArea.Top + ((screen.WorkingArea.Height - (base.Height * App.DpiXfactor)) / 2.0)) : base.Top;
            base.Left = ((base.Left >= (screen.WorkingArea.Left + screen.WorkingArea.Width)) || (base.Left < screen.WorkingArea.Left)) ? (screen.WorkingArea.Left + ((screen.WorkingArea.Width - (base.Width * App.DpiYfactor)) / 2.0)) : base.Left;
        }
        if (SettingsData.Instance.Window.WindowState == WindowState.Minimized)
        {
            (base.DataContext as MainViewModel).MainWindowState = WindowState.Normal;
        }
        else
        {
            (base.DataContext as MainViewModel).MainWindowState = SettingsData.Instance.Window.WindowState;
        }
        Messenger.Default.Register<GenericMessage<Tuple<UserAccountViewModel, bool>>>(this, ViewModelMessages.SetActiveAccount, delegate(GenericMessage<Tuple<UserAccountViewModel, bool>> o)
        {
            bool flag = false;
            ContentPresenter control = this.accountProfilesList.ItemContainerGenerator.ContainerFromItem(o.Content.Item1) as ContentPresenter;
            VisualStateManager.GoToState(control, "AccountActive", false);
            foreach (UserAccountViewModel model in App.AppState.Accounts)
            {
                if (model != o.Content.Item1)
                {
                    ContentPresenter presenter2 = this.accountProfilesList.ItemContainerGenerator.ContainerFromItem(model) as ContentPresenter;
                    VisualStateManager.GoToState(presenter2, "AccountInactive", false);
                }
            }
            DataTemplate contentTemplate = control.ContentTemplate;
            FrameworkElement element = null;
            if (control.IsLoaded)
            {
                FrameworkElement element2;
                if (App.AppState.ShowCompactAccountPane)
                {
                    element2 = contentTemplate.FindName("CompactLayout", control) as FrameworkElement;
                    element = element2.FindName("CompactUserAvatar") as FrameworkElement;
                }
                else
                {
                    element2 = contentTemplate.FindName("FullLayout", control) as FrameworkElement;
                    element = element2.FindName("FullUserAvatar") as FrameworkElement;
                }
            }
            if (element != null)
            {
                System.Windows.Point point = this.accountProfilesList.PointFromScreen(element.PointToScreen(new System.Windows.Point(0.0, 0.0)));
                double top = (point.Y + (element.ActualHeight / 2.0)) - (this.accountArrowIndicator.ActualHeight / 2.0);
                Thickness thickness = new Thickness(0.0, top, 0.0, 0.0);
                if (thickness.Top > this.accountArrowIndicator.Margin.Top)
                {
                    flag = true;
                }
                else if (thickness.Top <= this.accountArrowIndicator.Margin.Top)
                {
                    flag = false;
                }
                this.accountArrowIndicator.Margin = thickness;
                this.AccountsScrollViewer.ScrollToVerticalOffset(point.Y);
            }
            if (this.twitViewItems.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                if (App.AppState.LastActiveAccount != null)
                {
                    ContentPresenter templatedParent = this.twitViewItems.ItemContainerGenerator.ContainerFromItem(App.AppState.LastActiveAccount.TwitViewModel) as ContentPresenter;
                    DataTemplate template2 = templatedParent.ContentTemplate;
                    FrameworkElement lastActiveTwitView = template2.FindName("twitViewInstance", templatedParent) as FrameworkElement;
                    lastActiveTwitView.CacheMode = new BitmapCache();
                    Storyboard storyboard = new Storyboard();
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300.0))
                    };
                    ExponentialEase ease = new ExponentialEase
                    {
                        EasingMode = EasingMode.EaseOut,
                        Exponent = 6.0
                    };
                    animation.EasingFunction = ease;
                    storyboard.Children.Add(animation);
                    DoubleAnimation animation2 = new DoubleAnimation
                    {
                        From = 0.0,
                        To = new double?(flag ? ((double)50) : ((double)(-50))),
                        Duration = new Duration(TimeSpan.FromMilliseconds(300.0))
                    };
                    ExponentialEase ease2 = new ExponentialEase
                    {
                        EasingMode = EasingMode.EaseOut,
                        Exponent = 6.0
                    };
                    animation2.EasingFunction = ease2;
                    storyboard.Children.Add(animation2);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("(Opacity)", new object[0]));
                    Storyboard.SetTargetProperty(animation2, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)", new object[0]));
                    App.AppState.LastActiveAccount.TwitViewModel.IsActive = false;
                    storyboard.Completed += (s3, e3) => lastActiveTwitView.CacheMode = null;
                    lastActiveTwitView.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace);
                }
                if (o.Content.Item2)
                {
                    ContentPresenter presenter4 = this.twitViewItems.ItemContainerGenerator.ContainerFromItem(o.Content.Item1.TwitViewModel) as ContentPresenter;
                    DataTemplate template3 = presenter4.ContentTemplate;
                    FrameworkElement newActiveTwitView = template3.FindName("twitViewInstance", presenter4) as FrameworkElement;
                    o.Content.Item1.TwitViewModel.IsActive = true;
                    newActiveTwitView.CacheMode = new BitmapCache();
                    Storyboard storyboard2 = new Storyboard();
                    DoubleAnimation animation3 = new DoubleAnimation
                    {
                        From = 0.0,
                        To = 1.0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300.0))
                    };
                    ExponentialEase ease3 = new ExponentialEase
                    {
                        EasingMode = EasingMode.EaseOut,
                        Exponent = 6.0
                    };
                    animation3.EasingFunction = ease3;
                    storyboard2.Children.Add(animation3);
                    DoubleAnimation animation4 = new DoubleAnimation
                    {
                        From = new double?(flag ? ((double)(-50)) : ((double)50)),
                        To = 0.0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300.0))
                    };
                    ExponentialEase ease4 = new ExponentialEase
                    {
                        EasingMode = EasingMode.EaseOut,
                        Exponent = 6.0
                    };
                    animation4.EasingFunction = ease4;
                    storyboard2.Children.Add(animation4);
                    storyboard2.BeginTime = new TimeSpan?(new TimeSpan(150L));
                    Storyboard.SetTargetProperty(animation3, new PropertyPath("(Opacity)", new object[0]));
                    Storyboard.SetTargetProperty(animation4, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)", new object[0]));
                    storyboard2.Completed += (s3, e3) => newActiveTwitView.CacheMode = null;
                    newActiveTwitView.BeginStoryboard(storyboard2, HandoffBehavior.SnapshotAndReplace);
                }
                foreach (UserAccountViewModel model in App.AppState.Accounts)
                {
                    if ((model != o.Content.Item1) && model.TwitViewModel.IsActive)
                    {
                        model.TwitViewModel.IsActive = false;
                    }
                }
                o.Content.Item1.TwitViewModel.IsActive = true;
            }
            else
            {
                if (App.AppState.LastActiveAccount != null)
                {
                    App.AppState.LastActiveAccount.TwitViewModel.IsActive = false;
                }
                o.Content.Item1.TwitViewModel.IsActive = true;
            }
        });
    }

    [DebuggerStepThrough]
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
        this.ShowWindow((GenericMessage<object>) null);
      return IntPtr.Zero;
    }

    private void MainWindow_ContentRendered(object sender, EventArgs e)
    {
      this.TaskbarIcon.Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("/Resources/Twitter2.ico", UriKind.Relative)).Stream, new System.Drawing.Size(16, 16));
      this.TaskbarIcon.Visible = SettingsData.Instance.MinimisetoTray;
      this.TaskbarIcon.Click += new EventHandler(this.TaskbarIcon_Click);
      App.StartupStage(StartStage.UIRendered);
    }

    private void TaskbarIcon_Click(object sender, EventArgs e)
    {
      this.ShowWindow((GenericMessage<object>) null);
    }

    private void InitializeWindowSource(object sender, EventArgs e)
    {
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.Exit, (Action<GenericMessage<object>>) (o =>
      {
        SettingsData.Instance.Window.Left = this.Left;
        SettingsData.Instance.Window.Top = this.Top;
        if (this.WindowState != WindowState.Maximized)
        {
          SettingsData.Instance.Window.Height = this.Height;
          SettingsData.Instance.Window.Width = this.Width;
        }
        SettingsData.Instance.Window.WindowState = this.WindowState;
        if (this.TaskbarIcon != null)
        {
          this.TaskbarIcon.Visible = false;
          this.TaskbarIcon.Dispose();
          this.TaskbarIcon = (NotifyIcon) null;
        }
        SettingsData.Save();
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.SettingsVisible, (Action<GenericMessage<object>>) (o =>
      {
        if ((Visibility) o.Content == Visibility.Collapsed)
        {
          this.LayoutRoot.Children.Remove((UIElement) this.settingsView);
          this.settingsView = (SettingsView) null;
          this.twitViewItems.Visibility = Visibility.Visible;
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.TweetColumnAnimation);
          (this.DataContext as MainViewModel).LeftPaneVisible = Visibility.Visible;
          Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.AfterSettings);
        }
        else
          this.SettingsButton_Click((object) null, new RoutedEventArgs());
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.MainWindowShow, new Action<GenericMessage<object>>(this.ShowWindow));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.TrayIconVisible, (Action<GenericMessage<object>>) (o => System.Windows.Application.Current.Dispatcher.BeginInvoke((Action) (() => this.TaskbarIcon.Visible = SettingsData.Instance.MinimisetoTray), new object[0])));
      Messenger.Default.Register<GenericMessage<int>>((object) this, (object) ViewModelMessages.SizeUpdated, (Action<GenericMessage<int>>) (o =>
      {
        this.ManageAccountsButton.Height = (double) o.Content;
        this.tweetOverlay.Margin = new Thickness(0.0, 0.0, 0.0, (double) o.Content);
      }));
    }

    private void ShowWindow(GenericMessage<object> o)
    {
      this.Show();
      if ((this.DataContext as MainViewModel).MainWindowState == WindowState.Minimized)
        (this.DataContext as MainViewModel).MainWindowState = WindowState.Normal;
      this.Activate();
      this.Focus();
    }

    private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "OverlayCount"))
        return;
      System.Windows.Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        if ((this.DataContext as MainViewModel).OverlayCount > 0)
          this.TaskbarItemInfo.Overlay = (ImageSource) new DrawingImage((this.Resources[(object) "OverlayBrush"] as DrawingBrush).Drawing);
        else
          this.TaskbarItemInfo.Overlay = (ImageSource) null;
      }), new object[0]);
    }

    private void HeaderPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      PopupService.CloseView(false);
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
      SettingsData.Instance.WindowResizing = true;
      switch (this.WindowState)
      {
        case WindowState.Normal:
          this.MaximiseBtn.Visibility = Visibility.Visible;
          this.MaximizeMenuItem.Visibility = Visibility.Visible;
          this.RestoreBtn.Visibility = Visibility.Collapsed;
          this.RestoreMenuItem.Visibility = Visibility.Collapsed;
          this.CloseBtnMax.Visibility = Visibility.Collapsed;
          this.CloseBtn.Visibility = Visibility.Visible;
          break;
        case WindowState.Minimized:
          if (SettingsData.Instance.MinimisetoTray)
          {
            this.Hide();
            break;
          }
          else
            break;
        case WindowState.Maximized:
          this.MaximiseBtn.Visibility = Visibility.Collapsed;
          this.MaximizeMenuItem.Visibility = Visibility.Collapsed;
          this.RestoreBtn.Visibility = Visibility.Visible;
          this.RestoreMenuItem.Visibility = Visibility.Visible;
          this.CloseBtnMax.Visibility = Visibility.Visible;
          this.CloseBtn.Visibility = Visibility.Collapsed;
          break;
      }
      SettingsData.Instance.WindowResizing = false;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
      PopupService.CloseView(false);
      MainWindow mainWindow = this;
      SettingsView settingsView1 = new SettingsView();
      settingsView1.Margin = new Thickness(0.0, this.HeaderPanel.Height, 0.0, 0.0);
      SettingsView settingsView2 = settingsView1;
      mainWindow.settingsView = settingsView2;
      this.twitViewItems.Visibility = Visibility.Hidden;
      (this.DataContext as MainViewModel).LeftPaneVisible = Visibility.Hidden;
      this.LayoutRoot.Children.Add((UIElement) this.settingsView);
    }

    private void Main_Closed(object sender, EventArgs e)
    {
      System.Windows.Application.Current.Shutdown();
    }

    private void Main_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (InlinePopup.CurrentInline == null)
        return;
      InlinePopup.CurrentInline.Close();
    }

    private void DockedColumnButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      SettingsData.Instance.PopupTarget = (UIElement) this.accountArrowIndicator;
    }

    private void tweetOverlay_MouseDown_1(object sender, MouseButtonEventArgs e)
    {
      PopupService.CloseView(false);
    }

    private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.resizeTimer.Stop();
      this.resizeTimer.Start();
    }

    private void ResizingDone(object sender, ElapsedEventArgs e)
    {
      this.resizeTimer.Stop();
      System.Windows.Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.ReloadTweetListView);
        SettingsData.Instance.Window.Height = this.Height;
        SettingsData.Instance.Window.Width = this.Width;
      }), new object[0]);
    }

    
  }
}
