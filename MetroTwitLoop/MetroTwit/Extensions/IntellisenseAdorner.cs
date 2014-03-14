// Type: MetroTwit.Extensions.IntellisenseAdorner
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

namespace MetroTwit.Extensions
{
  public class IntellisenseAdorner : Adorner
  {
    private static readonly Key[] INSERTION_CHARACTERS_SUPPORTED = new Key[23]
    {
      Key.Space,
      Key.D1,
      Key.D2,
      Key.D3,
      Key.D4,
      Key.D5,
      Key.D6,
      Key.D7,
      Key.D8,
      Key.D9,
      Key.D0,
      Key.OemPlus,
      Key.Oem3,
      Key.Subtract,
      Key.Oem7,
      Key.Oem1,
      Key.OemComma,
      Key.OemPeriod,
      Key.Oem2,
      Key.Oem102,
      Key.Oem4,
      Key.Oem6,
      Key.Oem5
    };
    private static readonly Key[] ACCENT_CHARACTERS_SUPPORTED = new Key[8]
    {
      Key.Oem1,
      Key.Oem2,
      Key.Oem3,
      Key.Oem4,
      Key.Oem5,
      Key.Oem6,
      Key.Oem7,
      Key.Oem8
    };
    public static readonly string IGNORE_CHARACTERS = "\\(\\)\\,\\.\\?\\<\\>\\!\\#\\$\\%\\^\\&\\*\\-\\+\\=\\~\\`\\'\\|\\{\\}\\[\\]\\/\\\\\"";
    public static readonly Regex TAG_EXPRESSION = new Regex("#(\\w+)([^" + IntellisenseAdorner.IGNORE_CHARACTERS + "\\w]+|$)");
    public static readonly Regex MENTION_EXPRESSION = new Regex("@(\\w+)([^" + IntellisenseAdorner.IGNORE_CHARACTERS + "\\w]+|$)");
    public static readonly Regex DM_EXPRESSION = new Regex("^[Dd] (\\w+)");
    public static readonly Regex DM_EXPRESSION_WITH_END_SPACE = new Regex("^[Dd] (\\w+) ");
    private bool ignoreDisplay = false;
    private bool isEnabled = false;
    private Popup intellisensePopup;
    private ListBox intellisenseListBox;
    private RichTextBox richTextBox;

    public bool ExcludeMentions { get; set; }

    public bool IgnorePrefix { get; set; }

    public bool IsActive { get; set; }

    public Decimal TwitterAccountID { get; set; }

    static IntellisenseAdorner()
    {
    }

    public IntellisenseAdorner(RichTextBox richTextBox)
      : base((UIElement) richTextBox)
    {
      this.Unloaded += new RoutedEventHandler(this.IntellisenseAdorner_Unloaded);
      this.richTextBox = richTextBox;
      IntellisenseAdorner intellisenseAdorner1 = this;
      ListBox listBox1 = new ListBox();
      listBox1.Focusable = false;
      ListBox listBox2 = listBox1;
      intellisenseAdorner1.intellisenseListBox = listBox2;
      this.intellisenseListBox.SetResourceReference(FrameworkElement.StyleProperty, (object) "IntellisenseList");
      this.Enable();
      IntellisenseAdorner intellisenseAdorner2 = this;
      Popup popup1 = new Popup();
      popup1.AllowsTransparency = true;
      popup1.Focusable = false;
      popup1.IsOpen = false;
      popup1.Placement = PlacementMode.Top;
      popup1.PlacementTarget = (UIElement) richTextBox;
      popup1.StaysOpen = false;
      popup1.HorizontalAlignment = HorizontalAlignment.Left;
      popup1.Child = (UIElement) this.intellisenseListBox;
      Popup popup2 = popup1;
      intellisenseAdorner2.intellisensePopup = popup2;
    }

    private void IntellisenseAdorner_Unloaded(object sender, RoutedEventArgs e)
    {
      this.Disable();
    }

    public void Disable()
    {
      if (!this.isEnabled)
        return;
      this.isEnabled = false;
      this.intellisenseListBox.SelectionChanged -= new SelectionChangedEventHandler(this.intellisenseListBox_SelectionChanged);
      this.intellisenseListBox.MouseUp -= new MouseButtonEventHandler(this.intellisenseListBox_MouseUp);
      this.intellisenseListBox.MouseDoubleClick -= new MouseButtonEventHandler(this.intellisenseListBox_MouseDoubleClick);
      this.richTextBox.PreviewKeyDown -= new KeyEventHandler(this.richTextBox_PreviewKeyDown);
      this.richTextBox.PreviewKeyUp -= new KeyEventHandler(this.richTextBox_PreviewKeyUp);
      this.richTextBox.PreviewMouseUp -= new MouseButtonEventHandler(this.richTextBox_PreviewMouseUp);
    }

    public void Enable()
    {
      if (this.isEnabled)
        return;
      this.isEnabled = true;
      this.richTextBox.PreviewKeyDown += new KeyEventHandler(this.richTextBox_PreviewKeyDown);
      this.richTextBox.PreviewKeyUp += new KeyEventHandler(this.richTextBox_PreviewKeyUp);
      this.richTextBox.PreviewMouseUp += new MouseButtonEventHandler(this.richTextBox_PreviewMouseUp);
      this.intellisenseListBox.SelectionChanged += new SelectionChangedEventHandler(this.intellisenseListBox_SelectionChanged);
      this.intellisenseListBox.MouseUp += new MouseButtonEventHandler(this.intellisenseListBox_MouseUp);
      this.intellisenseListBox.MouseDoubleClick += new MouseButtonEventHandler(this.intellisenseListBox_MouseDoubleClick);
    }

    private static TextPointer GetPoint(TextPointer start, int x)
    {
      TextPointer textPointer = start;
      for (int index = 0; index < x && textPointer != null; textPointer = textPointer.GetPositionAtOffset(1, LogicalDirection.Forward))
      {
        if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && textPointer.Parent is Run)
          ++index;
        if (textPointer.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
          return textPointer;
      }
      return textPointer;
    }

    private void ShowMethodsPopup(Rect placementRect, RichTextBox target)
    {
      this.IsActive = true;
      if (this.intellisensePopup.IsOpen)
        return;
      this.intellisensePopup.PlacementTarget = (UIElement) target;
      this.intellisensePopup.PlacementRectangle = placementRect;
      this.intellisensePopup.IsOpen = true;
    }

    private void HideMethodsPopup()
    {
      this.intellisensePopup.IsOpen = false;
      this.IsActive = false;
    }

    private void intellisenseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.intellisensePopup.PlacementTarget.Focus();
    }

    private void intellisenseListBox_MouseUp(object sender, MouseButtonEventArgs e)
    {
      this.intellisensePopup.PlacementTarget.Focus();
    }

    private void intellisenseListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (!(e.Source is ListBox) || this.intellisenseListBox.SelectedItem == null)
        return;
      if (this.intellisenseListBox.ItemsSource is List<IntellisenseItem>)
      {
        List<IntellisenseItem> list = this.intellisenseListBox.ItemsSource as List<IntellisenseItem>;
        if (list.Count == 1 && list[0].FilterValue == "N/A")
          return;
      }
      this.InsertSelectedEntry(char.MinValue);
      this.HideMethodsPopup();
    }

    private void richTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (this.intellisenseListBox.ItemsSource is List<IntellisenseItem>)
        {
            List<IntellisenseItem> itemsSource = this.intellisenseListBox.ItemsSource as List<IntellisenseItem>;
            if ((itemsSource.Count == 1) && (itemsSource[0].FilterValue == "N/A"))
            {
                return;
            }
        }
        if ((e.Key == Key.Escape) && this.intellisensePopup.IsOpen)
        {
            this.HideMethodsPopup();
            e.Handled = true;
        }
        else if (!ACCENT_CHARACTERS_SUPPORTED.Contains<Key>(e.Key))
        {
            if (!((!this.IgnorePrefix || (e.Key != Key.Space)) || this.intellisensePopup.IsOpen))
            {
                e.Handled = true;
            }
            else if (this.intellisensePopup.IsOpen)
            {
                if (((e.Key == Key.Up) || (e.Key == Key.Down)) && (this.intellisenseListBox.SelectedIndex == -1))
                {
                    this.intellisenseListBox.SelectedIndex = 0;
                    this.intellisenseListBox.ScrollIntoView(this.intellisenseListBox.SelectedItem);
                }
                if (e.Key == Key.Up)
                {
                    if (this.intellisenseListBox.SelectedIndex > 0)
                    {
                        this.intellisenseListBox.SelectedIndex--;
                        this.intellisenseListBox.ScrollIntoView(this.intellisenseListBox.SelectedItem);
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    if (this.intellisenseListBox.SelectedIndex != (this.intellisenseListBox.Items.Count - 1))
                    {
                        this.intellisenseListBox.SelectedIndex++;
                        this.intellisenseListBox.ScrollIntoView(this.intellisenseListBox.SelectedItem);
                    }
                    e.Handled = true;
                }
                else
                {
                    char charFromKey = e.Key.GetCharFromKey();
                    if ((!char.IsLetterOrDigit(charFromKey) && (charFromKey != '_')) && (((e.Key == Key.Return) || (e.Key == Key.Tab)) || INSERTION_CHARACTERS_SUPPORTED.Contains<Key>(e.Key)))
                    {
                        if (this.intellisenseListBox.SelectedIndex != -1)
                        {
                            if ((e.Key == Key.Return) || (e.Key == Key.Tab))
                            {
                                charFromKey = '\0';
                            }
                            this.InsertSelectedEntry(charFromKey);
                            e.Handled = true;
                            this.HideMethodsPopup();
                        }
                        else
                        {
                            if ((e.Key == Key.Return) || (e.Key == Key.Tab))
                            {
                                charFromKey = '\t';
                            }
                            this.HideMethodsPopup();
                            e.Handled = false;
                        }
                    }
                }
            }
        }
    }

    private void InsertSelectedEntry(char characterInsertion)
    {
      if (this.intellisenseListBox.SelectedIndex <= -1 || !this.intellisensePopup.IsOpen)
        return;
      string text = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.CaretPosition).Text;
      int startIndex = text.LastIndexOf(' ');
      string currentWord = startIndex != -1 ? text.Substring(startIndex).TrimStart(new char[0]) : text.TrimStart(new char[0]);
      IntellisenseItem intellisenseItem = this.intellisenseListBox.SelectedItem as IntellisenseItem;
      bool flag = text.StartsWith("d ", StringComparison.InvariantCultureIgnoreCase) && !IntellisenseAdorner.DM_EXPRESSION_WITH_END_SPACE.IsMatch(text);
      string textData = this.IgnorePrefix || flag ? intellisenseItem.FilterValue : intellisenseItem.DisplayValue;
      if (textData.StartsWith("@") || textData.StartsWith("#"))
      {
        int charactersToDelete = IntellisenseAdorner.GetNumberOfCharactersToDelete(currentWord, textData.StartsWith("@") ? "@" : "#");
        if (currentWord.EndsWith(" "))
          this.richTextBox.CaretPosition.DeleteTextInRun(-(charactersToDelete - 1));
        else
          this.richTextBox.CaretPosition.DeleteTextInRun(-charactersToDelete);
      }
      else if (currentWord.EndsWith(" "))
        this.richTextBox.CaretPosition.DeleteTextInRun(-(currentWord.Length - 1));
      else
        this.richTextBox.CaretPosition.DeleteTextInRun(-currentWord.Length);
      if ((int) characterInsertion != 0 && !this.IgnorePrefix)
        textData = textData + (object) characterInsertion;
      this.richTextBox.CaretPosition = this.richTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward) ?? this.richTextBox.CaretPosition;
      if (!flag)
      {
        this.richTextBox.CaretPosition.InsertTextInRun(textData);
      }
      else
      {
        this.richTextBox.CaretPosition.InsertTextInRun(textData);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) intellisenseItem.FilterValue), (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.DirectMessage, intellisenseItem.TwitterAccountID));
      }
      this.ignoreDisplay = true;
    }

    private static int GetNumberOfCharactersToDelete(string currentWord, string prefix)
    {
      int startIndex = currentWord.IndexOf(prefix);
      if (startIndex > -1)
        return currentWord.Length - currentWord.Remove(startIndex).Length;
      else
        return currentWord.Length;
    }

    private void richTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
    {
        if (((e.Key != Key.Up) && (e.Key != Key.Down)) && ((e.Key != Key.Space) || !this.IgnorePrefix))
        {
            if ((e.Key == Key.Space) || (e.Key == Key.Escape))
            {
                this.HideMethodsPopup();
            }
            if (((((e.Key == Key.Escape) || (e.Key == Key.Left)) || ((e.Key == Key.Right) || (e.Key == Key.Return))) || ((this.IgnorePrefix && (e.Key == Key.D2)) && (Keyboard.Modifiers == ModifierKeys.Shift))) || this.ignoreDisplay)
            {
                this.HideMethodsPopup();
                this.ignoreDisplay = false;
            }
            else
            {
                IEnumerable<IntellisenseItem> results = null;
                string text = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.CaretPosition).Text;
                int startIndex = text.LastIndexOf(' ');
                string input = (startIndex != -1) ? text.Substring(startIndex).TrimStart(new char[0]) : text.TrimStart(new char[0]);
                if (((input == "@") || (input == "#")) && this.IgnorePrefix)
                {
                    this.HideMethodsPopup();
                    this.ignoreDisplay = false;
                }
                else if (!(this.ExcludeMentions || (((input != "@") && !input.EndsWith("@")) && !(text == "d "))))
                {
                    results = IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.GetMatches("@");
                    Rect rect = this.richTextBox.CaretPosition.GetCharacterRect(LogicalDirection.Forward);
                    this.BindResults(results, this.intellisenseListBox, rect, IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.ContainsEntries, delegate
                    {
                        this.ShowMethodsPopup(rect, this.richTextBox);
                        this.intellisenseListBox.SelectedIndex = -1;
                    });
                }
                else if ((input == "#") || input.EndsWith("#"))
                {
                    results = IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.GetMatches("#");
                    Rect rect = GetPoint(this.richTextBox.Document.ContentStart, startIndex).GetCharacterRect(LogicalDirection.Forward);
                    this.BindResults(results, this.intellisenseListBox, rect, IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.ContainsEntries, delegate
                    {
                        this.ShowMethodsPopup(rect, this.richTextBox);
                        this.intellisenseListBox.SelectedIndex = -1;
                    });
                }
                else if (!(this.ExcludeMentions || (!MENTION_EXPRESSION.IsMatch(input) && !this.IgnorePrefix)))
                {
                    string currentWordExcludingPrefix = string.Empty;
                    currentWordExcludingPrefix = this.IgnorePrefix ? input : MENTION_EXPRESSION.Match(input).Groups[1].Value;
                    results = IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.GetMatches(currentWordExcludingPrefix);
                    Rect rect = GetPoint(this.richTextBox.Document.ContentStart, startIndex).GetCharacterRect(LogicalDirection.Forward);
                    this.BindResults(results, this.intellisenseListBox, rect, IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.ContainsEntries, delegate
                    {
                        this.ShowMethodsPopup(rect, this.richTextBox);
                        this.SetSelection(currentWordExcludingPrefix);
                    });
                }
                else if (!((this.ExcludeMentions || !DM_EXPRESSION.IsMatch(text)) || DM_EXPRESSION_WITH_END_SPACE.IsMatch(text)))
                {
                    string currentWordExcludingPrefix = DM_EXPRESSION.Match(text).Groups[1].Value;
                    results = IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.GetMatches(currentWordExcludingPrefix);
                    Rect rect = GetPoint(this.richTextBox.Document.ContentStart, startIndex).GetCharacterRect(LogicalDirection.Forward);
                    this.BindResults(results, this.intellisenseListBox, rect, IntellisenseDataSource.Instance(this.TwitterAccountID).MentionsCollection.ContainsEntries, delegate
                    {
                        this.ShowMethodsPopup(rect, this.richTextBox);
                        this.SetSelection(currentWordExcludingPrefix);
                    });
                }
                else if (TAG_EXPRESSION.IsMatch(input))
                {
                    string partialWord = TAG_EXPRESSION.Match(input).Groups[1].Value;
                    results = IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.GetMatches(partialWord);
                    Rect rect = GetPoint(this.richTextBox.Document.ContentStart, startIndex).GetCharacterRect(LogicalDirection.Forward);
                    this.BindResults(results, this.intellisenseListBox, rect, IntellisenseDataSource.Instance(this.TwitterAccountID).TagsCollection.ContainsEntries, delegate
                    {
                        this.ShowMethodsPopup(rect, this.richTextBox);
                        this.intellisenseListBox.SelectedIndex = -1;
                    });
                }
                else
                {
                    this.HideMethodsPopup();
                }
            }
        }
    }

    private void SetSelection(string lastWord)
    {
      IEnumerable<IntellisenseItem> source = this.intellisenseListBox.ItemsSource as IEnumerable<IntellisenseItem>;
      int count = this.intellisenseListBox.Items.Count;
      IntellisenseItem intellisenseItem1 = (IntellisenseItem) null;
      if (count > 200)
      {
        if (count <= 0)
          return;
        this.intellisenseListBox.SelectedIndex = 0;
        this.intellisenseListBox.ScrollIntoView((object) Enumerable.FirstOrDefault<IntellisenseItem>(source));
      }
      else
      {
        IOrderedEnumerable<IntellisenseItem> orderedEnumerable = Enumerable.OrderBy<IntellisenseItem, string>(Enumerable.Where<IntellisenseItem>(source, (Func<IntellisenseItem, bool>) (i => i.FilterValue.StartsWith(lastWord, StringComparison.Ordinal))), (Func<IntellisenseItem, string>) (i => i.FilterValue));
        if (orderedEnumerable != null && Enumerable.Count<IntellisenseItem>((IEnumerable<IntellisenseItem>) orderedEnumerable) > 0)
          source = (IEnumerable<IntellisenseItem>) orderedEnumerable;
        int? nullable1 = new int?();
        foreach (IntellisenseItem intellisenseItem2 in source)
        {
          int num1 = intellisenseItem2.FilterValue.Length - lastWord.Length;
          if (!nullable1.HasValue)
          {
            nullable1 = new int?(num1);
            intellisenseItem1 = intellisenseItem2;
          }
          else
          {
            int num2 = num1;
            int? nullable2 = nullable1;
            if ((num2 >= nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
            {
              nullable1 = new int?(num1);
              intellisenseItem1 = intellisenseItem2;
            }
          }
        }
        this.intellisenseListBox.SelectedItem = (object) intellisenseItem1;
        this.intellisenseListBox.ScrollIntoView((object) intellisenseItem1);
      }
    }

    private void BindResults(IEnumerable<IntellisenseItem> results, ListBox boundingTarget, Rect rect, bool containsEntries, Action showResultsAction)
    {
      if (results == null || Enumerable.Count<IntellisenseItem>(results) == 0)
      {
        if (containsEntries)
        {
          boundingTarget.ItemsSource = (IEnumerable) null;
          this.HideMethodsPopup();
        }
        else
        {
          boundingTarget.ItemsSource = (IEnumerable) new List<IntellisenseItem>()
          {
            new IntellisenseItem()
            {
              DisplayValue = "Populating autocomplete...",
              FilterValue = "N/A"
            }
          };
          this.ShowMethodsPopup(rect, this.richTextBox);
        }
      }
      else
      {
        boundingTarget.ItemsSource = (IEnumerable) Enumerable.OrderBy<IntellisenseItem, string>(results, (Func<IntellisenseItem, string>) (x => x.DisplayValue));
        showResultsAction();
      }
    }

    private void richTextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      this.intellisensePopup.IsOpen = false;
    }
  }
}
