
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class UndoTweetDialogBehavior : Behavior<FrameworkElement>
  {
    private ProgressPromptView progressPromptView;

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) DialogType.UndoTweet, new Action<GenericMessage<object>>(this.ShowDialog));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.UndoTweetClose, (Action<GenericMessage<object>>) (o => this.progressPromptView.Close()));
    }

    private void ShowDialog(GenericMessage<object> dm)
    {
      if (this.AssociatedObject != null && this.AssociatedObject.GetType() == typeof (MainWindow))
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
      UndoTweetDialogBehavior tweetDialogBehavior = this;
      ProgressPromptView progressPromptView1 = new ProgressPromptView();
      progressPromptView1.DataContext = (object) (dm.Content as ProgressPromptViewModel);
      progressPromptView1.Owner = Application.Current.MainWindow;
      ProgressPromptView progressPromptView2 = progressPromptView1;
      tweetDialogBehavior.progressPromptView = progressPromptView2;
      this.progressPromptView.Show();
      if (this.AssociatedObject == null || !(this.AssociatedObject.GetType() == typeof (MainWindow)))
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
    }
  }
}
