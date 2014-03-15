
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.View
{
    public partial class ErrorView : Border, IComponentConnector
  {

    public ErrorView(string error)
    {
      this.InitializeComponent();
      this.errorMessage.Text = error;
    }

    private void ok_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.Exit);
      }
      catch
      {
      }
      Application.Current.Shutdown(-1);
    }

    private void restart_Click(object sender, RoutedEventArgs e)
    {
      this.ok_Click(sender, e);
      App.RestartApplication();
    }

   
  }
}
