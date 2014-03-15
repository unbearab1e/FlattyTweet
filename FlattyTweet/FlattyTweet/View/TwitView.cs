
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Twitterizer.Models;

namespace FlattyTweet.View
{
    public partial class TwitView : UserControl, IComponentConnector
  {
    private string[] supportedImageTypeExtensions = new string[4]
    {
      ".jpeg",
      ".jpg",
      ".png",
      ".gif"
    };
    private TwitViewModel viewModel;
    private ContextMenu NewTweetEditContextMenu;
   

    public TwitView()
    {
      this.InitializeComponent();
      this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.TwitView_DataContextChanged);
      this.EntryRow.Height = new GridLength((double) App.AppState.StatusEntryFieldHeight);
    }

    private void TwitView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (this.DataContext == null)
        return;
      this.viewModel = this.DataContext as TwitViewModel;
      this.InitialiseMessengerRegistrations();
      this.viewModel.LoadTweetViews(true);
      this.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.TwitView_DataContextChanged);
    }

    private void InitialiseMessengerRegistrations()
    {
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) ViewModelMessages.ReloadTweetListView, (Action<GenericMessage<object>>) (o => this.TweetColumns.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, (object) this.TweetColumns))));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.ResetColumnWidth, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o =>
      {
        if (this.viewModel.ColumnsToShow.IndexOf(o.Content as TweetListViewModel) <= -1)
          return;
        App.AppState.Accounts[this.viewModel.TwitterAccountID].Settings.Columns[(o.Content as TweetListViewModel).UniqueTweetListID].ColumnIsSetWidth = false;
        this.TweetColumns.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, (object) this.TweetColumns));
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.UpdateActualTweetText, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o => this.UpdateBoundTweetText()));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.NewTweetEditFocus, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o => this.NewTweetEdit.Focus()));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.SetNewTweetEntryOptionsContainer, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o => SettingsData.Instance.NewTweetEntryOptionsContainer = (FrameworkElement) this.NewTweetAndOptionsContainer));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.NoPopupColumn, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o =>
      {
        if (SettingsData.Instance.SelectedColumn != null)
          return;
        SettingsData.Instance.SelectedColumn = (UIElement) this.TweetColumns;
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.NewTweetEditUpdateText, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o =>
      {
        this.NewTweetEdit.Document = new FlowDocument()
        {
          Blocks = {
            (Block) new Paragraph()
            {
              Inlines = {
                this.viewModel.ActualTweetText
              }
            }
          }
        };
        this.NewTweetEdit.CaretPosition = this.NewTweetEdit.CaretPosition.DocumentEnd;
        this.NewTweetEdit.Focus();
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.SetPopupTarget, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o =>
      {
        if (o.Content.ToString() == "P")
          this.UpdatePopupTarget((UIElement) this.profileButton);
        else if (o.Content.ToString() == "S")
          this.UpdatePopupTarget((UIElement) this.searchButton);
        else if (o.Content.ToString() == "L")
          this.UpdatePopupTarget((UIElement) this.listsButton);
        else if (o.Content.ToString() == "T")
          this.UpdatePopupTarget((UIElement) this.trendsButton);
        else if (o.Content.ToString() == "A")
          this.UpdatePopupTarget((UIElement) this.addcolumnButton);
        SettingsData.Instance.SelectedColumn = (UIElement) null;
      }));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.SelectTweetText, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o => RTBExtensions.SelectText(this.NewTweetEdit, o.Content.ToString())));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.SavedSearchesUpdated, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o => this.Dispatcher.BeginInvoke((Action) (() =>
      {
        foreach (Control item_0 in Enumerable.ToArray<Control>(Enumerable.Cast<Control>((IEnumerable) this.SearchMenu.Items)))
        {
          if (item_0 != this.newSearchMenuItem)
            this.SearchMenu.Items.Remove((object) item_0);
        }
        this.SearchMenu.Items.Add((object) new Separator());
        foreach (TwitterSavedSearch item_1 in (Collection<TwitterSavedSearch>) App.AppState.Accounts[this.viewModel.TwitterAccountID].SavedSearches)
        {
          MenuItem local_2 = new MenuItem();
          local_2.Header = (object) item_1.Name;
          local_2.SetBinding(MenuItem.CommandProperty, (BindingBase) new Binding("SavedSearchCommand")
          {
            Mode = BindingMode.OneTime
          });
          local_2.CommandParameter = (object) item_1.Id;
          this.SearchMenu.Items.Add((object) local_2);
        }
      }), DispatcherPriority.Background, new object[0])));
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.ColumnsToShow, this.viewModel.TwitterAccountID), (Action<GenericMessage<object>>) (o =>
      {
        this.GenerateGridView((int) o.Content);
        this.TweetColumns.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent, (object) this.TweetColumns));
      }));
      Messenger.Default.Register<KeyboardShortcutMessage>((object) this, (object) CommonCommands.MultiAccountifyToken((Enum) ViewModelMessages.KeyboardShortcut, this.viewModel.TwitterAccountID), new Action<KeyboardShortcutMessage>(this.KeyboardShortcut));
    }

    private void KeyboardShortcut(KeyboardShortcutMessage k)
    {
      if (k == null)
        return;
      switch (k.Content)
      {
        case KeyboardShortcutType.ColumnLeft:
        case KeyboardShortcutType.ColumnRight:
          try
          {
            IEnumerable<DataGridColumn> source = Enumerable.Where<DataGridColumn>((IEnumerable<DataGridColumn>) this.TweetColumns.Columns, (Func<DataGridColumn, bool>) (x => x.Header == k.Sender));
            if (Enumerable.Count<DataGridColumn>(source) > 0)
            {
              List<DataGridColumn> list = Enumerable.ToList<DataGridColumn>((IEnumerable<DataGridColumn>) Enumerable.OrderBy<DataGridColumn, int>((IEnumerable<DataGridColumn>) this.TweetColumns.Columns, (Func<DataGridColumn, int>) (x => x.DisplayIndex)));
              int index1 = k.Content == KeyboardShortcutType.ColumnLeft ? (Enumerable.First<DataGridColumn>((IEnumerable<DataGridColumn>) list) == Enumerable.First<DataGridColumn>(source) ? list.Count - 1 : Enumerable.First<DataGridColumn>(source).DisplayIndex - 1) : (Enumerable.Last<DataGridColumn>((IEnumerable<DataGridColumn>) list) == Enumerable.First<DataGridColumn>(source) ? 0 : Enumerable.First<DataGridColumn>(source).DisplayIndex + 1);
              if ((list[index1].Header as TweetListView).tweets != null)
              {
                int index2 = (list[index1].Header as TweetListView).tweets.SelectedIndex;
                if (index2 == -1)
                  index2 = (list[index1].Header as TweetListView).tweets.Items.CurrentPosition;
                if (index2 == -1)
                  index2 = 0;
                if ((list[index1].Header as TweetListView).tweets.Items.Count > 0)
                {
                  (list[index1].Header as TweetListView).tweets.SelectedIndex = index2;
                  ((list[index1].Header as TweetListView).tweets.ItemContainerGenerator.ContainerFromIndex(index2) as ListBoxItem).Focus();
                  (list[index1].Header as TweetListView).tweets.ScrollIntoView((list[index1].Header as TweetListView).tweets.SelectedItem);
                }
                else
                  (list[index1].Header as TweetListView).tweets.Focus();
              }
              break;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            throw;
          }
      }
    }

    private void GenerateGridView(int ColumnToRemoveIndex = -1)
    {
      if (this.viewModel.ColumnsToShow.Count > this.TweetColumns.Columns.Count)
      {
        int index = 0;
        foreach (TweetListViewModel columnToShow in (Collection<TweetListViewModel>) this.viewModel.ColumnsToShow)
        {
          if (index + 1 > this.TweetColumns.Columns.Count)
          {
            this.TweetColumns.Columns.Add(this.CreateColumn(columnToShow));
            if (SettingsData.Instance.FirstLoad)
              this.TweetColumns.Columns[index].Width = App.AppState.Accounts[this.viewModel.TwitterAccountID].Settings.Columns[columnToShow.UniqueTweetListID].ColumnIsSetWidth ? new DataGridLength(App.AppState.Accounts[this.viewModel.TwitterAccountID].Settings.Columns[columnToShow.UniqueTweetListID].ColumnWidth, DataGridLengthUnitType.Pixel) : new DataGridLength(1.0, DataGridLengthUnitType.Pixel);
            columnToShow.Index = index;
          }
          ++index;
        }
        SettingsData.Instance.FirstLoad = false;
      }
      else if (ColumnToRemoveIndex >= 0 && this.TweetColumns.Columns.Count > ColumnToRemoveIndex)
      {
        IEnumerable<DataGridColumn> source = Enumerable.Where<DataGridColumn>((IEnumerable<DataGridColumn>) this.TweetColumns.Columns, (Func<DataGridColumn, bool>) (x => x.DisplayIndex == ColumnToRemoveIndex));
        if (Enumerable.Count<DataGridColumn>(source) > 0)
        {
          Enumerable.First<DataGridColumn>(source).Header = (object) null;
          this.TweetColumns.Columns.Remove(Enumerable.First<DataGridColumn>(source));
          if (ColumnToRemoveIndex < this.TweetColumns.Columns.Count - 1)
          {
            for (int index = ColumnToRemoveIndex; index <= this.TweetColumns.Columns.Count - 1; ++index)
              ((Collection<TweetListViewModel>) this.viewModel.ColumnsToShow)[index].Index = index - 1;
          }
        }
      }
    }

    private DataGridColumn CreateColumn(TweetListViewModel columnToShow)
    {
      DataGridColumn dataGridColumn1 = (DataGridColumn) new DataGridTextColumn();
      DataGridColumn dataGridColumn2 = dataGridColumn1;
      TweetListView tweetListView1 = new TweetListView();
      tweetListView1.DataContext = (object) columnToShow;
      TweetListView tweetListView2 = tweetListView1;
      dataGridColumn2.Header = (object) tweetListView2;
      dataGridColumn1.MinWidth = 300.0;
      return dataGridColumn1;
    }

    private void TweetColumns_ColumnReordered(object sender, DataGridColumnEventArgs e)
    {
      int displayIndex = e.Column.DisplayIndex;
      int oldIndex = this.viewModel.ColumnsToShow.IndexOf((e.Column.Header as TweetListView).DataContext as TweetListViewModel);
      if (displayIndex <= -1 || oldIndex <= -1)
        return;
      if (displayIndex < oldIndex)
      {
        for (int index = displayIndex; index <= oldIndex; ++index)
          ((Collection<TweetListViewModel>) this.viewModel.ColumnsToShow)[index].Index = index + 1;
      }
      if (displayIndex > oldIndex)
      {
        for (int index = oldIndex; index <= displayIndex; ++index)
          ((Collection<TweetListViewModel>) this.viewModel.ColumnsToShow)[index].Index = index - 1;
      }
      this.viewModel.ColumnsToShow.Move(oldIndex, displayIndex);
      ((Collection<TweetListViewModel>) this.viewModel.ColumnsToShow)[displayIndex].Index = displayIndex;
    }

    private DependencyObject FindChild(object sender, Type ChildType)
    {
      DependencyObject dependencyObject = new DependencyObject();
      DependencyObject reference = (DependencyObject) sender;
      while (reference.GetType() != ChildType)
        reference = VisualTreeHelper.GetChild(reference, 0);
      return reference;
    }

    private void NewTweetEdit_MouseEvent(object sender, RoutedEventArgs e)
    {
      Border border = this.FindChild((object) this.NewTweetUserControl, typeof (Border)) as Border;
      if (this.NewTweetEdit.IsFocused)
        VisualStateManager.GoToElementState((FrameworkElement) border, "KeyboardFocus", true);
      else if (this.NewTweetEdit.IsMouseOver)
        VisualStateManager.GoToElementState((FrameworkElement) border, "MouseOver", true);
      else
        VisualStateManager.GoToElementState((FrameworkElement) border, "BaseState", true);
      this.UpdateWhatsHappeningWatermark(this.viewModel);
    }

    private void TwitView_Loaded(object sender, RoutedEventArgs e)
    {
      this.NewTweetEdit.ContextMenuOpening += new ContextMenuEventHandler(this.NewTweetEdit_ContextMenuOpening);
      this.NewTweetEdit.ContextMenuClosing += new ContextMenuEventHandler(this.NewTweetEdit_ContextMenuClosing);
      this.NewTweetEditContextMenu = new ContextMenu();
      this.NewTweetEdit.ContextMenu = this.NewTweetEditContextMenu;
      DataObject.AddPastingHandler((DependencyObject) this.NewTweetEdit, new DataObjectPastingEventHandler(this.NewTweetEditPasting));
    }

    private void NewTweetEditPasting(object sender, DataObjectPastingEventArgs e)
    {
      if (!App.AppState.Accounts[this.viewModel.TwitterAccountID].AutomaticallyShortenLinksFalse)
        return;
      string textData = e.DataObject.GetData(DataFormats.UnicodeText) as string;
      new TextRange(this.NewTweetEdit.Selection.Start, this.NewTweetEdit.Selection.End).Text = string.Empty;
      this.NewTweetEdit.CaretPosition = this.NewTweetEdit.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward) ?? this.NewTweetEdit.CaretPosition;
      this.NewTweetEdit.CaretPosition.InsertTextInRun(textData);
      e.CancelCommand();
    }

    private void NewTweetEdit_ContextMenuClosing(object sender, ContextMenuEventArgs e)
    {
      this.NewTweetEditContextMenu.Items.Clear();
    }

    private void NewTweetEdit_ContextMenuOpening(object sender, RoutedEventArgs e)
    {
      this.NewTweetEditContextMenu.Items.Clear();
      SpellingError spellingError = this.NewTweetEdit.GetSpellingError(this.NewTweetEdit.CaretPosition);
      if (spellingError != null)
      {
        if (Enumerable.Count<string>(spellingError.Suggestions) > 0)
        {
          foreach (string str in spellingError.Suggestions)
          {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = (object) str;
            menuItem.FontWeight = FontWeights.Bold;
            menuItem.Command = (ICommand) EditingCommands.CorrectSpellingError;
            menuItem.CommandParameter = (object) str;
            menuItem.CommandTarget = (IInputElement) this.NewTweetEdit;
            this.NewTweetEditContextMenu.Items.Add((object) menuItem);
          }
        }
        else
        {
          MenuItem menuItem = new MenuItem();
          menuItem.Header = (object) "No suggestions";
          menuItem.IsEnabled = false;
          this.NewTweetEditContextMenu.Items.Add((object) menuItem);
        }
        this.NewTweetEditContextMenu.Items.Add((object) new Separator());
        MenuItem menuItem1 = new MenuItem();
        menuItem1.Header = (object) "Ignore All";
        menuItem1.Command = (ICommand) EditingCommands.IgnoreSpellingError;
        menuItem1.CommandTarget = (IInputElement) this.NewTweetEdit;
        this.NewTweetEditContextMenu.Items.Add((object) menuItem1);
        this.NewTweetEditContextMenu.Items.Add((object) new Separator());
      }
      MenuItem menuItem2 = new MenuItem();
      menuItem2.Command = (ICommand) ApplicationCommands.Cut;
      menuItem2.Click += new RoutedEventHandler(this.cutMenuItem_Click);
      this.NewTweetEditContextMenu.Items.Add((object) menuItem2);
      this.NewTweetEditContextMenu.Items.Add((object) new MenuItem()
      {
        Command = (ICommand) ApplicationCommands.Copy
      });
      this.NewTweetEditContextMenu.Items.Add((object) new MenuItem()
      {
        Command = (ICommand) ApplicationCommands.Paste
      });
    }

    private void cutMenuItem_Click(object sender, RoutedEventArgs e)
    {
      SettingsData.Instance.CutIsTriggered = true;
    }

    private void NewTweetEdit_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!this.NewTweetEdit.IsFocused && this.viewModel.ActualTweetText != "" && this.viewModel.ActualTweetText != null)
      {
        this.NewTweetEdit.Focus();
        this.NewTweetEdit.CaretPosition = this.NewTweetEdit.CaretPosition.DocumentEnd;
      }
      this.UpdateBoundTweetText();
      this.UpdateWhatsHappeningWatermark(this.viewModel);
    }

    private void UpdateWhatsHappeningWatermark(TwitViewModel viewModel)
    {
      if (viewModel.ActualTweetText.Length > 0 || this.NewTweetEdit.IsFocused)
        viewModel.WhatsHappeningVisibility = Visibility.Collapsed;
      else
        viewModel.WhatsHappeningVisibility = Visibility.Visible;
    }

    private void UpdateBoundTweetText()
    {
      this.viewModel.ActualTweetText = RTBExtensions.GetInlineText(this.NewTweetEdit);
      this.viewModel.CurrentTweetDocument = this.NewTweetEdit.Document;
    }

    private DependencyObject FindParent(object sender, Type ParentType)
    {
      DependencyObject reference = (DependencyObject) sender;
      while (reference != null && !ParentType.IsInstanceOfType((object) reference))
        reference = VisualTreeHelper.GetParent(reference);
      return reference;
    }

    private void addcolumnButton_Click(object sender, RoutedEventArgs e)
    {
      this.addcolumnButtonMenu.PlacementTarget = (UIElement) this;
      this.addcolumnButtonMenu.IsOpen = true;
    }

    private void NewTweetEdit_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        SettingsData.Instance.CutIsTriggered = true;
      RichTextBox richTextBox = sender as RichTextBox;
      Adorner[] adorners = AdornerLayer.GetAdornerLayer((Visual) richTextBox).GetAdorners((UIElement) richTextBox);
      IntellisenseAdorner intellisenseAdorner = (IntellisenseAdorner) null;
      if (adorners != null)
      {
        foreach (Adorner adorner in adorners)
        {
          if (adorner is IntellisenseAdorner)
            intellisenseAdorner = adorner as IntellisenseAdorner;
        }
      }
      if (e.Key == Key.Return && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
      {
        bool flag = false;
        if (this.NewTweetEdit.CaretPosition.GetAdjacentElement(LogicalDirection.Backward) == null)
          flag = true;
        this.NewTweetEdit.CaretPosition.InsertLineBreak();
        if (flag)
          this.NewTweetEdit.CaretPosition = this.NewTweetEdit.CaretPosition.DocumentEnd;
        e.Handled = true;
      }
      else if (e.Key == Key.Return && (intellisenseAdorner != null && !intellisenseAdorner.IsActive || intellisenseAdorner == null) && SettingsData.Instance.PostTweetOnEnter)
      {
        if (!this.viewModel.PostTweetEnabled)
          return;
        this.viewModel.PostTweetCommand.Execute((object) null);
        e.Handled = true;
      }
      else
      {
        if (e.Key != Key.Escape || intellisenseAdorner != null && intellisenseAdorner.IsActive)
          return;
        this.viewModel.ResetNewTweet(true);
        this.TweetColumns.Focus();
        e.Handled = true;
      }
    }

    private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = false;
      e.Handled = true;
    }

    private void NewTweetEdit_DragOver(object sender, DragEventArgs e)
    {
      e.Effects = DragDropEffects.Copy;
      e.Handled = true;
    }

    private void NewTweetEdit_Drop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;
      object data = e.Data.GetData(DataFormats.FileDrop);
      if (data != null)
      {
        string[] strArray = data as string[];
        if (strArray != null && strArray.Length > 0)
          (this.DataContext as TwitViewModel).GenerateAndAddSingleImage(strArray[0]);
      }
    }

    private void CanExecutePasteHandler(object sender, CanExecuteRoutedEventArgs e)
    {
      try
      {
        if (Clipboard.ContainsFileDropList())
        {
          StringCollection fileDropList = Clipboard.GetFileDropList();
          string path = fileDropList[0];
          if (fileDropList.Count != 1 || !Enumerable.Contains<string>((IEnumerable<string>) this.supportedImageTypeExtensions, Path.GetExtension(path).ToLower()))
            return;
          e.CanExecute = true;
          e.Handled = true;
        }
        else
        {
          if (!Clipboard.ContainsImage() || Clipboard.ContainsText())
            return;
          e.CanExecute = true;
          e.Handled = true;
        }
      }
      catch
      {
        e.CanExecute = true;
        e.Handled = false;
      }
    }

    private void ExecutedPasteHandler(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        if (Clipboard.ContainsFileDropList())
        {
          StringCollection fileDropList = Clipboard.GetFileDropList();
          string path = fileDropList[0];
          if (fileDropList.Count != 1 || !Enumerable.Contains<string>((IEnumerable<string>) this.supportedImageTypeExtensions, Path.GetExtension(path).ToLower()))
            return;
          (this.DataContext as TwitViewModel).GenerateAndAddSingleImage(fileDropList[0]);
        }
        else
        {
          if (!Clipboard.ContainsImage() || Clipboard.ContainsText())
            return;
          string str = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "png"));
          using (FileStream fileStream = new FileStream(str, FileMode.CreateNew))
          {
            FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap(Clipboard.GetImage(), PixelFormats.Rgb24, (BitmapPalette) null, 0.0);
            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) formatConvertedBitmap));
            pngBitmapEncoder.Save((Stream) fileStream);
          }
          (this.DataContext as TwitViewModel).GenerateAndAddSingleImage(str);
        }
      }
      catch
      {
      }
    }

    private void TwitViewButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      this.UpdatePopupTarget(sender as UIElement);
    }

    private void UpdatePopupTarget(UIElement element)
    {
      SettingsData.Instance.PopupTarget = element;
    }

    private void NewTweetAndOptionsContainer_MouseDown(object sender, MouseButtonEventArgs e)
    {
      PopupService.CloseView(false);
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.ActualWidth < 650.0)
      {
        this.EntryRow.Height = new GridLength(160.0);
        App.AppState.StatusEntryFieldHeight = 160;
        this.NewTweetAndOptionsContainer.Children.Remove((UIElement) this.LeftButtons);
        this.NewTweetAndOptionsContainer.Children.Remove((UIElement) this.RightButtons);
        this.NewTweetAndOptionsContainer.Children.Add((UIElement) this.LeftButtons);
        this.NewTweetAndOptionsContainer.Children.Add((UIElement) this.RightButtons);
        DockPanel.SetDock((UIElement) this.NewTweetUserControl, Dock.Bottom);
        this.NewTweetAndOptionsContainer.LastChildFill = false;
        this.NewTweetUserControl.Height = 80.0;
        Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(180), (object) ViewModelMessages.SizeUpdated);
      }
      else
      {
        this.EntryRow.Height = new GridLength(90.0);
        App.AppState.StatusEntryFieldHeight = 90;
        this.NewTweetAndOptionsContainer.Children.Remove((UIElement) this.LeftButtons);
        this.NewTweetAndOptionsContainer.Children.Remove((UIElement) this.RightButtons);
        this.NewTweetAndOptionsContainer.Children.Insert(0, (UIElement) this.LeftButtons);
        this.NewTweetAndOptionsContainer.Children.Insert(1, (UIElement) this.RightButtons);
        this.NewTweetAndOptionsContainer.LastChildFill = true;
        this.NewTweetUserControl.Height = (double) FrameworkElement.HeightProperty.DefaultMetadata.DefaultValue;
        Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(90), (object) ViewModelMessages.SizeUpdated);
      }
    }

   
  }
}
