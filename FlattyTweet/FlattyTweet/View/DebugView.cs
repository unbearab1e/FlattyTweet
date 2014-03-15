
using FlattyTweet.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace FlattyTweet.View
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
