
using System;

namespace FlattyTweet.Extensions
{
  [Serializable]
  public class MetroTwitColumn
  {
    public bool ColumnIsSetWidth = false;
    public bool ListRetweets = false;
    public bool TaskbarNotification = true;
    public bool ToastNotification = true;
    public bool SoundNotification = true;
    public bool ColumnPinned = true;
    public int Index;

    public string ColumnHeader { get; set; }

    public string ColumnValue { get; set; }

    public TweetListType ColumnType { get; set; }

    public double ColumnWidth { get; set; }

    public Decimal ColumnLastUpdateID { get; set; }

    public Guid ID { get; set; }

    public int NestID { get; set; }

    public Decimal? SearchID { get; set; }

    public MetroTwitColumn()
    {
      this.ListRetweets = true;
    }
  }
}
