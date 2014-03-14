// Type: MetroTwit.ViewModel.ActivitiesViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MetroTwit.ViewModel
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
