
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace FlattyTweet.Extensions
{
  [TemplatePart(Name = "PART_MainContainer", Type = typeof (DockPanel))]
  public class InlinePopup : Window
  {
    private double dpiXfactor;
    private double dpiYfactor;
    private DockPanel mainContainer;
    private System.Windows.Point relativeToTargetPoint;
    private double screenTop;
    private double screenLeft;
    private double screenWidth;
    private double screenHeight;
    private bool fitTop;
    private bool fitLeft;
    private bool fitRight;
    private bool fitBottom;

    public static InlinePopup CurrentInline { get; set; }

    private Action UpdatePosition { get; set; }

    static InlinePopup()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (InlinePopup), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (InlinePopup)));
    }

    public InlinePopup()
    {
      this.Closed += new EventHandler(this.InlinePopup_Closed);
      this.InitializeDPI();
      this.WindowStyle = WindowStyle.None;
      this.ResizeMode = ResizeMode.NoResize;
      this.Owner = System.Windows.Application.Current.MainWindow;
      this.ShowInTaskbar = false;
      if (InlinePopup.CurrentInline != null)
        InlinePopup.CurrentInline.Close();
      InlinePopup.CurrentInline = this;
    }

    private void InlinePopup_Closed(object sender, EventArgs e)
    {
      this.Content = (object) null;
      this.Owner.LocationChanged -= new EventHandler(this.Owner_LocationChanged);
      this.Owner.SizeChanged -= new SizeChangedEventHandler(this.Owner_SizeChanged);
      this.SizeChanged -= new SizeChangedEventHandler(this.InlinePopup_SizeChanged);
    }

    private void InitializeDPI()
    {
      PresentationSource presentationSource = PresentationSource.FromVisual((Visual) System.Windows.Application.Current.MainWindow);
      this.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      this.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    private void SetPosition(PlacementMode preferedPlacement, FrameworkElement targetPlacement, System.Windows.Point mousePosition, bool secondSetPositionCall = false)
    {
      Screen screen = this.CurrentScreen();
      this.relativeToTargetPoint = targetPlacement == null ? mousePosition : targetPlacement.PointToScreen(new System.Windows.Point(0.0, 0.0));
      this.screenTop = (double) screen.WorkingArea.Top;
      this.screenLeft = (double) screen.WorkingArea.Left;
      InlinePopup inlinePopup1 = this;
      Rectangle workingArea = screen.WorkingArea;
      double num1 = (double) workingArea.Height;
      inlinePopup1.screenHeight = num1;
      InlinePopup inlinePopup2 = this;
      workingArea = screen.WorkingArea;
      double num2 = (double) workingArea.Width;
      inlinePopup2.screenWidth = num2;
      this.fitLeft = this.relativeToTargetPoint.X - this.ActualWidth * this.dpiXfactor > this.screenLeft;
      this.fitRight = this.ActualWidth * this.dpiXfactor + this.relativeToTargetPoint.X < this.screenLeft + this.screenWidth;
      this.fitTop = this.relativeToTargetPoint.Y - this.ActualHeight * this.dpiYfactor > this.screenTop;
      this.fitBottom = this.ActualHeight * this.dpiYfactor + this.relativeToTargetPoint.Y < this.screenTop + this.screenHeight;
      switch (preferedPlacement)
      {
        case PlacementMode.Bottom:
          if (this.fitBottom)
          {
            this.relativeToTargetPoint.Y += targetPlacement != null ? targetPlacement.ActualHeight * this.dpiYfactor : 0.0;
            this.PositionBottom(this.relativeToTargetPoint, secondSetPositionCall);
          }
          else
          {
            this.relativeToTargetPoint.Y = this.screenTop + this.screenHeight - this.ActualHeight * this.dpiYfactor;
            this.PositionBottom(this.relativeToTargetPoint, secondSetPositionCall);
          }
          if (secondSetPositionCall)
            break;
          this.SetPosition(PlacementMode.Right, targetPlacement, mousePosition, true);
          break;
        case PlacementMode.Right:
          if (this.fitRight)
          {
            this.PositionRight(this.relativeToTargetPoint, secondSetPositionCall);
          }
          else
          {
            this.relativeToTargetPoint.X = this.screenLeft + this.screenWidth - this.ActualWidth * this.dpiXfactor;
            this.PositionRight(this.relativeToTargetPoint, secondSetPositionCall);
          }
          if (secondSetPositionCall)
            break;
          this.SetPosition(PlacementMode.Bottom, targetPlacement, mousePosition, true);
          break;
        case PlacementMode.Left:
          if (this.fitLeft)
          {
            this.PositionLeft(this.relativeToTargetPoint, secondSetPositionCall);
          }
          else
          {
            this.relativeToTargetPoint.X = this.screenLeft + this.ActualWidth * this.dpiXfactor;
            this.PositionLeft(this.relativeToTargetPoint, secondSetPositionCall);
          }
          if (secondSetPositionCall)
            break;
          this.SetPosition(PlacementMode.Bottom, targetPlacement, mousePosition, true);
          break;
        case PlacementMode.Top:
          if (this.fitTop)
          {
            this.PositionTop(this.relativeToTargetPoint, secondSetPositionCall);
          }
          else
          {
            this.relativeToTargetPoint.Y = this.screenTop + this.ActualHeight * this.dpiYfactor;
            this.PositionTop(this.relativeToTargetPoint, secondSetPositionCall);
          }
          if (secondSetPositionCall)
            break;
          this.SetPosition(PlacementMode.Right, targetPlacement, mousePosition, true);
          break;
      }
    }

    private void PositionTop(System.Windows.Point relativePoint, bool ignoreArrowPlacement)
    {
      this.Top = (relativePoint.Y - this.ActualHeight * this.dpiYfactor + this.mainContainer.Margin.Bottom * this.dpiYfactor) / this.dpiYfactor;
    }

    private void PositionBottom(System.Windows.Point relativePoint, bool ignoreArrowPlacement)
    {
      this.Top = (relativePoint.Y - this.mainContainer.Margin.Top * this.dpiYfactor) / this.dpiYfactor;
    }

    private void PositionLeft(System.Windows.Point relativePoint, bool ignoreArrowPlacement)
    {
      this.Left = (relativePoint.X - this.ActualWidth * this.dpiXfactor + this.mainContainer.Margin.Right * this.dpiXfactor) / this.dpiXfactor;
    }

    private void PositionRight(System.Windows.Point relativePoint, bool ignoreArrowPlacement)
    {
      this.Left = (relativePoint.X - this.mainContainer.Margin.Left * this.dpiXfactor) / this.dpiXfactor;
    }

    public Screen CurrentScreen()
    {
      return Screen.FromHandle((PresentationSource.FromVisual((Visual) System.Windows.Application.Current.MainWindow) as HwndSource).Handle);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.mainContainer = this.GetTemplateChild("PART_MainContainer") as DockPanel;
    }

    public void ShowAnimated(PlacementMode preferedPlacement, FrameworkElement targetPlacement, System.Windows.Point? mousePositionPoint = null)
    {
      this.Show();
      if (!mousePositionPoint.HasValue)
        mousePositionPoint = new System.Windows.Point?(System.Windows.Application.Current.MainWindow.PointToScreen(Mouse.GetPosition((IInputElement) System.Windows.Application.Current.MainWindow)));
      this.UpdatePosition = (Action) (() => this.SetPosition(preferedPlacement, targetPlacement, mousePositionPoint.Value, false));
      this.UpdatePosition();
      this.Owner.LocationChanged += new EventHandler(this.Owner_LocationChanged);
      this.Owner.SizeChanged += new SizeChangedEventHandler(this.Owner_SizeChanged);
      this.SizeChanged += new SizeChangedEventHandler(this.InlinePopup_SizeChanged);
    }

    private void InlinePopup_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.UpdatePosition();
    }

    private void Owner_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.UpdatePosition();
    }

    private void Owner_LocationChanged(object sender, EventArgs e)
    {
      this.UpdatePosition();
    }
  }
}
