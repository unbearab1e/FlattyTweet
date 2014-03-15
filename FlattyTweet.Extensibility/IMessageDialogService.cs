
using System.Windows;
using System.Windows.Markup;

namespace FlattyTweet.Extensibility
{
  public interface IMessageDialogService
  {
    MessageBoxResult InvokeShow(string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult);

    MessageBoxResult InvokeShow(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult);

    void InvokeShowNonModal(string messageBoxText, string caption);
  }
}
