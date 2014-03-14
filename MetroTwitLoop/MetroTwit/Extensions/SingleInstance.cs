// Type: MetroTwit.Extensions.SingleInstance
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Threading;

namespace MetroTwit.Extensions
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
      SingleInstance.mutex = new Mutex(true, string.Format("Local\\MetroTwit.{0}", (object) ProgramInfo.AssemblyGuid), out createdNew);
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
