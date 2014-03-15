
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Timers;

namespace FlattyTweet.Extensions
{
  internal static class OneTimer
  {
    private static Dictionary<TimeSpan, List<object>> Timers = new Dictionary<TimeSpan, List<object>>();
    private static Dictionary<TimeSpan, TimeSpan> CurrentTimeSpans = new Dictionary<TimeSpan, TimeSpan>();
    private static object timerLock = new object();
    private static Timer oneTimer;

    static OneTimer()
    {
    }

    internal static void RegisterTimer(TimeSpan Interval, object CallbackToken)
    {
      lock (OneTimer.timerLock)
      {
        if (OneTimer.Timers.ContainsKey(Interval))
        {
          OneTimer.Timers[Interval].Add(CallbackToken);
        }
        else
        {
          OneTimer.Timers.Add(Interval, new List<object>()
          {
            CallbackToken
          });
          OneTimer.CurrentTimeSpans.Add(Interval, TimeSpan.FromMinutes(0.0));
        }
        if (OneTimer.oneTimer != null)
          return;
        OneTimer.oneTimer = new Timer(TimeSpan.FromMinutes(1.0).TotalMilliseconds);
        OneTimer.oneTimer.Elapsed += new ElapsedEventHandler(OneTimer.oneTimer_Elapsed);
        OneTimer.oneTimer.AutoReset = true;
        OneTimer.oneTimer.Start();
      }
    }

    internal static void UnregisterTimer(object CallbackToken)
    {
      lock (OneTimer.timerLock)
      {
        TimeSpan? local_0 = OneTimer.FindInterval(CallbackToken);
        if (local_0.HasValue && OneTimer.Timers.ContainsKey(local_0.Value))
        {
          OneTimer.Timers[local_0.Value].Remove(CallbackToken);
          if (OneTimer.Timers[local_0.Value].Count == 0)
          {
            OneTimer.Timers.Remove(local_0.Value);
            OneTimer.CurrentTimeSpans.Remove(local_0.Value);
          }
          if (OneTimer.Timers.Count == 0)
          {
            OneTimer.oneTimer.Stop();
            OneTimer.oneTimer = (Timer) null;
          }
        }
      }
    }

    internal static bool ContainsTimer(TimeSpan Interval, object CallbackToken)
    {
      if (OneTimer.Timers.ContainsKey(Interval))
        return OneTimer.Timers[Interval].Contains(CallbackToken);
      else
        return false;
    }

    internal static bool AlterTimer(object OldCallbackToken, TimeSpan NewInterval, object NewCallbackToken, bool FireIfOverdue = false)
    {
      lock (OneTimer.timerLock)
      {
        TimeSpan? local_0 = OneTimer.FindInterval(OldCallbackToken);
        if (!local_0.HasValue || !OneTimer.Timers.ContainsKey(local_0.Value) || !OneTimer.Timers[local_0.Value].Contains(OldCallbackToken))
          return false;
        TimeSpan local_1 = TimeSpan.Zero;
        if (!OneTimer.CurrentTimeSpans.ContainsKey(NewInterval))
          local_1 = OneTimer.CurrentTimeSpans[local_0.Value];
        OneTimer.UnregisterTimer(OldCallbackToken);
        OneTimer.RegisterTimer(NewInterval, NewCallbackToken);
        if (local_1 != TimeSpan.Zero)
          OneTimer.CurrentTimeSpans[NewInterval] = local_1;
        if (FireIfOverdue && NewInterval <= local_1)
        {
          Messenger.Default.Send<TimerMessage>(new TimerMessage(), NewCallbackToken);
          OneTimer.CurrentTimeSpans[NewInterval] = TimeSpan.Zero;
        }
        return true;
      }
    }

    private static void oneTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      foreach (KeyValuePair<TimeSpan, TimeSpan> keyValuePair in Enumerable.ToArray<KeyValuePair<TimeSpan, TimeSpan>>((IEnumerable<KeyValuePair<TimeSpan, TimeSpan>>) OneTimer.CurrentTimeSpans))
        OneTimer.CurrentTimeSpans[keyValuePair.Key] = keyValuePair.Value.Add(TimeSpan.FromMinutes(1.0));
      foreach (KeyValuePair<TimeSpan, TimeSpan> keyValuePair in Enumerable.Where<KeyValuePair<TimeSpan, TimeSpan>>((IEnumerable<KeyValuePair<TimeSpan, TimeSpan>>) Enumerable.ToArray<KeyValuePair<TimeSpan, TimeSpan>>((IEnumerable<KeyValuePair<TimeSpan, TimeSpan>>) OneTimer.CurrentTimeSpans), (Func<KeyValuePair<TimeSpan, TimeSpan>, int, bool>) ((x, y) => x.Value >= x.Key)))
      {
        if (OneTimer.Timers.ContainsKey(keyValuePair.Key))
        {
          foreach (object token in OneTimer.Timers[keyValuePair.Key].ToArray())
            Messenger.Default.Send<TimerMessage>(new TimerMessage(), token);
          OneTimer.CurrentTimeSpans[keyValuePair.Key] = TimeSpan.Zero;
        }
      }
    }

    private static TimeSpan? FindInterval(object CallbackToken)
    {
      foreach (KeyValuePair<TimeSpan, List<object>> keyValuePair in OneTimer.Timers)
      {
        if (keyValuePair.Value.Contains(CallbackToken))
          return new TimeSpan?(keyValuePair.Key);
      }
      return new TimeSpan?();
    }

    [DllImport("user32.dll")]
    private static extern bool GetLastInputInfo(ref OneTimer.LASTINPUTINFO plii);

    internal static TimeSpan LastInputTime()
    {
      int num1 = 0;
      OneTimer.LASTINPUTINFO plii = new OneTimer.LASTINPUTINFO();
      plii.cbSize = (uint) Marshal.SizeOf((object) plii);
      plii.dwTime = 0U;
      int tickCount = Environment.TickCount;
      if (OneTimer.GetLastInputInfo(ref plii))
      {
        int num2 = (int) plii.dwTime;
        num1 = tickCount - num2;
      }
      return TimeSpan.FromTicks((long) num1);
    }

    private struct LASTINPUTINFO
    {
      public static readonly int SizeOf = Marshal.SizeOf(typeof (OneTimer.LASTINPUTINFO));
      [MarshalAs(UnmanagedType.U4)]
      public uint cbSize;
      [MarshalAs(UnmanagedType.U4)]
      public uint dwTime;

      static LASTINPUTINFO()
      {
      }
    }
  }
}
