using FlattyTweet.Extensibility;
using System;
using System.Reflection;

namespace FlattyTweet.AddIns
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
