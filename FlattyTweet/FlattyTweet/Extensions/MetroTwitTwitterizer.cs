
using FlattyTweet.Model;
using Twitterizer;

namespace FlattyTweet.Extensions
{
  public static class MetroTwitTwitterizer
  {
    public static UserTimelineOptions UserTimelineOptions
    {
      get
      {
        UserTimelineOptions userTimelineOptions1 = new UserTimelineOptions();
        userTimelineOptions1.Count = 50;
        userTimelineOptions1.UseSSL = true;
        UserTimelineOptions userTimelineOptions2 = userTimelineOptions1;
        if (SettingsData.Instance.UseProxlet)
          userTimelineOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return userTimelineOptions2;
      }
    }

    public static IncomingFriendshipsOptions IncomingFriendshipsOptions
    {
      get
      {
        IncomingFriendshipsOptions friendshipsOptions1 = new IncomingFriendshipsOptions();
        friendshipsOptions1.UseSSL = true;
        IncomingFriendshipsOptions friendshipsOptions2 = friendshipsOptions1;
        if (SettingsData.Instance.UseProxlet)
          friendshipsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return friendshipsOptions2;
      }
    }

    public static OutgoingFriendshipsOptions OutgoingFriendshipsOptions
    {
      get
      {
        OutgoingFriendshipsOptions friendshipsOptions1 = new OutgoingFriendshipsOptions();
        friendshipsOptions1.UseSSL = true;
        OutgoingFriendshipsOptions friendshipsOptions2 = friendshipsOptions1;
        if (SettingsData.Instance.UseProxlet)
          friendshipsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return friendshipsOptions2;
      }
    }

    public static TimelineOptions TimelineOptions
    {
      get
      {
        TimelineOptions timelineOptions1 = new TimelineOptions();
        timelineOptions1.Count = 50;
        timelineOptions1.UseSSL = true;
        TimelineOptions timelineOptions2 = timelineOptions1;
        if (SettingsData.Instance.UseProxlet)
          timelineOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return timelineOptions2;
      }
    }

    public static OptionalProperties Options
    {
      get
      {
        OptionalProperties optionalProperties = new OptionalProperties()
        {
          UseSSL = true
        };
        if (SettingsData.Instance.UseProxlet)
          optionalProperties.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return optionalProperties;
      }
    }

    public static StatusUpdateOptions StatusUpdateOptions
    {
      get
      {
        StatusUpdateOptions statusUpdateOptions1 = new StatusUpdateOptions();
        statusUpdateOptions1.UseSSL = true;
        StatusUpdateOptions statusUpdateOptions2 = statusUpdateOptions1;
        if (SettingsData.Instance.UseProxlet)
          statusUpdateOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return statusUpdateOptions2;
      }
    }

    public static UsersIdsOptions UsersIdsOptions
    {
      get
      {
        UsersIdsOptions usersIdsOptions1 = new UsersIdsOptions();
        usersIdsOptions1.UseSSL = true;
        UsersIdsOptions usersIdsOptions2 = usersIdsOptions1;
        if (SettingsData.Instance.UseProxlet)
          usersIdsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return usersIdsOptions2;
      }
    }

    public static ListStatusesOptions ListStatusesOptions
    {
      get
      {
        ListStatusesOptions listStatusesOptions1 = new ListStatusesOptions();
        listStatusesOptions1.UseSSL = true;
        listStatusesOptions1.IncludeEntites = true;
        ListStatusesOptions listStatusesOptions2 = listStatusesOptions1;
        if (SettingsData.Instance.UseProxlet)
          listStatusesOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return listStatusesOptions2;
      }
    }

    public static GetListMembersOptions GetListMembersOptions
    {
      get
      {
        GetListMembersOptions listMembersOptions1 = new GetListMembersOptions();
        listMembersOptions1.UseSSL = true;
        GetListMembersOptions listMembersOptions2 = listMembersOptions1;
        if (SettingsData.Instance.UseProxlet)
          listMembersOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return listMembersOptions2;
      }
    }

    public static GetListsOptions GetListsOptions
    {
      get
      {
        GetListsOptions getListsOptions1 = new GetListsOptions();
        getListsOptions1.UseSSL = true;
        GetListsOptions getListsOptions2 = getListsOptions1;
        if (SettingsData.Instance.UseProxlet)
          getListsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return getListsOptions2;
      }
    }

    public static GetListSubscriptionsOptions GetListSubscriptionsOptions
    {
      get
      {
        GetListSubscriptionsOptions subscriptionsOptions1 = new GetListSubscriptionsOptions();
        subscriptionsOptions1.UseSSL = true;
        GetListSubscriptionsOptions subscriptionsOptions2 = subscriptionsOptions1;
        if (SettingsData.Instance.UseProxlet)
          subscriptionsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return subscriptionsOptions2;
      }
    }

    public static ListMembershipsOptions ListMembershipsOptions
    {
      get
      {
        ListMembershipsOptions membershipsOptions1 = new ListMembershipsOptions();
        membershipsOptions1.UseSSL = true;
        ListMembershipsOptions membershipsOptions2 = membershipsOptions1;
        if (SettingsData.Instance.UseProxlet)
          membershipsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return membershipsOptions2;
      }
    }

    public static SearchOptions SearchOptions
    {
      get
      {
        SearchOptions searchOptions1 = new SearchOptions();
        searchOptions1.UseSSL = true;
        searchOptions1.IncludeEntities = true;
        SearchOptions searchOptions2 = searchOptions1;
        if (SettingsData.Instance.UseProxlet)
          searchOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return searchOptions2;
      }
    }

    public static ListFavoritesOptions ListFavoritesOptions
    {
      get
      {
        ListFavoritesOptions favoritesOptions1 = new ListFavoritesOptions();
        favoritesOptions1.UseSSL = true;
        ListFavoritesOptions favoritesOptions2 = favoritesOptions1;
        if (SettingsData.Instance.UseProxlet)
          favoritesOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return favoritesOptions2;
      }
    }

    public static TrendsOptions TrendsOptions
    {
      get
      {
        TrendsOptions trendsOptions1 = new TrendsOptions();
        trendsOptions1.UseSSL = true;
        TrendsOptions trendsOptions2 = trendsOptions1;
        if (SettingsData.Instance.UseProxlet)
          trendsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return trendsOptions2;
      }
    }

    public static LookupUsersOptions LookupUsersOptions
    {
      get
      {
        LookupUsersOptions lookupUsersOptions1 = new LookupUsersOptions();
        lookupUsersOptions1.UseSSL = true;
        LookupUsersOptions lookupUsersOptions2 = lookupUsersOptions1;
        if (SettingsData.Instance.UseProxlet)
          lookupUsersOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return lookupUsersOptions2;
      }
    }

    public static CreateFriendshipOptions CreateFriendshipOptions
    {
      get
      {
        CreateFriendshipOptions friendshipOptions1 = new CreateFriendshipOptions();
        friendshipOptions1.UseSSL = true;
        CreateFriendshipOptions friendshipOptions2 = friendshipOptions1;
        if (SettingsData.Instance.UseProxlet)
          friendshipOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return friendshipOptions2;
      }
    }

    public static DirectMessagesSentOptions DirectSentMessageOptions
    {
      get
      {
        DirectMessagesSentOptions messagesSentOptions1 = new DirectMessagesSentOptions();
        messagesSentOptions1.UseSSL = true;
        messagesSentOptions1.IncludeEntites = true;
        DirectMessagesSentOptions messagesSentOptions2 = messagesSentOptions1;
        if (SettingsData.Instance.UseProxlet)
          messagesSentOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return messagesSentOptions2;
      }
    }

    public static DirectMessagesOptions DirectMessageOptions
    {
      get
      {
        DirectMessagesOptions directMessagesOptions1 = new DirectMessagesOptions();
        directMessagesOptions1.UseSSL = true;
        directMessagesOptions1.IncludeEntites = true;
        DirectMessagesOptions directMessagesOptions2 = directMessagesOptions1;
        if (SettingsData.Instance.UseProxlet)
          directMessagesOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return directMessagesOptions2;
      }
    }

    public static RetweetsOfMeOptions RetweetsOfMeOptions
    {
      get
      {
        RetweetsOfMeOptions retweetsOfMeOptions1 = new RetweetsOfMeOptions();
        retweetsOfMeOptions1.UseSSL = true;
        RetweetsOfMeOptions retweetsOfMeOptions2 = retweetsOfMeOptions1;
        if (SettingsData.Instance.UseProxlet)
          retweetsOfMeOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return retweetsOfMeOptions2;
      }
    }

    public static RetweetsOptions RetweetsOptions
    {
      get
      {
        RetweetsOptions retweetsOptions1 = new RetweetsOptions();
        retweetsOptions1.UseSSL = true;
        RetweetsOptions retweetsOptions2 = retweetsOptions1;
        if (SettingsData.Instance.UseProxlet)
          retweetsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return retweetsOptions2;
      }
    }

    public static VerifyCredentialsOptions VerifyCredentialsOptions
    {
      get
      {
        VerifyCredentialsOptions credentialsOptions1 = new VerifyCredentialsOptions();
        credentialsOptions1.UseSSL = true;
        VerifyCredentialsOptions credentialsOptions2 = credentialsOptions1;
        if (SettingsData.Instance.UseProxlet)
          credentialsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return credentialsOptions2;
      }
    }

    public static BlockingIdOptions BlockingIdOptions
    {
      get
      {
        BlockingIdOptions blockingIdOptions1 = new BlockingIdOptions();
        blockingIdOptions1.UseSSL = true;
        BlockingIdOptions blockingIdOptions2 = blockingIdOptions1;
        if (SettingsData.Instance.UseProxlet)
          blockingIdOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return blockingIdOptions2;
      }
    }

    public static AvailableTrendsOptions AvailableTrendsOptions
    {
      get
      {
        AvailableTrendsOptions availableTrendsOptions1 = new AvailableTrendsOptions();
        availableTrendsOptions1.UseSSL = true;
        AvailableTrendsOptions availableTrendsOptions2 = availableTrendsOptions1;
        if (SettingsData.Instance.UseProxlet)
          availableTrendsOptions2.APIBaseAddress = SettingsData.Instance.ProxletAddress;
        return availableTrendsOptions2;
      }
    }
  }
}
