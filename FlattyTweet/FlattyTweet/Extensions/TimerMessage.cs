
using GalaSoft.MvvmLight.Messaging;

namespace FlattyTweet.Extensions
{
  internal class TimerMessage : GenericMessage<object>
  {
    internal TimerMessage()
      : base((object) null)
    {
    }
  }
}
