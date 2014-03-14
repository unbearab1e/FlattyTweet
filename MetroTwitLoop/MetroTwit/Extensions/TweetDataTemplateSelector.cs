// Type: MetroTwit.Extensions.TweetDataTemplateSelector
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace MetroTwit.Extensions
{
  public class TweetDataTemplateSelector : DataTemplateSelector
  {
    public DataTemplate StatusTemplate { get; set; }

    public DataTemplate DirectMessageTemplate { get; set; }

    public DataTemplate SearchStatusTemplate { get; set; }

    public DataTemplate ProfileStatusTemplate { get; set; }

    public DataTemplate RetweetUsersTemplate { get; set; }

    public DataTemplate BackLogTemplate { get; set; }

    public DataTemplate RetweetFriendsTemplate { get; set; }

    public DataTemplate RetweetOthersTemplate { get; set; }

    public DataTemplate UserTemplate { get; set; }

    public DataTemplate ListTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (item is TwitterStatusExtended)
      {
        try
        {
          TwitterStatusExtended twitterStatusExtended = item as TwitterStatusExtended;
          if (twitterStatusExtended.TweetType == TweetListType.RetweetUsers)
            return this.RetweetUsersTemplate;
          if (twitterStatusExtended.IsRetweet || twitterStatusExtended.RetweetCountVisibility == Visibility.Visible || twitterStatusExtended.TweetType != TweetListType.UserTimeline)
            return this.StatusTemplate;
          else
            return this.ProfileStatusTemplate;
        }
        catch
        {
          return this.StatusTemplate;
        }
      }
      else
      {
        if (item is TwitterDirectMessageExtended)
          return this.DirectMessageTemplate;
        if (item is MetroTwitUser)
          return this.UserTemplate;
        if (item is TwitterListExtended)
          return this.ListTemplate;
        switch ((item as MetroTwitStatusBase).TweetListSpecial)
        {
          case TweetListSpecial.NotSpecial:
            return this.StatusTemplate;
          case TweetListSpecial.Backlog:
            return this.BackLogTemplate;
          case TweetListSpecial.RetweetFriends:
            ListBoxItem listBoxItem1 = (ListBoxItem) CommonCommands.FindParent((object) container, typeof (ListBoxItem));
            if (listBoxItem1 != null)
              listBoxItem1.IsEnabled = false;
            return this.RetweetFriendsTemplate;
          case TweetListSpecial.RetweetOthers:
            ListBoxItem listBoxItem2 = (ListBoxItem) CommonCommands.FindParent((object) container, typeof (ListBoxItem));
            if (listBoxItem2 != null)
              listBoxItem2.IsEnabled = false;
            return this.RetweetOthersTemplate;
          default:
            return this.StatusTemplate;
        }
      }
    }
  }
}
