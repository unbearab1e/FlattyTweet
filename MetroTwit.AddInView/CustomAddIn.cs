// Type: MetroTwit.CustomAddInView.CustomAddIn
// Assembly: MetroTwit.AddInView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17DBB7BE-5B0A-4BC3-A52B-02D67328190E
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\AddInViews\MetroTwit.AddInView.dll

using System.AddIn.Pipeline;
using System.Windows;

namespace MetroTwit.CustomAddInView
{
  [AddInBase]
  public abstract class CustomAddIn
  {
    public abstract FrameworkElement GetUI(double width, double height);

    public abstract void ClearBrowser();

    public abstract void Navigate(string URL);
  }
}
