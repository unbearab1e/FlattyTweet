using Bugsense.WPF;
using System;

namespace FlattyTweet.Extensions
{
  public static class AsyncErrorHandler
  {
    public static void HandleException(Exception exception)
    {
      BugSense.SendException(exception);
    }
  }
}
