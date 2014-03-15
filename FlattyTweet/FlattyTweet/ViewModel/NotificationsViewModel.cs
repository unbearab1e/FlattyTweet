
using FlattyTweet.Extensions;

namespace FlattyTweet.ViewModel
{
  public class NotificationsViewModel : IPopupViewModel
  {
    public string PopupTitle { get; set; }

    public bool AllowPin { get; set; }

    public bool IsTransitioningToPinned { get; set; }
  }
}
