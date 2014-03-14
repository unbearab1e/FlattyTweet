// Type: MetroTwit.Behaviors.LoginDialogBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.View;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace MetroTwit.Behaviors
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
