
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
    public partial class LoginView : Window, IComponentConnector
  {

    public LoginView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.CloseLogin, (Action<GenericMessage<object>>) (o => this.Close()));
    }

    public static void Show(Window owner)
    {
      LoginView loginView = new LoginView();
      loginView.Owner = owner;
      loginView.Show();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
    }

    
  }
}
