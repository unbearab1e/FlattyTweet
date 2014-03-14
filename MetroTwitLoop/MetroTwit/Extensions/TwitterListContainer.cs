// Type: MetroTwit.Extensions.TwitterListContainer
// Assembly: MetroTwitLoop, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9EE80294-B7B6-499E-A0BD-9F673A2F8B62
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0002_ea17dec1c6cb85eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MetroTwit.Extensions
{
  public class TwitterListContainer : MultiAccountViewModelBase
  {
    private readonly int MAXIMUM_TWITTER_LISTS = 20;
    private ObservableCollection<TwitterListExtended> lists;
    private bool showAddButton;

      private int order;
    public int Order
    {
      get
      {
        return this.order;
      }
      set
      {
        if (this.order == value)
          return;
        this.order = value;
        base.RaisePropertyChanged("Order");
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
        base.RaisePropertyChanged("Name");
      }
    }
      private RelayCommand addListCommand;
    public RelayCommand AddListCommand
    {
      get
      {
        return this.addListCommand;
      }
      set
      {
        if (this.addListCommand == value)
          return;
        this.addListCommand = value;
        base.RaisePropertyChanged("AddListCommand");
      }
    }

    public ObservableCollection<TwitterListExtended> Lists
    {
      get
      {
        return this.lists;
      }
      set
      {
        if (this.lists == value)
          return;
        this.lists = value;
        base.RaisePropertyChanged("Lists");
      }
    }

    public bool ShowAddButton
    {
      get
      {
        return this.showAddButton;
      }
      set
      {
        if (this.showAddButton == value)
          return;
        this.showAddButton = value;
        base.RaisePropertyChanged("ShowAddButton");
      }
    }

    public TwitterListContainer()
    {
      this.AddListCommand = new RelayCommand(new Action(this.AddList));
    }

    public void AddList()
    {
      if (this.Lists == null || Enumerable.Count<TwitterListExtended>((IEnumerable<TwitterListExtended>) this.Lists) < this.MAXIMUM_TWITTER_LISTS)
      {
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) "L"), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.SetPopupTarget));
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ManageList));
      }
      else
      {
        int num = (int) MessageBoxView.Show(string.Format("sorry, but twitter doesn't let you have more than {0} accounts :(", (object) this.MAXIMUM_TWITTER_LISTS), "maxed out", MessageBoxButton.OK, MessageBoxResult.OK);
      }
    }
  }
}
