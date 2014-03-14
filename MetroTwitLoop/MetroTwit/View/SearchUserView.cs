// Type: MetroTwit.View.SearchUserView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace MetroTwit.View
{
    public partial class SearchUserView : UserControl, IComponentConnector
  {
    private bool ignoreTextChange;
   

    public SearchUserView()
    {
      this.InitializeComponent();
      DataObject.AddPastingHandler((DependencyObject) this.InputEdit, new DataObjectPastingEventHandler(this.OnPaste));
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.InputEdit.Focus();
    }

    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
      if (!(sender is RichTextBox))
        return;
      string textData = e.DataObject.GetData(DataFormats.Text) as string;
      (sender as RichTextBox).Document.ContentEnd.InsertTextInRun(textData);
      (sender as RichTextBox).CaretPosition = (sender as RichTextBox).Document.ContentEnd;
      e.CancelCommand();
    }

    private void InputEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!this.ignoreTextChange)
      {
        this.ignoreTextChange = true;
        TextPointer positionAtOffset = this.InputEdit.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
        if (positionAtOffset != null)
          this.InputEdit.CaretPosition = positionAtOffset;
        TextRange textRange = new TextRange(this.InputEdit.Document.ContentStart, this.InputEdit.Document.ContentEnd);
        textRange.Text = this.FilterInput(textRange.Text);
        (this.DataContext as SearchUserViewModel).SearchQuery = textRange.Text;
      }
      else
        this.ignoreTextChange = false;
    }

    private string FilterInput(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char c in input)
      {
        if (char.IsLetterOrDigit(c) || (int) c == 95 || (int) c == 10)
          stringBuilder.Append(c);
      }
      return ((object) stringBuilder).ToString();
    }

 
  }
}
