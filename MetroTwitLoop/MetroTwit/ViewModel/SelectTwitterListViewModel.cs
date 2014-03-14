// Type: MetroTwit.ViewModel.SelectTwitterListViewModel
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
    public class SelectTwitterListViewModel : ViewModelBase
    {
        private bool showAnimation;
        private bool showLists;
        private bool showFailedLists;
        private string progressText;
        private int totalCountOfLists;
        private int currentCount;
        private object countLock;

        private MetroTwitUser userToAdd;
        public MetroTwitUser UserToAdd
        {
            get
            {
                return this.userToAdd;
            }
            set
            {
                if (this.userToAdd == value)
                    return;
                this.userToAdd = value;
                base.RaisePropertyChanged("UserImage");
                base.RaisePropertyChanged("UserToAdd");
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
                base.RaisePropertyChanged("UserImage");
                base.RaisePropertyChanged("TwitterAccountID");
            }
        }
        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return this.cancelCommand;
            }
            set
            {
                if (this.cancelCommand == value)
                    return;
                this.cancelCommand = value;
                base.RaisePropertyChanged("CancelCommand");
            }
        }
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return this.saveCommand;
            }
            set
            {
                if (this.saveCommand == value)
                    return;
                this.saveCommand = value;
                base.RaisePropertyChanged("SaveCommand");
            }
        }
        private ObservableCollection<TwitterListExtended> twitterLists;
        public ObservableCollection<TwitterListExtended> TwitterLists
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
        private ObservableCollection<Tuple<IEnumerable<TwitterError>, TwitterList>> failedLists;
        public ObservableCollection<Tuple<IEnumerable<TwitterError>, TwitterList>> FailedLists
        {
            get
            {
                return this.failedLists;
            }
            set
            {
                if (this.failedLists == value)
                    return;
                this.failedLists = value;
                base.RaisePropertyChanged("FailedLists");
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

        public bool ShowFailedLists
        {
            get
            {
                return this.showFailedLists;
            }
            set
            {
                if (this.showFailedLists == value)
                    return;
                this.showFailedLists = value;
                base.RaisePropertyChanged("ShowFailedLists");
            }
        }

        public string ProgressText
        {
            get
            {
                return this.progressText;
            }
            set
            {
                if (string.Equals(this.progressText, value, StringComparison.Ordinal))
                    return;
                this.progressText = value;
                base.RaisePropertyChanged("ProgressText");
            }
        }

        public object UserImage
        {
            get
            {
                if (App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers.ContainsKey("@" + this.UserToAdd.ScreenName.ToLower()))
                    return App.AppState.Accounts[this.TwitterAccountID].Cache.NonCachedUsers["@" + this.UserToAdd.ScreenName.ToLower()].UserImage(54, false, this.TwitterAccountID).Result;
                if (App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers.ContainsKey("@" + this.UserToAdd.ScreenName.ToLower()))
                    return App.AppState.Accounts[this.TwitterAccountID].Cache.CachedUsers["@" + this.UserToAdd.ScreenName.ToLower()].UserImage(54, true, this.TwitterAccountID).Result;
                else
                    return (object)null;
            }
        }

        public BitmapImage UserDefaultImage
        {
            get
            {
                return CommonCommands.DefaultUserImage();
            }
        }

        public SelectTwitterListViewModel(Decimal TwitterAccountID, MetroTwitUser userToAdd)
        {
            SelectTwitterListViewModel twitterListViewModel = this;
            this.UserToAdd = userToAdd;
            this.TwitterAccountID = TwitterAccountID;
            this.TwitterLists = new ObservableCollection<TwitterListExtended>();
            this.FailedLists = new ObservableCollection<Tuple<IEnumerable<TwitterError>, TwitterList>>();
            this.SaveCommand = new RelayCommand(new Action(this.Save));
            this.CancelCommand = new RelayCommand(new Action(this.Cancel));
            this.ProgressText = "Loadings lists...";
            this.showAnimation = true;
            this.showLists = false;
            this.showFailedLists = false;
            this.countLock = new object();
            App.GetLists(this.TwitterAccountID, (Action<TwitterListCollection>)(listsCollection =>
            {
                if (listsCollection == null || listsCollection.Count <= 0)
                    return;
                MetroTwitTwitterizer.ListMembershipsOptions.FilterToOwnedLists = true;
                Lists.MembershipsAsync(App.AppState.Accounts[twitterListViewModel.TwitterAccountID].Tokens, userToAdd.ScreenName, MetroTwitTwitterizer.ListMembershipsOptions).ContinueWith((Action<Task<TwitterResponse<TwitterListCollection>>>)(response =>
                {
                    if (response.Result.Result == RequestResult.Success)
                    {
                        TwitterListCollection memberships = response.Result.ResponseObject;
                        foreach (TwitterList item_0 in (Collection<TwitterList>)listsCollection)
                        {
                            TwitterList item = item_0;
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                bool local_0 = Enumerable.FirstOrDefault<TwitterList>(Enumerable.Where<TwitterList>((IEnumerable<TwitterList>)memberships, (Func<TwitterList, bool>)(tl => tl.Id == item.Id))) != null;
                                ObservableCollection<TwitterListExtended> temp_155 = twitterListViewModel.TwitterLists;
                                TwitterListExtended temp_171 = new TwitterListExtended()
                                {
                                    TwitterAccountID = twitterListViewModel.TwitterAccountID,
                                    IsEditable = true,
                                    BaseListObject = item,
                                    IsSelected = local_0,
                                    AlreadyExistsInMembership = local_0
                                };
                                temp_155.Add(temp_171);
                            }));
                        }
                        twitterListViewModel.ShowAnimation = false;
                        twitterListViewModel.ShowLists = true;
                        twitterListViewModel.ShowFailedLists = false;
                    }
                    else
                        twitterListViewModel.ShowAnimation = false;
                }));
            }));
        }

        private void Cancel()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseCenterModalWindowHost);
        }

        private async void Save()
        {
            this.ShowLists = false;
            this.ShowAnimation = true;
            this.ProgressText = "Saving to lists...";
            this.totalCountOfLists = Enumerable.Count<TwitterListExtended>(Enumerable.Where<TwitterListExtended>((IEnumerable<TwitterListExtended>)this.TwitterLists, (Func<TwitterListExtended, bool>)(t => t.IsSelected && !t.AlreadyExistsInMembership || !t.IsSelected && t.AlreadyExistsInMembership)));
            UserAccountViewModel account = App.AppState.Accounts[this.TwitterAccountID];
            foreach (TwitterListExtended twitterListExtended in (Collection<TwitterListExtended>)this.TwitterLists)
            {
                if (twitterListExtended.IsSelected && !twitterListExtended.AlreadyExistsInMembership)
                {
                    TwitterResponse<TwitterList> response = await Lists.AddMemberAsync(account.Tokens, (Decimal)twitterListExtended.BaseListObject.Id, this.UserToAdd.Id, (OptionalProperties)MetroTwitTwitterizer.ListMembershipsOptions);
                    lock (this.countLock)
                    {
                        ++this.currentCount;
                        if (response.Result != RequestResult.Success)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.FailedLists.Add(new Tuple<IEnumerable<TwitterError>, TwitterList>(response.Errors, response.ResponseObject))));
                        }
                        else
                        {
                            TwitterListExtended local_0 = Enumerable.FirstOrDefault<TwitterListExtended>(Enumerable.Where<TwitterListExtended>((IEnumerable<TwitterListExtended>)this.TwitterLists, (Func<TwitterListExtended, bool>)(tl => tl.BaseListObject.Id == response.ResponseObject.Id)));
                            if (local_0 != null)
                                ++local_0.BaseListObject.NumberOfMembers;
                        }
                        this.CheckCurrentProgress();
                    }
                }
                else if (!twitterListExtended.IsSelected && twitterListExtended.AlreadyExistsInMembership)
                {
                    TwitterResponse<TwitterList> response = await Lists.RemoveMemberAsync(account.Tokens, (Decimal)twitterListExtended.BaseListObject.Id, this.UserToAdd.Id, (OptionalProperties)MetroTwitTwitterizer.ListMembershipsOptions);
                    lock (this.countLock)
                    {
                        ++this.currentCount;
                        if (response.Result != RequestResult.Success)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.FailedLists.Add(new Tuple<IEnumerable<TwitterError>, TwitterList>(response.Errors, response.ResponseObject))));
                        }
                        else
                        {
                            TwitterListExtended local_0_1 = Enumerable.FirstOrDefault<TwitterListExtended>(Enumerable.Where<TwitterListExtended>((IEnumerable<TwitterListExtended>)this.TwitterLists, (Func<TwitterListExtended, bool>)(tl => tl.BaseListObject.Id == response.ResponseObject.Id)));
                            if (local_0_1 != null)
                                --local_0_1.BaseListObject.NumberOfMembers;
                        }
                        this.CheckCurrentProgress();
                    }
                }
            }
        }

        private void CheckCurrentProgress()
        {
            if (this.currentCount != this.totalCountOfLists)
                return;
            if (this.FailedLists.Count > 0)
            {
                this.ShowAnimation = false;
                this.ShowFailedLists = true;
            }
            else
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.Cancel()));
        }
    }
}
