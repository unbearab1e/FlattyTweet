
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class NonModalDialogBehavior : Behavior<FrameworkElement>
  {
    public DialogType DialogType { get; set; }

    public string Caption { get; set; }

    public string Text { get; set; }

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<DialogMessage>((object) this, (object) this.DialogType, new Action<DialogMessage>(this.ShowDialog));
    }

    private void ShowDialog(DialogMessage dm)
    {
      MessageBoxView.ShowNonModal(this.Text, this.Caption);
    }
  }
}
