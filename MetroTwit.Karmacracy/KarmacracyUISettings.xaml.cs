// Type: MetroTwit.Karmacracy.KarmacracyUISettings
// Assembly: MetroTwit.Karmacracy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D8758AA4-2444-4BFE-8204-2AC009585A92
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Karmacracy.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace MetroTwit.Karmacracy
{
  public partial class KarmacracyUISettings : UserControl, IComponentConnector
  {
    internal Label usernameLabel;
    internal TextBox usernameTextBox;
    internal Label apiKeyLabel;
    internal TextBox apiKeyTextBox;
    private bool _contentLoaded;

    public KarmacracyUISettings(KarmacracySettings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) settings;
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Process.Start(new ProcessStartInfo(Uri.UnescapeDataString("http://karmacracy.com/settings?t=connections"))
      {
        UseShellExecute = true
      });
    }

    private void personalAccountRadioButton_Checked(object sender, RoutedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Delegate) (() => this.usernameTextBox.Focus()), DispatcherPriority.DataBind, new object[0]);
    }

    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MetroTwit.Karmacracy;component/karmacracyuisettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.usernameLabel = (Label) target;
          break;
        case 2:
          this.usernameTextBox = (TextBox) target;
          break;
        case 3:
          ((UIElement) target).PreviewMouseDown += new MouseButtonEventHandler(this.TextBlock_MouseDown);
          break;
        case 4:
          this.apiKeyLabel = (Label) target;
          break;
        case 5:
          this.apiKeyTextBox = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
