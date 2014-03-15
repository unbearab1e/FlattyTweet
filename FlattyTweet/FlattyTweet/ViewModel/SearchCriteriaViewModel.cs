
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.View;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class SearchCriteriaViewModel : MultiAccountViewModelBase
  {
    private ViewType viewType;
    private bool showListOfSavedSearches;
    private bool showDeletingSavedSearchThrobber;
    private bool showDeleteSavedSearchErrorPanel;
    private string deleteSavedSearchErrorMessage;
    private bool showNoSavedSearchesMessage;
    private bool expandSavedSearches;
    private bool showSaveSearchErrorPanel;
    private bool showMainContents;
    private bool showSavingSearchThrobber;
    private int deleteRetryAttempts;
    private string[] deleteRetryErrorMessages;
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
      private RelayCommand advancedCommand;
    public RelayCommand AdvancedCommand
    {
      get
      {
        return this.advancedCommand;
      }
      set
      {
        if (this.advancedCommand == value)
          return;
        this.advancedCommand = value;
        base.RaisePropertyChanged("AdvancedCommand");
      }
    }
      private RelayCommand discardSavedSearchErrorCommand;
    public RelayCommand DiscardSavedSearchErrorCommand
    {
      get
      {
        return this.discardSavedSearchErrorCommand;
      }
      set
      {
        if (this.discardSavedSearchErrorCommand == value)
          return;
        this.discardSavedSearchErrorCommand = value;
        base.RaisePropertyChanged("DiscardSavedSearchErrorCommand");
      }
    }
      private RelayCommand discardShowSaveSearchErrorCommand;
    public RelayCommand DiscardShowSaveSearchErrorCommand
    {
      get
      {
        return this.discardShowSaveSearchErrorCommand;
      }
      set
      {
        if (this.discardShowSaveSearchErrorCommand == value)
          return;
        this.discardShowSaveSearchErrorCommand = value;
        base.RaisePropertyChanged("DiscardShowSaveSearchErrorCommand");
      }
    }
      private RelayCommand <TwitterSavedSearch> deleteSavedSearchCommand;
    public RelayCommand<TwitterSavedSearch> DeleteSavedSearchCommand
    {
      get
      {
        return this.deleteSavedSearchCommand;
      }
      set
      {
        if (this.deleteSavedSearchCommand == value)
          return;
        this.deleteSavedSearchCommand = value;
        base.RaisePropertyChanged("DeleteSavedSearchCommand");
      }
    }
      private RelayCommand<TwitterSavedSearch> openSavedSearchCommand;
    public RelayCommand<TwitterSavedSearch> OpenSavedSearchCommand
    {
      get
      {
        return this.openSavedSearchCommand;
      }
      set
      {
        if (this.openSavedSearchCommand == value)
          return;
        this.openSavedSearchCommand = value;
        base.RaisePropertyChanged("OpenSavedSearchCommand");
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
      private string searchName;
    public string SearchName
    {
      get
      {
        return this.searchName;
      }
      set
      {
        if (string.Equals(this.searchName, value, StringComparison.Ordinal))
          return;
        this.searchName = value;
        base.RaisePropertyChanged("SearchName");
      }
    }

    public string DeleteSavedSearchErrorMessage
    {
      get
      {
        return this.deleteSavedSearchErrorMessage;
      }
      set
      {
        if (string.Equals(this.deleteSavedSearchErrorMessage, value, StringComparison.Ordinal))
          return;
        this.deleteSavedSearchErrorMessage = value;
        base.RaisePropertyChanged("DeleteSavedSearchErrorMessage");
      }
    }
      private bool saveThisSearch;
    public bool SaveThisSearch
    {
      get
      {
        return this.saveThisSearch;
      }
      set
      {
        if (this.saveThisSearch == value)
          return;
        this.saveThisSearch = value;
        base.RaisePropertyChanged("SaveThisSearch");
      }
    }

    public bool ExpandSavedSearches
    {
      get
      {
        return this.expandSavedSearches;
      }
      set
      {
        if (this.expandSavedSearches == value)
          return;
        this.expandSavedSearches = value;
        base.RaisePropertyChanged("ExpandSavedSearches");
      }
    }

    public bool ShowSavingSearchThrobber
    {
      get
      {
        return this.showSavingSearchThrobber;
      }
      set
      {
        if (this.showSavingSearchThrobber == value)
          return;
        this.showSavingSearchThrobber = value;
        base.RaisePropertyChanged("ShowSavingSearchThrobber");
      }
    }

    public bool ShowMainContents
    {
      get
      {
        return this.showMainContents;
      }
      set
      {
        if (this.showMainContents == value)
          return;
        this.showMainContents = value;
        base.RaisePropertyChanged("ShowMainContents");
      }
    }

    public bool ShowNoSavedSearchesMessage
    {
      get
      {
        return this.showNoSavedSearchesMessage;
      }
      set
      {
        if (this.showNoSavedSearchesMessage == value)
          return;
        this.showNoSavedSearchesMessage = value;
        base.RaisePropertyChanged("ShowNoSavedSearchesMessage");
      }
    }

    public bool ShowListOfSavedSearches
    {
      get
      {
        return this.showListOfSavedSearches;
      }
      set
      {
        if (this.showListOfSavedSearches == value)
          return;
        this.showListOfSavedSearches = value;
        base.RaisePropertyChanged("ShowListOfSavedSearches");
      }
    }

    public bool ShowDeletingSavedSearchThrobber
    {
      get
      {
        return this.showDeletingSavedSearchThrobber;
      }
      set
      {
        if (this.showDeletingSavedSearchThrobber == value)
          return;
        this.showDeletingSavedSearchThrobber = value;
        base.RaisePropertyChanged("ShowDeletingSavedSearchThrobber");
      }
    }

    public bool ShowDeleteSavedSearchErrorPanel
    {
      get
      {
        return this.showDeleteSavedSearchErrorPanel;
      }
      set
      {
        if (this.showDeleteSavedSearchErrorPanel == value)
          return;
        this.showDeleteSavedSearchErrorPanel = value;
        base.RaisePropertyChanged("ShowDeleteSavedSearchErrorPanel");
      }
    }

    public bool ShowSaveSearchErrorPanel
    {
      get
      {
        return this.showSaveSearchErrorPanel;
      }
      set
      {
        if (this.showSaveSearchErrorPanel == value)
          return;
        this.showSaveSearchErrorPanel = value;
        base.RaisePropertyChanged("ShowSaveSearchErrorPanel");
      }
    }

    public ObservableCollection<TwitterSavedSearch> SavedSearches
    {
      get
      {
        return App.AppState.Accounts[this.TwitterAccountID].SavedSearches;
      }
    }

    public SearchCriteriaViewModel(ViewType viewType, Decimal TwitterAccountID)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.CancelCommand = new RelayCommand(new Action(this.Cancel));
      this.OKCommand = new RelayCommand(new Action(this.OK));
      this.AdvancedCommand = new RelayCommand(new Action(this.Advanced));
      this.DiscardShowSaveSearchErrorCommand = new RelayCommand(new Action(this.DiscardShowSaveSearchError));
      this.DiscardSavedSearchErrorCommand = new RelayCommand(new Action(this.DiscardSavedSearchError));
      this.DeleteSavedSearchCommand = new RelayCommand<TwitterSavedSearch>(new Action<TwitterSavedSearch>(this.DeleteSavedSearch));
      this.OpenSavedSearchCommand = new RelayCommand<TwitterSavedSearch>(new Action<TwitterSavedSearch>(this.OpenSavedSearch));
      this.viewType = viewType;
      this.deleteRetryErrorMessages = new string[4]
      {
        "Damn, we couldn't delete it, maybe try again?",
        "Success!... Doh, false alarm, maybe give it a rest for a bit",
        "Third time lucky? Nope, try again a little later, sorry :(",
        "No really, it's not working, we promise we'll fix it soon"
      };
      this.ShowMainContents = true;
      this.ShowNoSavedSearchesMessage = App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Count == 0;
      this.ShowListOfSavedSearches = App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Count > 0;
      this.ExpandSavedSearches = App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Count > 0;
    }

    private void DiscardShowSaveSearchError()
    {
      this.ShowSaveSearchErrorPanel = false;
      this.ShowMainContents = true;
    }

    private void DiscardSavedSearchError()
    {
      this.ShowListOfSavedSearches = true;
      this.ShowDeletingSavedSearchThrobber = false;
      this.ShowDeleteSavedSearchErrorPanel = false;
    }

    private void OpenSavedSearch(TwitterSavedSearch savedSearch)
    {
      InlinePopup.CurrentInline.Close();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) new Tuple<string, string, Decimal?, ViewType>(savedSearch.Name, savedSearch.Query, new Decimal?(savedSearch.Id), this.viewType)), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.Search));
    }

    private async void DeleteSavedSearch(TwitterSavedSearch searchToDelete)
    {
        Action callback = null;
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(Visibility.Visible), ViewModelMessages.OverlayVisible);
        if (MessageBoxView.Show("Remove this saved search? It cannot be undone.", "delete saved search", MessageBoxButton.YesNo, MessageBoxResult.No) == MessageBoxResult.Yes)
        {
            this.ShowListOfSavedSearches = false;
            this.ShowDeletingSavedSearchThrobber = true;
            this.ShowDeleteSavedSearchErrorPanel = false;
            TwitterResponse<TwitterSavedSearch> searchdel = await Twitterizer.SavedSearches.DeleteAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, searchToDelete.Id, MetroTwitTwitterizer.Options);
            if (searchdel.Result == RequestResult.Success)
            {
                if (callback == null)
                {
                    callback = delegate
                    {
                        App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Remove(searchToDelete);
                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), this.MultiAccountifyToken(ViewModelMessages.SavedSearchesUpdated));
                    };
                }
                System.Windows.Application.Current.Dispatcher.Invoke(callback);
                this.ShowDeleteSavedSearchErrorPanel = false;
                this.ShowDeletingSavedSearchThrobber = false;
                if (App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Count == 0)
                {
                    this.ShowListOfSavedSearches = false;
                    this.ShowNoSavedSearchesMessage = true;
                    this.ExpandSavedSearches = false;
                }
                else
                {
                    this.ShowListOfSavedSearches = true;
                }
            }
            else
            {
                this.ShowDeletingSavedSearchThrobber = false;
                this.ShowDeleteSavedSearchErrorPanel = true;
                if ((this.deleteRetryAttempts + 1) < this.deleteRetryErrorMessages.Length)
                {
                    this.DeleteSavedSearchErrorMessage = this.deleteRetryErrorMessages[this.deleteRetryAttempts++];
                }
                else
                {
                    this.DeleteSavedSearchErrorMessage = this.deleteRetryErrorMessages[this.deleteRetryErrorMessages.Length - 1];
                }
            }
        }
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(Visibility.Collapsed), ViewModelMessages.OverlayVisible);
    }

    private void Advanced()
    {
      InlinePopup.CurrentInline.Close();
      SearchExpressionBuilderView expressionBuilderView1 = new SearchExpressionBuilderView();
      expressionBuilderView1.Owner = System.Windows.Application.Current.MainWindow;
      expressionBuilderView1.DataContext = (object) new SearchExpressionBuilderViewModel(this.viewType, this.TwitterAccountID);
      SearchExpressionBuilderView expressionBuilderView2 = expressionBuilderView1;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
      expressionBuilderView2.ShowDialog();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
    }

    private async void OK()
        {
            if (string.IsNullOrWhiteSpace(this.SearchQuery))
            {
                this.SearchQuery = "the meaning of life";
            }
            if (this.SaveThisSearch)
            {
                
                Action callback = null;
                this.ShowMainContents = false;
                this.ShowSavingSearchThrobber = true;

                TwitterResponse<TwitterSavedSearch> createresp = await Twitterizer.SavedSearches.CreateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, this.SearchQuery, MetroTwitTwitterizer.Options); ;
                
                
                if (createresp.Result == RequestResult.Success)
                {
                    if (callback == null)
                    {
                        callback = delegate {
                            App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Add(createresp.ResponseObject);
                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), this.MultiAccountifyToken(ViewModelMessages.SavedSearchesUpdated));
                            InlinePopup.CurrentInline.Close();
                            this.MaintainSearchMRU();
                            TwitterSavedSearch responseObject = createresp.ResponseObject;
                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(new Tuple<string, string, decimal?, ViewType>(this.SearchName, this.SearchQuery, new decimal?(responseObject.Id), this.viewType)), this.MultiAccountifyToken(ViewModelMessages.Search));
                        };
                    }
                    System.Windows.Application.Current.Dispatcher.Invoke(callback);
                }
                else
                {
                    this.ShowSaveSearchErrorPanel = true;
                    this.ShowSavingSearchThrobber = false;
                    this.ShowMainContents = false;
                }
            }
            else
            {
                InlinePopup.CurrentInline.Close();
                this.MaintainSearchMRU();
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(new Tuple<string, string, decimal?, ViewType>(this.SearchName, this.SearchQuery, null, this.viewType)), this.MultiAccountifyToken(ViewModelMessages.Search));
            }
        }

    private void MaintainSearchMRU()
    {
      if (App.MostRecentSearchTerms.Count == 7)
        App.MostRecentSearchTerms.RemoveAt(App.MostRecentSearchTerms.Count - 1);
      App.MostRecentSearchTerms.Insert(0, this.SearchQuery);
    }

    private void Cancel()
    {
      InlinePopup.CurrentInline.Close();
    }
  }
}
