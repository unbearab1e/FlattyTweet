﻿// Type: MetroTwit.View.SimpleErrorPrompt
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MetroTwit.View
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
