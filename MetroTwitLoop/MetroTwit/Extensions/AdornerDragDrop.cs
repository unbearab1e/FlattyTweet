// Type: MetroTwit.Extensions.AdornerDragDrop
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MetroTwit.Extensions
{
  public class AdornerDragDrop : Adorner
  {
    private UIElement elementToShow = (UIElement) null;
    private Point position;

    protected override int VisualChildrenCount
    {
      get
      {
        return 1;
      }
    }

    public AdornerDragDrop(UIElement element, UIElement elementToShow)
      : base(element)
    {
      this.elementToShow = (UIElement) AdornerDragDrop.CreateClone(elementToShow);
    }

    protected override Size MeasureOverride(Size constraint)
    {
      this.elementToShow.Measure(constraint);
      return constraint;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      this.elementToShow.Arrange(new Rect(finalSize));
      return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
      return (Visual) this.elementToShow;
    }

    public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
    {
      return (GeneralTransform) new GeneralTransformGroup()
      {
        Children = {
          transform,
          (GeneralTransform) new TranslateTransform(30.0, this.position.Y)
        }
      };
    }

    public void UpdatePosition(Point point)
    {
      this.position = point;
      AdornerLayer adornerLayer = this.Parent as AdornerLayer;
      if (adornerLayer == null)
        return;
      adornerLayer.Update(this.AdornedElement);
    }

    private static ContentControl CreateClone(UIElement element)
    {
      ContentControl contentControl1 = new ContentControl();
      ContentControl contentControl2 = element as ContentControl;
      if (contentControl2 != null)
      {
        contentControl1.Content = contentControl2.Content;
        contentControl1.ContentTemplate = contentControl2.ContentTemplate;
      }
      ContentPresenter contentPresenter = element as ContentPresenter;
      if (contentPresenter != null)
      {
        contentControl1.Content = contentPresenter.Content;
        contentControl1.ContentTemplate = contentPresenter.ContentTemplate;
      }
      return contentControl1;
    }
  }
}
