
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class GeoViewModel : ViewModelBase
  {
    private string geoImageURI;
    private string geoPlaceText;
    private string geoAddress;
    private bool showanimation;

    public string GeoImageURI
    {
      get
      {
        return this.geoImageURI;
      }
      set
      {
        if (string.Equals(this.geoImageURI, value, StringComparison.Ordinal))
          return;
        this.geoImageURI = value;
        base.RaisePropertyChanged("GeoImageURI");
      }
    }

    public string GeoPlaceText
    {
      get
      {
        return this.geoPlaceText;
      }
      set
      {
        if (string.Equals(this.geoPlaceText, value, StringComparison.Ordinal))
          return;
        this.geoPlaceText = value;
        base.RaisePropertyChanged("GeoPlaceText");
      }
    }

    public string GeoAddress
    {
      get
      {
        return this.geoAddress;
      }
      set
      {
        if (string.Equals(this.geoAddress, value, StringComparison.Ordinal))
          return;
        this.geoAddress = value;
        base.RaisePropertyChanged("GeoAddress");
      }
    }

    public bool ShowAnimation
    {
      get
      {
        return this.showanimation;
      }
      set
      {
        if (this.showanimation == value)
          return;
        this.showanimation = value;
        base.RaisePropertyChanged("ShowAnimation");
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
      private string liveMapURL;
    public string LiveMapURL
    {
      get
      {
        return this.liveMapURL;
      }
      set
      {
        if (string.Equals(this.liveMapURL, value, StringComparison.Ordinal))
          return;
        this.liveMapURL = value;
        base.RaisePropertyChanged("LiveMapURL");
      }
    }

    public GeoViewModel(MetroTwitStatusBase Tweet)
    {
      this.ShowAnimation = true;
      this.LinkCommand = new RelayCommand<string>(new Action<string>(CommonCommands.OpenLink));
      Coordinate coordinate = Tweet.Coordinates != null ? Tweet.Coordinates.Coordinate[0] : Tweet.Geo.BoundingBox.Coordinates[0];
      if (coordinate != null)
      {
        Task task = new Task((Action) (() => this.GeoLookup(coordinate)));
        task.ContinueWith((Action<Task>) (t => CommonCommands.CheckTaskExceptions(t)));
        task.Start();
        object resource = Application.Current.FindResource((object) "ModernColorFeature");
        string PinColour = resource == null ? "blue" : "0x" + resource.ToString().Remove(0, 3);
        this.GeoImageURI = CoreServices.Instance.CurrentMapService.StaticMapURL(coordinate.Latitude, coordinate.Longitude, 320, 320, PinColour);
        GeoViewModel geoViewModel = this;
        double num = coordinate.Latitude;
        string str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        string str2 = ", ";
        num = coordinate.Longitude;
        string str3 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        string str4 = str1 + str2 + str3;
        geoViewModel.GeoPlaceText = str4;
        this.LiveMapURL = CoreServices.Instance.CurrentMapService.LiveMapURL(coordinate.Latitude, coordinate.Longitude);
      }
      this.ShowAnimation = false;
    }

    private void GeoLookup(Coordinate coordinate)
    {
      string returntext = CoreServices.Instance.CurrentMapService.ReverseGeoLookup(coordinate.Latitude, coordinate.Longitude).ExpandedAddress;
      if (string.IsNullOrEmpty(returntext))
        returntext = "Unable to lookup Address";
      Application.Current.Dispatcher.BeginInvoke((Action) (() => this.GeoAddress = returntext), new object[0]);
    }
  }
}
