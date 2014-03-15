
using System.Windows.Media.Imaging;

namespace FlattyTweet.ViewModel
{
  public class SingleImageUploadRequest
  {
    public string FilePath { get; set; }

    public BitmapImage Image { get; set; }
  }
}
