
using System;
using System.Windows;

namespace FlattyTweet.Extensions
{
  public class TabItemExtension
  {
    public static RoutedEvent AnimateHeaderEvent = EventManager.RegisterRoutedEvent("AnimateHeader", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (TabItemExtension));

    static TabItemExtension()
    {
    }

    public static void AddAnimateHeaderHandler(DependencyObject o, RoutedEventHandler handler)
    {
      ((UIElement) o).AddHandler(TabItemExtension.AnimateHeaderEvent, (Delegate) handler);
    }

    public static void RemoveAnimateHeaderHandler(DependencyObject o, RoutedEventHandler handler)
    {
      ((UIElement) o).RemoveHandler(TabItemExtension.AnimateHeaderEvent, (Delegate) handler);
    }
  }
}
