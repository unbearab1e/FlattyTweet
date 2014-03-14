﻿// Type: MetroTwit.Extensibility.ISettingsService
// Assembly: MetroTwit.Extensibility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BABFD559-0F69-4CCB-AC2F-DD0824A6C2D7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Extensibility.dll

using System;
using System.Collections.Generic;

namespace MetroTwit.Extensibility
{
  public interface ISettingsService
  {
    void SaveObject<T>(Dictionary<Decimal, T> targetObjectToSave);

    void SaveSingleObject<T>(T targetObjectToSave);

    Dictionary<Decimal, T> LoadObject<T>(T type);

    T LoadSingleObject<T>(T type);
  }
}
