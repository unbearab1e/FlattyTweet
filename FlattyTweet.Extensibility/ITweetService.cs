
using System;
using System.Windows.Controls;

namespace FlattyTweet.Extensibility
{
  public interface ITweetService
  {
    string Name { get; }

    string UniqueID { get; }

    bool WantsPreCreation { get; }

    bool WantsPostCreation { get; }

    bool HasSettings { get; }

    bool OverrideTweetCharLimit { get; }

    UserControl GetUISettings { get; }

    void SaveSettings();

    void CancelSaveSettings();

    bool ValidateSettings();

    PreTweetCreationResponse PreTweetCreation(string TweetContent, int CharCount, string InReplyToID, string InReplyToScreenName, bool DirectMessage, string TwitterAccountName);

    PostTweetCreationResponse PostTweetCreation(string TweetContent, int CharCount, string InReplyToID, string InReplyToScreenName, bool DirectMessage, string TwitterAccountName, Decimal TweetID);
  }
}
