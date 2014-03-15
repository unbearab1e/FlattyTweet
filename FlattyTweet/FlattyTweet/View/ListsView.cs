
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
    public partial class ListsView : UserControl 
  {

    public ListsView()
    {
      this.InitializeComponent();
      this.Unloaded += new RoutedEventHandler(this.ListsView_Unloaded);
    }

    private void ListsView_Unloaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = (object) null;
    }

    private void heading_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) (sender as TextBlock).Text), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.ShowList, (this.DataContext as ListsViewModel).TwitterAccountID));
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(e.OriginalSource.GetType() != typeof (TabControl)))
        return;
      e.Handled = true;
    }

   
  }
}
