
namespace FlattyTweet.Extensions
{
  public interface IPopupViewModel
  {
    string PopupTitle { get; set; }

    bool AllowPin { get; set; }

    bool IsTransitioningToPinned { get; set; }
  }
}
