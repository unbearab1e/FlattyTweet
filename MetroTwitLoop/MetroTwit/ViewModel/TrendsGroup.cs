// Type: MetroTwit.ViewModel.TrendsGroup
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class TrendsGroup : ViewModelBase
  {
    private IEnumerable<TwitterTrend> currentTrends;
    private bool showAnimation;
    private bool showTrendsList;
    private bool showErrorPanel;
    private bool showSaveLocationButton;
    private string errorText;
    private RelayCommand tryAgainCommand;
    private int retryAttempts;
    private string[] retryAttemptMessages;
    private TrendsGroupType trendsGroupType;
    private DateTime fromDate;
    private string currentTrendLocationName;
    private TwitterTrendLocationCollection availableTrendLocations;
    private int selectedWOEID;

    public TwitterTrendLocationCollection AvailableTrendLocations
    {
      get
      {
        return this.availableTrendLocations;
      }
      set
      {
        if (this.availableTrendLocations == value)
          return;
        this.availableTrendLocations = value;
        base.RaisePropertyChanged("AvailableTrendLocations");
      }
    }

    public bool ShowSaveLocationButton
    {
      get
      {
        return this.showSaveLocationButton;
      }
      set
      {
        if (this.showSaveLocationButton == value)
          return;
        this.showSaveLocationButton = value;
        base.RaisePropertyChanged("ShowSaveLocationButton");
      }
    }

    public int SelectedWOEID
    {
      get
      {
        return this.selectedWOEID;
      }
      set
      {
        if (this.selectedWOEID == value)
          return;
        this.selectedWOEID = value;
        this.FetchCurrentTrends();
        this.ShowSaveLocationButton = SettingsData.Instance.TrendLocationWOEID.GetValueOrDefault(-1) != value;
        base.RaisePropertyChanged("SelectedWOEID");
      }
    }

    public DateTime FromDate
    {
      get
      {
        return this.fromDate;
      }
      set
      {
        if (DateTime.Equals(this.fromDate, value))
          return;
        this.fromDate = value;
        this.LoadTrends();
        base.RaisePropertyChanged("FromDate");
      }
    }

    public string CurrentTrendLocationName
    {
      get
      {
        return this.currentTrendLocationName;
      }
      set
      {
        if (string.Equals(this.currentTrendLocationName, value, StringComparison.Ordinal))
          return;
        this.currentTrendLocationName = value;
        base.RaisePropertyChanged("CurrentTrendLocationName");
      }
    }

    public IEnumerable<TwitterTrend> CurrentTrends
    {
      get
      {
        return this.currentTrends;
      }
      set
      {
        if (this.currentTrends == value)
          return;
        this.currentTrends = value;
        base.RaisePropertyChanged("CurrentTrends");
      }
    }

    public bool ShowAnimation
    {
      get
      {
        return this.showAnimation;
      }
      set
      {
        if (this.showAnimation == value)
          return;
        this.showAnimation = value;
        base.RaisePropertyChanged("ShowAnimation");
      }
    }

    public bool ShowTrendsList
    {
      get
      {
        return this.showTrendsList;
      }
      set
      {
        if (this.showTrendsList == value)
          return;
        this.showTrendsList = value;
        base.RaisePropertyChanged("ShowTrendsList");
      }
    }

    public bool ShowErrorPanel
    {
      get
      {
        return this.showErrorPanel;
      }
      set
      {
        if (this.showErrorPanel == value)
          return;
        this.showErrorPanel = value;
        base.RaisePropertyChanged("ShowErrorPanel");
      }
    }

    public string ErrorText
    {
      get
      {
        return this.errorText;
      }
      set
      {
        if (string.Equals(this.errorText, value, StringComparison.Ordinal))
          return;
        this.errorText = value;
        base.RaisePropertyChanged("ErrorText");
      }
    }
      private RelayCommand saveAsDefaultLocationCommand;
    public RelayCommand SaveAsDefaultLocationCommand
    {
      get
      {
        return this.saveAsDefaultLocationCommand;
      }
      set
      {
        if (this.saveAsDefaultLocationCommand == value)
          return;
        this.saveAsDefaultLocationCommand = value;
        base.RaisePropertyChanged("SaveAsDefaultLocationCommand");
      }
    }

    public RelayCommand TryAgainCommand
    {
      get
      {
        return this.tryAgainCommand;
      }
      set
      {
        if (this.tryAgainCommand == value)
          return;
        this.tryAgainCommand = value;
        base.RaisePropertyChanged("TryAgainCommand");
      }
    }

    public TrendsGroup(TrendsGroupType trendsGroupType)
    {
      this.currentTrendLocationName = "Worldwide";
      this.trendsGroupType = trendsGroupType;
      this.retryAttemptMessages = new string[3]
      {
        "You might be too trendy for trends. Or perhaps an error occured.",
        "Nope, you're still too cool for trends",
        "Maybe the trendy people are asleep. Try again later."
      };
      this.TryAgainCommand = new RelayCommand(new Action(this.LoadTrends));
      this.SaveAsDefaultLocationCommand = new RelayCommand(new Action(this.SaveAsDefaultLocation));
      this.LoadTrends();
    }

    private async void LoadTrends()
    {
        this.ShowErrorPanel = false;
        this.ShowAnimation = true;
        this.ShowTrendsList = false;
        if (this.trendsGroupType == TrendsGroupType.Current)
        {
            if (App.AvailableTrendLocations == null)
            {
                TwitterResponse<TwitterTrendLocationCollection> asyncVariable0 = await Trends.AvailableAsync(App.AppState.CurrentActiveAccount.Tokens, MetroTwitTwitterizer.AvailableTrendsOptions);
                if (asyncVariable0.Result == RequestResult.Success)
                {
                    TwitterTrendLocationCollection responseObject = asyncVariable0.ResponseObject;
                    TwitterTrendLocationCollection oc = new TwitterTrendLocationCollection();
                    oc.AddRange<TwitterTrendLocation>((from tt in responseObject
                                                       orderby tt.Name
                                                       select tt).ToList<TwitterTrendLocation>());
                    TwitterTrendLocation item = (from ttl in oc
                                                 where ttl.Name.ToLower() == "worldwide"
                                                 select ttl).FirstOrDefault<TwitterTrendLocation>();
                    if (item != null)
                    {
                        oc.Remove(item);
                        oc.Insert(0, item);
                    }
                    App.AvailableTrendLocations = oc;
                    if (this.AvailableTrendLocations == null)
                    {
                        this.AvailableTrendLocations = oc;
                    }
                    this.SetLocalLocation();
                }
            }
            else
            {
                if (this.AvailableTrendLocations == null)
                {
                    this.AvailableTrendLocations = App.AvailableTrendLocations;
                }
                this.SetLocalLocation();
            }
        }
    }


    private void SaveAsDefaultLocation()
    {
      SettingsData.Instance.TrendLocationWOEID = new int?(this.SelectedWOEID);
      this.ShowSaveLocationButton = false;
    }

    private void SetLocalLocation()
    {
      if (!SettingsData.Instance.TrendLocationWOEID.HasValue)
      {
        string currentRegion = RegionInfo.CurrentRegion.DisplayName;
        TwitterTrendLocation twitterTrendLocation = Enumerable.FirstOrDefault<TwitterTrendLocation>(Enumerable.Where<TwitterTrendLocation>((IEnumerable<TwitterTrendLocation>) App.AvailableTrendLocations, (Func<TwitterTrendLocation, bool>) (t => t.Country == currentRegion && t.Name == currentRegion)));
        int num = 1;
        if (twitterTrendLocation != null)
        {
          num = twitterTrendLocation.WOEID;
          this.CurrentTrendLocationName = twitterTrendLocation.Name;
        }
        this.SelectedWOEID = num;
      }
      else
        this.SelectedWOEID = SettingsData.Instance.TrendLocationWOEID.Value;
    }

    private async void FetchCurrentTrends()
    {
      this.ShowErrorPanel = false;
      this.ShowAnimation = true;
      this.ShowTrendsList = false;
      TwitterTrendLocation currentTrendLocation = Enumerable.FirstOrDefault<TwitterTrendLocation>(Enumerable.Where<TwitterTrendLocation>((IEnumerable<TwitterTrendLocation>) App.AvailableTrendLocations, (Func<TwitterTrendLocation, bool>) (t => t.WOEID == this.SelectedWOEID)));
      this.CurrentTrendLocationName = currentTrendLocation != null ? currentTrendLocation.Name : "Worldwide";
      TwitterResponse<TwitterTrendCollection> trendAsyncResponse = await Trends.TrendsAsync(App.AppState.CurrentActiveAccount.Tokens, this.SelectedWOEID, (LocalTrendsOptions) MetroTwitTwitterizer.TrendsOptions);
      if (trendAsyncResponse.Errors != null)
      {
        if (this.retryAttempts < this.retryAttemptMessages.Length)
          ++this.retryAttempts;
        this.ShowAnimation = false;
        this.ShowErrorPanel = true;
        this.ErrorText = this.retryAttemptMessages[this.retryAttempts - 1];
      }
      else
      {
        this.ShowAnimation = false;
        this.ShowTrendsList = true;
        this.CurrentTrends = (IEnumerable<TwitterTrend>) trendAsyncResponse.ResponseObject;
      }
    }
  }
}
