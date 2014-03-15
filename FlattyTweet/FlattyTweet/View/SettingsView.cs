
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensibility;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace FlattyTweet.View
{
    public partial class SettingsView : UserControl 
  {
    

    public SettingsView()
    {
      this.InitializeComponent();
      this.DataContext = (object) new SettingsViewModel();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.ExpandSettingsServices, (Action<GenericMessage<object>>) (o => this.tabSettings.SelectedIndex = this.tabSettings.Items.IndexOf((object) this.servicesTab)));
    }

    private void tabSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.OriginalSource.GetType() != typeof (TabControl))
        e.Handled = true;
      if (this.tabSettings.SelectedIndex != this.tabSettings.Items.IndexOf((object) this.servicesTab))
        return;
      this.tweetServicesOptionsPanel.Children.Clear();
      foreach (ITweetService tweetService in CoreServices.Instance.TweetServices)
      {
        if (tweetService.HasSettings)
          this.tweetServicesOptionsPanel.Children.Add((UIElement) tweetService.GetUISettings);
      }
    }

    private void geosenseLink_Click(object sender, RoutedEventArgs e)
    {
      CommonCommands.OpenLink("http://www.geosenseforwindows.com");
    }

    private void proxletLink_Click(object sender, RoutedEventArgs e)
    {
      CommonCommands.OpenLink("http://proxlet.com/");
    }

    private void urlShorteningServicesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.DisplayURLShorteningServiceOptions(sender as ComboBox);
    }

    private void DisplayURLShorteningServiceOptions(ComboBox sender)
    {
      UserControl userControl = (UserControl) null;
      foreach (object obj in (sender.Parent as StackPanel).Children)
      {
        if (obj is UserControl)
        {
          userControl = obj as UserControl;
          break;
        }
      }
      if (userControl != null)
        (sender.Parent as StackPanel).Children.Remove((UIElement) userControl);
      IURLShorteningService shorteningService = sender.SelectedItem as IURLShorteningService;
      if (shorteningService == null || !shorteningService.HasSettings)
        return;
      UserControl uiSettings = shorteningService.GetUISettings((sender.DataContext as UserAccountViewModel).TwitterAccountID);
      (sender.Parent as StackPanel).Children.Add((UIElement) uiSettings);
    }

    private void NotificationPositionClick(object sender, RoutedEventArgs e)
    {
      int childrenCount = VisualTreeHelper.GetChildrenCount((sender as ToggleButton).Parent);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        ToggleButton toggleButton = VisualTreeHelper.GetChild((sender as ToggleButton).Parent, childIndex) as ToggleButton;
        if (toggleButton != null)
          toggleButton.IsChecked = new bool?(false);
      }
      (sender as ToggleButton).IsChecked = new bool?(true);
    }

    private void Settings_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void urlShorteningServicesComboBox_Loaded(object sender, RoutedEventArgs e)
    {
      this.DisplayURLShorteningServiceOptions(sender as ComboBox);
    }

    private void RichTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
      RichTextBox richTextBox = sender as RichTextBox;
      string textData = (e.DataObject.GetData(DataFormats.UnicodeText) as string).Replace("\n", " ").Replace("\r", "");
      new TextRange(richTextBox.Selection.Start, richTextBox.Selection.End).Text = string.Empty;
      richTextBox.CaretPosition = richTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward) ?? richTextBox.CaretPosition;
      richTextBox.CaretPosition.InsertTextInRun(textData);
      e.CancelCommand();
    }

 
  }
}
