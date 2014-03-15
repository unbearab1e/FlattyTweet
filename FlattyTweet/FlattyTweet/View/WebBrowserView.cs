
using FlattyTweet;
using FlattyTweet.HostView;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

namespace FlattyTweet.View
{
    public partial class WebBrowserView : Window, IComponentConnector
  {
    private FrameworkElement _placementTarget;

    public WebBrowserView(FrameworkElement placementTarget, string URL, double width, double height)
    {
      WebBrowserView webBrowserView = this;
      this.InitializeComponent();
      this._placementTarget = placementTarget;
      Window owner = Window.GetWindow((DependencyObject) placementTarget);
      if (App.AddIns == null || App.AddIns.Count == 1 && App.AddIns[0] == null)
        App.LoadAddins();
      if (App.AddIns != null && App.AddIns.Count == 1 && App.AddIns[0] != null)
        this.LayoutRoot.Children.Add((UIElement) App.AddIns[0].GetUI(width, height));
      if (owner != null)
      {
        owner.SizeChanged += (SizeChangedEventHandler) ((param0, param1) => this.OnSizeLocationChanged());
        owner.LocationChanged += (EventHandler) ((param0, param1) => this.OnSizeLocationChanged());
        owner.Closing += new CancelEventHandler(this.owner_Closing);
        owner.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.owner_IsVisibleChanged);
      }
      this._placementTarget.SizeChanged += (SizeChangedEventHandler) ((param0, param1) => this.OnSizeLocationChanged());
      if (owner == null)
        return;
      if (owner.IsVisible)
      {
        this.Owner = owner;
        this.Show();
        if (App.AddIns != null && App.AddIns.Count == 1 && App.AddIns[0] != null)
          App.AddIns[0].Navigate(URL);
        this.OnSizeLocationChanged();
      }
      else
        owner.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((param0, param1) =>
        {
          if (!owner.IsVisible)
            return;
          webBrowserView.Owner = owner;
          webBrowserView.Show();
          if (App.AddIns != null && App.AddIns.Count == 1 && App.AddIns[0] != null)
            App.AddIns[0].Navigate(URL);
          webBrowserView.OnSizeLocationChanged();
        });
    }

    private void owner_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
    }

    private void owner_Closing(object sender, CancelEventArgs e)
    {
      Window window = Window.GetWindow((DependencyObject) this._placementTarget);
      window.SizeChanged -= (SizeChangedEventHandler) ((param0, param1) => this.OnSizeLocationChanged());
      window.LocationChanged -= (EventHandler) ((param0, param1) => this.OnSizeLocationChanged());
      window.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.owner_IsVisibleChanged);
      this.Close();
      this.LayoutRoot.Children.Clear();
      if (App.AddIns == null || App.AddIns.Count != 1 || App.AddIns[0] == null)
        return;
      try
      {
        App.AddIns[0].ClearBrowser();
      }
      finally
      {
        App.AddIns[0] = (CustomAddIn) null;
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      if (this.IsVisible)
        return;
      this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.WebBrowserView_IsVisibleChanged);
      e.Cancel = true;
    }

    private void WebBrowserView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!this.IsVisible)
        return;
      this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.WebBrowserView_IsVisibleChanged);
      this.Close();
    }

    private void OnSizeLocationChanged()
    {
      Point point1 = this._placementTarget.TranslatePoint(new Point(), (UIElement) this.Owner);
      Point point2 = new Point(this._placementTarget.ActualWidth, this._placementTarget.ActualHeight);
      HwndSource hwndSource1 = (HwndSource) PresentationSource.FromVisual((Visual) this.Owner);
      CompositionTarget compositionTarget = (CompositionTarget) hwndSource1.CompositionTarget;
      Point pt1 = compositionTarget.TransformToDevice.Transform(point1);
      Point pt2 = compositionTarget.TransformToDevice.Transform(point2);
      WebBrowserView.Win32.POINT lpPoint = new WebBrowserView.Win32.POINT(pt1);
      WebBrowserView.Win32.ClientToScreen(hwndSource1.Handle, ref lpPoint);
      WebBrowserView.Win32.POINT point3 = new WebBrowserView.Win32.POINT(pt2);
      HwndSource hwndSource2 = (HwndSource) PresentationSource.FromVisual((Visual) this);
      if (hwndSource2 == null)
        return;
      WebBrowserView.Win32.MoveWindow(hwndSource2.Handle, lpPoint.X, lpPoint.Y, point3.X, point3.Y, true);
    }

    

    private static class Win32
    {
      [DllImport("user32.dll")]
      internal static extern bool ClientToScreen(IntPtr hWnd, ref WebBrowserView.Win32.POINT lpPoint);

      [DllImport("user32.dll")]
      internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

      public struct POINT
      {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
          this.X = x;
          this.Y = y;
        }

        public POINT(Point pt)
        {
          this.X = Convert.ToInt32(pt.X);
          this.Y = Convert.ToInt32(pt.Y);
        }
      }
    }
  }
}
