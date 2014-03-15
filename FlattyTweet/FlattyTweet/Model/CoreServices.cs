
using FlattyTweet;
using FlattyTweet.Extensibility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace FlattyTweet.Model
{
  [Export(typeof (ICoreServices))]
  public class CoreServices : ICoreServices
  {
    private static CoreServices instance;
    private IURLShorteningService currentURLShorteningService;
    private IImageUploadService currentImageUploadingService;
    private IMapService currentMapService;
    private IRestService restService;
    private IMessageDialogService messageDialogService;

    public static CoreServices Instance
    {
      get
      {
        if (CoreServices.instance == null)
          CoreServices.instance = new CoreServices();
        return CoreServices.instance;
      }
    }

    [ImportMany(typeof (IImageUploadService))]
    public ObservableCollection<IImageUploadService> ImageUploadingServices { get; set; }

    public IImageUploadService CurrentImageUploadingService
    {
      get
      {
        return this.currentImageUploadingService;
      }
      set
      {
        this.currentImageUploadingService = value;
      }
    }

    [ImportMany(typeof (IURLShorteningService))]
    public IURLShorteningService[] URLShorteningServices { get; set; }

    [ImportMany(typeof (IMapService))]
    public IMapService[] MapServices { get; set; }

    public IMapService CurrentMapService
    {
      get
      {
        return this.currentMapService;
      }
      set
      {
        this.currentMapService = value;
      }
    }

    [ImportMany(typeof (ITweetService))]
    public ITweetService[] TweetServices { get; set; }

    public IRestService RestService
    {
      get
      {
        return this.restService;
      }
    }

    public IMessageDialogService MessageDialogService
    {
      get
      {
        return this.messageDialogService;
      }
    }

    public CoreServices()
    {
      this.restService = (IRestService) new RestService();
      this.messageDialogService = (IMessageDialogService) new MessageDialogService();
      this.ImageUploadingServices = new ObservableCollection<IImageUploadService>();
    }

    public IURLShorteningService CurrentURLShorteningService(Decimal TwitterAccountID)
    {
      return App.AppState.Accounts[TwitterAccountID].Settings.URLShorteningService;
    }

    public ISettingsService SettingService(Type AddinType)
    {
      if (AddinType == typeof (IURLShorteningService))
        return (ISettingsService) App.AppState;
      else
        return (ISettingsService) SettingsData.Instance;
    }
  }
}
