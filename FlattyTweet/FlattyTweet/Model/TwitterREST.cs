using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.Model
{
    public static class TwitterREST
    {
        public static void TwitterRest(Guid unqiueID, TweetListType TweetType, Decimal TwitterAccountID, RefreshTypes RefreshType, Action initialCallback, string SearchTerm = null, Decimal InReplyToID = 0M, Decimal LastUpdateID = 0M, Decimal OldestTweetID = 0M, bool ListRetweets = true)
        {
            Func<string> MessengerToken = (Func<string>)(() => ((object)ViewModelMessages.RestUpdate).ToString() + unqiueID.ToString());
            Action<Task<TwitterResponse<SearchResult>>> continuationAction1 = (Action<Task<TwitterResponse<SearchResult>>>)(searchResponse =>
            {
                Action local_0 = initialCallback;
                try
                {
                    TwitterStatusCollection local_1 = searchResponse.Result.ResponseObject.Statuses ?? new TwitterStatusCollection();
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                    {
                        Tweets = local_1,
                        RefreshType = RefreshType,
                        RequestResult = searchResponse.Result.Result,
                        Error = searchResponse.Result.Errors
                    }), (object)MessengerToken());
                    Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                    App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, searchResponse.Result.RateLimiting, "");
                }
                catch
                {
                }
                finally
                {
                    if (local_0 != null)
                        local_0();
                }
            });
            Action<Task<TwitterResponse<TwitterStatusCollection>>> continuationAction2 = (Action<Task<TwitterResponse<TwitterStatusCollection>>>)(response =>
            {
                Action local_0 = initialCallback;
                try
                {
                    TwitterStatusCollection local_1 = response.Result.ResponseObject ?? new TwitterStatusCollection();
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                    {
                        Tweets = local_1,
                        RefreshType = RefreshType,
                        RequestResult = response.Result.Result,
                        Error = response.Result.Errors
                    }), (object)MessengerToken());
                    Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                    App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, response.Result.RateLimiting, "");
                }
                catch
                {
                }
                finally
                {
                    if (local_0 != null)
                        local_0();
                }
            });
            try
            {
                Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(1), (object)ViewModelMessages.ProgressVisible);
                TimelineOptions timelineOptions = MetroTwitTwitterizer.TimelineOptions;
                if (TweetType != TweetListType.Search && TweetType != TweetListType.DirectMessages && TweetType != TweetListType.Favourites && TweetType != TweetListType.Conversation)
                {
                    if (RefreshType != RefreshTypes.ForeverScroll)
                        timelineOptions.SinceStatusId = LastUpdateID;
                    else
                        timelineOptions.MaxStatusId = OldestTweetID;
                }
                Action action1;
                switch (TweetType)
                {
                    case TweetListType.FriendsTimeline:
                        Timelines.HomeTimelineAsync(App.AppState.Accounts[TwitterAccountID].Tokens, timelineOptions).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.DirectMessages:
                        DirectMessagesOptions directMessageOptions = MetroTwitTwitterizer.DirectMessageOptions;
                        DirectMessagesSentOptions sentMessageOptions = MetroTwitTwitterizer.DirectSentMessageOptions;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                        {
                            directMessageOptions.SinceStatusId = LastUpdateID;
                            sentMessageOptions.SinceStatusId = LastUpdateID;
                        }
                        else
                        {
                            directMessageOptions.MaxStatusId = OldestTweetID;
                            sentMessageOptions.MaxStatusId = OldestTweetID;
                        }
                        try
                        {
                            DirectMessages.SentAsync(App.AppState.Accounts[TwitterAccountID].Tokens, sentMessageOptions).ContinueWith((Action<Task<TwitterResponse<TwitterDirectMessageCollection>>>)(directMessagesSentResponse =>
                            {
                                if (directMessagesSentResponse.Result.Result == RequestResult.Success)
                                {
                                    DirectMessages.ReceivedAsync(App.AppState.Accounts[TwitterAccountID].Tokens, directMessageOptions).ContinueWith((Action<Task<TwitterResponse<TwitterDirectMessageCollection>>>)(directMessagesReceived =>
                                    {
                                        if (directMessagesReceived.Result.Result == RequestResult.Success && directMessagesReceived.Result.ResponseObject != null && directMessagesSentResponse.Result.ResponseObject != null)
                                        {
                                            IOrderedEnumerable<TwitterDirectMessage> local_0 = Enumerable.OrderByDescending<TwitterDirectMessage, DateTime>(Enumerable.Union<TwitterDirectMessage>((IEnumerable<TwitterDirectMessage>)directMessagesReceived.Result.ResponseObject, (IEnumerable<TwitterDirectMessage>)directMessagesSentResponse.Result.ResponseObject), (Func<TwitterDirectMessage, DateTime>)(s => s.CreatedDate)) ?? (IOrderedEnumerable<TwitterDirectMessage>)new TwitterDirectMessageCollection();
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<IEnumerable<TwitterDirectMessage>>()
                                            {
                                                Tweets = (IEnumerable<TwitterDirectMessage>)local_0,
                                                RefreshType = RefreshType,
                                                RequestResult = directMessagesReceived.Result.Result
                                            }), (object)MessengerToken());
                                            Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                            if (initialCallback != null)
                                            {
                                                initialCallback();
                                                initialCallback = (Action)null;
                                            }
                                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, directMessagesSentResponse.Result.RateLimiting, "S");
                                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, directMessagesReceived.Result.RateLimiting, "R");
                                        }
                                        else
                                        {
                                            IEnumerable<TwitterError> local_3 = directMessagesReceived.Result.Errors;
                                            if (local_3 != null)
                                            {
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<IEnumerable<TwitterDirectMessage>>()
                                                {
                                                    Tweets = (IEnumerable<TwitterDirectMessage>)new TwitterDirectMessageCollection(),
                                                    RefreshType = RefreshType,
                                                    Error = local_3,
                                                    RequestResult = directMessagesReceived.Result.Result
                                                }), (object)MessengerToken());
                                                Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                            }
                                            if (initialCallback != null)
                                            {
                                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)MessengerToken());
                                                initialCallback();
                                                initialCallback = (Action)null;
                                            }
                                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, directMessagesReceived.Result.RateLimiting, "R");
                                        }
                                    }));
                                }
                                else
                                {
                                    IEnumerable<TwitterError> local_0 = directMessagesSentResponse.Result.Errors;
                                    if (local_0 != null)
                                    {
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<IEnumerable<TwitterDirectMessage>>()
                                        {
                                            Tweets = (IEnumerable<TwitterDirectMessage>)new TwitterDirectMessageCollection(),
                                            RefreshType = RefreshType,
                                            Error = local_0,
                                            RequestResult = directMessagesSentResponse.Result.Result
                                        }), (object)MessengerToken());
                                        Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                    }
                                    if (initialCallback != null)
                                    {
                                        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)MessengerToken());
                                        initialCallback();
                                        initialCallback = (Action)null;
                                    }
                                    App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, directMessagesSentResponse.Result.RateLimiting, "S");
                                }
                            }));
                            break;
                        }
                        catch
                        {
                            break;
                        }
                    case TweetListType.Search:
                        SearchOptions searchOptions = MetroTwitTwitterizer.SearchOptions;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                            searchOptions.SinceId = (Decimal)(long)LastUpdateID;
                        else
                            searchOptions.MaxId = (Decimal)(long)OldestTweetID;
                        Search.SearchAsync(App.AppState.Accounts[TwitterAccountID].Tokens, SearchTerm, searchOptions).ContinueWith(continuationAction1);
                        break;
                    case TweetListType.UserTimeline:
                        UserTimelineOptions userTimelineOptions1 = MetroTwitTwitterizer.UserTimelineOptions;
                        userTimelineOptions1.IncludeRetweets = true;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                            userTimelineOptions1.SinceStatusId = LastUpdateID;
                        else
                            userTimelineOptions1.MaxStatusId = OldestTweetID;
                        userTimelineOptions1.ScreenName = SearchTerm.Replace("@", "").Trim();
                        Timelines.UserTimelineAsync(App.AppState.Accounts[TwitterAccountID].Tokens, userTimelineOptions1).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.List:
                        ListStatusesOptions listStatusesOptions = MetroTwitTwitterizer.ListStatusesOptions;
                        listStatusesOptions.IncludeRetweets = ListRetweets;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                            listStatusesOptions.SinceId = (Decimal)(long)LastUpdateID;
                        else
                            listStatusesOptions.MaxId = (Decimal)(long)OldestTweetID;
                        string[] strArray = SearchTerm.Split(new char[1]
            {
              '/'
            });
                        Lists.StatusesAsync(App.AppState.Accounts[TwitterAccountID].Tokens, strArray[1], strArray[0].Replace("@", ""), listStatusesOptions).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.MentionsMyTweetsRetweeted:
                        Task<TwitterResponse<TwitterStatusCollection>> mentionsresponse = Timelines.MentionsAsync(App.AppState.Accounts[TwitterAccountID].Tokens, timelineOptions);
                        mentionsresponse.Wait();
                        Action action2 = initialCallback;
                        if (mentionsresponse.Result.Result == RequestResult.Success)
                        {
                            RetweetsOfMeOptions retweetsOfMeOptions = MetroTwitTwitterizer.RetweetsOfMeOptions;
                            if (RefreshType != RefreshTypes.ForeverScroll)
                                retweetsOfMeOptions.SinceStatusId = (Decimal)(long)LastUpdateID;
                            else
                                retweetsOfMeOptions.MaxStatusId = (Decimal)(long)OldestTweetID;
                            Task<TwitterResponse<TwitterStatusCollection>> task = Timelines.RetweetsOfMeAsync(App.AppState.Accounts[TwitterAccountID].Tokens, retweetsOfMeOptions);
                            task.Wait();
                            if (task.Result.Result == RequestResult.Success && task.Result.ResponseObject != null && mentionsresponse.Result.ResponseObject != null)
                            {
                                if (mentionsresponse.Result.ResponseObject.Count > 0)
                                {
                                    foreach (Status status in Enumerable.Where<Status>((IEnumerable<Status>)Enumerable.ToArray<Status>((IEnumerable<Status>)task.Result.ResponseObject), (Func<Status, int, bool>)((x, r) => x.Id < Enumerable.Last<Status>((IEnumerable<Status>)mentionsresponse.Result.ResponseObject).Id)))
                                        task.Result.ResponseObject.Remove(status);
                                }
                                IOrderedEnumerable<Status> orderedEnumerable = Enumerable.OrderByDescending<Status, DateTime>(Enumerable.Union<Status>((IEnumerable<Status>)mentionsresponse.Result.ResponseObject, (IEnumerable<Status>)task.Result.ResponseObject), (Func<Status, DateTime>)(s => s.CreatedDate));
                                if (Enumerable.Count<Status>((IEnumerable<Status>)orderedEnumerable) > 0)
                                {
                                    TwitterStatusCollection statusCollection = new TwitterStatusCollection();
                                    statusCollection.AddTwitterRange<Status>(orderedEnumerable.ToList<Status>());
                                    // statusCollection, (IEnumerable<Status>) Enumerable.ToList<Status>((IEnumerable<Status>) orderedEnumerable));
                                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                                    {
                                        Tweets = statusCollection,
                                        RefreshType = RefreshType,
                                        RequestResult = task.Result.Result
                                    }), (object)MessengerToken());
                                    Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                }
                                else
                                    continuationAction2(mentionsresponse);
                                App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, mentionsresponse.Result.RateLimiting, "M");
                                App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, task.Result.RateLimiting, "R");
                            }
                            else
                            {
                                continuationAction2(mentionsresponse);
                                IEnumerable<TwitterError> errors = task.Result.Errors;
                                if (errors != null)
                                {
                                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                                    {
                                        Tweets = new TwitterStatusCollection(),
                                        RefreshType = RefreshType,
                                        Error = errors,
                                        RequestResult = task.Result.Result
                                    }), (object)MessengerToken());
                                    Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                }
                                App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, task.Result.RateLimiting, "R");
                            }
                        }
                        else
                        {
                            IEnumerable<TwitterError> errors = mentionsresponse.Result.Errors;
                            if (errors != null)
                            {
                                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                                {
                                    Tweets = new TwitterStatusCollection(),
                                    RefreshType = RefreshType,
                                    Error = errors,
                                    RequestResult = mentionsresponse.Result.Result
                                }), (object)MessengerToken());
                                Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                            }
                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, mentionsresponse.Result.RateLimiting, "M");
                        }
                        if (action2 == null)
                            break;
                        action2();
                        action1 = (Action)null;
                        break;
                    case TweetListType.MyTweets:
                        UserTimelineOptions userTimelineOptions2 = MetroTwitTwitterizer.UserTimelineOptions;
                        userTimelineOptions2.IncludeRetweets = true;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                            userTimelineOptions2.SinceStatusId = LastUpdateID;
                        else
                            userTimelineOptions2.MaxStatusId = OldestTweetID;
                        Timelines.UserTimelineAsync(App.AppState.Accounts[TwitterAccountID].Tokens, userTimelineOptions2).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.Favourites:
                        ListFavoritesOptions favoritesOptions = MetroTwitTwitterizer.ListFavoritesOptions;
                        if (RefreshType != RefreshTypes.ForeverScroll)
                            favoritesOptions.SinceStatusId = (Decimal)(long)LastUpdateID;
                        else
                            favoritesOptions.MaxStatusId = (Decimal)(long)OldestTweetID;
                        Favorites.ListAsync(App.AppState.Accounts[TwitterAccountID].Tokens, favoritesOptions).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.Conversation:
                        Decimal statusId1 = TweetType != TweetListType.Conversation || !(InReplyToID == new Decimal(0)) ? InReplyToID : LastUpdateID;
                        if (!(statusId1 > new Decimal(0)))
                            break;
                        Task<TwitterResponse<Status>> task1 = Tweets.ShowAsync(App.AppState.Accounts[TwitterAccountID].Tokens, statusId1, MetroTwitTwitterizer.Options);
                        task1.Wait();
                        Action action3 = initialCallback;
                        try
                        {
                            Status responseObject1 = task1.Result.ResponseObject;
                            TwitterStatusCollection statusCollection1 = new TwitterStatusCollection();
                            if (responseObject1 != null)
                                statusCollection1.Add(responseObject1);
                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                            {
                                Tweets = statusCollection1,
                                RefreshType = RefreshType,
                                RequestResult = task1.Result.Result,
                                Error = task1.Result.Errors
                            }), (object)MessengerToken());
                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, task1.Result.RateLimiting, "");
                            if (responseObject1 != null)
                            {
                                Decimal statusId2 = responseObject1.InReplyToStatusId;
                                while (statusId2 > new Decimal(0))
                                {
                                    if (statusId2 > new Decimal(0))
                                    {
                                        Task<TwitterResponse<Status>> task2 = Tweets.ShowAsync(App.AppState.Accounts[TwitterAccountID].Tokens, statusId2, MetroTwitTwitterizer.Options);
                                        task2.Wait();
                                        try
                                        {
                                            Status responseObject2 = task2.Result.ResponseObject;
                                            TwitterStatusCollection statusCollection2 = new TwitterStatusCollection();
                                            if (responseObject2 != null)
                                                statusCollection2.Add(responseObject2);
                                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)new MetroRestResponse<TwitterStatusCollection>()
                                            {
                                                Tweets = statusCollection2,
                                                RefreshType = RefreshType,
                                                RequestResult = task2.Result.Result,
                                                Error = task2.Result.Errors
                                            }), (object)MessengerToken());
                                            statusId2 = responseObject2 == null ? new Decimal(0) : responseObject2.InReplyToStatusId;
                                            App.AppState.Accounts[TwitterAccountID].UpdateRateLimits(TweetType, task2.Result.RateLimiting, "");
                                        }
                                        catch
                                        {
                                            Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            Messenger.Default.Send<GenericMessage<int>>(new GenericMessage<int>(-1), (object)ViewModelMessages.ProgressVisible);
                        }
                        finally
                        {
                            if (action3 != null)
                            {
                                action3();
                                action1 = (Action)null;
                            }
                        }
                        break;
                    case TweetListType.RetweetUsers:
                        Tweets.RetweetsAsync(App.AppState.Accounts[TwitterAccountID].Tokens, Decimal.Parse(SearchTerm), MetroTwitTwitterizer.RetweetsOptions).ContinueWith(continuationAction2);
                        break;
                    case TweetListType.Followers:
                        Friendship.FollowersIdsAsync(App.AppState.Accounts[TwitterAccountID].Tokens, new UsersIdsOptions()
                        {
                            ScreenName = SearchTerm
                        }).ContinueWith((Action<Task<TwitterResponse<UserIdCollection>>>)(r => Users.LookupAsync(App.AppState.Accounts[TwitterAccountID].Tokens, new LookupUsersOptions()
                        {
                            UserIds = (TwitterIdCollection)r.Result.ResponseObject
                        }).Wait()));
                        break;
                }
            }
            catch
            {
            }
        }
    }
}
