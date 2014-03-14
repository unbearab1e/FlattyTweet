// Type: MetroTwit.Extensions.RTBExtensions
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MetroTwit.Extensions
{
  public static class RTBExtensions
  {
    public static string GetInlineText(RichTextBox myRichTextBox)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Block block in (TextElementCollection<Block>) myRichTextBox.Document.Blocks)
      {
        if (block is Paragraph)
        {
          foreach (Inline inline in (TextElementCollection<Inline>) ((Paragraph) block).Inlines)
          {
            if (inline is InlineUIContainer)
            {
              InlineUIContainer inlineUiContainer = (InlineUIContainer) inline;
              if (inlineUiContainer.Child is Button)
                stringBuilder.Append(((ContentControl) inlineUiContainer.Child).Content);
            }
            else if (inline is LineBreak)
              stringBuilder.Append(Environment.NewLine);
            else if (inline is Run)
            {
              Run run = (Run) inline;
              stringBuilder.Append(run.Text);
            }
          }
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static string GetSelectedInlineText(RichTextBox myRichTextBox)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Block block in (TextElementCollection<Block>) myRichTextBox.Document.Blocks)
      {
        if (block is Paragraph)
        {
          foreach (Inline inline in (TextElementCollection<Inline>) ((Paragraph) block).Inlines)
          {
            if (inline is InlineUIContainer)
            {
              InlineUIContainer inlineUiContainer = (InlineUIContainer) inline;
              if (inlineUiContainer.Child is Button && ((TextRange) myRichTextBox.Selection).Contains(inlineUiContainer.ElementEnd) || ((TextRange) myRichTextBox.Selection).Contains(inlineUiContainer.ElementStart))
                stringBuilder.Append(((ContentControl) inlineUiContainer.Child).Content.ToString());
            }
            else if (inline is Run)
            {
              Run run = (Run) inline;
              for (TextPointer textPointer = run.ContentStart; textPointer.CompareTo(run.ContentEnd) < 0; textPointer = textPointer.GetPositionAtOffset(1, LogicalDirection.Forward))
              {
                if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && ((TextRange) myRichTextBox.Selection).Contains(textPointer) && ((TextRange) myRichTextBox.Selection).Contains(textPointer.GetPositionAtOffset(1, LogicalDirection.Forward)))
                  stringBuilder.Append(new TextRange(textPointer, textPointer.GetPositionAtOffset(1, LogicalDirection.Forward)).Text);
              }
            }
          }
        }
      }
      return ((object) stringBuilder).ToString();
    }

    public static void SelectText(RichTextBox myRichTextBox, string input)
    {
      TextPointer textPointer = (TextPointer) null;
      string str = input;
      if (string.IsNullOrEmpty(str))
        return;
      for (TextPointer position1 = myRichTextBox.Document.ContentStart; position1 != null && position1.CompareTo(myRichTextBox.Document.ContentEnd) < 0; position1 = position1.GetNextInsertionPosition(LogicalDirection.Forward) ?? myRichTextBox.Document.ContentStart)
      {
        TextPointer position2 = position1;
        for (int index = 0; position2 != null && index < str.Length; ++index)
          position2 = position2.GetNextInsertionPosition(LogicalDirection.Forward);
        if (position2 != null)
        {
          TextRange textRange = new TextRange(position1, position2);
          if (textRange.Text == str)
          {
            myRichTextBox.Focus();
            myRichTextBox.Selection.Select(textRange.Start, textRange.End);
            textPointer = position2.GetNextInsertionPosition(LogicalDirection.Forward);
            break;
          }
        }
      }
    }
  }
}
