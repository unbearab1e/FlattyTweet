
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace FlattyTweet.Extensions
{
  public class IntellisenseExtension
  {
    public static readonly DependencyProperty IsEnabledProperty;
    public static readonly DependencyProperty TwitterAccountIDProperty;
    public static readonly DependencyProperty IgnorePrefixProperty;
    public static readonly DependencyProperty ExcludeMentionsProperty;

    static IntellisenseExtension()
    {
      string name1 = "IsEnabled";
      Type propertyType1 = typeof (bool);
      Type ownerType1 = typeof (IntellisenseExtension);
      FrameworkPropertyMetadata propertyMetadata1 = new FrameworkPropertyMetadata();
      propertyMetadata1.BindsTwoWayByDefault = true;
      propertyMetadata1.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        if (local_0.IsLoaded)
        {
          IntellisenseAdorner local_1 = IntellisenseExtension.GetIntellisenseAdorner(local_0);
          if (local_1 == null)
            return;
          if ((bool) e.NewValue)
            local_1.Enable();
          else
            local_1.Disable();
        }
        else
          local_0.Loaded += new RoutedEventHandler(IntellisenseExtension.richTextBox_Loaded);
      });
      FrameworkPropertyMetadata propertyMetadata2 = propertyMetadata1;
      IntellisenseExtension.IsEnabledProperty = DependencyProperty.RegisterAttached(name1, propertyType1, ownerType1, (PropertyMetadata) propertyMetadata2);
      string name2 = "TwitterAccountID";
      Type propertyType2 = typeof (Decimal);
      Type ownerType2 = typeof (IntellisenseExtension);
      FrameworkPropertyMetadata propertyMetadata3 = new FrameworkPropertyMetadata();
      propertyMetadata3.BindsTwoWayByDefault = true;
      propertyMetadata3.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        if (local_0.IsLoaded)
          IntellisenseExtension.GetIntellisenseAdorner(local_0).TwitterAccountID = (Decimal) e.NewValue;
        else
          local_0.Loaded += new RoutedEventHandler(IntellisenseExtension.richTextBox_Loaded);
      });
      FrameworkPropertyMetadata propertyMetadata4 = propertyMetadata3;
      IntellisenseExtension.TwitterAccountIDProperty = DependencyProperty.RegisterAttached(name2, propertyType2, ownerType2, (PropertyMetadata) propertyMetadata4);
      string name3 = "IgnorePrefix";
      Type propertyType3 = typeof (bool);
      Type ownerType3 = typeof (IntellisenseExtension);
      FrameworkPropertyMetadata propertyMetadata5 = new FrameworkPropertyMetadata();
      propertyMetadata5.BindsTwoWayByDefault = true;
      propertyMetadata5.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        IntellisenseAdorner local_1 = IntellisenseExtension.GetIntellisenseAdorner(local_0);
        if (!local_0.IsLoaded)
          return;
        local_1.IgnorePrefix = (bool) e.NewValue;
      });
      FrameworkPropertyMetadata propertyMetadata6 = propertyMetadata5;
      IntellisenseExtension.IgnorePrefixProperty = DependencyProperty.RegisterAttached(name3, propertyType3, ownerType3, (PropertyMetadata) propertyMetadata6);
      string name4 = "ExcludeMentions";
      Type propertyType4 = typeof (bool);
      Type ownerType4 = typeof (IntellisenseExtension);
      FrameworkPropertyMetadata propertyMetadata7 = new FrameworkPropertyMetadata();
      propertyMetadata7.BindsTwoWayByDefault = true;
      propertyMetadata7.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        RichTextBox local_0 = (RichTextBox) obj;
        IntellisenseAdorner local_1 = IntellisenseExtension.GetIntellisenseAdorner(local_0);
        if (!local_0.IsLoaded)
          return;
        local_1.ExcludeMentions = (bool) e.NewValue;
      });
      FrameworkPropertyMetadata propertyMetadata8 = propertyMetadata7;
      IntellisenseExtension.ExcludeMentionsProperty = DependencyProperty.RegisterAttached(name4, propertyType4, ownerType4, (PropertyMetadata) propertyMetadata8);
    }

    public static bool GetIsEnabled(RichTextBox richTextBox)
    {
      if (richTextBox == null)
        throw new ArgumentNullException("richTextBox");
      else
        return (bool) richTextBox.GetValue(IntellisenseExtension.IsEnabledProperty);
    }

    public static void SetIsEnabled(RichTextBox richTextBox, bool value)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException("richTextBox");
        }
        richTextBox.SetValue(IsEnabledProperty, value);
    }

    public static Decimal GetTwitterAccountID(TextBoxBase textBoxBase)
    {
      if (textBoxBase == null)
        throw new ArgumentNullException("textBoxBase");
      else
        return (Decimal) textBoxBase.GetValue(IntellisenseExtension.TwitterAccountIDProperty);
    }

    public static void SetTwitterAccountID(TextBoxBase textBoxBase, Decimal value)
    {
      if (textBoxBase == null)
        throw new ArgumentNullException("textBoxBase");
      textBoxBase.SetValue(IntellisenseExtension.TwitterAccountIDProperty, (object) value);
    }

    public static bool GetIgnorePrefix(RichTextBox richTextBox)
    {
      if (richTextBox == null)
        throw new ArgumentNullException("richTextBox");
      else
        return (bool) richTextBox.GetValue(IntellisenseExtension.IgnorePrefixProperty);
    }

    public static void SetIgnorePrefix(RichTextBox richTextBox, bool value)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException("richTextBox");
        }
        richTextBox.SetValue(IgnorePrefixProperty, value);
    }

    public static bool GetExcludeMentions(RichTextBox richTextBox)
    {
      if (richTextBox == null)
        throw new ArgumentNullException("richTextBox");
      else
        return (bool) richTextBox.GetValue(IntellisenseExtension.ExcludeMentionsProperty);
    }

    public static void SetExcludeMentions(RichTextBox richTextBox, bool value)
    {
        if (richTextBox == null)
        {
            throw new ArgumentNullException("richTextBox");
        }
        richTextBox.SetValue(ExcludeMentionsProperty, value);
    }

    private static void richTextBox_Loaded(object sender, RoutedEventArgs e)
    {
      IntellisenseExtension.InitialiseIntellisenseAdorner(sender as RichTextBox);
    }

    private static void InitialiseIntellisenseAdorner(RichTextBox richTextBox)
    {
      IntellisenseAdorner intellisenseAdorner = IntellisenseExtension.GetIntellisenseAdorner(richTextBox);
      if (intellisenseAdorner != null)
      {
        intellisenseAdorner.IgnorePrefix = IntellisenseExtension.GetIgnorePrefix(richTextBox);
        intellisenseAdorner.ExcludeMentions = IntellisenseExtension.GetExcludeMentions(richTextBox);
        intellisenseAdorner.TwitterAccountID = IntellisenseExtension.GetTwitterAccountID((TextBoxBase) richTextBox);
        intellisenseAdorner.Enable();
        richTextBox.Loaded -= new RoutedEventHandler(IntellisenseExtension.richTextBox_Loaded);
      }
      else
      {
        if (intellisenseAdorner != null || richTextBox.IsVisible)
          return;
        richTextBox.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IntellisenseExtension.richTextBox_IsVisibleChanged);
      }
    }

    private static void richTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      RichTextBox richTextBox = sender as RichTextBox;
      richTextBox.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(IntellisenseExtension.richTextBox_IsVisibleChanged);
      IntellisenseExtension.InitialiseIntellisenseAdorner(richTextBox);
    }

    private static IntellisenseAdorner GetIntellisenseAdorner(RichTextBox richTextBox)
    {
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) richTextBox);
      if (adornerLayer == null)
        return (IntellisenseAdorner) null;
      Adorner[] adorners = adornerLayer.GetAdorners((UIElement) richTextBox);
      if (adorners != null)
      {
        foreach (Adorner adorner in adorners)
        {
          if (adorner is IntellisenseAdorner)
            return adorner as IntellisenseAdorner;
        }
      }
      IntellisenseAdorner intellisenseAdorner = new IntellisenseAdorner(richTextBox);
      adornerLayer.Add((Adorner) intellisenseAdorner);
      return intellisenseAdorner;
    }
  }
}
