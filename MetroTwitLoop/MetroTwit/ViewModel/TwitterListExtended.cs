// Type: MetroTwit.ViewModel.TwitterListExtended
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.View;
using System;
using System.Windows;
using System.Windows.Controls;
using Twitterizer;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class TwitterListExtended : MultiAccountViewModelBase
  {
    private bool isSelected;
      private RelayCommand removeListCommand;
   public RelayCommand RemoveListCommand
    {
      get
      {
        return this.removeListCommand;
      }
      set
      {
        if (this.removeListCommand == value)
          return;
        this.removeListCommand = value;
        base.RaisePropertyChanged("RemoveListCommand");
      }
    }
      private RelayCommand editListCommand;
    public RelayCommand EditListCommand
    {
      get
      {
        return this.editListCommand;
      }
      set
      {
        if (this.editListCommand == value)
          return;
        this.editListCommand = value;
        base.RaisePropertyChanged("EditListCommand");
      }
    }
      private RelayCommand unsubscribeCommand;
    public RelayCommand UnsubscribeCommand
    {
      get
      {
        return this.unsubscribeCommand;
      }
      set
      {
        if (this.unsubscribeCommand == value)
          return;
        this.unsubscribeCommand = value;
        base.RaisePropertyChanged("UnsubscribeCommand");
      }
    }
      private bool isEditable;
    public bool IsEditable
    {
      get
      {
        return this.isEditable;
      }
      set
      {
        if (this.isEditable == value)
          return;
        this.isEditable = value;
        base.RaisePropertyChanged("IsEditable");
      }
    }
      private bool isUnsubscribeVisible;
    public bool IsUnsubscribeVisible
    {
      get
      {
        return this. isUnsubscribeVisible;
      }
      set
      {
        if (this. isUnsubscribeVisible == value)
          return;
        this. isUnsubscribeVisible = value;
        base.RaisePropertyChanged("IsUnsubscribeVisible");
      }
    }
      private TwitterList baseListObject;
    public TwitterList BaseListObject
    {
      get
      {
        return this.baseListObject;
      }
      set
      {
        if (this.baseListObject == value)
          return;
        this.baseListObject = value;
        base.RaisePropertyChanged("BaseListObject");
      }
    }
      private TwitterListContainer parentContainer;
    public TwitterListContainer ParentContainer
    {
      get
      {
        return this.parentContainer;
      }
      set
      {
        if (this.parentContainer == value)
          return;
        this.parentContainer = value;
        base.RaisePropertyChanged("ParentContainer");
      }
    }

    public bool IsSelected
    {
      get
      {
        return this.isSelected;
      }
      set
      {
        if (this.isSelected == value)
          return;
        this.isSelected = value;
        base.RaisePropertyChanged("IsSelected");
      }
    }
      private bool alreadyExistsInMembership;
    public bool AlreadyExistsInMembership
    {
      get
      {
        return this.alreadyExistsInMembership;
      }
      set
      {
        if (this.alreadyExistsInMembership == value)
          return;
        this.alreadyExistsInMembership = value;
        base.RaisePropertyChanged("AlreadyExistsInMembership");
      }
    }

    public TwitterListExtended()
    {
      this.EditListCommand = new RelayCommand(new Action(this.EditList));
      this.RemoveListCommand = new RelayCommand(new Action(this.RemoveList));
      this.UnsubscribeCommand = new RelayCommand(new Action(this.UnsubscribeList));
    }

    private async void UnsubscribeList()
        {
            if (MessageBoxView.Show(string.Format("Unsubscribe from the list \"{0}\"?", this.BaseListObject.FullName), "unsubscribe list", MessageBoxButton.YesNo, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                UserAccountViewModel account = App.AppState.Accounts[this.TwitterAccountID];
                GenericProgressAndMessagePopupViewModel genericMessageViewModel = new GenericProgressAndMessagePopupViewModel(new Action(this.NavigateBack)) {
                    ProgressText = "Unsubscribing from list...",
                    ShowAnimation = true
                };
                GenericProgressAndMessagePopupView content = new GenericProgressAndMessagePopupView {
                    DataContext = genericMessageViewModel
                };
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(content), this.MultiAccountifyToken(ViewModelMessages.ShowExistingViewInPopup));
                TwitterResponse<TwitterList> listUnsubscribeResult = await Lists.UnSubscribeAsync(account.Tokens, this.BaseListObject.Id, MetroTwitTwitterizer.Options);
                if (listUnsubscribeResult.Result == RequestResult.Success)
                {
                    this.ParentContainer.Lists.Remove(this);
                    this.ParentContainer = null;
                    this.NavigateBack();
                }
                else
                {
                    string responseResultMessage = CommonCommands.GetResponseResultMessage(listUnsubscribeResult.Result);
                    genericMessageViewModel.ShowAnimation = false;
                    genericMessageViewModel.ShowMessage = true;
                    genericMessageViewModel.Message = responseResultMessage;
                }
            }
        }

    private async void RemoveList()
    {
        if (MessageBoxView.Show(string.Format("Delete the \"{0}\" list? It cannot be recovered.", this.BaseListObject.FullName), "delete list", MessageBoxButton.YesNo, MessageBoxResult.No) == MessageBoxResult.Yes)
        {
            UserAccountViewModel account = App.AppState.Accounts[this.TwitterAccountID];
            GenericProgressAndMessagePopupViewModel genericMessageViewModel = new GenericProgressAndMessagePopupViewModel(new Action(this.BackToLists))
            {
                ProgressText = "Deleting list...",
                ShowAnimation = true
            };
            GenericProgressAndMessagePopupView content = new GenericProgressAndMessagePopupView
            {
                DataContext = genericMessageViewModel
            };
            Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(content), this.MultiAccountifyToken(ViewModelMessages.ShowExistingViewInPopup));
            TwitterResponse<TwitterList> listdel = await Lists.DeleteAsync(account.Tokens, this.BaseListObject.Id.ToString(), MetroTwitTwitterizer.Options);
            if (listdel.Result == RequestResult.Success)
            {
                this.ParentContainer.Lists.Remove(this);
                this.ParentContainer = null;
                this.BackToLists();
            }
            else
            {
                string responseResultMessage = CommonCommands.GetResponseResultMessage(listdel.Result);
                genericMessageViewModel.ShowAnimation = false;
                genericMessageViewModel.ShowMessage = true;
                genericMessageViewModel.Message = responseResultMessage;
            }
        }
    }

    private void NavigateBack()
    {
      System.Windows.Application.Current.Dispatcher.Invoke((Action) (() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.NavigateBack)));
    }

    private void BackToLists()
    {
      System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) "L"), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.SetPopupTarget));
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ShowLists));
      }));
    }

    private void EditList()
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) "L"), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.SetPopupTarget));
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) this.BaseListObject), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ManageList));
    }
  }
}
