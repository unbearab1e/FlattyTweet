
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class DialogBehavior : Behavior<FrameworkElement>
  {
    public DialogType DialogType { get; set; }

    public string Caption { get; set; }

    public string Text { get; set; }

    public MessageBoxButton Buttons { get; set; }

    public MessageBoxResult DefaultResult { get; set; }

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<DialogMessage>((object) this, (object) this.DialogType, new Action<DialogMessage>(this.ShowDialog));
    }

    private void ShowDialog(DialogMessage dm)
    {
      if (this.AssociatedObject != null && this.AssociatedObject.GetType() == typeof (MainWindow))
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
      MessageBoxResult messageBoxResult = MessageBoxView.Show(!(this.AssociatedObject.GetType() != typeof (Window)) ? (Window) this.AssociatedObject : (Window) CommonCommands.FindParent((object) this.AssociatedObject, typeof (Window)) ?? Application.Current.MainWindow, string.Format(this.Text, dm.Content == null ? (object) "" : (object) ((object) dm.Content).ToString()), string.Format(this.Caption, dm.Content == null ? (object) "" : (object) dm.Content.ToLower()), this.Buttons, this.DefaultResult);
      if (this.AssociatedObject != null && this.AssociatedObject.GetType() == typeof (MainWindow))
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
      if (dm.Callback == null)
        return;
      dm.Callback(messageBoxResult);
    }
  }
}
