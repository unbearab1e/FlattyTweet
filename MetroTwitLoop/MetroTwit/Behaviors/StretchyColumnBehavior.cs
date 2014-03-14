// Type: MetroTwit.Behaviors.StretchyColumnBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace MetroTwit.Behaviors
{
  public class StretchyColumnBehavior : Behavior<TwitView>
  {
    private int columnresizingindex = -1;
    private const double MinWidth = 300.0;
    private static DispatcherTimer resizetimer;
    private object oldthumb;
    private int oldindex;

    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObject_Loaded);
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObject_Loaded);
      base.OnDetaching();
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
      DataGrid dataGrid = this.AssociatedObject.TweetColumns;
      if (dataGrid == null)
        throw new ArgumentException("Unable to find attached DataGrid");
      dataGrid.Loaded += new RoutedEventHandler(this.lv_Loaded);
      dataGrid.SizeChanged += new SizeChangedEventHandler(this.lv_SizeChanged);
      dataGrid.Unloaded += new RoutedEventHandler(this.lv_Unloaded);
      dataGrid.Columns.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Columns_CollectionChanged);
    }

    private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      PropertyDescriptor propertyDescriptor = (PropertyDescriptor) DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof (DataGridColumn));
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (DataGridColumn dataGridColumn in (IEnumerable) e.NewItems)
          propertyDescriptor.AddValueChanged((object) dataGridColumn, new EventHandler(this.ColumnWidthPropertyChanged));
      }
      if (e.Action != NotifyCollectionChangedAction.Remove)
        return;
      foreach (DataGridColumn dataGridColumn in (IEnumerable) e.OldItems)
        propertyDescriptor.AddValueChanged((object) dataGridColumn, new EventHandler(this.ColumnWidthPropertyChanged));
    }

    private void lv_Unloaded(object sender, RoutedEventArgs e)
    {
      (sender as DataGrid).SizeChanged -= new SizeChangedEventHandler(this.lv_SizeChanged);
    }

    private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (StretchyColumnBehavior.resizetimer == null)
      {
        StretchyColumnBehavior.resizetimer = new DispatcherTimer();
        StretchyColumnBehavior.resizetimer.Interval = TimeSpan.FromMilliseconds(0.5);
        StretchyColumnBehavior.resizetimer.Tick += (EventHandler) ((s, a) =>
        {
          this.SetColumnWidths();
          (s as DispatcherTimer).Stop();
        });
      }
      StretchyColumnBehavior.resizetimer.Stop();
      StretchyColumnBehavior.resizetimer.Start();
    }

    private void lv_Loaded(object sender, RoutedEventArgs e)
    {
      this.SetColumnWidths();
    }

    private void ColumnWidthPropertyChanged(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      this.columnresizingindex = this.AssociatedObject.TweetColumns.Columns.IndexOf(sender as DataGridColumn);
      Mouse.AddPreviewMouseUpHandler((DependencyObject) this.AssociatedObject, new MouseButtonEventHandler(this.BaseDataGrid_MouseLeftButtonUp));
    }

    private void BaseDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.columnresizingindex < 0 || !(e.OriginalSource is Thumb) || this.oldthumb == e.OriginalSource && this.oldindex != this.columnresizingindex)
        return;
      TwitViewModel twitViewModel = this.AssociatedObject.DataContext as TwitViewModel;
      TweetListViewModel tweetListViewModel = (this.AssociatedObject.TweetColumns.Columns[this.columnresizingindex].Header as TweetListView).DataContext as TweetListViewModel;
      App.AppState.Accounts[twitViewModel.TwitterAccountID].Settings.Columns[tweetListViewModel.UniqueTweetListID].ColumnIsSetWidth = true;
      twitViewModel.ColumnsToShow[tweetListViewModel.UniqueTweetListID].ResetWidthVisibility = true;
      this.oldthumb = e.OriginalSource;
      this.oldindex = this.columnresizingindex;
      this.columnresizingindex = -1;
      this.SetColumnWidths();
    }

    private void SetColumnWidths()
    {
      if (this.AssociatedObject == null || this.AssociatedObject.DataContext == null)
        return;
      TwitViewModel twitViewModel = this.AssociatedObject.DataContext as TwitViewModel;
      if (twitViewModel != null && App.AppState.Accounts[twitViewModel.TwitterAccountID] != null)
      {
        List<MetroTwitColumn> list = Enumerable.ToList<MetroTwitColumn>((IEnumerable<MetroTwitColumn>) Enumerable.OrderBy<MetroTwitColumn, int>(Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>) App.AppState.Accounts[twitViewModel.TwitterAccountID].Settings.Columns, (Func<MetroTwitColumn, bool>) (x => x.ColumnPinned)), (Func<MetroTwitColumn, int>) (y => y.Index)));
        IOrderedEnumerable<DataGridColumn> orderedEnumerable = Enumerable.OrderBy<DataGridColumn, int>((IEnumerable<DataGridColumn>) this.AssociatedObject.TweetColumns.Columns, (Func<DataGridColumn, int>) (x => x.DisplayIndex));
        double num1 = 0.0;
        int index = 0;
        int num2 = 0;
        if (list == null)
          return;
        foreach (DataGridColumn dataGridColumn in (IEnumerable<DataGridColumn>) orderedEnumerable)
        {
          if (list.Count < 0 || index >= list.Count)
            return;
          if (list[index].ColumnIsSetWidth && dataGridColumn.Width.Value < 300.0)
            list[index].ColumnIsSetWidth = false;
          if (!list[index].ColumnIsSetWidth)
          {
            dataGridColumn.Width = new DataGridLength(0.0, DataGridLengthUnitType.Pixel);
            ++num2;
          }
          else
          {
            list[index].ColumnWidth = dataGridColumn.ActualWidth;
            num1 += dataGridColumn.ActualWidth;
          }
          ++index;
        }
        int num3 = 0;
        foreach (DataGridColumn dataGridColumn in (IEnumerable<DataGridColumn>) orderedEnumerable)
        {
          if (dataGridColumn.Width.Value < 300.0)
          {
            double num4 = num2 > 0 ? (this.AssociatedObject.TweetColumns.ActualWidth - num1 - 7.0) / (double) num2 : this.AssociatedObject.TweetColumns.ActualWidth - num1 - 7.0;
            if (num4 > 300.0)
              this.AssociatedObject.TweetColumns.Columns[this.AssociatedObject.TweetColumns.Columns.IndexOf(dataGridColumn)].Width = new DataGridLength(num4, DataGridLengthUnitType.Pixel);
            else
              this.AssociatedObject.TweetColumns.Columns[this.AssociatedObject.TweetColumns.Columns.IndexOf(dataGridColumn)].Width = new DataGridLength(300.0, DataGridLengthUnitType.Pixel);
          }
          ++num3;
        }
      }
    }
  }
}
