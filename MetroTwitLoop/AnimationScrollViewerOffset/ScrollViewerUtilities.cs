// Type: AnimationScrollViewerOffset.ScrollViewerUtilities
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

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
