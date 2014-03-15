
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class RetweetDialogBehavior : Behavior<FrameworkElement>
  {
    private RetweetMessage dialogmessage;
    private RetweetQuestionView messageBox;

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<RetweetMessage>((object) this, (object) DialogType.RetweetDialog, new Action<RetweetMessage>(this.ShowDialog));
    }

    private void ShowDialog(RetweetMessage dm)
    {
      this.dialogmessage = dm;
      this.messageBox = new RetweetQuestionView();
      this.messageBox.Closing += new CancelEventHandler(this.messageBox_Closing);
      this.messageBox.ShowAnimated(PlacementMode.Bottom, SettingsData.Instance.DialogActiveControl, new Point?());
    }

    private void messageBox_Closing(object sender, CancelEventArgs e)
    {
      this.messageBox.Closing -= new CancelEventHandler(this.messageBox_Closing);
      this.dialogmessage.Callback(this.messageBox.MessageBoxResult, this.messageBox.Account);
    }
  }
}
