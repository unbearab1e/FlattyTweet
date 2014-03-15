
using System.Windows.Controls;
using FlattyTweet;
using FlattyTweet.ViewModel;
using FlattyTweet.Model;
using System;
using System.Runtime.CompilerServices;

namespace FlattyTweet.View
{
  public partial class BaseTweetListView : UserControl
  {
    public virtual ListBox TweetListBox { get; set; }
  }
}
