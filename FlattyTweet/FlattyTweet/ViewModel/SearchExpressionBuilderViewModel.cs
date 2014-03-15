
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Text;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
  public class SearchExpressionBuilderViewModel : MultiAccountViewModelBase
  {
    private ViewType viewType;
    private bool showSaveSearchErrorPanel;
    private bool showMainContents;
    private bool showSavingSearchThrobber;

      private ObservableCollection<SearchTerm> searchTerms;
    public ObservableCollection<SearchTerm> SearchTerms
    {
      get
      {
        return this.searchTerms;
      }
      set
      {
        if (this.searchTerms == value)
          return;
        this.searchTerms = value;
        base.RaisePropertyChanged("SearchTerms");
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
      private RelayCommand<SearchTerm> removeSearchTermCommand;
    public RelayCommand<SearchTerm> RemoveSearchTermCommand
    {
      get
      {
        return this.removeSearchTermCommand;
      }
      set
      {
        if (this.removeSearchTermCommand == value)
          return;
        this.removeSearchTermCommand = value;
        base.RaisePropertyChanged("RemoveSearchTermCommand");
      }
    }
      private RelayCommand addSearchTermCommand;
    public RelayCommand AddSearchTermCommand
    {
      get
      {
        return this.addSearchTermCommand;
      }
      set
      {
        if (this.addSearchTermCommand == value)
          return;
        this.addSearchTermCommand = value;
        base.RaisePropertyChanged("AddSearchTermCommand");
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
      private string exclusions;
    public string Exclusions
    {
      get
      {
        return this.exclusions;
      }
      set
      {
        if (string.Equals(this.exclusions, value, StringComparison.Ordinal))
          return;
        this.exclusions = value;
        base.RaisePropertyChanged("Exclusions");
      }
    }
      private string tags;
    public string Tags
    {
      get
      {
        return this.tags;
      }
      set
      {
        if (string.Equals(this.tags, value, StringComparison.Ordinal))
          return;
        this.tags = value;
        base.RaisePropertyChanged("Tags");
      }
    }
      private string sources;
    public string Sources
    {
      get
      {
        return this.sources;
      }
      set
      {
        if (string.Equals(this.sources, value, StringComparison.Ordinal))
          return;
        this.sources = value;
        base.RaisePropertyChanged("Sources");
      }
    }
      private string fromPeople;
    public string FromPeople
    {
      get
      {
        return this.fromPeople;
      }
      set
      {
        if (string.Equals(this.fromPeople, value, StringComparison.Ordinal))
          return;
        this.fromPeople = value;
        base.RaisePropertyChanged("FromPeople");
      }
    }
      private string toPeople;
    public string ToPeople
    {
      get
      {
        return this.toPeople;
      }
      set
      {
        if (string.Equals(this.toPeople, value, StringComparison.Ordinal))
          return;
        this.toPeople = value;
        base.RaisePropertyChanged("ToPeople");
      }
    }
      private string mentioning;
    public string Mentioning
    {
      get
      {
        return this.mentioning;
      }
      set
      {
        if (string.Equals(this.mentioning, value, StringComparison.Ordinal))
          return;
        this.mentioning = value;
        base.RaisePropertyChanged("Mentioning");
      }
    }
      private bool positive;
    public bool Positive
    {
      get
      {
        return this.positive;
      }
      set
      {
        if (this.positive == value)
          return;
        this.positive = value;
        base.RaisePropertyChanged("Positive");
      }
    }
      private bool negative;
    public bool Negative
    {
      get
      {
        return this.negative;
      }
      set
      {
        if (this.negative == value)
          return;
        this.negative = value;
        base.RaisePropertyChanged("Negative");
      }
    }
      private bool question;
    public bool Question
    {
      get
      {
        return this.question;
      }
      set
      {
        if (this.question == value)
          return;
        this.question = value;
        base.RaisePropertyChanged("Question");
      }
    }
      private bool containsLinks;
    public bool ContainsLinks
    {
      get
      {
        return this.containsLinks;
      }
      set
      {
        if (this.containsLinks == value)
          return;
        this.containsLinks = value;
        base.RaisePropertyChanged("ContainsLinks");
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

    public SearchExpressionBuilderViewModel(ViewType viewType, Decimal TwitterAccountID)
    {
      this.TwitterAccountID = TwitterAccountID;
      this.viewType = viewType;
      this.SearchTerms = new ObservableCollection<SearchTerm>();
      this.SearchTerms.Add(new SearchTerm());
      this.SaveCommand = new RelayCommand(new Action(this.Save));
      this.CancelCommand = new RelayCommand(new Action(this.Cancel));
      this.RemoveSearchTermCommand = new RelayCommand<SearchTerm>(new Action<SearchTerm>(this.RemoveSearchTerm));
      this.AddSearchTermCommand = new RelayCommand(new Action(this.AddSearchTerm));
      this.DiscardShowSaveSearchErrorCommand = new RelayCommand(new Action(this.DiscardShowSaveSearchError));
      this.ShowMainContents = true;
    }

    private void DiscardShowSaveSearchError()
    {
      this.ShowSaveSearchErrorPanel = false;
      this.ShowMainContents = true;
    }

    private async void Save()
    {
      string query = this.BuildSearchQuery();
      if (string.IsNullOrWhiteSpace(query))
        query = "the meaning of life";
      if (this.SaveThisSearch)
      {
        this.ShowMainContents = false;
        this.ShowSavingSearchThrobber = true;
        TwitterResponse<TwitterSavedSearch> response = await SavedSearches.CreateAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, query, MetroTwitTwitterizer.Options);
        if (response.Result == RequestResult.Success)
        {
          System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.SearchBuilderClose);
            App.AppState.Accounts[this.TwitterAccountID].SavedSearches.Add(response.ResponseObject);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.SavedSearchesUpdated));
            InlinePopup.CurrentInline.Close();
            this.MaintainSearchMRU(query);
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) new Tuple<string, string, Decimal?, ViewType>(this.SearchName, query, new Decimal?(response.ResponseObject.Id), this.viewType)), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.Search));
          }));
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
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.SearchBuilderClose);
        this.MaintainSearchMRU(query);
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) new Tuple<string, string, Decimal?, ViewType>(this.SearchName, query, new Decimal?(), this.viewType)), (object) this.MultiAccountifyToken((Enum) ViewModelMessages.Search));
      }
    }

    private void MaintainSearchMRU(string query)
    {
      if (App.MostRecentSearchTerms.Count == 10)
        App.MostRecentSearchTerms.RemoveAt(0);
      App.MostRecentSearchTerms.Add(query);
    }

    private void Cancel()
    {
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) null), (object) ViewModelMessages.SearchBuilderClose);
    }

    private void AddSearchTerm()
    {
      this.SearchTerms.Add(new SearchTerm());
    }

    private void RemoveSearchTerm(SearchTerm searchTerm)
    {
      this.SearchTerms.Remove(searchTerm);
    }

    private string BuildSearchQuery()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (SearchTerm searchTerm in (Collection<SearchTerm>) this.SearchTerms)
      {
        if (searchTerm.IsExact)
        {
          if (!string.IsNullOrEmpty(searchTerm.SearchText))
          {
            stringBuilder.Append("\"");
            stringBuilder.Append(searchTerm.SearchText);
            stringBuilder.Append("\"");
          }
        }
        else if (!string.IsNullOrEmpty(searchTerm.SearchText))
          stringBuilder.Append(searchTerm.SearchText);
        switch (searchTerm.SearchOperator.Content.ToString())
        {
          case "AND":
            stringBuilder.Append(' ');
            break;
          case "OR":
            stringBuilder.Append(" OR ");
            break;
        }
      }
      if (!string.IsNullOrEmpty(this.Exclusions))
      {
        string str1 = this.Exclusions.Trim();
        string[] separator = new string[1]
        {
          Environment.NewLine
        };
        int num = 1;
        foreach (string str2 in str1.Split(separator, (StringSplitOptions) num))
          stringBuilder.Append(string.Format("-{0} ", (object) str2));
      }
      if (!string.IsNullOrEmpty(this.Tags))
      {
        string str1 = this.Tags.Trim();
        string[] separator = new string[1]
        {
          Environment.NewLine
        };
        int num = 1;
        foreach (string str2 in str1.Split(separator, (StringSplitOptions) num))
          stringBuilder.Append(string.Format("#{0} OR ", (object) str2.Replace("#", "")));
      }
      if (!string.IsNullOrEmpty(this.Sources))
      {
        string str1 = this.Sources.Trim();
        string[] separator = new string[1]
        {
          Environment.NewLine
        };
        int num = 1;
        foreach (string str2 in str1.Split(separator, (StringSplitOptions) num))
          stringBuilder.Append(string.Format("source:{0} ", (object) str2));
      }
      if (!string.IsNullOrEmpty(this.FromPeople))
      {
        string[] strArray = this.FromPeople.Trim().Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
        int num = 0;
        foreach (string str in strArray)
          stringBuilder.Append(string.Format(num++ == strArray.Length - 1 ? "from:{0} " : "from:{0} OR ", (object) str.Replace("@", "")));
      }
      if (!string.IsNullOrEmpty(this.ToPeople))
      {
        string[] strArray = this.ToPeople.Trim().Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
        int num = 0;
        foreach (string str in strArray)
          stringBuilder.Append(string.Format(num++ == strArray.Length - 1 ? "to:{0} " : "to:{0} OR ", (object) str.Replace("@", "")));
      }
      if (!string.IsNullOrEmpty(this.Mentioning))
      {
        string[] strArray = this.Mentioning.Trim().Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
        int num = 0;
        foreach (string str in strArray)
          stringBuilder.Append(string.Format(num++ == strArray.Length - 1 ? "@{0} " : "@{0} OR ", (object) str.Replace("@", "")));
      }
      if (this.Positive)
        stringBuilder.Append(":) ");
      if (this.Negative)
        stringBuilder.Append(":( ");
      if (this.Question)
        stringBuilder.Append("? ");
      if (this.ContainsLinks)
        stringBuilder.Append("filter:links ");
      if (string.IsNullOrWhiteSpace(((object) stringBuilder).ToString()))
        stringBuilder.Append("the meaning of life");
      return ((object) stringBuilder).ToString();
    }
  }
}
