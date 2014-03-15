
using System;
using System.Threading;

namespace FlattyTweet.Extensions
{
  public static class SingleInstance
  {
    public static readonly int WM_SHOWFIRSTINSTANCE = WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", new object[1]
    {
      (object) ProgramInfo.AssemblyGuid
    });
    private static Mutex mutex;

    static SingleInstance()
    {
    }

    public static bool Start()
    {
      bool createdNew = false;
      SingleInstance.mutex = new Mutex(true, string.Format("Local\\FlattyTweet.{0}", (object) ProgramInfo.AssemblyGuid), out createdNew);
      return createdNew;
    }

    public static void ShowFirstInstance()
    {
      WinApi.PostMessage((IntPtr) ((int) ushort.MaxValue), SingleInstance.WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
    }

    public static void Stop()
    {
      try
      {
        SingleInstance.mutex.ReleaseMutex();
      }
      catch
      {
      }
    }
  }
}
