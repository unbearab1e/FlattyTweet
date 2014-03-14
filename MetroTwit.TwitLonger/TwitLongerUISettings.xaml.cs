// Type: MetroTwit.TwitLonger.TwitLongerUISettings
// Assembly: MetroTwit.TwitLonger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1724F65-D568-4C76-942B-059AE183E337
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.TwitLonger.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MetroTwit.TwitLonger
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
