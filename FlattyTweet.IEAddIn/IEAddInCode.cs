
using FlattyTweet.CustomAddInView;
using System.AddIn;
using System.Windows;

namespace FlattyTweet.IEAddIn
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
