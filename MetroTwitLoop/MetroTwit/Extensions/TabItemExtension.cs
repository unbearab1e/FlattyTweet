// Type: MetroTwit.Extensions.TabItemExtension
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Windows;

namespace MetroTwit.Extensions
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
