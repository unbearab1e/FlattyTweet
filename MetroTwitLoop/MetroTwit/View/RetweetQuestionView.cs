// Type: MetroTwit.View.RetweetQuestionView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MetroTwit.View
{
    public partial class RetweetQuestionView : InlinePopup, IComponentConnector
  {
    private MessageBoxResult _result = MessageBoxResult.None;
    

    public MessageBoxResult MessageBoxResult
    {
      get
      {
        return this._result;
      }
      private set
      {
        this._result = value;
      }
    }

    public Decimal Account
    {
      get
      {
        return this.AccountCombo.SelectedItem != null ? (this.AccountCombo.SelectedItem as ComboAccountItem).TwitterAccountID : App.AppState.CurrentActiveAccount.TwitterAccountID;
      }
    }

    public bool AccountEnabled
    {
      get
      {
        return App.AppState.Accounts.Count > 1;
      }
    }

    public IEnumerable<ComboAccountItem> Accounts
    {
      get
      {
        return Enumerable.Select<UserAccountViewModel, ComboAccountItem>((IEnumerable<UserAccountViewModel>) App.AppState.Accounts, (Func<UserAccountViewModel, ComboAccountItem>) (a => new ComboAccountItem()
        {
          TwitterAccountID = a.TwitterAccountID,
          TwitterUserImage = a.Settings.TwitterUserImage,
          TwitterAccountName = a.TwitterAccountName
        }));
      }
    }

    public RetweetQuestionView()
    {
      this.InitializeComponent();
      this.AccountCombo.SelectedIndex = App.AppState.CurrentActiveAccount.Settings.Index;
      this._yes.Focus();
    }

    private void no_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.No;
      this.Close();
    }

    private void yes_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.Yes;
      this.Close();
    }

   
  }
}
