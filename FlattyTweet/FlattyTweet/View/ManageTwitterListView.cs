
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.View
{
    public partial class ManageTwitterListView : UserControl, IComponentConnector
  {
    

    public ManageTwitterListView()
    {
      this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.listNameTextBox.Focus();
    }

    
  }
}
