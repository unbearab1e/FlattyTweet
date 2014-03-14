// Type: MetroTwit.Behaviors.UndoTweetDialogBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace MetroTwit.Behaviors
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
