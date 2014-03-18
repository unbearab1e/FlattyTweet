//using Bugsense.WPF;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.ViewModel
{
    public class AccountManagementViewModel : MultiAccountViewModelBase
    {
        private bool progressVisible = true;
        private Visibility followVisible = Visibility.Collapsed;
        private string followText = "follow us";
        private bool followIsEnabled = true;
        private string tweetusText = "tweet";
        private bool tweetUsIsEnabled = true;
        private OAuthTokenResponse requestToken;
        private string url;

        private bool isOOBE;
        public bool IsOOBE
        {
            get
            {
                return this.isOOBE;
            }
            set
            {
                if (this.isOOBE == value)
                    return;
                this.isOOBE = value;
                base.RaisePropertyChanged("IsOOBE");
            }
        }

        public string URL
        {
            get
            {
                return this.url;
            }
            set
            {
                if (string.Equals(this.url, value, StringComparison.Ordinal))
                    return;
                this.url = value;
                if (this.URL.Contains("https://twitter.com/FlattyTweet"))
                    this.SignIn();
                else
                    base.RaisePropertyChanged("URL");
            }
        }

        public bool ProgressVisible
        {
            get
            {
                return this.progressVisible;
            }
            set
            {
                if (this.progressVisible == value)
                    return;
                this.progressVisible = value;
                base.RaisePropertyChanged("ProgressVisible");
            }
        }

        public Visibility FollowVisible
        {
            get
            {
                return this.followVisible;
            }
            set
            {
                if (this.followVisible == value)
                    return;
                this.followVisible = value;
                base.RaisePropertyChanged("FollowVisible");
            }
        }

        public string FollowText
        {
            get
            {
                return this.followText;
            }
            set
            {
                if (string.Equals(this.followText, value, StringComparison.Ordinal))
                    return;
                this.followText = value;
                base.RaisePropertyChanged("FollowText");
            }
        }

        public bool FollowIsEnabled
        {
            get
            {
                return this.followIsEnabled;
            }
            set
            {
                if (this.followIsEnabled == value)
                    return;
                this.followIsEnabled = value;
                base.RaisePropertyChanged("FollowIsEnabled");
            }
        }

        public string TweetUsText
        {
            get
            {
                return this.tweetusText;
            }
            set
            {
                if (string.Equals(this.tweetusText, value, StringComparison.Ordinal))
                    return;
                this.tweetusText = value;
                base.RaisePropertyChanged("TweetUsText");
            }
        }

        public bool TweetUsIsEnabled
        {
            get
            {
                return this.tweetUsIsEnabled;
            }
            set
            {
                if (this.tweetUsIsEnabled == value)
                    return;
                this.tweetUsIsEnabled = value;
                base.RaisePropertyChanged("TweetUsIsEnabled");
            }
        }

        private RelayCommand addUserAccountCommand;
        public RelayCommand AddUserAccountCommand
        {
            get
            {
                return this.addUserAccountCommand;
            }
            private set
            {
                if (this.addUserAccountCommand == value)
                    return;
                this.addUserAccountCommand = value;
                base.RaisePropertyChanged("AddUserAccountCommand");
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return this.cancelCommand;
            }
            private set
            {
                if (this.cancelCommand == value)
                    return;
                this.cancelCommand = value;
                base.RaisePropertyChanged("CancelCommand");
            }
        }

        private RelayCommand quitCommand;
        public RelayCommand QuitCommand
        {
            get
            {
                return this.quitCommand;
            }
            private set
            {
                if (this.quitCommand == value)
                    return;
                this.quitCommand = value;
                base.RaisePropertyChanged("QuitCommand");
            }
        }
        private RelayCommand manageAccountsCommand;
        public RelayCommand ManageAccountsCommand
        {
            get
            {
                return this.manageAccountsCommand;
            }
            private set
            {
                if (this.manageAccountsCommand == value)
                    return;
                this.manageAccountsCommand = value;
                base.RaisePropertyChanged("ManageAccountsCommand");
            }
        }
        private RelayCommand followUsCommand;
        public RelayCommand FollowUsCommand
        {
            get
            {
                return this.followUsCommand;
            }
            private set
            {
                if (this.followUsCommand == value)
                    return;
                this.followUsCommand = value;
                base.RaisePropertyChanged("FollowUsCommand");
            }
        }
        private RelayCommand<string> tweetUsCommand;
        public RelayCommand<string> TweetUsCommand
        {
            get
            {
                return this.tweetUsCommand;
            }
            private set
            {
                if (this.tweetUsCommand == value)
                    return;
                this.tweetUsCommand = value;
                base.RaisePropertyChanged("TweetUsCommand");
            }
        }
        

        public MTAccountCollection AllUserAccounts
        {
            get
            {
                return App.AppState.Accounts;
            }
        }

        public AccountManagementViewModel(Decimal TwitterAccountID)
        {
            this.TwitterAccountID = TwitterAccountID;
            this.AddUserAccountCommand = new RelayCommand(new Action(this.AddUserAccount));
            this.QuitCommand = new RelayCommand(new Action(this.Quit));
            this.CancelCommand = new RelayCommand(new Action(this.Cancel));
            this.ManageAccountsCommand = new RelayCommand(new Action(this.ManageAccounts));
            this.FollowUsCommand = new RelayCommand(new Action(this.FollowUs));
            this.TweetUsCommand = new RelayCommand<string>(new Action<string>(this.TweetUs));
        }

        private void FollowUs()
        {
            UserAccountViewModel activeAccount = Enumerable.FirstOrDefault<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>)App.AppState.Accounts, (Func<UserAccountViewModel, bool>)(u => u.TwitViewModel.IsActive)));
            Friendship.CreateAsync(activeAccount.Tokens, "FlattyTweet", MetroTwitTwitterizer.CreateFriendshipOptions).ContinueWith((Action<Task<TwitterResponse<User>>>)(r =>
            {
                if (r.Result.Result == RequestResult.Success)
                {
                    activeAccount.Cache.AddFollowedUser(new MetroTwitUser(r.Result.ResponseObject));
                    this.FollowText = "thanks";
                    this.FollowIsEnabled = false;
                }
            }));
        }

        private void TweetUs(string TweetText)
        {
            Tweets.UpdateAsync(Enumerable.FirstOrDefault<UserAccountViewModel>(Enumerable.Where<UserAccountViewModel>((IEnumerable<UserAccountViewModel>)App.AppState.Accounts, (Func<UserAccountViewModel, bool>)(u => u.TwitViewModel.IsActive))).Tokens, TweetText, MetroTwitTwitterizer.StatusUpdateOptions).ContinueWith((Action<Task<TwitterResponse<Status>>>)(r =>
            {
                if (r.Result.Result == RequestResult.Success)
                {
                    this.TweetUsText = "thanks";
                    this.TweetUsIsEnabled = false;
                }
                else
                {
                    //IEnumerable<TwitterError> local_0 = r.Result.Errors;
                    //if (local_0 != null)
                    //    BugSense.SendException(new Exception(Enumerable.First<TwitterError>(local_0).Message));
                }
            }));
        }

        private void ManageAccounts()
        {
            this.IsOOBE = false;
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>(null), ViewModelMessages.CloseCenterModalWindowHost);
            ManageAccountView content = new ManageAccountView
            {
                DataContext = this
            };
            Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(content), ViewModelMessages.ShowCenterModalWindowHost);
        }

        private void Cancel()
        {
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseCenterModalWindowHost);
            if (App.AppState.Accounts.Count > 0)
            {
                if (this.TwitterAccountID > new Decimal(0))
                {
                    App.AppState.Accounts[this.TwitterAccountID].UpdateProfile(true);
                    Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.ReloadTweetViews));
                }
                else
                {
                    Decimal twitterAccountId = App.AppState.CurrentActiveAccount.TwitterAccountID;
                    App.AppState.SwitchToAccount(App.AppState.LastActiveAccount != null ? App.AppState.LastActiveAccount.TwitterAccountID : Enumerable.ElementAt<UserAccountViewModel>((IEnumerable<UserAccountViewModel>)App.AppState.Accounts, 0).TwitterAccountID);
                    App.AppState.SwitchToAccount(twitterAccountId);
                }
            }
            else
                this.AddUserAccount();
        }

        private void Quit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void AddUserAccount()
        {

            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseCenterModalWindowHost);
            AddAccountView addAccountView = new AddAccountView(new Decimal(0), this.IsOOBE);
            Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>((UserControl)addAccountView), (object)ViewModelMessages.ShowCenterModalWindowHost);
            (addAccountView.DataContext as AccountManagementViewModel).Authorise();

        }

        public void Authorise()
        {
            try
            {
                OAuth.RequestTokenAsync(SettingsData.Instance.TwitterConsumerKey, SettingsData.Instance.TwitterConsumerSecret, "https://twitter.com/FlattyTweet").ContinueWith((Action<Task<OAuthTokenResponse>>)(r =>
                {
                    if (r.Result != null)
                    {
                        if (!(r.Result.Token != string.Empty))
                            return;
                        this.requestToken = r.Result;
                        if (this.requestToken != null)
                        {
                            string authorisationurl = OAuth.BuildAuthorizationUri(this.requestToken.Token, false).AbsoluteUri;
                            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => this.URL = authorisationurl), DispatcherPriority.Background);
                        }
                    }
                    else
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.SignInSomethingWrong)));
                }));
            }
            catch
            {
            }
        }

        private async void SignIn()
        {
            if (this.requestToken != null)
            {
                this.requestToken.VerificationString = AccountManagementViewModel.ParseQuerystringParameter("oauth_verifier", this.URL);
                try
                {
                    OAuthTokenResponse response = await OAuth.AccessTokenAsync(SettingsData.Instance.TwitterConsumerKey, SettingsData.Instance.TwitterConsumerSecret, this.requestToken.Token, this.requestToken.VerificationString);
                    if (!string.IsNullOrEmpty(response.TokenSecret))
                    {
                        this.TwitterAccountID = (Decimal)response.UserId;
                        UserAccountViewModel accountViewModel = new UserAccountViewModel(this.TwitterAccountID);
                        App.AppState.Accounts.Add(accountViewModel);
                        App.AppState.SwitchToAccount(this.TwitterAccountID);
                        Messenger.Default.Send<GenericMessage<TwitViewModel>>(new GenericMessage<TwitViewModel>(accountViewModel.TwitViewModel), (object)ViewModelMessages.AddTwitView);
                        App.AppState.Accounts[this.TwitterAccountID].Settings.TwitterAccountID = (Decimal)response.UserId;
                        App.AppState.Accounts[this.TwitterAccountID].Settings.TwitterAccountName = response.ScreenName;
                        App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthToken = response.Token;
                        App.AppState.Accounts[this.TwitterAccountID].Settings.UserAuthSecret = response.TokenSecret;
                        App.AppState.Accounts[this.TwitterAccountID].Settings.Save(this.TwitterAccountID);
                        if (!SettingsData.Instance.OOBEScreenDisplayed)
                            SettingsData.Instance.OOBEScreenDisplayed = true;
                        this.requestToken = (OAuthTokenResponse)null;
                        this.CheckifFollowing();
                    }
                    else
                    {
                        Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(r => { })), (object)DialogType.SignInAuthError);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(ex.Message, (Action<MessageBoxResult>)(r => { })), (object)DialogType.SignInTwitterizerAuthError);
                    return;
                }
            }
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.CloseCenterModalWindowHost);
            if (this.IsOOBE)
            {
                FollowUsView content = new FollowUsView
                {
                    DataContext = this
                };
                Messenger.Default.Send<GenericMessage<UserControl>>(new GenericMessage<UserControl>(content), ViewModelMessages.ShowCenterModalWindowHost);
            }
            else
            {
                App.AppState.Accounts[this.TwitterAccountID].UpdateProfile(true);
                Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), (object)this.MultiAccountifyToken((Enum)ViewModelMessages.ReloadTweetViews));
            }
        }

        private static string ParseQuerystringParameter(string parameterName, string text)
        {
            Match match = Regex.Match(text, string.Format("{0}=(?<value>[^&]+)", (object)parameterName));
            if (!match.Success)
                return string.Empty;
            else
                return match.Groups["value"].Value;
        }

        private async void CheckifFollowing()
        {
            TwitterResponse<TwitterRelationship> r = await Friendship.ShowAsync(App.AppState.Accounts[this.TwitterAccountID].Tokens, "metrotwitapp", App.AppState.Accounts[this.TwitterAccountID].TwitterAccountName, MetroTwitTwitterizer.Options);
            if (r.Result == RequestResult.Success)
                this.FollowVisible = !r.ResponseObject.Source.Following ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
