
using FlattyTweet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  public class UnreadMarkerListBoxBehavior : Behavior<ListBox>
  {
    private ScrollViewer scrollviewer;
    private Button unreadmarker;

    public TweetListViewModel ViewModel { get; set; }

    protected override void OnAttached()
    {
      base.OnAttached();
      if (this.AssociatedObject == null)
        return;
      this.AssociatedObject.ApplyTemplate();
      object name1 = this.AssociatedObject.Template.FindName("Scroller", (FrameworkElement) this.AssociatedObject);
      if (name1 != null && name1 is ScrollViewer)
      {
        this.scrollviewer = name1 as ScrollViewer;
        this.scrollviewer.ApplyTemplate();
        object name2 = this.scrollviewer.Template.FindName("PART_VerticalScrollBar", (FrameworkElement) this.scrollviewer);
        if (name2 != null && name2 is ScrollBar)
        {
          ScrollBar scrollBar = name2 as ScrollBar;
          scrollBar.ApplyTemplate();
          object name3 = scrollBar.Template.FindName("PART_UnreadMarker", (FrameworkElement) scrollBar);
          if (name3 != null && name3 is Button)
            this.unreadmarker = name3 as Button;
          if (this.unreadmarker != null)
          {
            this.unreadmarker.PreviewMouseDown += new MouseButtonEventHandler(this.unreadmarker_MouseDown);
            this.scrollviewer.ScrollChanged += new ScrollChangedEventHandler(this.scrollviewer_ScrollChanged);
          }
        }
      }
    }

    private void scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
      if (e.ExtentHeightChange == 0.0 || this.AssociatedObject.Items == null || this.AssociatedObject.Items.Count <= 0)
        return;
      this.SetMarker();
    }

    private void unreadmarker_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.AssociatedObject.Items == null || this.AssociatedObject.Items.Count <= 0)
        return;
      IEnumerable<MetroTwitStatusBase> source = Enumerable.Where<MetroTwitStatusBase>(this.AssociatedObject.ItemsSource as IEnumerable<MetroTwitStatusBase>, (Func<MetroTwitStatusBase, int, bool>) ((x, r) => x != null && x.FirstRead));
      if (source != null)
      {
        if (Enumerable.Count<MetroTwitStatusBase>(source) > 0)
        {
          this.AssociatedObject.ScrollIntoView((object) Enumerable.First<MetroTwitStatusBase>(source));
          this.AssociatedObject.UpdateLayout();
        }
        else
        {
          this.AssociatedObject.ScrollIntoView(this.AssociatedObject.Items[this.AssociatedObject.Items.Count - 1]);
          this.AssociatedObject.UpdateLayout();
        }
      }
      else
        this.SetMarker();
    }

    public void SetMarker()
    {
      if (this.unreadmarker == null)
        return;
      int unreadCount = this.ViewModel.UnreadCount;
      if (unreadCount > 0)
      {
        double num1 = (double) unreadCount / (double) this.AssociatedObject.Items.Count;
        if (num1 == 0.0)
          this.unreadmarker.Margin = new Thickness(this.unreadmarker.Margin.Left, 0.0, this.unreadmarker.Margin.Right, 0.0);
        else if (num1 == 1.0)
          this.unreadmarker.Margin = new Thickness(this.unreadmarker.Margin.Left, this.scrollviewer.ViewportHeight - this.unreadmarker.ActualHeight, this.unreadmarker.Margin.Right, 0.0);
        else if (this.scrollviewer != null)
        {
          double num2 = this.scrollviewer.ViewportHeight * num1 - this.unreadmarker.ActualHeight / 2.0 > 0.0 ? this.scrollviewer.ViewportHeight * num1 - this.unreadmarker.ActualHeight / 2.0 : 0.0;
          Button button = this.unreadmarker;
          Thickness margin = this.unreadmarker.Margin;
          double left = margin.Left;
          double top = num2;
          margin = this.unreadmarker.Margin;
          double right = margin.Right;
          double bottom = 0.0;
          Thickness thickness = new Thickness(left, top, right, bottom);
          button.Margin = thickness;
        }
        this.unreadmarker.Visibility = Visibility.Visible;
      }
      else
        this.unreadmarker.Visibility = Visibility.Collapsed;
    }

    protected override void OnDetaching()
    {
      if (this.unreadmarker != null)
        this.unreadmarker.PreviewMouseDown -= new MouseButtonEventHandler(this.unreadmarker_MouseDown);
      if (this.scrollviewer != null)
        this.scrollviewer.ScrollChanged -= new ScrollChangedEventHandler(this.scrollviewer_ScrollChanged);
      this.unreadmarker = (Button) null;
      this.scrollviewer = (ScrollViewer) null;
      base.OnDetaching();
    }
  }
}
