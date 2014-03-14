// Type: MetroTwit.Extensibility.ITweetService
// Assembly: MetroTwit.Extensibility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BABFD559-0F69-4CCB-AC2F-DD0824A6C2D7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Extensibility.dll

using System;
using System.Windows.Controls;

namespace MetroTwit.Extensibility
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
