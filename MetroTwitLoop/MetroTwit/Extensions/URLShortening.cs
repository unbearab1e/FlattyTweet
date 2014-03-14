// Type: MetroTwit.Extensions.URLShortening
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace MetroTwit.Extensions
{
  public class URLShortening
  {
    public static readonly DependencyProperty IsEnabledProperty;
    public static readonly DependencyProperty TwitterAccountIDProperty;

    static URLShortening()
    {
      string name1 = "IsEnabled";
      Type propertyType1 = typeof (bool);
      Type ownerType1 = typeof (URLShortening);
      FrameworkPropertyMetadata propertyMetadata1 = new FrameworkPropertyMetadata();
      propertyMetadata1.BindsTwoWayByDefault = true;
      propertyMetadata1.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        if (local_0.IsLoaded)
        {
          URLAdorner local_1 = URLShortening.GetURLShorteningAdorner(local_0);
          if (local_1 == null)
            return;
          if ((bool) e.NewValue)
            local_1.Enable();
          else
            local_1.Disable();
        }
        else
          local_0.Loaded += new RoutedEventHandler(URLShortening.richTextBox_Loaded);
      });
      FrameworkPropertyMetadata propertyMetadata2 = propertyMetadata1;
      URLShortening.IsEnabledProperty = DependencyProperty.RegisterAttached(name1, propertyType1, ownerType1, (PropertyMetadata) propertyMetadata2);
      string name2 = "TwitterAccountID";
      Type propertyType2 = typeof (Decimal);
      Type ownerType2 = typeof (URLShortening);
      FrameworkPropertyMetadata propertyMetadata3 = new FrameworkPropertyMetadata();
      propertyMetadata3.BindsTwoWayByDefault = true;
      propertyMetadata3.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        if (local_0.IsLoaded)
          URLShortening.GetURLShorteningAdorner(local_0).TwitterAccountID = (Decimal) e.NewValue;
        else
          local_0.Loaded += new RoutedEventHandler(URLShortening.richTextBox_Loaded);
      });
      FrameworkPropertyMetadata propertyMetadata4 = propertyMetadata3;
      URLShortening.TwitterAccountIDProperty = DependencyProperty.RegisterAttached(name2, propertyType2, ownerType2, (PropertyMetadata) propertyMetadata4);
    }

    public static bool GetIsEnabled(TextBoxBase textBoxBase)
    {
      if (textBoxBase == null)
        throw new ArgumentNullException("textBoxBase");
      else
        return (bool) textBoxBase.GetValue(URLShortening.IsEnabledProperty);
    }

    public static void SetIsEnabled(TextBoxBase textBoxBase, bool value)
    {
        if (textBoxBase == null)
        {
            throw new ArgumentNullException("textBoxBase");
        }
        textBoxBase.SetValue(IsEnabledProperty, value);
    }

    public static Decimal GetTwitterAccountID(TextBoxBase textBoxBase)
    {
      if (textBoxBase == null)
        throw new ArgumentNullException("textBoxBase");
      else
        return (Decimal) textBoxBase.GetValue(URLShortening.TwitterAccountIDProperty);
    }

    public static void SetTwitterAccountID(TextBoxBase textBoxBase, Decimal value)
    {
      if (textBoxBase == null)
        throw new ArgumentNullException("textBoxBase");
      textBoxBase.SetValue(URLShortening.TwitterAccountIDProperty, (object) value);
    }

    private static void richTextBox_Loaded(object sender, RoutedEventArgs e)
    {
      RichTextBox richTextBox = sender as RichTextBox;
      URLShortening.InitialiseURLShorteningAdorner(richTextBox);
      richTextBox.Loaded -= new RoutedEventHandler(URLShortening.richTextBox_Loaded);
    }

    private static void InitialiseURLShorteningAdorner(RichTextBox richTextBox)
    {
      URLAdorner shorteningAdorner = URLShortening.GetURLShorteningAdorner(richTextBox);
      if (shorteningAdorner != null)
      {
        if (URLShortening.GetIsEnabled((TextBoxBase) richTextBox))
          shorteningAdorner.Enable();
        else
          shorteningAdorner.Disable();
        shorteningAdorner.TwitterAccountID = URLShortening.GetTwitterAccountID((TextBoxBase) richTextBox);
      }
      else
      {
        if (shorteningAdorner != null || richTextBox.IsVisible)
          return;
        richTextBox.IsVisibleChanged += new DependencyPropertyChangedEventHandler(URLShortening.richTextBox_IsVisibleChanged);
      }
    }

    private static void richTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      RichTextBox richTextBox = sender as RichTextBox;
      richTextBox.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(URLShortening.richTextBox_IsVisibleChanged);
      URLShortening.InitialiseURLShorteningAdorner(richTextBox);
    }

    private static URLAdorner GetURLShorteningAdorner(RichTextBox richTextBox)
    {
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) richTextBox);
      if (adornerLayer == null)
        return (URLAdorner) null;
      Adorner[] adorners = adornerLayer.GetAdorners((UIElement) richTextBox);
      if (adorners != null)
      {
        foreach (Adorner adorner in adorners)
        {
          if (adorner is URLAdorner)
            return adorner as URLAdorner;
        }
      }
      URLAdorner urlAdorner = new URLAdorner((UIElement) richTextBox);
      adornerLayer.Add((Adorner) urlAdorner);
      return urlAdorner;
    }
  }
}
