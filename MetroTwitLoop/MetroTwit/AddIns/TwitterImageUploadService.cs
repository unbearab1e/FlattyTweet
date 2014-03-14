// Type: MetroTwit.AddIns.TwitterImageUploadService
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensibility;
using System;
using System.Reflection;

namespace MetroTwit.AddIns
{
  public class TwitterImageUploadService : IImageUploadService
  {
    public string Name
    {
      get
      {
        return "Twitter Image";
      }
    }

    public string UniqueID
    {
      get
      {
        return ((object) Assembly.GetExecutingAssembly()).GetType().ToString() + "+TwitterImage";
      }
    }

    public Action UploadImage(ImageUploadRequest uploadRequest)
    {
      return (Action) null;
    }
  }
}
