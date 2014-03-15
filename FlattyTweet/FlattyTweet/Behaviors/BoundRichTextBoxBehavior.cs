
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
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
