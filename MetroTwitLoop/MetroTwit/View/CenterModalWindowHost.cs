// Type: MetroTwit.View.CenterModalWindowHost
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using MetroTwit.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;

namespace MetroTwit.View
{
    public partial class CenterModalWindowHost : Window, IComponentConnector
  {
    private double dpiXfactor;
    private double dpiYfactor;

    public CenterModalWindowHost()
    {
      this.InitializeComponent();
      if (this.Owner != null)
        this.Owner.SizeChanged += new SizeChangedEventHandler(this.Owner_SizeChanged);
      this.SetWindowHeightAndPosition();
      this.Unloaded += new RoutedEventHandler(this.CenterModalWindowHost_Unloaded);
    }

    private void InitializeDPI()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      this.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      this.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    private void SetWindowHeightAndPosition()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      this.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      this.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
      Screen screen = CommonCommands.CurrentScreen();
      Rectangle workingArea;
      double num1;
      if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = screen.WorkingArea;
        num1 = (double) workingArea.Y / this.dpiYfactor;
      }
      else
        num1 = System.Windows.Application.Current.MainWindow.Top;
      double num2 = num1;
      double num3;
      if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = screen.WorkingArea;
        num3 = (double) workingArea.X / this.dpiXfactor;
      }
      else
        num3 = System.Windows.Application.Current.MainWindow.Left;
      double num4 = num3;
      double num5;
      if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = screen.WorkingArea;
        num5 = (double) workingArea.Width / this.dpiXfactor;
      }
      else
        num5 = System.Windows.Application.Current.MainWindow.Width;
      double num6 = num5;
      double num7;
      if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = screen.WorkingArea;
        num7 = (double) workingArea.Height / this.dpiYfactor;
      }
      else
        num7 = System.Windows.Application.Current.MainWindow.Height;
      this.Height = num7 - (double) App.AppState.StatusEntryFieldHeight;
      this.Top = num2;
      int num8 = 0;
      this.Left = num4 + (double) num8 + ((num6 - (double) num8) / 2.0 - this.Width / 2.0);
    }

    private void CenterModalWindowHost_Unloaded(object sender, RoutedEventArgs e)
    {
      this.Owner.SizeChanged -= new SizeChangedEventHandler(this.Owner_SizeChanged);
    }

    private void Owner_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.SetWindowHeightAndPosition();
    }

    
  }
}
