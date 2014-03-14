// Type: MetroTwit.IEAddIn.IEAddInCode
// Assembly: MetroTwit.IEAddIn, Version=1.0.5168.42598, Culture=neutral, PublicKeyToken=null
// MVID: A02BE231-FC8A-4301-9B33-194F130CD844
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\AddIns\IeAddIn\MetroTwit.IEAddIn.dll

using MetroTwit.CustomAddInView;
using System.AddIn;
using System.Windows;

namespace MetroTwit.IEAddIn
{
  [AddIn("IEAddIn")]
  public class IEAddInCode : CustomAddIn
  {
    private WebBrowserControl a;

    public IEAddInCode()
    {
    }

    public override FrameworkElement GetUI(double width, double height)
    {
      if (this.a == null)
      {
        this.a = new WebBrowserControl();
        this.a.Width = width;
        this.a.Height = height;
      }
      return (FrameworkElement) this.a;
    }

    public override void ClearBrowser()
    {
      this.a.browser.Navigate("about:blank");
    }

    public override void Navigate(string URL)
    {
      this.a.browser.NavigateToString(URL);
    }
  }
}
