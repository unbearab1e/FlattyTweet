
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.View
{
    public partial class ProgressPromptView : Window, IComponentConnector
  {

    public ProgressPromptView()
    {
      this.InitializeComponent();
    }

    private void ok_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

   
  }
}
