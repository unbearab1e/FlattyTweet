
using FlattyTweet.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace FlattyTweet.View
{
    public partial class ManageAccountView : UserControl, IComponentConnector
  {
    private AdornerDragDrop adorner = (AdornerDragDrop) null;
    private ListBox dragSource = (ListBox) null;
    private int oldindex;
    private int newindex;
   

    public ManageAccountView()
    {
      this.InitializeComponent();
    }

    private void MouseDownHandler(object sender, MouseButtonEventArgs e)
    {
      ListBox source = (ListBox) sender;
      this.dragSource = source;
      UIElement dataContainer;
      object objectDataFromPoint = ManageAccountView.GetObjectDataFromPoint(source, e.GetPosition((IInputElement) source), out dataContainer);
      this.oldindex = source.Items.IndexOf(objectDataFromPoint);
      if (objectDataFromPoint == null)
        return;
      this.RemoveAdorner();
      this.adorner = new AdornerDragDrop((UIElement) this.LayoutDock, dataContainer);
      AdornerLayer.GetAdornerLayer((Visual) this.LayoutDock).Add((Adorner) this.adorner);
      int num = (int) DragDrop.DoDragDrop((DependencyObject) source, objectDataFromPoint, DragDropEffects.Move);
    }

    private void SourceDrop(object sender, DragEventArgs e)
    {
      ListBox source;
      if (sender is TextBlock)
      {
        source = this.AccountList;
        this.newindex = 0;
      }
      else
      {
        source = (ListBox) sender;
        e.Data.GetData(typeof (string));
        UIElement dataContainer;
        object objectDataFromPoint = ManageAccountView.GetObjectDataFromPoint(source, e.GetPosition((IInputElement) source), out dataContainer);
        this.newindex = source.Items.IndexOf(objectDataFromPoint);
      }
      if (this.newindex == -1)
        this.newindex = source.Items.Count - 1;
      (source.ItemsSource as MTAccountCollection).Move(this.oldindex, this.newindex);
      this.RemoveAdorner();
    }

    private void DragOverHandler(object sender, DragEventArgs e)
    {
      this.adorner.UpdatePosition(e.GetPosition((IInputElement) this.LayoutDock));
    }

    private static object GetObjectDataFromPoint(ListBox source, Point point, out UIElement dataContainer)
    {
      dataContainer = (UIElement) null;
      UIElement uiElement = source.InputHitTest(point) as UIElement;
      if (uiElement != null)
      {
        object obj = DependencyProperty.UnsetValue;
        while (obj == DependencyProperty.UnsetValue && obj != null)
        {
          if (source.ItemContainerGenerator == null)
            return (object) null;
          obj = source.ItemContainerGenerator.ItemFromContainer((DependencyObject) uiElement);
          if (obj == DependencyProperty.UnsetValue || obj == null)
            uiElement = VisualTreeHelper.GetParent((DependencyObject) uiElement) as UIElement;
          if (uiElement == source)
            return (object) null;
        }
        if (obj != DependencyProperty.UnsetValue)
        {
          dataContainer = uiElement;
          return obj;
        }
      }
      return (object) null;
    }

    private void RemoveAdorner()
    {
      if (this.adorner == null)
        return;
      AdornerLayer.GetAdornerLayer((Visual) this.LayoutDock).Remove((Adorner) this.adorner);
    }

    
  }
}
