using System.Windows;

namespace FlattyTweet.Extensions
{
  public static class AttachedEvents
  {
    public static readonly RoutedEvent PreviewMouseEnterEvent = EventManager.RegisterRoutedEvent("PreviewMouseEnter", RoutingStrategy.Tunnel, typeof (RoutedEventHandler), typeof (AttachedEvents));

    static AttachedEvents()
    {
    }

    internal static RoutedEventArgs RaisePreviewMouseEnterEvent(DependencyObject target)
    {
      if (target == null)
        return (RoutedEventArgs) null;
      RoutedEventArgs e = new RoutedEventArgs();
      e.RoutedEvent = AttachedEvents.PreviewMouseEnterEvent;
      if (target is UIElement)
        (target as UIElement).RaiseEvent(e);
      else if (target is ContentElement)
        (target as ContentElement).RaiseEvent(e);
      return e;
    }
  }
}
