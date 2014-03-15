
using System;

namespace FlattyTweet.Extensibility
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
