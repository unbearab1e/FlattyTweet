
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.ViewModel;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class InlineMessageBehavior : Behavior<FrameworkElement>
  {
    public DialogType DialogType { get; set; }

    public string Text { get; set; }

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<DialogMessage>((object) this, (object) this.DialogType, new Action<DialogMessage>(this.ShowDialog));
    }

    private void ShowDialog(DialogMessage dm)
    {
      if (this.AssociatedObject == null || !(this.AssociatedObject.GetType() == typeof (MainWindow)))
        return;
      (this.AssociatedObject.DataContext as MainViewModel).ErrorMessage = new MetroTwitErrorViewModel()
      {
        Text = this.Text
      };
    }
  }
}
