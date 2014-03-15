
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
    public partial class SearchExpressionBuilderView : Window, IComponentConnector
  {

    public SearchExpressionBuilderView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.SearchBuilderClose, (Action<GenericMessage<object>>) (o => this.Close()));
    }

  }
}
