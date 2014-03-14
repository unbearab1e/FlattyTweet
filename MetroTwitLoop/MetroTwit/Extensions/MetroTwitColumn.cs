// Type: MetroTwit.Extensions.MetroTwitColumn
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;

namespace MetroTwit.Extensions
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
