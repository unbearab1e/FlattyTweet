﻿// Type: MetroTwit.View.DebugView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace MetroTwit.View
{
    public partial class DebugView : InlinePopup, IComponentConnector
  {

    public DebugView()
    {
      this.InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      WebBrowserView webBrowserView = new WebBrowserView((FrameworkElement) this.ControlHost, "", 380.0, 380.0);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
    
  }
}
