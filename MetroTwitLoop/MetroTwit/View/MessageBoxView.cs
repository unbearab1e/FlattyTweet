// Type: MetroTwit.View.MessageBoxView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetroTwit.View
{
    public partial class MessageBoxView : Window, IComponentConnector
  {
    public static readonly DependencyProperty DefaultResultProperty = DependencyProperty.Register("DefaultResult", typeof (MessageBoxResult), typeof (MessageBoxView), (PropertyMetadata) new UIPropertyMetadata((object) MessageBoxResult.None));
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof (MetroTwitStatusBase), typeof (MessageBoxView), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty MessageBoxButtonProperty = DependencyProperty.Register("MessageBoxButton", typeof (MessageBoxButton), typeof (MessageBoxView), (PropertyMetadata) new UIPropertyMetadata((object) MessageBoxButton.OK));
    public static readonly DependencyProperty MessageBoxImageProperty = DependencyProperty.Register("MessageBoxImage", typeof (MessageBoxImage), typeof (MessageBoxView), (PropertyMetadata) new UIPropertyMetadata((object) MessageBoxImage.None));
    private MessageBoxResult _result = MessageBoxResult.None;
    private Button _close;
   

    public string Caption
    {
      get
      {
        return this.Title;
      }
      set
      {
        this.Title = value;
      }
    }

    public MessageBoxResult MessageBoxResult
    {
      get
      {
        return this._result;
      }
      private set
      {
        this._result = value;
        if (MessageBoxResult.Cancel == this._result)
          this.DialogResult = new bool?(false);
        else
          this.DialogResult = new bool?(true);
      }
    }

    public MessageBoxResult DefaultResult
    {
      get
      {
        return (MessageBoxResult) this.GetValue(MessageBoxView.DefaultResultProperty);
      }
      set
      {
        this.SetValue(MessageBoxView.DefaultResultProperty, (object) value);
        switch (value)
        {
          case MessageBoxResult.OK:
            this._ok.IsDefault = true;
            this._ok.Focus();
            break;
          case MessageBoxResult.Cancel:
            this._cancel.IsDefault = true;
            this._cancel.Focus();
            break;
          case MessageBoxResult.Yes:
            this._yes.IsDefault = true;
            this._yes.Focus();
            break;
          case MessageBoxResult.No:
            this._no.IsDefault = true;
            this._no.Focus();
            break;
        }
      }
    }

    public MetroTwitStatusBase Message
    {
      get
      {
        return (MetroTwitStatusBase) this.GetValue(MessageBoxView.MessageProperty);
      }
      set
      {
        this.SetValue(MessageBoxView.MessageProperty, (object) value);
      }
    }

    public MessageBoxButton MessageBoxButton
    {
      get
      {
        return (MessageBoxButton) this.GetValue(MessageBoxView.MessageBoxButtonProperty);
      }
      set
      {
        this.SetValue(MessageBoxView.MessageBoxButtonProperty, (object) value);
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
          default:
            this._yes.Visibility = Visibility.Collapsed;
            this._no.Visibility = Visibility.Collapsed;
            this._cancel.Visibility = Visibility.Collapsed;
            this._ok.Visibility = Visibility.Collapsed;
            break;
        }
      }
    }

    public MessageBoxImage MessageBoxImage
    {
      get
      {
        return (MessageBoxImage) this.GetValue(MessageBoxView.MessageBoxImageProperty);
      }
      set
      {
        this.SetValue(MessageBoxView.MessageBoxImageProperty, (object) value);
      }
    }

    static MessageBoxView()
    {
    }

    public MessageBoxView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.ThemeChangeUI, (Action<GenericMessage<object>>) (o => this.Close()));
    }

    public static void ShowNonModal(string messageBoxText, string caption)
    {
      MessageBoxView messageBoxView = new MessageBoxView();
      messageBoxView.Caption = caption;
      try
      {
        if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
          messageBoxView.Owner = Application.Current.MainWindow;
      }
      catch
      {
      }
      if (!string.IsNullOrEmpty(messageBoxText))
        messageBoxView.Message = new MetroTwitStatusBase()
        {
          RawText = messageBoxText,
          Entities = RegularExpressions.ExtractEntities(messageBoxText)
        };
      messageBoxView.LayoutRoot.Triggers.Clear();
      messageBoxView.Show();
    }

    public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
    {
      return MessageBoxView.Show(Application.Current.MainWindow, messageBoxText, caption, button, defaultResult);
    }

    public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
    {
      MessageBoxView messageBoxView = new MessageBoxView();
      messageBoxView.Caption = caption;
      try
      {
        messageBoxView.DefaultResult = defaultResult;
        if (owner != null && owner.IsVisible)
          messageBoxView.Owner = owner;
      }
      catch
      {
      }
      if (!string.IsNullOrEmpty(messageBoxText))
        messageBoxView.Message = new MetroTwitStatusBase()
        {
          RawText = messageBoxText,
          Entities = RegularExpressions.ExtractEntities(messageBoxText)
        };
      messageBoxView.MessageBoxButton = button;
      messageBoxView.MessageBoxImage = MessageBoxImage.None;
      bool? nullable = messageBoxView.ShowDialog();
      if ((nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        return MessageBoxResult.Cancel;
      else
        return messageBoxView.MessageBoxResult;
    }

    private void cancel_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.Cancel;
    }

    private void no_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.No;
    }

    private void ok_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.OK;
    }

    private void this_Loaded(object sender, RoutedEventArgs e)
    {
      Storyboard storyboard = (Storyboard) this.TryFindResource((object) "MetroFadeZoom");
      if (storyboard != null)
        this.LayoutRoot.BeginStoryboard(storyboard.Clone(), HandoffBehavior.SnapshotAndReplace, false);
      this._close = (Button) this.Template.FindName("PART_Close", (FrameworkElement) this);
      if (null == this._close || this._cancel.IsVisible)
        return;
      this._close.IsCancel = false;
    }

    private void yes_Click(object sender, RoutedEventArgs e)
    {
      this.MessageBoxResult = MessageBoxResult.Yes;
    }

   
  }
}
