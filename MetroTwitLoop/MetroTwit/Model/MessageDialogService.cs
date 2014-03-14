// Type: MetroTwit.Model.MessageDialogService
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensibility;
using MetroTwit.Extensions;
using MetroTwit.View;
using System;
using System.Windows;

namespace MetroTwit.Model
{
  public class MessageDialogService : IMessageDialogService
  {
    public MessageBoxResult InvokeShow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
    {
      MessageBoxResult retval = MessageBoxResult.None;
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
        retval = MessageBoxView.Show(messageBoxText, caption, button, defaultResult);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
      }));
      return retval;
    }

    public MessageBoxResult InvokeShow(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
    {
      MessageBoxResult retval = MessageBoxResult.None;
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
        retval = MessageBoxView.Show(owner, messageBoxText, caption, button, defaultResult);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
      }));
      return retval;
    }

    public void InvokeShowNonModal(string messageBoxText, string caption)
    {
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
        MessageBoxView.ShowNonModal(messageBoxText, caption);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
      }));
    }
  }
}
