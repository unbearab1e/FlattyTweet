
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class ManageTwitterListViewModel : MultiAccountViewModelBase, IDataErrorInfo, IPopupViewModel
  {
    private bool skipInitialValidation;
    private bool showAnimation;
    private bool showForm;
    private bool showMembersList;
    private bool isPublic;
    private TwitterList existingList;
    private string progressText;
    private string errorMessage;
    private bool showErrorMessage;
    private int totalCountOfUsers;
    private int currentCount;
    private object countLock;
      private RelayCommand saveCommand;
   public RelayCommand SaveCommand
    {
      get
      {
        return this.saveCommand;
      }
      private set
      {
        if (this.saveCommand == value)
          return;
        this.saveCommand = value;
        base.RaisePropertyChanged("SaveCommand");
      }
    }
      private RelayCommand oKCommand;
    public RelayCommand OKCommand
    {
      get
      {
        return this.oKCommand;
      }
      private set
      {
        if (this.oKCommand == value)
          return;
        this.oKCommand = value;
        base.RaisePropertyChanged("OKCommand");
      }
    }

    public bool IsValid
    {
      get
      {
        bool flag = true;
        foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        {
          if (!string.IsNullOrEmpty(this[propertyInfo.Name]))
          {
            base.RaisePropertyChanged(propertyInfo.Name);
            flag = false;
            break;
          }
        }
        return flag;
      }
    }
      private ObservableCollection<MetroTwitUser> usersInThisList;
    public ObservableCollection<MetroTwitUser> UsersInThisList
    {
      get
      {
        return this.usersInThisList;
      }
      set
      {
        if (this.usersInThisList == value)
          return;
        this.usersInThisList = value;
        base.RaisePropertyChanged("UsersInThisList");
      }
    }
      private string name;
    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        if (string.Equals(this.name, value, StringComparison.Ordinal))
          return;
        this.name = value;
        base.RaisePropertyChanged("Item");
        base.RaisePropertyChanged("IsValid");
        base.RaisePropertyChanged("Name");
      }
    }
      private string description;
    public string Description
    {
      get
      {
        return this.description;
      }
      set
      {
        if (string.Equals(this.description, value, StringComparison.Ordinal))
          return;
        this.description = value;
        base.RaisePropertyChanged("Description");
      }
    }

    public bool IsPublic
    {
      get
      {
        return this.isPublic;
      }
      set
      {
        if (this.isPublic == value)
          return;
        this.isPublic = value;
        base.RaisePropertyChanged("IsPublic");
      }
    }

    public bool ShowMembersList
    {
      get
      {
        return this.showMembersList;
      }
      set
      {
        if (this.showMembersList == value)
          return;
        this.showMembersList = value;
        base.RaisePropertyChanged("ShowMembersList");
      }
    }

    public bool ShowForm
    {
      get
      {
        return this.showForm;
      }
      set
      {
        if (this.showForm == value)
          return;
        this.showForm = value;
        base.RaisePropertyChanged("ShowForm");
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

    public bool ShowErrorMessage
    {
      get
      {
        return this.showErrorMessage;
      }
      set
      {
        if (this.showErrorMessage == value)
          return;
        this.showErrorMessage = value;
        base.RaisePropertyChanged("ShowErrorMessage");
      }
    }

    public string ErrorMessage
    {
      get
      {
        return this.errorMessage;
      }
      set
      {
        if (string.Equals(this.errorMessage, value, StringComparison.Ordinal))
          return;
        this.errorMessage = value;
        base.RaisePropertyChanged("ErrorMessage");
      }
    }

    public string Error
    {
      get
      {
        return (string) null;
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

    public string this[string columnName]
    {
      get
      {
        if (!this.skipInitialValidation && (columnName == "Name" && string.IsNullOrEmpty(this.Name)))
          return "Please enter in a name";
        this.skipInitialValidation = false;
        return (string) null;
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

    public ManageTwitterListViewModel(TwitterList existingList, Decimal TwitterAccountID)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.SaveCommand = new RelayCommand(new Action(this.Save));
      this.OKCommand = new RelayCommand(new Action(this.OK));
      this.skipInitialValidation = true;
      this.showForm = false;
      this.Description = string.Empty;
      this.UsersInThisList = new ObservableCollection<MetroTwitUser>();
      this.existingList = existingList;
      this.ShowAnimation = true;
      this.ProgressText = "Loading list...";
      this.countLock = new object();
      this.IsPublic = true;
      this.ShowMembersList = false;
      if (existingList != null)
      {
        this.Name = existingList.Name;
        this.Description = existingList.Description;
        this.IsPublic = existingList.IsPublic;
        Lists.MembersAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, (Decimal) existingList.Id, MetroTwitTwitterizer.GetListMembersOptions).ContinueWith((Action<Task<TwitterResponse<TwitterUserCollection>>>) (response =>
        {
          if (response.Result.Result != RequestResult.Success)
            return;
          System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            foreach (User item_0 in (Collection<User>) response.Result.ResponseObject)
              this.UsersInThisList.Add(new MetroTwitUser(item_0)
              {
                AlreadyExistsInList = true,
                IsSelected = true
              });
            this.ShowAnimation = false;
            this.ShowForm = true;
            this.ShowMembersList = this.UsersInThisList.Count > 0;
          }));
        }));
      }
      else
      {
        this.ShowAnimation = false;
        this.ShowForm = true;
      }
    }

    private async void Save()
    {
        if (this.IsValid)
        {
            string responseResultMessage;
            this.ProgressText = "Saving list...";
            this.ShowForm = false;
            this.ShowAnimation = true;
            UserAccountViewModel account = App.AppState.Accounts[this.TwitterAccountID];
            if (this.existingList == null)
            {
                TwitterResponse<TwitterList> response = await Lists.NewAsync(account.Tokens, this.Name, this.IsPublic, this.Description, MetroTwitTwitterizer.Options);
                TwitterResponse<TwitterList> newListResponse = response;
                if (newListResponse.Result == RequestResult.Success)
                {
                    this.OK();
                }
                else
                {
                    responseResultMessage = CommonCommands.GetResponseResultMessage(newListResponse.Result);
                    this.ShowAnimation = false;
                    this.ShowErrorMessage = true;
                    this.ErrorMessage = responseResultMessage;
                }
            }
            else
            {
                this.totalCountOfUsers = (from u in this.UsersInThisList
                                          where !u.IsSelected && u.AlreadyExistsInList
                                          select u).Count<MetroTwitUser>();
                UpdateListOptions options = new UpdateListOptions
                {
                    Name = this.Name,
                    Description = this.Description,
                    IsPublic = new bool?(this.IsPublic),
                    UseSSL = true
                };
                TwitterResponse < TwitterList > updateListResponse = await Lists.UpdateAsync(account.Tokens, this.existingList.Id.ToString(), options);
                if (updateListResponse.Result == RequestResult.Success)
                {
                    foreach (MetroTwitUser user in this.UsersInThisList)
                    {
                        if (!user.IsSelected && user.AlreadyExistsInList)
                        {
                            TwitterResponse<TwitterList> asyncVariable0 = await Lists.RemoveMemberAsync(account.Tokens, this.existingList.Id, user.BaseUser.Id, MetroTwitTwitterizer.Options);
                            
                            lock (this.countLock)
                            {
                                this.currentCount++;
                                if (asyncVariable0.Result == RequestResult.Success)
                                {
                                    this.existingList.NumberOfMembers--;
                                }
                                if (this.totalCountOfUsers == this.currentCount)
                                {
                                    this.OK();
                                }
                            }
                        }
                    }
                }
                else
                {
                    responseResultMessage = CommonCommands.GetResponseResultMessage(updateListResponse.Result);
                    this.ShowAnimation = false;
                    this.ShowErrorMessage = true;
                    this.ErrorMessage = responseResultMessage;
                }
            }
        }
    }

    private void OK()
    {
      System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        PopupService.PurgeHistory();
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ShowLists));
      }));
    }
  }
}
