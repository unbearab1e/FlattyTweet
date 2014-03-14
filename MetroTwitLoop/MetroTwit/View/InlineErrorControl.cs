// Type: MetroTwit.View.InlineErrorControl
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace MetroTwit.View
{
    public partial class InlineErrorControl : UserControl, IComponentConnector
  {
    private Storyboard ProgressAnimation;

    public InlineErrorControl()
    {
      this.InitializeComponent();
      this.Visibility = Visibility.Collapsed;
      this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.InlineErrorControl_DataContextChanged);
      this.Loaded += new RoutedEventHandler(this.InlineErrorControl_Loaded);
    }

    private void InlineErrorControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.ProgressAnimation = (Application.Current.FindResource((object) "AlertProgress") as Storyboard).Clone();
      if (this.ProgressAnimation == null)
        return;
      (Enumerable.First<Timeline>((IEnumerable<Timeline>) this.ProgressAnimation.Children) as DoubleAnimation).Duration = (Duration) TimeSpan.FromSeconds(8.0);
      this.ProgressAnimation.Completed += new EventHandler(this.ProgressAnimation_Completed);
      this.ProgressAnimation.Freeze();
    }

    private void ProgressAnimation_Completed(object sender, EventArgs e)
    {
      if (this.ProgressAnimation != null && !this.ProgressAnimation.IsFrozen)
        this.ProgressAnimation.Completed -= new EventHandler(this.ProgressAnimation_Completed);
      this.Alert_Click((object) this, new EventArgs());
    }

    private void InlineErrorControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (this.DataContext != null)
      {
        this.Visibility = Visibility.Visible;
        if (this.ProgressAnimation == null)
          return;
        this.ProgressAnimation.Begin((FrameworkElement) this, HandoffBehavior.Compose, true);
      }
      else
        this.Visibility = Visibility.Collapsed;
    }

    private void Alert_Click(object sender, EventArgs e)
    {
      if (this.ProgressAnimation != null)
        this.ProgressAnimation.Stop();
      this.Visibility = Visibility.Collapsed;
    }

    
  }
}
