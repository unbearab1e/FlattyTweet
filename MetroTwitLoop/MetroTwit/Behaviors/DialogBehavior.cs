﻿// Type: MetroTwit.Behaviors.DialogBehavior
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