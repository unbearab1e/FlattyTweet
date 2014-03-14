// Type: MetroTwit.ViewModel.MediaPreviewViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroTwit.Extensions;
using MetroTwit.Model;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class MediaPreviewViewModel : ViewModelBase
  {
    private double maximgsize = 500.0;

      private UrlEntity link;
     public UrlEntity Link
    {
      get
      {
        return this.link;
      }
      set
      {
        if (this.link == value)
          return;
        this.link = value;
        base.RaisePropertyChanged("Link");
      }
    }
      private Visibility throbberVisible;
    public Visibility ThrobberVisible
    {
      get
      {
        return this.throbberVisible;
      }
      set
      {
        if (this.throbberVisible == value)
          return;
        this.throbberVisible = value;
        base.RaisePropertyChanged("ThrobberVisible");
      }
    }
      private Visibility descriptionVisibility;
    public Visibility DescriptionVisibility
    {
      get
      {
        return this.descriptionVisibility;
      }
      set
      {
        if (this.descriptionVisibility == value)
          return;
        this.descriptionVisibility = value;
        base.RaisePropertyChanged("DescriptionVisibility");
      }
    }
      private Visibility titleVisibility;
    public Visibility TitleVisibility
    {
      get
      {
        return this.titleVisibility;
      }
      set
      {
        if (this.titleVisibility == value)
          return;
        this.titleVisibility = value;
        base.RaisePropertyChanged("TitleVisibility");
      }
    }
      private Visibility webBrowserVisible;
    public Visibility WebBrowserVisible
    {
      get
      {
        return this.webBrowserVisible;
      }
      set
      {
        if (this.webBrowserVisible == value)
          return;
        this.webBrowserVisible = value;
        base.RaisePropertyChanged("WebBrowserVisible");
      }
    }
      private Visibility imageVisible;
    public Visibility ImageVisible
    {
      get
      {
        return this.imageVisible;
      }
      set
      {
        if (this.imageVisible == value)
          return;
        this.imageVisible = value;
        base.RaisePropertyChanged("ImageVisible");
      }
    }
      private bool closed;
    public bool Closed
    {
      get
      {
        return this.closed;
      }
      set
      {
        if (this.closed == value)
          return;
        this.closed = value;
        base.RaisePropertyChanged("Closed");
      }
    }
      private MetroTwitStatusBase description;
    public MetroTwitStatusBase Description
    {
      get
      {
        return this.description;
      }
      set
      {
        if (this.description == value)
          return;
        this.description = value;
        base.RaisePropertyChanged("Description");
      }
    }
      private string title;
    public string Title
    {
      get
      {
        return this.title;
      }
      set
      {
        if (string.Equals(this.title, value, StringComparison.Ordinal))
          return;
        this.title = value;
        base.RaisePropertyChanged("Title");
      }
    }
      private string imageUrl;
    public string ImageUrl
    {
      get
      {
        return this.imageUrl;
      }
      set
      {
        if (string.Equals(this.imageUrl, value, StringComparison.Ordinal))
          return;
        this.imageUrl = value;
        base.RaisePropertyChanged("ImageUrl");
      }
    }
      private string html;
    public string HTML
    {
      get
      {
        return this.html;
      }
      set
      {
        if (string.Equals(this.html, value, StringComparison.Ordinal))
          return;
        this.html = value;
        base.RaisePropertyChanged("HTML");
      }
    }
      private BitmapImage image;
    public BitmapImage Image
    {
      get
      {
        return this.image;
      }
      set
      {
        if (this.image == value)
          return;
        this.image = value;
        base.RaisePropertyChanged("Image");
      }
    }
      private double mediaWidth;
    public double MediaWidth
    {
      get
      {
        return this.mediaWidth;
      }
      set
      {
        if (this.mediaWidth == value)
          return;
        this.mediaWidth = value;
        base.RaisePropertyChanged("MediaWidth");
      }
    }
      private double mediaHeight;
    public double MediaHeight
    {
      get
      {
        return this.mediaHeight;
      }
      set
      {
        if (this.mediaHeight == value)
          return;
        this.mediaHeight = value;
        base.RaisePropertyChanged("MediaHeight");
      }
    }
      private RelayCommand<string> linkCommand;
    public RelayCommand<string> LinkCommand
    {
      get
      {
        return this.linkCommand;
      }
      private set
      {
        if (this.linkCommand == value)
          return;
        this.linkCommand = value;
        base.RaisePropertyChanged("LinkCommand");
      }
    }
      private RelayCommand closeCommand;
    public RelayCommand CloseCommand
    {
      get
      {
        return this.closeCommand;
      }
      private set
      {
        if (this.closeCommand == value)
          return;
        this.closeCommand = value;
        base.RaisePropertyChanged("CloseCommand");
      }
    }

    public MediaPreviewViewModel(UrlEntity url)
    {
      this.ThrobberVisible = Visibility.Visible;
      this.TitleVisibility = Visibility.Collapsed;
      this.WebBrowserVisible = Visibility.Collapsed;
      this.ImageVisible = Visibility.Collapsed;
      this.DescriptionVisibility = Visibility.Collapsed;
      this.MediaWidth = 400.0;
      this.MediaHeight = 0.0;
      this.Link = url;
      this.LinkCommand = new RelayCommand<string>(new Action<string>(this.ExecuteLinkWithClose));
      this.CloseCommand = new RelayCommand(new Action(this.Close));
      Task.Run((Action) (() => this.LoadMedia()));
    }

    private void Close()
    {
      this.Closed = true;
    }

    private void ExecuteLinkWithClose(string link)
    {
      try
      {
        CommonCommands.ExecuteLink(!string.IsNullOrEmpty(link) ? (object) link : (object) this.Link.Url);
        this.Closed = true;
      }
      catch
      {
      }
    }

    private async void LoadMedia()
    {
        Action asyncVariable0 = null;
        Action asyncVariable1 = null;
        Action asyncVariable2 = null;
        bool problemOccured = false;
        OembedResponse result = null;
        if (((this.Link is MediaEntity) && !string.IsNullOrEmpty((this.Link as MediaEntity).MediaUrlSecure)) && ((this.Link as MediaEntity).MediaType == MediaEntity.MediaTypes.Photo))
        {
            result = new OembedResponse();
            if (!(this.Link as MediaEntity).MediaUrlSecure.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                result.Type = "photo";
                result.Url = (this.Link as MediaEntity).MediaUrlSecure;
            }
            else
            {
                result.Type = "rich";
                result.Html = "<img src=\"" + (this.Link as MediaEntity).MediaUrlSecure + "\" />";
            }
            if ((this.Link as MediaEntity).Sizes != null)
            {
                MediaEntity.MediaSize size = (this.Link as MediaEntity).Sizes.Find(x => x.Size == MediaEntity.MediaSize.MediaSizes.Large);
                if (size != null)
                {
                    result.Width = new int?(size.Width);
                    result.Height = new int?(size.Height);
                }
            }
        }
        else if (((this.Link != null) && !string.IsNullOrEmpty(this.Link.ExpandedUrl)) && ((this.Link.ExpandedUrl.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) || this.Link.ExpandedUrl.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) || this.Link.ExpandedUrl.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
        {
            result = new OembedResponse();
            if (!this.Link.ExpandedUrl.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                result.Type = "photo";
                result.Url = this.Link.ExpandedUrl;
            }
            else
            {
                result.Type = "rich";
                result.Html = "<img src=\"" + this.Link.ExpandedUrl + "\" />";
            }
        }
        else if (this.Link.ExpandedUrl.Contains("youtube.com", StringComparison.InvariantCultureIgnoreCase) || this.Link.ExpandedUrl.Contains("youtu.be", StringComparison.InvariantCultureIgnoreCase))
        {
            result = await oEmbed.GetOembed("http://www.youtube.com/oembed", HttpUtility.UrlEncode(this.Link.ExpandedUrl), 640);
            if (result != null)
            {
                int? nullable2 = result.Height + 30;
                result.Html = result.Html.Replace("height=\"" + result.Height.ToString() + "\"", "height=\"" + nullable2.ToString() + "\"");
                result.Height += 30;
            }
            else
            {
                if (asyncVariable0 == null)
                {
                    asyncVariable0 = () => this.Closed = true;
                }
                Application.Current.Dispatcher.BeginInvoke(asyncVariable0, DispatcherPriority.Background, new object[0]);
            }
        }
        else if (this.Link.ExpandedUrl.Contains("vine.co", StringComparison.InvariantCultureIgnoreCase))
        {
            result = new OembedResponse
            {
                Type = "rich",
                Width = 500,
                Height = 500,
                Html = "<iframe width=\"500\" height=\"500\" scrolling=\"no\" frameborder=\"0\" src=\"" + this.Link.ExpandedUrl + "/card\"></iframe>"
            };
        }
        else if (this.Link.ExpandedUrl.Contains("sdrv.ms", StringComparison.InvariantCultureIgnoreCase) || this.Link.ExpandedUrl.Contains("skydrive.live.com", StringComparison.InvariantCultureIgnoreCase))
        {
            result.Type = "photo";
            result.Url = (this.Link as MediaEntity).MediaUrlSecure;
        }
        else
        {
            if (asyncVariable1 == null)
            {
                asyncVariable1 = () => this.Closed = true;
            }
            Application.Current.Dispatcher.BeginInvoke(asyncVariable1, DispatcherPriority.Background, new object[0]);
        }
        if (((!this.Closed && (result != null)) && (result.Type != "error")) && (result.Type != null))
        {
            Action method = null;
            Action action3 = null;
            OembedResponse noEmbed = result;
            Application.Current.Dispatcher.BeginInvoke((Action) (()=>
            {
                this.Title = WebUtility.HtmlDecode(noEmbed.Title);
                if (!string.IsNullOrEmpty(noEmbed.Title))
                {
                    this.TitleVisibility = Visibility.Visible;
                }
            }), DispatcherPriority.Background, new object[0]);
            if ((noEmbed.Type == "video") || (noEmbed.Type == "rich"))
            {
                if (method == null)
                {
                    method = delegate
                    {
                        this.MediaWidth = (noEmbed.Width.HasValue && (noEmbed.Width.Value != 0)) ? ((double)noEmbed.Width.Value) : ((double)500);
                        this.MediaHeight = (noEmbed.Height.HasValue && (noEmbed.Height.Value != 0)) ? ((double)noEmbed.Height.Value) : ((double)0x132);
                        if (!string.IsNullOrEmpty(noEmbed.Html))
                        {
                            string darkThemeCSS = null;
                            if (SettingsData.Instance.Theme == "Dark")
                            {
                                darkThemeCSS = MediaConsts.DarkThemeCSS;
                            }
                            this.HTML = MediaConsts.DoctypeStyleHtml + this.MediaHeight.ToString() + MediaConsts.StyleHtml + darkThemeCSS + MediaConsts.OpeningHtml + noEmbed.Html + MediaConsts.ClosingHtml;
                        }
                        else
                        {
                            problemOccured = true;
                        }
                        this.ImageVisible = Visibility.Collapsed;
                        this.WebBrowserVisible = Visibility.Visible;
                    };
                }
                Application.Current.Dispatcher.BeginInvoke(method, DispatcherPriority.Background, new object[0]);
            }
            if (((noEmbed.Type == "photo") && !problemOccured) && !string.IsNullOrEmpty(noEmbed.Url))
            {
                try
                {
                    byte[] buffer = CommonCommands.DownloadFile(noEmbed.Url);
                    if (buffer != null)
                    {
                        Action action = null;
                        MemoryStream mem = new MemoryStream(buffer);
                        if (mem != null)
                        {
                            if (CommonCommands.IsValidImage(mem))
                            {
                                try
                                {
                                    mem.Seek(0L, SeekOrigin.Begin);
                                }
                                catch
                                {
                                }
                                if (action == null)
                                {
                                    action = delegate
                                    {
                                        this.ImageUrl = noEmbed.Url;
                                        this.MediaWidth = (noEmbed.Width.HasValue && (noEmbed.Width.Value != 0)) ? ((double)noEmbed.Width.Value) : this.maximgsize;
                                        this.MediaHeight = (noEmbed.Height.HasValue && (noEmbed.Height.Value != 0)) ? ((double)noEmbed.Height.Value) : this.maximgsize;
                                        double num = 0.0;
                                        if ((this.MediaWidth > this.maximgsize) || (this.MediaHeight > this.maximgsize))
                                        {
                                            if (this.MediaWidth >= this.MediaHeight)
                                            {
                                                num = this.MediaWidth / this.maximgsize;
                                            }
                                            else
                                            {
                                                num = this.MediaHeight / this.maximgsize;
                                            }
                                            this.MediaWidth /= num;
                                            this.MediaHeight /= num;
                                        }
                                        BitmapImage image = new BitmapImage();
                                        image.BeginInit();
                                        if (noEmbed.Width.HasValue)
                                        {
                                            image.DecodePixelWidth = (int)this.MediaWidth;
                                        }
                                        if (noEmbed.Width.HasValue)
                                        {
                                            image.DecodePixelHeight = (int)this.MediaHeight;
                                        }
                                        image.CacheOption = BitmapCacheOption.OnLoad;
                                        image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                                        image.StreamSource = mem;
                                        image.EndInit();
                                        image.Freeze();
                                        this.Image = image;
                                        this.ImageVisible = Visibility.Visible;
                                        this.WebBrowserVisible = Visibility.Collapsed;
                                    };
                                }
                                Application.Current.Dispatcher.BeginInvoke(action, DispatcherPriority.Background, new object[0]);
                            }
                            else
                            {
                                problemOccured = true;
                            }
                        }
                        else
                        {
                            problemOccured = true;
                        }
                    }
                    else
                    {
                        problemOccured = true;
                    }
                }
                catch
                {
                    problemOccured = true;
                }
            }
            if (!problemOccured)
            {
                if (action3 == null)
                {
                    action3 = delegate
                    {
                        if (noEmbed.Type == "link")
                        {
                            this.ImageVisible = Visibility.Collapsed;
                            this.WebBrowserVisible = Visibility.Collapsed;
                            if (string.IsNullOrEmpty(noEmbed.AuthorName))
                            {
                                problemOccured = true;
                                return;
                            }
                        }
                        this.MediaWidth = (this.MediaWidth == 0.0) ? this.maximgsize : this.MediaWidth;
                        this.MediaWidth = (this.MediaWidth < 300.0) ? 300.0 : this.MediaWidth;
                        this.ThrobberVisible = Visibility.Collapsed;
                    };
                }
                Application.Current.Dispatcher.BeginInvoke(action3, DispatcherPriority.Background, new object[0]);
            }
        }
        else if (!this.Closed)
        {
            problemOccured = true;
        }
        if (problemOccured)
        {
            if (asyncVariable2 == null)
            {
                asyncVariable2 = () => this.LinkCommand.Execute(this.Link.Url);
            }
            Application.Current.Dispatcher.BeginInvoke(asyncVariable2, DispatcherPriority.Background, new object[0]);
        }
    }
  }
}
