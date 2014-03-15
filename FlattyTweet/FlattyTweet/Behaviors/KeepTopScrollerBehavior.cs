
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  public class KeepTopScrollerBehavior : Behavior<BaseTweetListView>
  {
    private ScrollViewer scrollviewer;

    protected override void OnAttached()
    {
      base.OnAttached();
      if (this.AssociatedObject.TweetListBox == null)
        return;
      this.AssociatedObject.TweetListBox.ApplyTemplate();
      object name = this.AssociatedObject.TweetListBox.Template.FindName("Scroller", (FrameworkElement) this.AssociatedObject.TweetListBox);
      if (name != null && name is ScrollViewer)
      {
        this.scrollviewer = name as ScrollViewer;
        if (this.scrollviewer != null)
          this.scrollviewer.ScrollChanged += new ScrollChangedEventHandler(this.scrollviewer_ScrollChanged);
      }
    }

    private void scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
      if ((this.AssociatedObject.DataContext as TweetListViewModel).LastCollectionState != RefreshTypes.ForeverScroll && e.ExtentWidthChange != e.ExtentWidth && (e.ExtentWidthChange == 0.0 && e.ExtentHeightChange != e.ExtentHeight) && (e.ExtentHeightChange > 0.0 && this.AssociatedObject.TweetListBox.Items != null) && this.AssociatedObject.TweetListBox.Items.Count > 0 && ((e.VerticalOffset != 0.0 || e.VerticalOffset != e.VerticalChange || !SettingsData.Instance.KeepScrollPositionatTop && e.VerticalOffset == 0.0) && (this.AssociatedObject.DataContext as TweetListViewModel).LastCollectionState != RefreshTypes.InitialLoadForeverScroll))
        this.scrollviewer.ScrollToVerticalOffset(e.VerticalOffset - e.VerticalChange + e.ExtentHeightChange);
      if (e.VerticalChange != 0.0 && e.VerticalOffset >= e.ExtentHeight - e.ViewportHeight - (e.ExtentHeight - e.ViewportHeight) / 100.0 * 10.0)
        (this.AssociatedObject.DataContext as TweetListViewModel).ForeverScroll();
      (this.AssociatedObject.DataContext as TweetListViewModel).ScrollNearTop = e.VerticalOffset <= e.ExtentHeight - e.ViewportHeight - (e.ExtentHeight - e.ViewportHeight) / 100.0 * 90.0;
    }

    protected override void OnDetaching()
    {
      if (this.scrollviewer != null)
        this.scrollviewer.ScrollChanged -= new ScrollChangedEventHandler(this.scrollviewer_ScrollChanged);
      this.scrollviewer = (ScrollViewer) null;
      base.OnDetaching();
    }
  }
}
