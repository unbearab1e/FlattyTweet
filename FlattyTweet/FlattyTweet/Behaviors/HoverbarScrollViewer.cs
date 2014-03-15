
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class HoverbarScrollViewer : Behavior<ScrollViewer>
  {
    private ScrollViewer _scrollViewer;

    protected override void OnAttached()
    {
      base.OnAttached();
      this._scrollViewer = this.AssociatedObject;
      if (this._scrollViewer == null || this._scrollViewer == null)
        return;
      this._scrollViewer.IsMouseDirectlyOverChanged += new DependencyPropertyChangedEventHandler(this._scrollViewer_IsMouseDirectlyOverChanged);
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      if (this._scrollViewer == null)
        return;
      this._scrollViewer.IsMouseDirectlyOverChanged -= new DependencyPropertyChangedEventHandler(this._scrollViewer_IsMouseDirectlyOverChanged);
    }

    private void _scrollViewer_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      Debug.WriteLine(e.NewValue.ToString());
    }
  }
}
