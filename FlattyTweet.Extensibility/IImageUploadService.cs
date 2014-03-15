
using System;

namespace FlattyTweet.Extensibility
{
  public interface IImageUploadService
  {
    string Name { get; }

    string UniqueID { get; }

    Action UploadImage(ImageUploadRequest uploadRequest);
  }
}
