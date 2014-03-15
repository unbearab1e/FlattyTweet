
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.View
{
  public partial class SearchCriteriaView : UserControl, IComponentConnector
  {


    public SearchCriteriaView()
    {
      this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.InputEdit.Focus();
    }

    private void InputEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
    }
  }
}
