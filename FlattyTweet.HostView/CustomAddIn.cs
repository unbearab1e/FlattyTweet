
using System.Windows;

namespace FlattyTweet.HostView
{
  public abstract class CustomAddIn
  {
    public abstract FrameworkElement GetUI(double width, double height);

    public abstract void ClearBrowser();

    public abstract void Navigate(string URL);
  }
}
