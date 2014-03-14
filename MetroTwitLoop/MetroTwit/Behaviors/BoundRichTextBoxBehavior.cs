// Type: MetroTwit.Behaviors.BoundRichTextBoxBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace MetroTwit.Behaviors
{
  internal class BoundRichTextBoxBehavior : Behavior<RichTextBox>
  {
    private bool TextChanging = false;
    public static readonly DependencyProperty BindingProperty;

    public object Binding
    {
      get
      {
        return this.GetValue(BoundRichTextBoxBehavior.BindingProperty);
      }
      set
      {
        this.SetValue(BoundRichTextBoxBehavior.BindingProperty, value);
      }
    }

    static BoundRichTextBoxBehavior()
    {
      string name = "Binding";
      Type propertyType = typeof (object);
      Type ownerType = typeof (BoundRichTextBoxBehavior);
      FrameworkPropertyMetadata propertyMetadata1 = new FrameworkPropertyMetadata();
      propertyMetadata1.BindsTwoWayByDefault = true;
      propertyMetadata1.PropertyChangedCallback = (PropertyChangedCallback) ((s, e) =>
      {
        BoundRichTextBoxBehavior local_0 = s as BoundRichTextBoxBehavior;
        if (local_0.AssociatedObject == null || !local_0.AssociatedObject.IsLoaded)
          return;
        local_0.UpdateRTB();
      });
      FrameworkPropertyMetadata propertyMetadata2 = propertyMetadata1;
      BoundRichTextBoxBehavior.BindingProperty = DependencyProperty.RegisterAttached(name, propertyType, ownerType, (PropertyMetadata) propertyMetadata2);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      if (this.AssociatedObject.IsLoaded)
        this.richTextBox_Loaded((object) this.AssociatedObject, new RoutedEventArgs());
      else
        this.AssociatedObject.Loaded += new RoutedEventHandler(this.richTextBox_Loaded);
      this.AssociatedObject.LostFocus += new RoutedEventHandler(this.AssociatedObject_LostFocus);
    }

    private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
    {
      if (this.TextChanging)
        return;
      this.Binding = (object) RTBExtensions.GetInlineText(this.AssociatedObject);
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.LostFocus -= new RoutedEventHandler(this.AssociatedObject_LostFocus);
      this.AssociatedObject.Loaded -= new RoutedEventHandler(this.richTextBox_Loaded);
      base.OnDetaching();
    }

    private void richTextBox_Loaded(object sender, RoutedEventArgs e)
    {
      this.UpdateRTB();
      if (!(this.AssociatedObject.DataContext is UserAccountViewModel))
        return;
      IntellisenseExtension.SetIsEnabled(this.AssociatedObject, SettingsData.Instance.UseAutoComplete);
    }

    private void UpdateRTB()
    {
      if (!(this.Binding is string))
        return;
      this.TextChanging = true;
      this.AssociatedObject.Document.Blocks.Clear();
      this.AssociatedObject.Document.Blocks.Add((Block) new Paragraph((Inline) new Run(this.Binding.ToString())));
      this.TextChanging = false;
    }
  }
}
