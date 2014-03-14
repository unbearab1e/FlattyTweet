// Type: MetroTwit.HostView.CustomAddIn
// Assembly: MetroTwit.HostView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9BD2E35C-5E7E-45A1-87D3-770896774B1E
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.HostView.dll

using System.Windows;

namespace MetroTwit.HostView
{
  public abstract class CustomAddIn
  {
    public abstract FrameworkElement GetUI(double width, double height);

    public abstract void ClearBrowser();

    public abstract void Navigate(string URL);
  }
}
