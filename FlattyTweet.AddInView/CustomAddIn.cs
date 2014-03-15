
using System.AddIn.Pipeline;
using System.Windows;

namespace FlattyTweet.CustomAddInView
{
  [AddInBase]
  public abstract class CustomAddIn
  {
    public abstract FrameworkElement GetUI(double width, double height);

    public abstract void ClearBrowser();

    public abstract void Navigate(string URL);
  }
}
