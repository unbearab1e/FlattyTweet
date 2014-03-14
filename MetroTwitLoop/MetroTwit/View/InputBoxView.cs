// Type: MetroTwit.View.InputBoxView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.MVVM.Messages;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Twitterizer.Models;

namespace MetroTwit.View
{
    public partial class InputBoxView : StackPanel, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof (string), typeof (InputBoxView), (PropertyMetadata) new UIPropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty MessageBoxButtonProperty = DependencyProperty.Register("MessageBoxButton", typeof (MessageBoxButton), typeof (InputBoxView), (PropertyMetadata) new UIPropertyMetadata((object) MessageBoxButton.OK));
    private bool isClearingProperties;
    private bool initialFocusComplete;

    public string Title { get; set; }

    public Action<string, MessageBoxResult> MessageResultCallback { get; set; }

    public string Message
    {
      get
      {
        return (string) this.GetValue(InputBoxView.MessageProperty);
      }
      set
      {
        this.SetValue(InputBoxView.MessageProperty, (object) value);
      }
    }

    public MessageBoxButton MessageBoxButton
    {
      get
      {
        return (MessageBoxButton) this.GetValue(InputBoxView.MessageBoxButtonProperty);
      }
      set
      {
        this.SetValue(InputBoxView.MessageBoxButtonProperty, (object) value);
        switch (value)
        {
          case MessageBoxButton.OK:
            this._ok.Visibility = Visibility.Visible;
            this._ok.IsCancel = true;
            break;
          case MessageBoxButton.OKCancel:
            this._ok.Visibility = Visibility.Visible;
            this._cancel.Visibility = Visibility.Visible;
            break;
          case MessageBoxButton.YesNoCancel:
            this._yes.Visibility = Visibility.Visible;
            this._no.Visibility = Visibility.Visible;
            this._cancel.Visibility = Visibility.Visible;
            break;
          case MessageBoxButton.YesNo:
            this._yes.Visibility = Visibility.Visible;
            this._no.Visibility = Visibility.Visible;
            break;
        }
      }
    }

    static InputBoxView()
    {
    }

    public InputBoxView()
    {
      this.InitializeComponent();
      this.DataContext = (object) new InputBoxViewModel();
      this.GotFocus += new RoutedEventHandler(this.InputBoxView_GotFocus);
    }

    private void InputBoxView_GotFocus(object sender, RoutedEventArgs e)
    {
      if (this.initialFocusComplete)
        return;
      Keyboard.ClearFocus();
      this.InputEdit.Focus();
      this.initialFocusComplete = true;
    }

    public static void Input(Action<string, MessageBoxResult> messageResultCallback, string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult, InputBoxType type, string initialText = "", IEnumerable<TwitterSavedSearch> SavedSearches = null)
    {
      InputBoxView inputBoxView = new InputBoxView();
      (inputBoxView.DataContext as InputBoxViewModel).Title = caption;
      switch (defaultResult)
      {
        case MessageBoxResult.OK:
          inputBoxView._ok.IsDefault = true;
          break;
        case MessageBoxResult.Cancel:
          inputBoxView._cancel.IsDefault = true;
          break;
        case MessageBoxResult.Yes:
          inputBoxView._yes.IsDefault = true;
          break;
        case MessageBoxResult.No:
          inputBoxView._no.IsDefault = true;
          break;
      }
      (inputBoxView.DataContext as InputBoxViewModel).Message = messageBoxText;
      inputBoxView.MessageResultCallback = messageResultCallback;
      inputBoxView.MessageBoxButton = button;
      inputBoxView.InputEdit.Document.Blocks.Clear();
      if (!string.IsNullOrEmpty(initialText))
        inputBoxView.InputEdit.Document.Blocks.Add((Block) new Paragraph((Inline) new Span((Inline) new Run(initialText))));
      inputBoxView.InputEdit.Visibility = Visibility.Visible;
      inputBoxView.InputEdit.SelectAll();
      switch (type)
      {
        case InputBoxType.UserProfile:
          IntellisenseExtension.SetIsEnabled(inputBoxView.InputEdit, SettingsData.Instance.UseAutoComplete);
          IntellisenseExtension.SetIgnorePrefix(inputBoxView.InputEdit, true);
          break;
        case InputBoxType.SearchTwitter:
          IntellisenseExtension.SetIsEnabled(inputBoxView.InputEdit, SettingsData.Instance.UseAutoComplete);
          IntellisenseExtension.SetExcludeMentions(inputBoxView.InputEdit, true);
          break;
      }
      Messenger.Default.Send<PromptMessage>(new PromptMessage()
      {
        IsModal = true,
        IsCentered = true,
        PromptView = (FrameworkElement) inputBoxView
      }, (object) ViewModelMessages.ShowSlidePrompt);
    }

    private void cancel_Click(object sender, RoutedEventArgs e)
    {
      if (this.MessageResultCallback == null)
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.HideSlidePrompt);
      this.MessageResultCallback((string) null, MessageBoxResult.Cancel);
    }

    private void no_Click(object sender, RoutedEventArgs e)
    {
      if (this.MessageResultCallback == null)
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.HideSlidePrompt);
      this.MessageResultCallback((string) null, MessageBoxResult.No);
    }

    private void ok_Click(object sender, RoutedEventArgs e)
    {
      if (this.MessageResultCallback == null)
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.HideSlidePrompt);
      this.MessageResultCallback(this.GetText(), MessageBoxResult.OK);
    }

    private void yes_Click(object sender, RoutedEventArgs e)
    {
      if (this.MessageResultCallback == null)
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.HideSlidePrompt);
      this.MessageResultCallback(this.GetText(), MessageBoxResult.Yes);
    }

    private void this_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private string GetText()
    {
      return new TextRange(this.InputEdit.Document.ContentStart, this.InputEdit.Document.ContentEnd).Text.Trim();
    }

    private void InputEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.isClearingProperties)
        return;
      TextRange textRange = new TextRange(this.InputEdit.Document.ContentStart, this.InputEdit.Document.ContentEnd);
      this.isClearingProperties = true;
      textRange.ClearAllProperties();
      this.isClearingProperties = false;
    }

   
  }
}
