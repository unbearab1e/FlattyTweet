
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class UpdateDialogBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<DialogMessage>((object) this, (object) DialogType.UpdateStart, new Action<DialogMessage>(this.ShowDialog));
    }

    private void ShowDialog(DialogMessage dm)
    {
      UpdateView.DoUpdate();
    }
  }
}
