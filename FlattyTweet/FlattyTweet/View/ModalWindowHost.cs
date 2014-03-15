
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.View
{
    public partial class ModalWindowHost : Window, IComponentConnector
  {

    public ModalWindowHost()
    {
      this.InitializeComponent();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.Owner == null)
        return;
      this.Width = this.Owner.Width;
    }

    
  }
}
