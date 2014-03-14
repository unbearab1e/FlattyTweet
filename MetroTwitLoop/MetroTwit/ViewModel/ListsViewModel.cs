// Type: MetroTwit.ViewModel.ListsViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using MetroTwit;
using MetroTwit.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class ListsViewModel : ViewModelBase, IPopupViewModel
  {
    private ObservableCollection<TwitterListContainer> twitterLists;
    private List<TwitterListContainer> tempTwitterList;
    private bool showAnimation;
    private bool showLists;
    private int totalLoadCount;
    private int currentCount;
    private object countLock;

    public ObservableCollection<TwitterListContainer> TwitterLists
    {
      get
      {
        return this.twitterLists;
      }
      set
      {
        if (this.twitterLists == value)
          return;
        this.twitterLists = value;
        base.RaisePropertyChanged("TwitterLists");
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

    public bool ShowLists
    {
      get
      {
        return this.showLists;
      }
      set
      {
        if (this.showLists == value)
          return;
        this.showLists = value;
        base.RaisePropertyChanged("ShowLists");
      }
    }
      private Decimal twitterAccountID;
    public Decimal TwitterAccountID
    {
      get
      {
        return this.twitterAccountID;
      }
      set
      {
        if (Decimal.Equals(this.twitterAccountID, value))
          return;
        this.twitterAccountID = value;
        base.RaisePropertyChanged("TwitterAccountID");
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

    public ListsViewModel(decimal TwitterAccountID)
    {
        Action<TwitterListCollection> callback = null;
        Action<Task<TwitterResponse<TwitterListCollection>>> continuationAction = null;
        Action<Task<TwitterResponse<TwitterListCollection>>> action3 = null;
        this.TwitterAccountID = TwitterAccountID;
        this.TwitterLists = new ObservableCollection<TwitterListContainer>();
        this.tempTwitterList = new List<TwitterListContainer>();
        this.showAnimation = true;
        this.showLists = false;
        this.countLock = new object();
        this.totalLoadCount = 3;
        if (callback == null)
        {
            callback = delegate(TwitterListCollection listsCollection)
            {
                Func<TwitterList, TwitterListExtended> selector = null;
                TwitterListContainer personalListsContainer = new TwitterListContainer
                {
                    Name = "my lists",
                    TwitterAccountID = this.TwitterAccountID,
                    Order = 0
                };
                if ((listsCollection != null) && (listsCollection.Count > 0))
                {
                    if (selector == null)
                    {
                        selector = listItem => new TwitterListExtended { TwitterAccountID = this.TwitterAccountID, IsEditable = true, BaseListObject = listItem, ParentContainer = personalListsContainer };
                    }
                    personalListsContainer.Lists = new ObservableCollection<TwitterListExtended>((from listItem in listsCollection
                                                                                                  where listItem.User.ScreenName == App.AppState.CurrentActiveAccount.TwitterAccountName
                                                                                                  select listItem).Select<TwitterList, TwitterListExtended>(selector).Distinct<TwitterListExtended>(new ListEqualityComparer()).ToList<TwitterListExtended>());
                }
                personalListsContainer.ShowAddButton = true;
                this.tempTwitterList.Add(personalListsContainer);
                lock (this.countLock)
                {
                    this.currentCount++;
                    this.CheckState();
                }
            };
        }
        App.GetLists(this.TwitterAccountID, callback);
        if (continuationAction == null)
        {
            continuationAction = delegate(Task<TwitterResponse<TwitterListCollection>> subscribedListResponse)
            {
                Func<TwitterList, TwitterListExtended> selector = null;
                TwitterListContainer subscribedListsContainer = new TwitterListContainer
                {
                    TwitterAccountID = this.TwitterAccountID,
                    Name = "followed lists",
                    Order = 1
                };
                if (((subscribedListResponse.Result.Result == RequestResult.Success) && (subscribedListResponse.Result.ResponseObject != null)) && (subscribedListResponse.Result.ResponseObject.Count > 0))
                {
                    if (selector == null)
                    {
                        selector = listItem => new TwitterListExtended { TwitterAccountID = this.TwitterAccountID, IsEditable = false, BaseListObject = listItem, IsUnsubscribeVisible = true, ParentContainer = subscribedListsContainer };
                    }
                    subscribedListsContainer.Lists = new ObservableCollection<TwitterListExtended>(subscribedListResponse.Result.ResponseObject.Select<TwitterList, TwitterListExtended>(selector).ToList<TwitterListExtended>());
                }
                this.tempTwitterList.Add(subscribedListsContainer);
                lock (this.countLock)
                {
                    this.currentCount++;
                    this.CheckState();
                }
            };
        }
        Lists.SubscriptionsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, MetroTwitTwitterizer.GetListSubscriptionsOptions).ContinueWith(continuationAction);
        if (action3 == null)
        {
            action3 = delegate(Task<TwitterResponse<TwitterListCollection>> listsFollowingYouResponse)
            {
                Func<TwitterList, TwitterListExtended> selector = null;
                TwitterListContainer item = new TwitterListContainer
                {
                    TwitterAccountID = this.TwitterAccountID,
                    Name = "listed in",
                    Order = 2
                };
                if (((listsFollowingYouResponse.Result.Result == RequestResult.Success) && (listsFollowingYouResponse.Result.ResponseObject != null)) && (listsFollowingYouResponse.Result.ResponseObject.Count > 0))
                {
                    if (selector == null)
                    {
                        selector = listItem => new TwitterListExtended { TwitterAccountID = this.TwitterAccountID, IsEditable = false, BaseListObject = listItem };
                    }
                    item.Lists = new ObservableCollection<TwitterListExtended>(listsFollowingYouResponse.Result.ResponseObject.Select<TwitterList, TwitterListExtended>(selector).ToList<TwitterListExtended>());
                }
                this.tempTwitterList.Add(item);
                lock (this.countLock)
                {
                    this.currentCount++;
                    this.CheckState();
                }
            };
        }
        Lists.MembershipsAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, MetroTwitTwitterizer.ListMembershipsOptions).ContinueWith(action3);
    }

    private void CheckState()
    {
      if (this.currentCount != this.totalLoadCount)
        return;
      System.Windows.Application.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        foreach (TwitterListContainer item_0 in Enumerable.ToList<TwitterListContainer>((IEnumerable<TwitterListContainer>) Enumerable.OrderBy<TwitterListContainer, int>((IEnumerable<TwitterListContainer>) this.tempTwitterList, (Func<TwitterListContainer, int>) (o => o.Order))))
          this.TwitterLists.Add(item_0);
        this.ShowAnimation = false;
        this.ShowLists = true;
      }), new object[0]);
    }
  }
}
