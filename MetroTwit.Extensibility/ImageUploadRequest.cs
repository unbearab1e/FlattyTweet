// Type: MetroTwit.Extensibility.ImageUploadRequest
// Assembly: MetroTwit.Extensibility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BABFD559-0F69-4CCB-AC2F-DD0824A6C2D7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Extensibility.dll

using System;

namespace MetroTwit.Extensibility
{
  public class ImageUploadRequest
  {
    public string Caption { get; set; }

    public string FilePath { get; set; }

    public string AccessToken { get; set; }

    public string AccessTokenSecret { get; set; }

    public string ConsumerKey { get; set; }

    public string ConsumerSecret { get; set; }

    public Action<int> ProgressCallback { get; set; }

    public Action<string> CompletedCallback { get; set; }

    public Action<string> ErrorCallback { get; set; }
  }
}
