
using GalaSoft.MvvmLight.Messaging;
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
    public partial class SimpleErrorPrompt : Grid, IComponentConnector
  {

    public SimpleErrorPrompt()
    {
      this.InitializeComponent();
    }

    private void ok_Click(object sender, RoutedEventArgs e)
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.HideSlidePrompt);
    }

   
  }
}
