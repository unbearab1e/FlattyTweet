
using System;

namespace FlattyTweet.Extensibility
{
  public interface ICoreServices
  {
    IRestService RestService { get; }

    IMessageDialogService MessageDialogService { get; }

    ISettingsService SettingService(Type AddinType);
  }
}
