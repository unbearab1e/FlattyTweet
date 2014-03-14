// Type: MetroTwit.ViewModel.MetroTwitUser
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using Bugsense.WPF;
using MetroTwit;
using MetroTwit.Extensions;
using System;
using Twitterizer.Models;

namespace MetroTwit.ViewModel
{
  public class MetroTwitUser : MultiAccountViewModelBase
  {
    private bool isSelected;
      private User baseUser;
    public User BaseUser
    {
      get
      {
        return this.baseUser;
      }
      set
      {
        if (this.baseUser == value)
          return;
        this.baseUser = value;
        base.RaisePropertyChanged("BaseUser");
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
      private bool alreadyExistsInList;
    public bool AlreadyExistsInList
    {
      get
      {
        return this.alreadyExistsInList;
      }
      set
      {
        if (this.alreadyExistsInList == value)
          return;
        this.alreadyExistsInList = value;
        base.RaisePropertyChanged("AlreadyExistsInList");
      }
    }
      private bool contributorsEnabled;
    public bool ContributorsEnabled
    {
      get
      {
        return this.contributorsEnabled;
      }
      set
      {
        if (this.contributorsEnabled == value)
          return;
        this.contributorsEnabled = value;
        base.RaisePropertyChanged("ContributorsEnabled");
      }
    }
      private DateTime? createdDate;
    public DateTime? CreatedDate
    {
      get
      {
        return this.createdDate;
      }
      set
      {
        if (Nullable.Equals<DateTime>(this.createdDate, value))
          return;
        this.createdDate = value;
        base.RaisePropertyChanged("CreatedDateLongString");
        base.RaisePropertyChanged("CreatedDate");
      }
    }
      
    public string CreatedDateLongString
    {
      get
      {
        if (this.CreatedDate.HasValue)
          return this.CreatedDate.Value.ToString("d MMM yyyy");
        else
          return string.Empty;
      }
    }
      private bool defaultProfile;
    public bool DefaultProfile
    {
      get
      {
        return this.defaultProfile;
      }
      set
      {
        if (this.defaultProfile == value)
          return;
        this.defaultProfile = value;
        base.RaisePropertyChanged("DefaultProfile");
      }
    }
      private bool defaultProfileImage;
    public bool DefaultProfileImage
    {
      get
      {
        return this.defaultProfileImage;
      }
      set
      {
        if (this.defaultProfileImage == value)
          return;
        this.defaultProfileImage = value;
        base.RaisePropertyChanged("DefaultProfileImage");
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
      private bool followRequestSent;
    public bool FollowRequestSent
    {
      get
      {
        return this.followRequestSent;
      }
      set
      {
        if (this.followRequestSent == value)
          return;
        this.followRequestSent = value;
        base.RaisePropertyChanged("FollowRequestSent");
      }
    }
      private Decimal id;
    public Decimal Id
    {
      get
      {
        return this.id;
      }
      set
      {
        if (Decimal.Equals(this.id, value))
          return;
        this.id = value;
        base.RaisePropertyChanged("Id");
      }
    }
      private bool isFollowedBy;
    public bool IsFollowedBy
    {
      get
      {
        return this.isFollowedBy;
      }
      set
      {
        if (this.isFollowedBy == value)
          return;
        this.isFollowedBy = value;
        base.RaisePropertyChanged("IsFollowedBy");
      }
    }
      private bool isFollowing;
    public bool IsFollowing
    {
      get
      {
        return this.isFollowing;
      }
      set
      {
        if (this.isFollowing == value)
          return;
        this.isFollowing = value;
        base.RaisePropertyChanged("IsFollowing");
      }
    }
      private bool isGeoEnabled;
    public bool IsGeoEnabled
    {
      get
      {
        return this.isGeoEnabled;
      }
      set
      {
        if (this.isGeoEnabled == value)
          return;
        this.isGeoEnabled = value;
        base.RaisePropertyChanged("IsGeoEnabled");
      }
    }
      private bool isProfileBackgroundTiled;
    public bool IsProfileBackgroundTiled
    {
      get
      {
        return this.isProfileBackgroundTiled;
      }
      set
      {
        if (this.isProfileBackgroundTiled == value)
          return;
        this.isProfileBackgroundTiled = value;
        base.RaisePropertyChanged("IsProfileBackgroundTiled");
      }
    }
      private bool isProtected;
    public bool IsProtected
    {
      get
      {
        return this.isProtected;
      }
      set
      {
        if (this.isProtected == value)
          return;
        this.isProtected = value;
        base.RaisePropertyChanged("IsProtected");
      }
    }
      private bool isTranslator;
    public bool IsTranslator
    {
      get
      {
        return this.isTranslator;
      }
      set
      {
        if (this.isTranslator == value)
          return;
        this.isTranslator = value;
        base.RaisePropertyChanged("IsTranslator");
      }
    }
      private bool canDM;
    public bool CanDM
    {
      get
      {
        return this.canDM;
      }
      set
      {
        if (this.canDM == value)
          return;
        this.canDM = value;
        base.RaisePropertyChanged("CanDM");
      }
    }
      private bool relationshipsInitialised;
    public bool RelationshipsInitialised
    {
      get
      {
        return this.relationshipsInitialised;
      }
      set
      {
        if (this.relationshipsInitialised == value)
          return;
        this.relationshipsInitialised = value;
        base.RaisePropertyChanged("RelationshipsInitialised");
      }
    }
      private string language;
    public string Language
    {
      get
      {
        return this.language;
      }
      set
      {
        if (string.Equals(this.language, value, StringComparison.Ordinal))
          return;
        this.language = value;
        base.RaisePropertyChanged("Language");
      }
    }
      private int listedCount;
    public int ListedCount
    {
      get
      {
        return this.listedCount;
      }
      set
      {
        if (this.listedCount == value)
          return;
        this.listedCount = value;
        base.RaisePropertyChanged("ListedCount");
      }
    }
      private string location;
    public string Location
    {
      get
      {
        return this.location;
      }
      set
      {
        if (string.Equals(this.location, value, StringComparison.Ordinal))
          return;
        this.location = value;
        base.RaisePropertyChanged("Location");
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
      private int numberOfFavorites;
    public int NumberOfFavorites
    {
      get
      {
        return this.numberOfFavorites;
      }
      set
      {
        if (this.numberOfFavorites == value)
          return;
        this.numberOfFavorites = value;
        base.RaisePropertyChanged("NumberOfFavorites");
      }
    }
      private int numberOfFollowers;
    public int NumberOfFollowers
    {
      get
      {
        return this.numberOfFollowers;
      }
      set
      {
        if (this.numberOfFollowers == value)
          return;
        this.numberOfFollowers = value;
        base.RaisePropertyChanged("NumberOfFollowers");
      }
    }
      private int numberOfFriends;
    public int NumberOfFriends
    {
      get
      {
        return this.numberOfFriends;
      }
      set
      {
        if (this.numberOfFriends == value)
          return;
        this.numberOfFriends = value;
        base.RaisePropertyChanged("NumberOfFriends");
      }
    }
      private int numberOfStatuses;
    public int NumberOfStatuses
    {
      get
      {
        return this.numberOfStatuses;
      }
      set
      {
        if (this.numberOfStatuses == value)
          return;
        this.numberOfStatuses = value;
        base.RaisePropertyChanged("NumberOfStatuses");
      }
    }
      private string profileBannerLocation;
    public string ProfileBannerLocation
    {
      get
      {
        return this.profileBannerLocation;
      }
      set
      {
        if (string.Equals(this.profileBannerLocation, value, StringComparison.Ordinal))
          return;
        this.profileBannerLocation = value;
        base.RaisePropertyChanged("ProfileBannerLocation");
      }
    }
      private string profileBackgroundColor;
    public string ProfileBackgroundColor
    {
      get
      {
        return this.profileBackgroundColor;
      }
      set
      {
        if (string.Equals(this.profileBackgroundColor, value, StringComparison.Ordinal))
          return;
        this.profileBackgroundColor = value;
        base.RaisePropertyChanged("ProfileBackgroundColor");
      }
    }
      private string profileBackgroundImageLocation;
    public string ProfileBackgroundImageLocation
    {
      get
      {
        return this.profileBackgroundImageLocation;
      }
      set
      {
        if (string.Equals(this.profileBackgroundImageLocation, value, StringComparison.Ordinal))
          return;
        this.profileBackgroundImageLocation = value;
        base.RaisePropertyChanged("ProfileBackgroundImageLocation");
      }
    }
      private string profileBackgroundImageSecureLocation;
    public string ProfileBackgroundImageSecureLocation
    {
      get
      {
        return this.profileBackgroundImageSecureLocation;
      }
      set
      {
        if (string.Equals(this.profileBackgroundImageSecureLocation, value, StringComparison.Ordinal))
          return;
        this.profileBackgroundImageSecureLocation = value;
        base.RaisePropertyChanged("ProfileBackgroundImageSecureLocation");
      }
    }
      private string profileImageLocation;
    public string ProfileImageLocation
    {
      get
      {
        return this.profileImageLocation;
      }
      set
      {
        if (string.Equals(this.profileImageLocation, value, StringComparison.Ordinal))
          return;
        this.profileImageLocation = value;
        base.RaisePropertyChanged("ProfileBigImageLocation");
        base.RaisePropertyChanged("ProfileOriginalImageLocation");
        base.RaisePropertyChanged("ProfileImageLocation");
      }
    }

    public string ProfileBigImageLocation
    {
      get
      {
        if (this.ProfileImageLocation != null)
          return this.ProfileImageLocation.Replace("_normal", "_bigger");
        else
          return (string) null;
      }
    }

    public string ProfileOriginalImageLocation
    {
      get
      {
        if (this.ProfileImageLocation != null)
          return this.ProfileImageLocation.Replace("_normal", "");
        else
          return (string) null;
      }
    }
      private string profileImageSecureLocation;
    public string ProfileImageSecureLocation
    {
      get
      {
        return this.profileImageSecureLocation;
      }
      set
      {
        if (string.Equals(this.profileImageSecureLocation, value, StringComparison.Ordinal))
          return;
        this.profileImageSecureLocation = value;
        base.RaisePropertyChanged("ProfileImageSecureLocation");
      }
    }
      private string profileLinkColor;
    public string ProfileLinkColor
    {
      get
      {
        return this.profileLinkColor;
      }
      set
      {
        if (string.Equals(this.profileLinkColor, value, StringComparison.Ordinal))
          return;
        this.profileLinkColor = value;
        base.RaisePropertyChanged("ProfileLinkColor");
      }
    }
      private string profileSidebarBorderColor;
    public string ProfileSidebarBorderColor
    {
      get
      {
        return this.profileSidebarBorderColor;
      }
      set
      {
        if (string.Equals(this.profileSidebarBorderColor, value, StringComparison.Ordinal))
          return;
        this.profileSidebarBorderColor = value;
        base.RaisePropertyChanged("ProfileSidebarBorderColor");
      }
    }
      private string profileSidebarFillColor;
    public string ProfileSidebarFillColor
    {
      get
      {
        return this.profileSidebarFillColor;
      }
      set
      {
        if (string.Equals(this.profileSidebarFillColor, value, StringComparison.Ordinal))
          return;
        this.profileSidebarFillColor = value;
        base.RaisePropertyChanged("ProfileSidebarFillColor");
      }
    }
    private string profileTextColor;
    public string ProfileTextColor
    {
      get
      {
        return this.profileTextColor;
      }
      set
      {
        if (string.Equals(this.profileTextColor, value, StringComparison.Ordinal))
          return;
        this.profileTextColor = value;
        base.RaisePropertyChanged("ProfileTextColor");
      }
    }
    private bool profileUseBackgroundImage;
    public bool ProfileUseBackgroundImage
    {
      get
      {
        return this.profileUseBackgroundImage;
      }
      set
      {
        if (this.profileUseBackgroundImage == value)
          return;
        this.profileUseBackgroundImage = value;
        base.RaisePropertyChanged("ProfileUseBackgroundImage");
      }
    }
    private string screenName;
    public string ScreenName
    {
      get
      {
        return this.screenName;
      }
      set
      {
        if (string.Equals(this.screenName, value, StringComparison.Ordinal))
          return;
        this.screenName = value;
        base.RaisePropertyChanged("ScreenName");
      }
    }
    private bool showAllInlineMedia;
    public bool ShowAllInlineMedia
    {
      get
      {
        return this.showAllInlineMedia;
      }
      set
      {
        if (this.showAllInlineMedia == value)
          return;
        this.showAllInlineMedia = value;
        base.RaisePropertyChanged("ShowAllInlineMedia");
      }
    }
    private Status status;
    public Status Status
    {
      get
      {
        return this.status;
      }
      set
      {
        if (this.status == value)
          return;
        this.status = value;
        base.RaisePropertyChanged("Status");
      }
    }
    private string stringId;
    public string StringId
    {
      get
      {
        return this.stringId;
      }
      set
      {
        if (string.Equals(this.stringId, value, StringComparison.Ordinal))
          return;
        this.stringId = value;
        base.RaisePropertyChanged("StringId");
      }
    }
    private string timeZone;
    public string TimeZone
    {
      get
      {
        return this.timeZone;
      }
      set
      {
        if (string.Equals(this.timeZone, value, StringComparison.Ordinal))
          return;
        this.timeZone = value;
        base.RaisePropertyChanged("TimeZone");
      }
    }
    private double? timeZoneOffset;
    public double? TimeZoneOffset
    {
      get
      {
        return this.timeZoneOffset;
      }
      set
      {
        if (Nullable.Equals<double>(this.timeZoneOffset, value))
          return;
        this.timeZoneOffset = value;
        base.RaisePropertyChanged("TimeZoneOffset");
      }
    }
    private bool verified;
    public bool Verified
    {
      get
      {
        return this.verified;
      }
      set
      {
        if (this.verified == value)
          return;
        this.verified = value;
        base.RaisePropertyChanged("Verified");
      }
    }
    private string website;
    public string Website
    {
      get
      {
        return this.website;
      }
      set
      {
        if (string.Equals(this.website, value, StringComparison.Ordinal))
          return;
        this.website = value;
        base.RaisePropertyChanged("WebsiteWithoutProtocol");
        base.RaisePropertyChanged("Website");
      }
    }

    public string WebsiteWithoutProtocol
    {
      get
      {
        if (this.Website != null)
          return this.Website.Replace("http://", "");
        else
          return string.Empty;
      }
    }
    private string withheldInCountries;
    public string WithheldInCountries
    {
      get
      {
        return this.withheldInCountries;
      }
      set
      {
        if (string.Equals(this.withheldInCountries, value, StringComparison.Ordinal))
          return;
        this.withheldInCountries = value;
        base.RaisePropertyChanged("WithheldInCountries");
      }
    }
    private WithheldScope withheldScope;
    public WithheldScope WithheldScope
    {
      get
      {
        return this.withheldScope;
      }
      set
      {
        if (this.withheldScope == value)
          return;
        this.withheldScope = value;
        base.RaisePropertyChanged("WithheldScope");
      }
    }

    public MetroTwitUser(User User)
    {
      this.Reload(User);
    }

    public void Reload(User User)
    {
      try
      {
        if (User == null)
          return;
        this.ContributorsEnabled = User.ContributorsEnabled;
        this.CreatedDate = User.CreatedDate;
        base.RaisePropertyChanged("CreatedDateLongString");
        this.DefaultProfile = User.DefaultProfile;
        this.DefaultProfileImage = User.DefaultProfileImage;
        this.Description = User.Description;
        this.FollowRequestSent = User.FollowRequestSent;
        this.Id = User.Id;
        this.IsFollowedBy = User.IsFollowedBy || App.AppState.CurrentActiveAccount != null && App.AppState.CurrentActiveAccount.Cache.CachedUsers.ContainsKey("@" + User.ScreenName.ToLower());
        this.IsFollowing = User.IsFollowing;
        this.IsGeoEnabled = User.IsGeoEnabled;
        this.IsProfileBackgroundTiled = User.IsProfileBackgroundTiled;
        this.IsProtected = User.IsProtected;
        this.IsTranslator = User.IsTranslator;
        this.Language = User.Language;
        this.ListedCount = User.ListedCount;
        this.Location = User.Location;
        this.Name = User.Name;
        this.NumberOfFavorites = User.NumberOfFavorites;
        this.NumberOfFollowers = User.NumberOfFollowers;
        this.NumberOfFriends = User.NumberOfFriends;
        this.NumberOfStatuses = User.NumberOfStatuses;
        this.ProfileBannerLocation = User.ProfileBannerLocation;
        this.ProfileBackgroundColor = User.ProfileBackgroundColor;
        this.ProfileBackgroundImageLocation = User.ProfileBackgroundImageLocation;
        this.ProfileBackgroundImageSecureLocation = User.ProfileBackgroundImageSecureLocation;
        this.ProfileImageLocation = User.ProfileImageLocation;
        this.ProfileImageSecureLocation = User.ProfileImageSecureLocation;
        this.ProfileLinkColor = User.ProfileLinkColor;
        this.ProfileSidebarBorderColor = User.ProfileSidebarBorderColor;
        this.ProfileSidebarFillColor = User.ProfileSidebarFillColor;
        this.ProfileTextColor = User.ProfileTextColor;
        this.ProfileUseBackgroundImage = User.ProfileUseBackgroundImage;
        this.ScreenName = User.ScreenName;
        this.ShowAllInlineMedia = User.ShowAllInlineMedia;
        this.Status = User.Status;
        this.StringId = User.StringId;
        this.TimeZone = User.TimeZone;
        this.TimeZoneOffset = User.TimeZoneOffset;
        this.Verified = User.Verified;
        this.Website = User.Website;
        this.WithheldInCountries = User.WithheldInCountries;
        this.WithheldScope = User.WithheldScope;
      }
      catch (Exception ex)
      {
        BugSense.SendException(ex);
      }
    }
  }
}
