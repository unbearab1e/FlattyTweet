
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class KeyboardShortcutMessage : GenericMessage<KeyboardShortcutType>
  {
    public KeyboardShortcutMessage(FrameworkElement Sender, KeyboardShortcutType Type)
      : base(Type)
    {
      this.Sender = (object) Sender;
    }
  }
}
