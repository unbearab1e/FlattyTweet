
using System;
using System.Collections.Generic;

namespace FlattyTweet.Extensibility
{
  public interface ISettingsService
  {
    void SaveObject<T>(Dictionary<Decimal, T> targetObjectToSave);

    void SaveSingleObject<T>(T targetObjectToSave);

    Dictionary<Decimal, T> LoadObject<T>(T type);

    T LoadSingleObject<T>(T type);
  }
}
