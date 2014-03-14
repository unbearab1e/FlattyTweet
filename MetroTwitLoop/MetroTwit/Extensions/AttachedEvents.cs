// Type: MetroTwit.Extensions.AttachedEvents
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Windows;

namespace MetroTwit.Extensions
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
