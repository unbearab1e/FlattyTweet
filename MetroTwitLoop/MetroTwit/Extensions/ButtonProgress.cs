// Type: MetroTwit.Extensions.ButtonProgress
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MetroTwit.Extensions
{
  public class ButtonProgress : Button, INotifyPropertyChanged
  {
    private double currentProgressPercentage;
    private double minimumValue;
    private Rectangle progressFillRect;
    private Rectangle progressBorderRect;
    private Button cancelButton;
    private string previousSetText;

    public double ProgressPercentage
    {
      get
      {
        return this.currentProgressPercentage;
      }
      set
      {
        if (this.currentProgressPercentage == value || (value < 0.0 || value > 100.0))
          return;
        this.currentProgressPercentage = value;
        this.OnPropertyChanged(new PropertyChangedEventArgs("ProgressPercentage"));
        if (this.previousSetText == string.Empty)
          this.previousSetText = this.Content.ToString();
        this.SetProgressText();
        this.SetProgressRectangleWidth();
        this.OnPropertyChanged(new PropertyChangedEventArgs("ProgressPercentage"));
      }
    }

    public event EventHandler Canceled;

    public event PropertyChangedEventHandler PropertyChanged;

    static ButtonProgress()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ButtonProgress), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ButtonProgress)));
    }

    public ButtonProgress()
    {
      this.previousSetText = string.Empty;
      this.currentProgressPercentage = 0.0;
      this.SetProgressText();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.cancelButton = this.GetTemplateChild("PART_CancelButton") as Button;
      if (this.cancelButton != null)
        this.cancelButton.Click += (RoutedEventHandler) ((param0, param1) =>
        {
          if (this.Canceled == null)
            return;
          this.Canceled((object) this, EventArgs.Empty);
        });
      this.progressBorderRect = this.GetTemplateChild("PART_ProgressBorderRect") as Rectangle;
      this.progressFillRect = this.GetTemplateChild("PART_ProgressFillRect") as Rectangle;
      this.minimumValue = this.progressFillRect.MinWidth;
      this.progressFillRect.MinWidth = 0.0;
      this.progressFillRect.Width = 0.0;
    }

    public void ResetContentToInitial()
    {
      this.Content = (object) this.previousSetText;
      this.previousSetText = string.Empty;
    }

    private void SetProgressText()
    {
      this.Content = (object) string.Format("{0}%", (object) (int) this.currentProgressPercentage);
    }

    private void SetProgressRectangleWidth()
    {
      if (this.progressFillRect == null)
        return;
      this.progressFillRect.HorizontalAlignment = HorizontalAlignment.Left;
      this.progressFillRect.MaxWidth = this.progressBorderRect.ActualWidth;
      if (this.currentProgressPercentage == 1.0)
      {
        this.progressFillRect.MinWidth = 0.0;
        this.progressFillRect.Width = this.minimumValue;
      }
      else if (this.currentProgressPercentage == 0.0)
        this.progressFillRect.Width = 0.0;
      else if (this.currentProgressPercentage == 100.0)
      {
        this.progressFillRect.MaxWidth = double.PositiveInfinity;
        this.progressFillRect.HorizontalAlignment = HorizontalAlignment.Stretch;
        this.progressFillRect.Width = double.NaN;
      }
      else if (this.progressBorderRect.ActualWidth * 0.01 * this.currentProgressPercentage > this.minimumValue)
        this.progressFillRect.Width = this.progressBorderRect.ActualWidth * 0.01 * this.currentProgressPercentage;
      else
        this.progressFillRect.Width = this.minimumValue;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
      base.OnRenderSizeChanged(sizeInfo);
      this.SetProgressRectangleWidth();
      this.progressFillRect.MaxWidth = this.progressBorderRect.ActualWidth;
    }

    protected void OnPropertyChanged(PropertyChangedEventArgs args)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, args);
    }
  }
}
