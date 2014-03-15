// Type: MetroTwit.Bitly.BitlyUISettings
// Assembly: MetroTwit.Bitly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B2DBF1B-8845-4660-8620-D7CA34A41F2D
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bitly.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace MetroTwit.Bitly
{
  public partial class BitlyUISettings : UserControl, IComponentConnector
  {
    internal RadioButton anonymousAccountRadioButton;
    internal RadioButton personalAccountRadioButton;
    internal Label usernameLabel;
    internal TextBox usernameTextBox;
    internal Label apiKeyLabel;
    internal TextBox apiKeyTextBox;
    private bool _contentLoaded;

    public BitlyUISettings(BitlySettings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) settings;
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      Process.Start(new ProcessStartInfo(Uri.UnescapeDataString("http://bitly.com/a/your_api_key"))
      {
        UseShellExecute = true
      });
    }

    private void personalAccountRadioButton_Checked(object sender, RoutedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Delegate) (() => this.usernameTextBox.Focus()), DispatcherPriority.DataBind, new object[0]);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MetroTwit.Bitly;component/bitlyuisettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.anonymousAccountRadioButton = (RadioButton) target;
          break;
        case 2:
          this.personalAccountRadioButton = (RadioButton) target;
          this.personalAccountRadioButton.Checked += new RoutedEventHandler(this.personalAccountRadioButton_Checked);
          break;
        case 3:
          ((UIElement) target).PreviewMouseDown += new MouseButtonEventHandler(this.TextBlock_MouseDown);
          break;
        case 4:
          this.usernameLabel = (Label) target;
          break;
        case 5:
          this.usernameTextBox = (TextBox) target;
          break;
        case 6:
          this.apiKeyLabel = (Label) target;
          break;
        case 7:
          this.apiKeyTextBox = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
