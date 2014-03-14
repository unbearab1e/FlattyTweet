// Type: MetroTwit.View.NotificationControlView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetroTwit.View
{
    public partial class NotificationControlView : UserControl, IComponentConnector
  {
    private Storyboard ProgressAnimation;
    private Storyboard notificationOutAnimation;
    private DoubleAnimationUsingKeyFrames opacityAnimation;
   

    public NotificationControlView()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.NotificationControlView_Loaded);
    }

    private void NotificationControlView_Loaded(object sender, RoutedEventArgs e)
    {
      (this.DataContext as NotificationControlViewModel).PropertyChanged += new PropertyChangedEventHandler(this.NotificationControlView_PropertyChanged);
      NotificationControlView notificationControlView = this;
      DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
      animationUsingKeyFrames1.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(0.0));
      DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = animationUsingKeyFrames1;
      notificationControlView.opacityAnimation = animationUsingKeyFrames2;
      this.opacityAnimation.KeyFrames.Add((DoubleKeyFrame) new EasingDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0.0))));
      this.opacityAnimation.KeyFrames.Add((DoubleKeyFrame) new EasingDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200.0))));
      this.opacityAnimation.Completed += new EventHandler(this.opacityAnimationLoaded_Completed);
      this.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) this.opacityAnimation);
      this.ProgressAnimation = (Application.Current.FindResource((object) "NotificationProgress") as Storyboard).Clone();
      if (this.ProgressAnimation == null)
        return;
      (Enumerable.First<Timeline>((IEnumerable<Timeline>) this.ProgressAnimation.Children) as DoubleAnimation).Duration = (Duration) TimeSpan.FromSeconds(SettingsData.Instance.NotificationDisplayTime);
      this.ProgressAnimation.Completed += new EventHandler(this.ProgressAnimation_Completed);
      this.ProgressAnimation.Freeze();
      this.ProgressAnimation.Begin((FrameworkElement) this, HandoffBehavior.Compose, true);
    }

    private void ProgressAnimation_Completed(object sender, EventArgs e)
    {
      if (this.DataContext != null && (this.DataContext as NotificationControlViewModel).CloseNotification != null)
        (this.DataContext as NotificationControlViewModel).CloseNotification.Execute((object) null);
      if (this.ProgressAnimation.IsFrozen)
        return;
      this.ProgressAnimation.Completed -= new EventHandler(this.ProgressAnimation_Completed);
    }

    private void NotificationControlView_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Closing")
      {
        this.ProgressAnimation.Stop((FrameworkElement) this);
        NotificationControlView notificationControlView = this;
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
        animationUsingKeyFrames1.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(0.0));
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = animationUsingKeyFrames1;
        notificationControlView.opacityAnimation = animationUsingKeyFrames2;
        this.opacityAnimation.KeyFrames.Add((DoubleKeyFrame) new EasingDoubleKeyFrame(1.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0.0))));
        this.opacityAnimation.KeyFrames.Add((DoubleKeyFrame) new EasingDoubleKeyFrame(0.0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200.0))));
        this.opacityAnimation.Completed += new EventHandler(this.opacityAnimationUnloaded_Completed);
        this.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) this.opacityAnimation);
        (this.DataContext as NotificationControlViewModel).PropertyChanged -= new PropertyChangedEventHandler(this.NotificationControlView_PropertyChanged);
      }
      if (e.PropertyName == "CurrentTweetIndex")
      {
        try
        {
          this.notificationOutAnimation = (Application.Current.FindResource((object) "NotificationSlideOut") as Storyboard).Clone();
          this.notificationOutAnimation.Completed += new EventHandler(this.opacityAnimation_Completed);
          this.NotificationTweet.BeginStoryboard(this.notificationOutAnimation);
        }
        catch
        {
        }
      }
      if (!(e.PropertyName == "CurrentTweet"))
        return;
      try
      {
        this.NotificationTweet.BeginStoryboard((Application.Current.FindResource((object) "NotificationSlideIn") as Storyboard).Clone());
      }
      catch
      {
      }
    }

    private void opacityAnimation_Completed(object sender, EventArgs e)
    {
      if (this.DataContext != null && this.DataContext.GetType() == typeof (NotificationControlViewModel))
        (this.DataContext as NotificationControlViewModel).UpdateTweet();
      this.notificationOutAnimation.Completed -= new EventHandler(this.opacityAnimation_Completed);
    }

    private void opacityAnimationLoaded_Completed(object sender, EventArgs e)
    {
      if (this.DataContext != null && this.DataContext.GetType() == typeof (NotificationControlViewModel))
        (this.DataContext as NotificationControlViewModel).tweetTimerStart();
      this.opacityAnimation.Completed -= new EventHandler(this.opacityAnimation_Completed);
    }

    private void opacityAnimationUnloaded_Completed(object sender, EventArgs e)
    {
      this.opacityAnimation.Completed -= new EventHandler(this.opacityAnimationUnloaded_Completed);
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(this.DataContext), (object) ViewModelMessages.CloseNotification);
    }

    private void UserControl_MouseEnter(object sender, MouseEventArgs e)
    {
      this.ProgressAnimation.Pause((FrameworkElement) this);
    }

    private void UserControl_MouseLeave(object sender, MouseEventArgs e)
    {
      this.ProgressAnimation.Resume((FrameworkElement) this);
    }

    private void userControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.RightButton != MouseButtonState.Pressed)
        return;
      this.MouseLeave -= new MouseEventHandler(this.UserControl_MouseLeave);
      this.MouseEnter -= new MouseEventHandler(this.UserControl_MouseEnter);
      this.ProgressAnimation.Stop((FrameworkElement) this);
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(this.DataContext), (object) ViewModelMessages.CloseNotification);
    }

    private void userControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      Decimal id = (this.DataContext as NotificationControlViewModel).CurrentTweet.TwitterAccountID;
      Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<Tuple<UserAccountViewModel, bool>>>(new GenericMessage<Tuple<UserAccountViewModel, bool>>(new Tuple<UserAccountViewModel, bool>(App.AppState.Accounts[id], true)), (object) ViewModelMessages.SetActiveAccount);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.MainWindowShow);
      }), new object[0]);
    }

   
  }
}
