
using System;
using System.Windows.Controls;

namespace FlattyTweet.Extensibility
{
  public interface IURLShorteningService
  {
    string Name { get; }

    string UniqueID { get; }

    bool HasSettings { get; }

    string ShortenURL(Decimal TwitterAccountID, string originalURL);

    void SaveSettings();

    void CancelSaveSettings();

    UserControl GetUISettings(Decimal TwitterAccountID);

    bool ValidateSettings(Decimal TwitterAccountID);
  }
}
