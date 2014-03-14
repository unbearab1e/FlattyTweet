// Type: MetroTwit.Behaviors.HorizontalMouseScroll
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MetroTwit.Behaviors
{
  internal class HorizontalMouseScroll : Behavior<ScrollViewer>
  {
    private const int TILT_HORIZ_FACTOR = 4;
    private const int WM_MOUSEWHEEL = 526;
    private ScrollViewer _scrollViewer;
    private HwndSource _source;

    protected override void OnAttached()
    {
      base.OnAttached();
      this._scrollViewer = this.AssociatedObject;
      this._scrollViewer.Loaded += new RoutedEventHandler(this.OnLoaded);
      this._scrollViewer.PreviewMouseWheel += new MouseWheelEventHandler(this._scrollViewer_MouseWheel);
    }

    private void _scrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (Keyboard.Modifiers != ModifierKeys.Shift)
        return;
      if (e.Delta > 0)
        this._scrollViewer.LineLeft();
      else
        this._scrollViewer.LineRight();
      e.Handled = true;
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
      if (this._scrollViewer.IsVisible)
        this.ChangeScrollViewerHook(true);
      this._scrollViewer.Loaded -= new RoutedEventHandler(this.OnLoaded);
      this._scrollViewer.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((o, a) => this.ChangeScrollViewerHook((bool) a.NewValue));
    }

    private void ChangeScrollViewerHook(bool add)
    {
      this._source = (HwndSource) PresentationSource.FromDependencyObject((DependencyObject) this._scrollViewer);
      if (add)
      {
        if (this._source == null)
          return;
        this._source.AddHook(new HwndSourceHook(this.WindowProc));
      }
      else if (this._source != null)
        this._source.RemoveHook(new HwndSourceHook(this.WindowProc));
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      if (this._source != null)
        this._source.RemoveHook(new HwndSourceHook(this.WindowProc));
      this._scrollViewer.PreviewMouseWheel -= new MouseWheelEventHandler(this._scrollViewer_MouseWheel);
    }

    private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      if (msg == 526)
      {
        this.Scroll(Int32Extensions.GetHighWord(IntPtrExtensions.ToLowInt32(wParam)));
        handled = true;
      }
      return IntPtr.Zero;
    }

    private void Scroll(int delta)
    {
      if (delta > 0)
      {
        for (int index = 0; index < 4; ++index)
          this._scrollViewer.LineRight();
      }
      else
      {
        if (delta >= 0)
          return;
        for (int index = 0; index < 4; ++index)
          this._scrollViewer.LineLeft();
      }
    }
  }
}
