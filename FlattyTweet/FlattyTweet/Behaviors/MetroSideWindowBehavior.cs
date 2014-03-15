
using FlattyTweet.Extensions;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  public class MetroSideWindowBehavior : Behavior<Window>
  {
    private double dpiXfactor;
    private double dpiYfactor;

    private void InitializeDPI()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      this.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      this.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this.InitializeDPI();
      this.AssociatedObject.Owner = Application.Current.MainWindow;
      Window associatedObject1 = this.AssociatedObject;
      Rectangle workingArea;
      double num1;
      if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = CommonCommands.CurrentScreen().WorkingArea;
        num1 = (double) workingArea.Height / this.dpiYfactor;
      }
      else
        num1 = Application.Current.MainWindow.Height;
      associatedObject1.Height = num1;
      this.AssociatedObject.Width = 600.0;
      Window associatedObject2 = this.AssociatedObject;
      double num2;
      if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = CommonCommands.CurrentScreen().WorkingArea;
        num2 = (double) workingArea.Y / this.dpiYfactor;
      }
      else
        num2 = Application.Current.MainWindow.Top;
      associatedObject2.Top = num2;
      Window associatedObject3 = this.AssociatedObject;
      double num3;
      if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
      {
        workingArea = CommonCommands.CurrentScreen().WorkingArea;
        num3 = (double) workingArea.X / this.dpiYfactor;
      }
      else
        num3 = Application.Current.MainWindow.Left;
      double num4 = (Application.Current.MainWindow.Width - this.AssociatedObject.Width) / 2.0;
      double num5 = num3 + num4;
      associatedObject3.Left = num5;
      this.AssociatedObject.Closing += new CancelEventHandler(this.AssociatedObject_Closing);
    }

    private void AssociatedObject_Closing(object sender, CancelEventArgs e)
    {
      if (this.AssociatedObject.Owner == null)
        return;
      this.AssociatedObject.Owner.Activate();
      this.AssociatedObject.Owner.Focus();
    }
  }
}
