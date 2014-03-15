
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class RetweetMessage : DialogMessage
  {
    public int RetweetAccount { get; set; }

    public Action<MessageBoxResult, Decimal> Callback { get; set; }

    public RetweetMessage(Action<MessageBoxResult, Decimal> callback)
      : base(string.Empty, (Action<MessageBoxResult>) null)
    {
      this.Callback = callback;
    }
  }
}
