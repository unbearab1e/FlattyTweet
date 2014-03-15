
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class LoginDialogBehavior : Behavior<FrameworkElement>
  {
    private static LoginView loginView;

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<DialogMessage>((object) this, (object) DialogType.LoginView, new Action<DialogMessage>(this.ShowDialog));
    }

    private void ShowDialog(DialogMessage dm)
    {
      if (this.AssociatedObject != null && this.AssociatedObject.GetType() == typeof (MainWindow))
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
      if (LoginDialogBehavior.loginView == null)
      {
        LoginDialogBehavior.loginView = new LoginView();
        LoginDialogBehavior.loginView.Owner = Application.Current.MainWindow;
      }
      LoginDialogBehavior.loginView.Show();
      if (this.AssociatedObject == null || !(this.AssociatedObject.GetType() == typeof (MainWindow)))
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
    }
  }
}
