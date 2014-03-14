// Type: MetroTwit.Model.UndoTweetState
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.ViewModel;
using System;

namespace MetroTwit.Model
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
