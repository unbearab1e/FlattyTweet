using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FlattyTweet.Extensions
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
