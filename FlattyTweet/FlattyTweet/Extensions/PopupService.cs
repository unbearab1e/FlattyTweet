
using AnimationScrollViewerOffset;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace FlattyTweet.Extensions
{
  public class PopupService : Window
  {
    private System.Windows.Controls.UserControl activeView = (System.Windows.Controls.UserControl) null;
    private static Dictionary<System.Type, System.Type> pinViewMapping;
    private static PopupService popupServiceInstance;
    private static double dpiXfactor;
    private static double dpiYfactor;
    private TextBlock titleLabel;
    private System.Windows.Controls.Button backButton;
    private System.Windows.Controls.Button pinButton;
    private Border popupContainer;
    private StackPanel contentContainer;
    private ScrollViewer scrollViewer;
    private Stack<System.Windows.Controls.UserControl> previousViews;
    private double contentWidth;

    static PopupService()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PopupService), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PopupService)));
      PopupService.pinViewMapping = new Dictionary<System.Type, System.Type>()
      {
        {
          typeof (ProfileView),
          typeof (TweetListView)
        },
        {
          typeof (TweetListView),
          typeof (TweetListView)
        }
      };
    }

    public PopupService()
    {
      this.previousViews = new Stack<System.Windows.Controls.UserControl>();
      this.Owner = System.Windows.Application.Current.MainWindow;
      this.WindowStyle = WindowStyle.None;
      this.ResizeMode = ResizeMode.NoResize;
      this.ShowInTaskbar = false;
      this.Width = 700.0;
      this.SetWindowHeightAndPosition();
      this.KeyDown += new System.Windows.Input.KeyEventHandler(this.PopupService_KeyDown);
      this.PreviewMouseDown += new MouseButtonEventHandler(this.PopupService_PreviewMouseDown);
      this.Owner.SizeChanged += new SizeChangedEventHandler(this.Owner_SizeChanged);
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.NavigateBack, (Action<GenericMessage<object>>) (o => this.InternalGoBack((System.Windows.Controls.UserControl) null)));
    }

    private void Owner_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.SetWindowHeightAndPosition();
    }

    private void SetWindowHeightAndPosition()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      PopupService.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      PopupService.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
      Screen screen = CommonCommands.CurrentScreen();
      double num1 = System.Windows.Application.Current.MainWindow.WindowState != WindowState.Maximized ? System.Windows.Application.Current.MainWindow.Top : (double) screen.WorkingArea.Y / PopupService.dpiYfactor;
      double num2 = System.Windows.Application.Current.MainWindow.WindowState != WindowState.Maximized ? System.Windows.Application.Current.MainWindow.Left : (double) screen.WorkingArea.X / PopupService.dpiXfactor;
      double num3 = System.Windows.Application.Current.MainWindow.WindowState != WindowState.Maximized ? System.Windows.Application.Current.MainWindow.Width : (double) screen.WorkingArea.Width / PopupService.dpiXfactor;
      this.Height = (System.Windows.Application.Current.MainWindow.WindowState != WindowState.Maximized ? System.Windows.Application.Current.MainWindow.Height : (double) screen.WorkingArea.Height / PopupService.dpiYfactor) - (double) App.AppState.StatusEntryFieldHeight;
      this.Top = num1;
      int num4 = 0;
      this.Left = num2 + (double) num4 + ((num3 - (double) num4) / 2.0 - this.Width / 2.0);
    }

    private void PopupService_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.XButton1 == MouseButtonState.Pressed)
      {
        this.InternalGoBack((System.Windows.Controls.UserControl) null);
      }
      else
      {
        if (InlinePopup.CurrentInline == null)
          return;
        InlinePopup.CurrentInline.Close();
      }
    }

    private void PopupService_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        PopupService.CloseView(false);
      if (e.Key != Key.Back && e.Key != Key.BrowserBack && (Keyboard.Modifiers != ModifierKeys.Alt || e.SystemKey != Key.Left))
        return;
      this.InternalGoBack((System.Windows.Controls.UserControl) null);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.backButton = this.GetTemplateChild("PART_BackButton") as System.Windows.Controls.Button;
      if (this.backButton != null)
      {
        this.backButton.Visibility = Visibility.Collapsed;
        this.backButton.Click += new RoutedEventHandler(this.backButton_Click);
        this.backButton.ContextMenu = (System.Windows.Controls.ContextMenu) null;
      }
      this.popupContainer = this.GetTemplateChild("PART_PopupContainer") as Border;
      this.contentContainer = this.GetTemplateChild("PART_ContentContainer") as StackPanel;
      this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
      this.pinButton = this.GetTemplateChild("PART_PinButton") as System.Windows.Controls.Button;
      if (this.pinButton != null)
      {
        this.pinButton.Visibility = Visibility.Hidden;
        this.pinButton.Click += new RoutedEventHandler(this.pinButton_Click);
      }
      this.titleLabel = this.GetTemplateChild("PART_Title") as TextBlock;
      this.titleLabel.MouseDown += new MouseButtonEventHandler(this.titleLabel_MouseDown);
      PopupService popupService = this;
      double width = this.Width;
      double num1 = this.popupContainer.BorderThickness.Left + this.popupContainer.BorderThickness.Right;
      Thickness margin = this.contentContainer.Margin;
      double left = margin.Left;
      double num2 = num1 + left;
      margin = this.contentContainer.Margin;
      double right = margin.Right;
      double num3 = num2 + right;
      double num4 = width - num3;
      popupService.contentWidth = num4;
    }

    private void titleLabel_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.activeView == null)
        return;
      ScrollViewer scrollViewer1 = (ScrollViewer) null;
      if (this.activeView is TweetListView && (this.activeView as TweetListView).tweets.Items.Count > 0)
        scrollViewer1 = (this.activeView as TweetListView).tweets.Template.FindName("Scroller", (FrameworkElement) (this.activeView as TweetListView).tweets) as ScrollViewer;
      if (this.activeView is ProfileView && (this.activeView as ProfileView).tweets.Items.Count > 0)
        scrollViewer1 = (this.activeView as ProfileView).tweets.Template.FindName("Scroller", (FrameworkElement) (this.activeView as ProfileView).tweets) as ScrollViewer;
      ScrollViewer scrollViewer2;
      if (this.activeView is ListsView)
      {
        if ((this.activeView as ListsView).lists.Items.Count <= 0)
          return;
        scrollViewer2 = (this.activeView as ListsView).lists.Template.FindName("Scroller", (FrameworkElement) (this.activeView as ListsView).lists) as ScrollViewer;
      }
      else if (this.activeView is TrendsView)
      {
        if ((this.activeView as TrendsView).trends.Items.Count <= 0)
          return;
        scrollViewer2 = (this.activeView as TrendsView).trends.Template.FindName("Scroller", (FrameworkElement) (this.activeView as TrendsView).trends) as ScrollViewer;
      }
      else if (scrollViewer1 != null)
      {
        DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        doubleAnimation1.From = new double?(scrollViewer1.VerticalOffset);
        doubleAnimation1.To = new double?(0.0);
        DoubleAnimation doubleAnimation2 = doubleAnimation1;
        ExponentialEase exponentialEase1 = new ExponentialEase();
        exponentialEase1.EasingMode = EasingMode.EaseOut;
        exponentialEase1.Exponent = 6.0;
        ExponentialEase exponentialEase2 = exponentialEase1;
        doubleAnimation2.EasingFunction = (IEasingFunction) exponentialEase2;
        doubleAnimation1.Duration = new Duration(TimeSpan.FromMilliseconds(200.0));
        scrollViewer1.BeginAnimation(ScrollViewerUtilities.VerticalOffsetProperty, (AnimationTimeline) doubleAnimation1);
      }
    }

    private void pinButton_Click(object sender, RoutedEventArgs e)
    {
      if (!PopupService.pinViewMapping.ContainsKey(this.activeView.GetType()))
        return;
      System.Windows.Controls.UserControl userControl = (System.Windows.Controls.UserControl) Activator.CreateInstance(PopupService.pinViewMapping[this.activeView.GetType()], (object[]) null);
      IPopupViewModel popupViewModel = this.activeView.DataContext as IPopupViewModel;
      if (popupViewModel != null)
        popupViewModel.IsTransitioningToPinned = true;
      userControl.DataContext = this.activeView.DataContext;
      PinViewEventArgs content = new PinViewEventArgs()
      {
        View = userControl
      };
      PopupService.CloseView(true);
      Messenger.Default.Send<GenericMessage<PinViewEventArgs>>(new GenericMessage<PinViewEventArgs>(content), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.PinPopup, (content.View.DataContext as TweetListViewModel).TwitterAccountID));
    }

    private void backButton_Click(object sender, RoutedEventArgs e)
    {
      this.InternalGoBack((System.Windows.Controls.UserControl) null);
    }

    private void InternalGoBack(System.Windows.Controls.UserControl viewToGoBackTo = null)
    {
      if (this.previousViews.Count <= 0)
        return;
      if (viewToGoBackTo == null)
        viewToGoBackTo = this.previousViews.Pop();
      System.Windows.Controls.UserControl currentView = this.activeView;
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      doubleAnimation1.From = new double?(this.scrollViewer.HorizontalOffset);
      doubleAnimation1.To = new double?((double) this.contentContainer.Children.IndexOf((UIElement) viewToGoBackTo) * this.contentWidth);
      DoubleAnimation doubleAnimation2 = doubleAnimation1;
      ExponentialEase exponentialEase1 = new ExponentialEase();
      exponentialEase1.EasingMode = EasingMode.EaseOut;
      exponentialEase1.Exponent = 6.0;
      ExponentialEase exponentialEase2 = exponentialEase1;
      doubleAnimation2.EasingFunction = (IEasingFunction) exponentialEase2;
      doubleAnimation1.Duration = new Duration(TimeSpan.FromMilliseconds(400.0));
      doubleAnimation1.Completed += (EventHandler) ((s, e2) =>
      {
        if (this.contentContainer == null)
          return;
        this.contentContainer.Children.Remove((UIElement) currentView);
      });
      this.scrollViewer.BeginAnimation(ScrollViewerUtilities.HorizontalOffsetProperty, (AnimationTimeline) doubleAnimation1);
      this.activeView = viewToGoBackTo;
      IPopupViewModel popupViewModel = this.activeView.DataContext as IPopupViewModel;
      if (popupViewModel != null && (this.titleLabel != null && this.pinButton != null))
      {
        if (!string.IsNullOrEmpty(popupViewModel.PopupTitle))
        {
          this.titleLabel.Text = popupViewModel.PopupTitle;
          this.titleLabel.Visibility = Visibility.Visible;
        }
        else
          this.titleLabel.Visibility = Visibility.Collapsed;
        this.pinButton.Visibility = popupViewModel.AllowPin ? Visibility.Visible : Visibility.Collapsed;
      }
      if (this.previousViews.Count == 0)
      {
        if (this.backButton != null)
          this.backButton.Visibility = Visibility.Collapsed;
      }
      else if (this.previousViews.Peek() != null && this.previousViews.Peek().DataContext is IPopupViewModel)
        this.backButton.ToolTip = (object) string.Format("Back to {0}", (object) (this.previousViews.Peek().DataContext as IPopupViewModel).PopupTitle);
    }

    private void PushView(System.Windows.Controls.UserControl newView)
    {
      bool flag = this.activeView != null;
      IPopupViewModel popupViewModel1 = newView.DataContext as IPopupViewModel;
      string str = popupViewModel1 != null ? popupViewModel1.PopupTitle : string.Empty;
      if (this.contentContainer.Children.Contains((UIElement) newView))
        return;
      if (flag)
      {
        System.Windows.Controls.UserControl userControl = this.activeView;
        this.contentContainer.Children.Add((UIElement) newView);
        DoubleAnimation doubleAnimation1 = new DoubleAnimation();
        doubleAnimation1.From = new double?(this.scrollViewer.HorizontalOffset);
        doubleAnimation1.To = new double?((double) (this.contentContainer.Children.IndexOf((UIElement) newView) + 1) * this.contentWidth);
        DoubleAnimation doubleAnimation2 = doubleAnimation1;
        ExponentialEase exponentialEase1 = new ExponentialEase();
        exponentialEase1.EasingMode = EasingMode.EaseIn;
        exponentialEase1.Exponent = 6.0;
        ExponentialEase exponentialEase2 = exponentialEase1;
        doubleAnimation2.EasingFunction = (IEasingFunction) exponentialEase2;
        doubleAnimation1.Duration = new Duration(TimeSpan.FromMilliseconds(200.0));
        this.scrollViewer.BeginAnimation(ScrollViewerUtilities.HorizontalOffsetProperty, (AnimationTimeline) doubleAnimation1);
        IPopupViewModel popupViewModel2 = this.activeView.DataContext as IPopupViewModel;
        this.backButton.Visibility = Visibility.Visible;
        this.backButton.ToolTip = (object) string.Format("Back to {0}", (object) popupViewModel2.PopupTitle);
        this.previousViews.Push(userControl);
      }
      else
        this.contentContainer.Children.Add((UIElement) newView);
      if (popupViewModel1 != null)
      {
        this.titleLabel.Text = str;
        this.pinButton.Visibility = popupViewModel1.AllowPin ? Visibility.Visible : Visibility.Collapsed;
      }
      this.activeView = newView;
    }

    private void backHistoryMenuItem_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Controls.MenuItem menuItem = sender as System.Windows.Controls.MenuItem;
      if (menuItem == null)
        return;
      System.Windows.Controls.UserControl viewToGoBackTo = menuItem.Tag as System.Windows.Controls.UserControl;
      while (this.previousViews.Peek() != viewToGoBackTo)
        this.contentContainer.Children.Remove((UIElement) this.previousViews.Pop());
      this.backButton.ContextMenu.Items.IndexOf((object) menuItem);
      for (object obj = this.backButton.ContextMenu.Items[0]; obj != menuItem; obj = this.backButton.ContextMenu.Items[0])
        this.backButton.ContextMenu.Items.RemoveAt(0);
      this.backButton.ContextMenu.Items.Remove((object) menuItem);
      if (this.backButton.ContextMenu.Items.Count == 0)
        this.backButton.ContextMenu = (System.Windows.Controls.ContextMenu) null;
      this.previousViews.Pop();
      this.InternalGoBack(viewToGoBackTo);
    }

    public static void ShowView(System.Windows.Controls.UserControl view)
    {
      if (PopupService.popupServiceInstance == null)
      {
        PopupService.popupServiceInstance = new PopupService();
        PopupService.popupServiceInstance.Show();
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.TweetOverlayVisible);
        App.AppState.EnableMainWindowControlBox = false;
      }
      view.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
      view.Width = PopupService.popupServiceInstance.contentWidth;
      PopupService.popupServiceInstance.PushView(view);
    }

    public static void CloseView(bool doNotDisposeActiveView = false)
    {
      if (PopupService.popupServiceInstance == null)
        return;
      PopupService.popupServiceInstance.previousViews.Clear();
      PopupService.popupServiceInstance.Close();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.TweetOverlayVisible);
      App.AppState.EnableMainWindowControlBox = true;
      foreach (System.Windows.Controls.UserControl view in PopupService.popupServiceInstance.contentContainer.Children)
      {
        TweetListViewModel tweetListViewModel = view.DataContext as TweetListViewModel;
        if ((!doNotDisposeActiveView || view != PopupService.popupServiceInstance.activeView) && (tweetListViewModel == null || tweetListViewModel.TweetType != TweetListType.MentionsMyTweetsRetweeted && tweetListViewModel.TweetType != TweetListType.DirectMessages && tweetListViewModel.TweetType != TweetListType.FriendsTimeline))
          PopupService.DisposeViewDataContext(view);
      }
      PopupService.popupServiceInstance.activeView = (System.Windows.Controls.UserControl) null;
      PopupService.popupServiceInstance.contentContainer.Children.Clear();
      PopupService.popupServiceInstance.Owner.SizeChanged -= new SizeChangedEventHandler(PopupService.popupServiceInstance.Owner_SizeChanged);
      PopupService.popupServiceInstance = (PopupService) null;
    }

    private static void DisposeViewDataContext(System.Windows.Controls.UserControl view)
    {
      if (view.DataContext != null && view.DataContext is IDisposable)
        (view as IDisposable).Dispose();
      if (view.DataContext != null && view.DataContext is ViewModelBase)
        (view.DataContext as ViewModelBase).Cleanup();
      TweetListView tweetListView = view as TweetListView;
      if (tweetListView == null || !App.TemporarilyRootedTweetListViews.Contains(tweetListView))
        return;
      App.TemporarilyRootedTweetListViews.Remove(tweetListView);
    }

    public static void PurgeHistory()
    {
      foreach (System.Windows.Controls.UserControl view in PopupService.popupServiceInstance.previousViews)
        PopupService.DisposeViewDataContext(view);
      PopupService.popupServiceInstance.previousViews.Clear();
      PopupService.DisposeViewDataContext(PopupService.popupServiceInstance.activeView);
      PopupService.popupServiceInstance.activeView = (System.Windows.Controls.UserControl) null;
      PopupService.popupServiceInstance.contentContainer.Children.Clear();
      PopupService.popupServiceInstance.backButton.Visibility = Visibility.Collapsed;
      PopupService.popupServiceInstance.backButton.ToolTip = (object) "Back";
    }
  }
}
