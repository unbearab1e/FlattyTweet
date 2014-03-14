// Type: MetroTwit.View.MediaPreviewView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using MetroTwit.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MetroTwit.View
{
    public partial class MediaPreviewView : InlinePopup, IComponentConnector
  {
    private double dpiXfactor;
    private double dpiYfactor;
   

    public MediaPreviewView(MediaPreviewViewModel VM)
    {
      this.InitializeComponent();
      this.InitializeDPI();
      this.DataContext = (object) VM;
      if (VM.Closed)
      {
        this.Close();
      }
      else
      {
        VM.PropertyChanged += new PropertyChangedEventHandler(this.VM_PropertyChanged);
        this.Unloaded += new RoutedEventHandler(this.MediaPreviewView_Unloaded);
        this.MaxWidth = (double) CommonCommands.CurrentScreen().WorkingArea.Width / this.dpiXfactor;
        this.MaxHeight = (double) CommonCommands.CurrentScreen().WorkingArea.Height / this.dpiYfactor;
      }
    }

    private void InitializeDPI()
    {
      PresentationSource presentationSource = CommonCommands.CurrentSource();
      this.dpiXfactor = presentationSource.CompositionTarget.TransformToDevice.M11;
      this.dpiYfactor = presentationSource.CompositionTarget.TransformToDevice.M22;
    }

    private void MediaPreviewView_Unloaded(object sender, RoutedEventArgs e)
    {
      (this.DataContext as MediaPreviewViewModel).Closed = true;
    }

    private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Closed" && (this.DataContext as MediaPreviewViewModel).Closed)
      {
        try
        {
          this.Close();
        }
        catch
        {
        }
      }
      if (!(e.PropertyName == "HTML"))
        return;
      try
      {
        WebBrowserView webBrowserView = new WebBrowserView((FrameworkElement) this.ControlHost, (this.DataContext as MediaPreviewViewModel).HTML, (this.DataContext as MediaPreviewViewModel).MediaWidth, (this.DataContext as MediaPreviewViewModel).MediaHeight);
      }
      catch
      {
        this.Close();
      }
    }

    
  }
}
