
using System;
using System.Windows.Threading;

namespace FlattyTweet.Extensions
{
  public static class DispatcherExtensions
  {
    public static void InvokeIfRequired(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
    {
      if (!dispatcher.CheckAccess())
        dispatcher.Invoke(priority, (Delegate) action);
      else
        action();
    }

    public static void InvokeIfRequired(this Dispatcher dispatcher, Action action)
    {
      if (!dispatcher.CheckAccess())
        dispatcher.Invoke(DispatcherPriority.Normal, (Delegate) action);
      else
        action();
    }
  }
}
