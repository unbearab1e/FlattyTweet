
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Model;
using FlattyTweet.MVVM.Messages;
using FlattyTweet.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlattyTweet.Extensions
{
    public class URLAdorner : Adorner
    {
        private int URL_LENGTH_THRESHOLD = 15;
        private RichTextBox richTextBox;
        private string lastWord;
        private bool performURLParsing;
        private bool isEnabled;

        public Decimal TwitterAccountID { get; set; }

        public URLAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            if (!(adornedElement is RichTextBox))
                return;
            this.richTextBox = adornedElement as RichTextBox;
        }

        public void Enable()
        {
            if (this.isEnabled)
                return;
            this.isEnabled = true;
            this.richTextBox.PreviewKeyUp += new KeyEventHandler(this.adornedElement_PreviewKeyUp);
            DataObject.AddPastingHandler((DependencyObject)this.richTextBox, new DataObjectPastingEventHandler(this.DataObjectPastingHandler));
            DataObject.AddCopyingHandler((DependencyObject)this.richTextBox, new DataObjectCopyingEventHandler(this.DataObjectCopyingHandler));
        }

        public void Disable()
        {
            this.isEnabled = false;
            this.richTextBox.PreviewKeyUp -= new KeyEventHandler(this.adornedElement_PreviewKeyUp);
            DataObject.RemovePastingHandler((DependencyObject)this.richTextBox, new DataObjectPastingEventHandler(this.DataObjectPastingHandler));
            DataObject.RemoveCopyingHandler((DependencyObject)this.richTextBox, new DataObjectCopyingEventHandler(this.DataObjectCopyingHandler));
        }

        private void DataObjectPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            object data = e.DataObject.GetData(DataFormats.UnicodeText);
            if (data == null)
                return;
            string str1 = data.ToString();
            this.richTextBox.Selection.Text = string.Empty;
            string[] strArray = str1.Split(new char[1]
      {
        ' '
      });
            if (strArray != null && strArray.Length > 0)
            {
                for (int index = 0; index < strArray.Length; ++index)
                {
                    string str2 = strArray[index];
                    TextPointer positionAtOffset = this.richTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
                    if (positionAtOffset != null)
                        this.richTextBox.CaretPosition = positionAtOffset;
                    string text = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.CaretPosition).Text;
                    if (this.richTextBox.CaretPosition != null)
                    {
                        if (str2.Contains("http://") || str2.Contains("www.") || str2.Contains("https://"))
                        {
                            if (text.EndsWith(" "))
                                this.richTextBox.CaretPosition.InsertTextInRun(string.Format("{0} ", (object)str2));
                            else
                                this.richTextBox.CaretPosition.InsertTextInRun(string.Format(" {0} ", (object)str2));
                            this.performURLParsing = true;
                            this.adornedElement_PreviewKeyUp((object)this.richTextBox, (KeyEventArgs)null);
                        }
                        else if (index == strArray.Length - 1)
                            this.richTextBox.CaretPosition.InsertTextInRun(string.Format("{0}", (object)str2));
                        else
                            this.richTextBox.CaretPosition.InsertTextInRun(string.Format("{0} ", (object)str2));
                    }
                }
            }
            e.CancelCommand();
        }

        private void DataObjectCopyingHandler(object sender, DataObjectCopyingEventArgs e)
        {
            try
            {
                Clipboard.SetText(RTBExtensions.GetSelectedInlineText(this.richTextBox));
                if (SettingsData.Instance.CutIsTriggered)
                {
                    this.richTextBox.Selection.Text = string.Empty;
                    SettingsData.Instance.CutIsTriggered = false;
                }
                e.CancelCommand();
            }
            catch
            {
            }
        }

        private bool IsAlreadyShortened(string url)
        {
            return false;
        }

        private void adornedElement_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((this.richTextBox != null) && (((e != null) && (e.Key == Key.Space)) || ((e == null) && this.performURLParsing)))
            {
                bool isWWWPrefixed;
                this.performURLParsing = false;
                string text = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.Document.ContentEnd).Text;
                string str2 = new TextRange(this.richTextBox.Document.ContentStart, this.richTextBox.CaretPosition).Text;
                int length = str2.Length;
                if (((length - 1) <= text.Length) && (length != 0))
                {
                    int startIndex = -1;
                    if ((length - 2) >= 0)
                    {
                        startIndex = text.LastIndexOf("http://", (int)(length - 2));
                        startIndex = (startIndex == -1) ? text.LastIndexOf("https://", (int)(length - 2)) : startIndex;
                        startIndex = (startIndex == -1) ? (startIndex = text.LastIndexOf("www.", (int)(length - 2))) : startIndex;
                    }
                    if (startIndex == -1)
                    {
                        startIndex = 0;
                    }
                    if (str2.Substring(0, str2.Length - 1).EndsWith("\r\n"))
                    {
                        startIndex = length - 1;
                    }
                    this.lastWord = text.Substring(startIndex, length - startIndex);
                    isWWWPrefixed = false;
                    int num3 = this.lastWord.LastIndexOf("http://");
                    num3 = (num3 == -1) ? this.lastWord.LastIndexOf("https://") : num3;
                    if (num3 == -1)
                    {
                        num3 = this.lastWord.LastIndexOf("www.");
                        isWWWPrefixed = true;
                    }
                    if (num3 != -1)
                    {
                        string url;
                        ContextMenu buttonContextMenu;
                        Button button;
                        MenuItem expandContractMenuItem;
                        MenuItem gotoLinkMenuitem;
                        Func<string> shortenURLDelegate;
                        int index = this.lastWord.IndexOf(' ', num3);
                        if (index != -1)
                        {
                            url = this.lastWord.Substring(num3, index - num3);
                            int num5 = url.Length;
                            if (isWWWPrefixed)
                            {
                                num5 += "http://".Length;
                            }
                            if ((num5 >= this.URL_LENGTH_THRESHOLD) && !this.IsAlreadyShortened(url))
                            {
                                int x = text.IndexOf(url) - GetInlineUIContainerCountUpToPointer(this.richTextBox, this.richTextBox.CaretPosition);
                                TextPointer start = new TextRange(GetPoint(this.richTextBox.Document.ContentStart, x), this.richTextBox.Document.ContentEnd).Start;
                                start.DeleteTextInRun(url.Length);
                                buttonContextMenu = new ContextMenu();
                                button = new Button();
                                button.SetResourceReference(FrameworkElement.StyleProperty, "URLButton");
                                button.Content = "shortening...";
                                button.Click += delegate(object s, RoutedEventArgs e2)
                                {
                                    buttonContextMenu.PlacementTarget = button;
                                    buttonContextMenu.Placement = PlacementMode.Bottom;
                                    buttonContextMenu.IsOpen = true;
                                };
                                expandContractMenuItem = new MenuItem
                                {
                                    Header = "Expand link",
                                    IsEnabled = false
                                };
                                gotoLinkMenuitem = new MenuItem
                                {
                                    Header = "Open link in browser",
                                    IsEnabled = false
                                };
                                buttonContextMenu.Items.Add(gotoLinkMenuitem);
                                buttonContextMenu.Items.Add(expandContractMenuItem);
                                InlineUIContainer container = new InlineUIContainer(button, start);
                                shortenURLDelegate = delegate
                                {
                                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                                    if (CoreServices.Instance.CurrentURLShorteningService(this.TwitterAccountID) != null)
                                    {
                                        string key = CoreServices.Instance.CurrentURLShorteningService(this.TwitterAccountID).Name;
                                        if (!App.ShortenedURLCache.ContainsKey(key))
                                        {
                                            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                                            App.ShortenedURLCache.Add(key, dictionary2);
                                            dictionary = dictionary2;
                                        }
                                        else
                                        {
                                            dictionary = App.ShortenedURLCache[key];
                                        }
                                    }
                                    string text1 = url;
                                    if (isWWWPrefixed)
                                    {
                                        text1 = string.Format("http://{0}", text1);
                                    }
                                    string str3 = string.Empty;
                                    if (App.ShortenedURLCache.ContainsKey(text1))
                                    {
                                        return dictionary[text1];
                                    }
                                    str3 = this.ShortenURL(text1);
                                    dictionary[text1] = str3;
                                    return str3;
                                };
                                shortenURLDelegate.BeginInvoke(delegate(IAsyncResult result)
                                {

                                    string originalURL = url;
                                    if (isWWWPrefixed)
                                    {
                                        originalURL = string.Format("http://{0}", originalURL);
                                    }
                                    string returnedValue = shortenURLDelegate.EndInvoke(result);
                                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {

                                        Button wrappedButton = button;
                                        wrappedButton.Content = returnedValue;
                                        wrappedButton.ToolTip = originalURL.ToString();
                                        gotoLinkMenuitem.Click += (gs, ge) => Process.Start(returnedValue);
                                        expandContractMenuItem.Click += delegate(object sc, RoutedEventArgs ec)
                                        {
                                            if (expandContractMenuItem.Header.ToString() == "Expand link")
                                            {
                                                wrappedButton.Content = originalURL;
                                                expandContractMenuItem.Header = "Shorten link";
                                            }
                                            else
                                            {
                                                wrappedButton.Content = returnedValue;
                                                expandContractMenuItem.Header = "Expand link";
                                            }
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), CommonCommands.MultiAccountifyToken(ViewModelMessages.UpdateActualTweetText, this.TwitterAccountID));
                                        };
                                        expandContractMenuItem.IsEnabled = true;
                                        gotoLinkMenuitem.IsEnabled = true;
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), CommonCommands.MultiAccountifyToken(ViewModelMessages.UpdateActualTweetText, this.TwitterAccountID));
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), CommonCommands.MultiAccountifyToken(ViewModelMessages.DecrementPostTweetCounter, this.TwitterAccountID));
                                    }), DispatcherPriority.ContextIdle, new object[0]);
                                }, null);
                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), CommonCommands.MultiAccountifyToken(ViewModelMessages.IncrementPostTweetCounter, this.TwitterAccountID));
                            }
                        }
                    }
                }
            }
        }

        private string ShortenURL(string originalURL)
        {
            try
            {
                string str = string.Empty;
                if (CoreServices.Instance.CurrentURLShorteningService(this.TwitterAccountID) != null)
                {
                    str = CoreServices.Instance.CurrentURLShorteningService(this.TwitterAccountID).ShortenURL(this.TwitterAccountID, originalURL);
                    if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                        str = originalURL;
                }
                return str;
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    string local_0 = string.Format("We received the following error trying to shorten the url. {0}", (object)ex.Message);
                    SimpleErrorPrompt local_1 = new SimpleErrorPrompt()
                    {
                        DataContext = (object)new
                        {
                            ErrorHeading = "url shortening failed",
                            ErrorText = local_0
                        }
                    };
                    Messenger.Default.Send<PromptMessage>(new PromptMessage()
                    {
                        IsModal = false,
                        PromptView = (FrameworkElement)local_1,
                        IsCentered = false
                    }, (object)ViewModelMessages.ShowSlidePrompt);
                }), DispatcherPriority.ContextIdle, new object[0]);
                return originalURL;
            }
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

        private static int GetInlineUIContainerCountUpToPointer(RichTextBox myRichTextBox, TextPointer pointer)
        {
            int num = 0;
            foreach (Block block in (TextElementCollection<Block>)myRichTextBox.Document.Blocks)
            {
                if (block is Paragraph)
                {
                    foreach (Inline inline in (TextElementCollection<Inline>)((Paragraph)block).Inlines)
                    {
                        if (inline is InlineUIContainer && ((InlineUIContainer)inline).Child is Button)
                            ++num;
                        if (pointer.Parent is Inline && (Inline)pointer.Parent == inline)
                            break;
                    }
                }
            }
            return num;
        }
    }
}
