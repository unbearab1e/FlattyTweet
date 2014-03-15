
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace FlattyTweet.View
{
    public partial class TrendsView : UserControl 
  {


    public TrendsView()
    {
      this.InitializeComponent();
    }

    private void trendItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) (sender as TextBlock).Text), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.Search, (this.DataContext as TrendsViewModel).TwitterAccountID));
    }

  
  }
}
