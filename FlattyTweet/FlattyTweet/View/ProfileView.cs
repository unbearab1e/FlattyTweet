
using AnimationScrollViewerOffset;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Behaviors;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace FlattyTweet.View
{
  public partial class ProfileView : BaseTweetListView 
  {
    private UnreadMarkerListBoxBehavior unreadmarker;
    private KeepTopScrollerBehavior scrollposition;
    private AnimatedTweetListBox animatedtweetlistbox;

    public override ListBox TweetListBox
    {
      get
      {
        return this.tweets;
      }
    }

    public ProfileView()
    {
      this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      (this.DataContext as TweetListViewModel).PropertyChanged += new PropertyChangedEventHandler(this.TweetListView_PropertyChanged);
    }

    private void TweetListView_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ShowContentVisibility" && (this.DataContext as TweetListViewModel).ShowContentVisibility)
      {
        if ((this.DataContext as TweetListViewModel).TitleBarVisible)
        {
          this.unreadmarker = new UnreadMarkerListBoxBehavior();
          if (this.unreadmarker != null)
            Interaction.GetBehaviors((DependencyObject) this.tweets).Add((Behavior) this.unreadmarker);
        }
        this.scrollposition = new KeepTopScrollerBehavior();
        if (this.scrollposition != null)
          Interaction.GetBehaviors((DependencyObject) this).Add((Behavior) this.scrollposition);
        this.animatedtweetlistbox = new AnimatedTweetListBox();
        if (this.animatedtweetlistbox != null)
          Interaction.GetBehaviors((DependencyObject) this).Add((Behavior) this.animatedtweetlistbox);
      }
      if (!(e.PropertyName == "CurrentTweetID") || (this.unreadmarker == null || !(this.DataContext as TweetListViewModel).TitleBarVisible))
        return;
      this.unreadmarker.SetMarker();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
      TweetListViewModel tweetListViewModel = this.DataContext as TweetListViewModel;
      tweetListViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.TweetListView_PropertyChanged);
      if (!tweetListViewModel.IsTransitioningToPinned)
      {
        tweetListViewModel.Cleanup();
        tweetListViewModel.IsTransitioningToPinned = false;
      }
      if (!(SettingsData.Instance.PopupTarget is Button))
        SettingsData.Instance.PopupTarget = (UIElement) null;
      SettingsData.Instance.SelectedColumn = (UIElement) null;
      SettingsData.Instance.DialogActiveControl = (FrameworkElement) null;
      if (this.unreadmarker != null)
        Interaction.GetBehaviors((DependencyObject) this.tweets).Remove((Behavior) this.unreadmarker);
      if (this.scrollposition != null)
        Interaction.GetBehaviors((DependencyObject) this).Remove((Behavior) this.scrollposition);
      if (this.animatedtweetlistbox != null)
        Interaction.GetBehaviors((DependencyObject) this).Remove((Behavior) this.animatedtweetlistbox);
      this.unreadmarker = (UnreadMarkerListBoxBehavior) null;
      this.scrollposition = (KeepTopScrollerBehavior) null;
      this.animatedtweetlistbox = (AnimatedTweetListBox) null;
    }

    private void TweetDisplay_KeyDown(object sender, KeyEventArgs e)
    {
      if (!this.IsEnabled || (!Keyboard.IsKeyUp(Key.LeftShift) || !Keyboard.IsKeyUp(Key.RightShift)))
        return;
      if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.Right))
        Messenger.Default.Send<KeyboardShortcutMessage>(new KeyboardShortcutMessage((FrameworkElement) this, Keyboard.IsKeyDown(Key.Left) ? KeyboardShortcutType.ColumnLeft : KeyboardShortcutType.ColumnRight), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.KeyboardShortcut, (this.DataContext as TweetListViewModel).TwitterAccountID));
      if (Keyboard.IsKeyDown(Key.D))
      {
        MetroTwitStatusBase metroTwitStatusBase = this.tweets.SelectedItem as MetroTwitStatusBase;
        if (metroTwitStatusBase != null)
        {
          if (!metroTwitStatusBase.IsFavourited)
            CommonCommands.Favourite(metroTwitStatusBase.ID, (this.DataContext as TweetListViewModel).TwitterAccountID);
          else
            CommonCommands.UnFavourite(metroTwitStatusBase.ID, (this.DataContext as TweetListViewModel).TwitterAccountID);
        }
      }
    }

    private void tweets_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.tweets.SelectedIndex == -1 || !((this.tweets.SelectedItem as MetroTwitStatusBase).ID > (this.DataContext as TweetListViewModel).CurrentTweetID))
        return;
      (this.DataContext as TweetListViewModel).CurrentTweetID = (this.tweets.SelectedItem as MetroTwitStatusBase).ID;
      (this.DataContext as TweetListViewModel).UpdateReadTweets(new Decimal?());
      if (this.unreadmarker != null && (this.DataContext as TweetListViewModel).TitleBarVisible)
        this.unreadmarker.SetMarker();
    }

    private void TweetAd_MouseEvent(object sender, MouseEventArgs e)
    {
      DockPanel dockPanel = sender as DockPanel;
      if (dockPanel.IsMouseOver)
        VisualStateManager.GoToElementState((FrameworkElement) dockPanel, "MouseOver", true);
      else
        VisualStateManager.GoToElementState((FrameworkElement) dockPanel, "BaseState", true);
    }

    private void ProfileOptionsButton_Click(object sender, RoutedEventArgs e)
    {
      this.UserOptionsMenu.PlacementTarget = (UIElement) this;
      this.UserOptionsMenu.IsOpen = true;
    }

    private void AdControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      SettingsData.Instance.PopupTarget = (UIElement) this.AdControl;
    }

    private void ColumnTitle_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.tweets.Items.Count <= 0 || e.LeftButton != MouseButtonState.Pressed && e.ClickCount != 2)
        return;
      ScrollViewer scrollViewer = this.tweets.Template.FindName("Scroller", (FrameworkElement) this.tweets) as ScrollViewer;
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      doubleAnimation1.From = new double?(scrollViewer.VerticalOffset);
      doubleAnimation1.To = new double?(0.0);
      DoubleAnimation doubleAnimation2 = doubleAnimation1;
      ExponentialEase exponentialEase1 = new ExponentialEase();
      exponentialEase1.EasingMode = EasingMode.EaseOut;
      exponentialEase1.Exponent = 6.0;
      ExponentialEase exponentialEase2 = exponentialEase1;
      doubleAnimation2.EasingFunction = (IEasingFunction) exponentialEase2;
      doubleAnimation1.Duration = new Duration(TimeSpan.FromMilliseconds(400.0));
      scrollViewer.BeginAnimation(ScrollViewerUtilities.VerticalOffsetProperty, (AnimationTimeline) doubleAnimation1);
      if (e.ClickCount == 2)
        (this.DataContext as TweetListViewModel).MarkasReadCommand.Execute((object) null);
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      TweetListViewModel tweetListViewModel = this.DataContext as TweetListViewModel;
      tweetListViewModel.DisplayTypeInt = this.Display.SelectedIndex;
      switch (tweetListViewModel.DisplayType)
      {
        case DisplayType.Tweets:
          this.tweets.ItemsSource = (IEnumerable) tweetListViewModel.Tweets;
          break;
        case DisplayType.Followers:
          this.tweets.ItemsSource = (IEnumerable) tweetListViewModel.Followers.Tweets;
          break;
        case DisplayType.Following:
          this.tweets.ItemsSource = (IEnumerable) tweetListViewModel.Following.Tweets;
          break;
        case DisplayType.ListedIn:
          this.tweets.ItemsSource = (IEnumerable) tweetListViewModel.ListedIn.Tweets;
          break;
      }
    }

    


  }
}
