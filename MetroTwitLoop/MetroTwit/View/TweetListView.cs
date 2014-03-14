// Type: MetroTwit.View.TweetListView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using AnimationScrollViewerOffset;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Behaviors;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetroTwit.View
{
    public partial class TweetListView : BaseTweetListView 
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

    public TweetListView()
    {
      this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.TweetColumnAnimation, (Action<GenericMessage<object>>) (o => this.LayoutRoot.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, (object) this.LayoutRoot))));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.ThemeChanged, (Action<GenericMessage<object>>) (o => this.ThemeChanged()));
      (this.DataContext as TweetListViewModel).PropertyChanged += new PropertyChangedEventHandler(this.TweetListView_PropertyChanged);
      this.RootCurrentView();
      this.CheckandSetupBehaviours();
    }

    private void ThemeChanged()
    {
      this.RemoveBehaviours();
      this.CheckandSetupBehaviours();
    }

    private void RemoveBehaviours()
    {
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

    private void CheckandSetupBehaviours()
    {
      if (!(this.DataContext as TweetListViewModel).ShowContentVisibility)
        return;
      if ((this.DataContext as TweetListViewModel).TitleBarVisible)
      {
        if (this.unreadmarker == null)
        {
          this.unreadmarker = new UnreadMarkerListBoxBehavior();
          this.unreadmarker.ViewModel = this.DataContext as TweetListViewModel;
        }
        if (!Interaction.GetBehaviors((DependencyObject) this.tweets).Contains((Behavior) this.unreadmarker))
          Interaction.GetBehaviors((DependencyObject) this.tweets).Add((Behavior) this.unreadmarker);
      }
      if (this.scrollposition == null)
        this.scrollposition = new KeepTopScrollerBehavior();
      if (!Interaction.GetBehaviors((DependencyObject) this).Contains((Behavior) this.scrollposition))
        Interaction.GetBehaviors((DependencyObject) this).Add((Behavior) this.scrollposition);
      if (this.animatedtweetlistbox == null)
        this.animatedtweetlistbox = new AnimatedTweetListBox();
      if (!Interaction.GetBehaviors((DependencyObject) this).Contains((Behavior) this.animatedtweetlistbox))
        Interaction.GetBehaviors((DependencyObject) this).Add((Behavior) this.animatedtweetlistbox);
    }

    private void TweetListView_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ColumnBackVisible")
      {
        if ((this.DataContext as TweetListViewModel).TweetType == TweetListType.Search)
        {
          if ((this.DataContext as TweetListViewModel).ColumnBackVisible)
            VisualStateManager.GoToElementState((FrameworkElement) this.SearchViewSettings, "SearchColumnSettingsOpen", true);
          else
            VisualStateManager.GoToElementState((FrameworkElement) this.SearchViewSettings, "SearchColumnSettingsClosed", true);
        }
        else if ((this.DataContext as TweetListViewModel).ColumnBackVisible)
          VisualStateManager.GoToElementState((FrameworkElement) this.TweetListViewSettings, "ColumnSettingsOpen", true);
        else
          VisualStateManager.GoToElementState((FrameworkElement) this.TweetListViewSettings, "ColumnSettingsClosed", true);
      }
      if (e.PropertyName == "ShowContentVisibility")
        this.CheckandSetupBehaviours();
      if (!(e.PropertyName == "CurrentTweetID") || (this.unreadmarker == null || !(this.DataContext as TweetListViewModel).TitleBarVisible))
        return;
      this.unreadmarker.SetMarker();
    }

    private void RootCurrentView()
    {
      if (App.TemporarilyRootedTweetListViews.Contains(this))
        return;
      TweetListViewModel tweetListViewModel = this.DataContext as TweetListViewModel;
      TweetListType tweetType = tweetListViewModel.TweetType;
      int num;
      switch (tweetType)
      {
        case TweetListType.DirectMessages:
        case TweetListType.MentionsMyTweetsRetweeted:
          num = 1;
          break;
        default:
          num = tweetType == TweetListType.FriendsTimeline ? 1 : 0;
          break;
      }
      if (num == 0)
      {
        App.TemporarilyRootedTweetListViews.Add(this);
      }
      else
      {
        if (!App.PermanentSpecialTweetListViews.ContainsKey(tweetListViewModel.TwitterAccountID))
          App.PermanentSpecialTweetListViews.Add(tweetListViewModel.TwitterAccountID, new Dictionary<TweetListType, TweetListView>());
        if (!App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID].ContainsKey(tweetType))
          App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID].Add(tweetType, this);
        else
          App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID][tweetType] = this;
      }
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
      TweetListViewModel tweetListViewModel = this.DataContext as TweetListViewModel;
      if (!App.TemporarilyRootedTweetListViews.Contains(this) && App.PermanentSpecialTweetListViews.ContainsKey(tweetListViewModel.TwitterAccountID) && (!App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID].ContainsKey(TweetListType.MentionsMyTweetsRetweeted) && !App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID].ContainsKey(TweetListType.DirectMessages)) && !App.PermanentSpecialTweetListViews[tweetListViewModel.TwitterAccountID].ContainsKey(TweetListType.FriendsTimeline))
      {
        tweetListViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.TweetListView_PropertyChanged);
        if (!tweetListViewModel.IsTransitioningToPinned)
          tweetListViewModel.Cleanup();
        if (!(SettingsData.Instance.PopupTarget is Button))
          SettingsData.Instance.PopupTarget = (UIElement) null;
        SettingsData.Instance.SelectedColumn = (UIElement) null;
        SettingsData.Instance.DialogActiveControl = (FrameworkElement) null;
        this.RemoveBehaviours();
        Messenger.Default.Unregister<GenericMessage<object>>((object) this, (object) ViewModelMessages.ThemeChanged);
      }
      tweetListViewModel.IsTransitioningToPinned = false;
    }

    private void TweetDisplay_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.OriginalSource != null)
      {
        MetroTwitStatusBase metroTwitStatusBase = !(e.OriginalSource is FrameworkContentElement) ? (e.OriginalSource as FrameworkElement).DataContext as MetroTwitStatusBase : (e.OriginalSource as FrameworkContentElement).DataContext as MetroTwitStatusBase;
        ListBoxItem listBoxItem = (sender as ListBox).ItemContainerGenerator.ContainerFromItem((object) metroTwitStatusBase) as ListBoxItem;
        if (listBoxItem != null)
        {
          listBoxItem.IsSelected = true;
          try
          {
            if (metroTwitStatusBase.ID > (this.DataContext as TweetListViewModel).CurrentTweetID)
              (this.DataContext as TweetListViewModel).CurrentTweetID = metroTwitStatusBase.ID;
            if (this.unreadmarker != null && (this.DataContext as TweetListViewModel).TitleBarVisible)
              this.unreadmarker.SetMarker();
          }
          catch
          {
          }
        }
      }
      SettingsData.Instance.SelectedColumn = (UIElement) this;
      if (e.OriginalSource.GetType() == typeof (Button))
        SettingsData.Instance.DialogActiveControl = e.OriginalSource as FrameworkElement;
      else
        SettingsData.Instance.DialogActiveControl = (FrameworkElement) null;
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

    private void ColumnOptionsButton_Click_1(object sender, RoutedEventArgs e)
    {
      this.ColumnOptionsButtonMenu.PlacementTarget = (UIElement) this;
      this.ColumnOptionsButtonMenu.IsOpen = true;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.tweets.Items.Count <= 0)
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
    }

   
  }
}
