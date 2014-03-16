using System.Windows;
using System.Windows.Controls;

namespace AnimationScrollViewerOffset
{
  public class ScrollViewerUtilities
  {
    public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof (double), typeof (ScrollViewerUtilities), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, new PropertyChangedCallback(ScrollViewerUtilities.OnHorizontalOffsetChanged)));
    public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof (double), typeof (ScrollViewerUtilities), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, new PropertyChangedCallback(ScrollViewerUtilities.OnVerticalOffsetChanged)));

    static ScrollViewerUtilities()
    {
    }

    public static double GetHorizontalOffset(DependencyObject d)
    {
      return (double) d.GetValue(ScrollViewerUtilities.HorizontalOffsetProperty);
    }

    public static void SetHorizontalOffset(DependencyObject d, double value)
    {
      d.SetValue(ScrollViewerUtilities.HorizontalOffsetProperty, (object) value);
    }

    private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ScrollViewer) d).ScrollToHorizontalOffset((double) e.NewValue);
    }

    public static double GetVerticalOffset(DependencyObject d)
    {
      return (double) d.GetValue(ScrollViewerUtilities.VerticalOffsetProperty);
    }

    public static void SetVerticalOffset(DependencyObject d, double value)
    {
      d.SetValue(ScrollViewerUtilities.VerticalOffsetProperty, (object) value);
    }

    private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ScrollViewer) d).ScrollToVerticalOffset((double) e.NewValue);
    }
  }
}
