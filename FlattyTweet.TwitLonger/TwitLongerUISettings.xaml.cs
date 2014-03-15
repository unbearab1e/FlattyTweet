
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FlattyTweet.TwitLonger
{
  public partial class TwitLongerUISettings : UserControl, IComponentConnector
  {

    public TwitLongerUISettings(TwitLongerSettings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) settings;
    }

    
  }
}
