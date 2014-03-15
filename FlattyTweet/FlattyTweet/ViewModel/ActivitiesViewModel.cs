
using FlattyTweet.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace FlattyTweet.ViewModel
{
  public class ActivitiesViewModel : MultiAccountViewModelBase, IPopupViewModel
  {
    private bool showActivityList;
      private ObservableCollection<TwitterStreamEventExtended> activities;
    public ObservableCollection<TwitterStreamEventExtended> Activities
    {
      get
      {
        return this.activities;
      }
      set
      {
        if (this.activities == value)
          return;
        this.activities = value;
        base.RaisePropertyChanged("Activities");
      }
    }
      private ICollectionView activitiesView;
    public ICollectionView ActivitiesView
    {
      get
      {
        return this.activitiesView;
      }
      set
      {
        if (this.activitiesView == value)
          return;
        this.activitiesView = value;
        base.RaisePropertyChanged("ActivitiesView");
      }
    }

    public bool ShowActivityList
    {
      get
      {
        return this.showActivityList;
      }
      set
      {
        if (this.showActivityList == value)
          return;
        this.showActivityList = value;
        base.RaisePropertyChanged("ShowActivityList");
      }
    }
      private string popupTitle;
    public string PopupTitle
    {
      get
      {
        return this.popupTitle;
      }
      set
      {
        if (string.Equals(this.popupTitle, value, StringComparison.Ordinal))
          return;
        this.popupTitle = value;
        base.RaisePropertyChanged("PopupTitle");
      }
    }
      private bool allowPin;
    public bool AllowPin
    {
      get
      {
        return this.allowPin;
      }
      set
      {
        if (this.allowPin == value)
          return;
        this.allowPin = value;
        base.RaisePropertyChanged("AllowPin");
      }
    }
      private bool isTransitioningToPinned;
    public bool IsTransitioningToPinned
    {
      get
      {
        return this.isTransitioningToPinned;
      }
      set
      {
        if (this.isTransitioningToPinned == value)
          return;
        this.isTransitioningToPinned = value;
        base.RaisePropertyChanged("IsTransitioningToPinned");
      }
    }

    public ActivitiesViewModel()
    {
      this.Activities = new ObservableCollection<TwitterStreamEventExtended>();
      this.ActivitiesView = CollectionViewSource.GetDefaultView((object) this.Activities);
      this.ActivitiesView.SortDescriptions.Add(new SortDescription("CreatedAt", ListSortDirection.Descending));
    }
  }
}
