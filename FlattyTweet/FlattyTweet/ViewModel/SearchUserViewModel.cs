
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using System;

namespace FlattyTweet.ViewModel
{
  public class SearchUserViewModel : MultiAccountViewModelBase
  {
    private ViewType viewType;
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
      private RelayCommand okCommand;
    public RelayCommand OKCommand
    {
      get
      {
        return this.okCommand;
      }
      set
      {
        if (this.okCommand == value)
          return;
        this.okCommand = value;
        base.RaisePropertyChanged("OKCommand");
      }
    }
      private string searchQuery;
    public string SearchQuery
    {
      get
      {
        return this.searchQuery;
      }
      set
      {
        if (string.Equals(this.searchQuery, value, StringComparison.Ordinal))
          return;
        this.searchQuery = value;
        base.RaisePropertyChanged("SearchQuery");
      }
    }

    public SearchUserViewModel(ViewType viewType, Decimal TwitterAccountID)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.CancelCommand = new RelayCommand(new Action(this.Cancel));
      this.OKCommand = new RelayCommand(new Action(this.OK));
      this.viewType = viewType;
    }

    private void OK()
    {
      InlinePopup.CurrentInline.Close();
      if (string.IsNullOrEmpty(this.SearchQuery))
        return;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) new Tuple<string, ViewType>(this.SearchQuery, this.viewType)), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.ShowUserProfile));
    }

    private void Cancel()
    {
      InlinePopup.CurrentInline.Close();
    }
  }
}
