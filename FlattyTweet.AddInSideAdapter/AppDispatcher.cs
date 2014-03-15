
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FlattyTweet.AddInSideAdapter
{
  internal class AppDispatcher
  {
    private string eventName = "appStarted:";
    private Application app;
    private EventWaitHandle appStarted;
    private Thread myThread;
    private static AppDispatcher _dispatcher;

    public static AppDispatcher Current
    {
      get
      {
        if (AppDispatcher._dispatcher == null)
          AppDispatcher._dispatcher = new AppDispatcher();
        return AppDispatcher._dispatcher;
      }
    }

    private AppDispatcher()
    {
      this.appStarted = new EventWaitHandle(false, EventResetMode.ManualReset, this.eventName);
      Thread thread = new Thread((ThreadStart) (() =>
      {
        if (Application.Current == null)
        {
          this.app = new Application();
          this.myThread = Thread.CurrentThread;
          this.appStarted.Set();
          this.appStarted.Close();
          this.app.Run();
        }
        else
        {
          this.app = Application.Current;
          this.appStarted.Set();
          this.appStarted.Close();
        }
      }));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      this.appStarted.WaitOne();
    }

    public void DoWork(Worker d)
    {
      if (!Thread.CurrentThread.Equals((object) this.myThread))
        this.app.Dispatcher.Invoke(DispatcherPriority.Normal, (Delegate) d);
      else
        d();
    }
  }
}
