
using FlattyTweet.ViewModel;
using System;

namespace FlattyTweet.Model
{
  public class UndoTweetState
  {
    public Decimal ReplyToID;
    public string UserName;

    public Decimal Id { get; set; }

    public TwitViewModel.NewTweetType TweetType { get; set; }

    public string LastTweetText { get; set; }
  }
}
