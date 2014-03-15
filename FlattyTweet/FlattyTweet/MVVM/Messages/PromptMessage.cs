
using System.Windows;

namespace FlattyTweet.MVVM.Messages
{
  public class PromptMessage
  {
    public FrameworkElement PromptView { get; set; }

    public bool IsModal { get; set; }

    public bool IsCentered { get; set; }
  }
}
