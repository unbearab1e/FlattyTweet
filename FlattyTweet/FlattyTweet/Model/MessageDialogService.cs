
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Windows;

namespace FlattyTweet.Model
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
