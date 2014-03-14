// Type: MetroTwit.Extensions.IntellisenseDataSource
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace MetroTwit.Extensions
{
  public class IntellisenseDataSource
  {
    private static Dictionary<Decimal, IntellisenseDataSource> instances = new Dictionary<Decimal, IntellisenseDataSource>();

    public WordMatcher MentionsCollection { get; set; }

    public WordMatcher TagsCollection { get; set; }

    static IntellisenseDataSource()
    {
    }

    private IntellisenseDataSource()
    {
      this.MentionsCollection = new WordMatcher();
      this.TagsCollection = new WordMatcher();
    }

    public static IntellisenseDataSource Instance(Decimal twitterAccountID)
    {
      if (!IntellisenseDataSource.instances.ContainsKey(twitterAccountID))
        IntellisenseDataSource.instances.Add(twitterAccountID, new IntellisenseDataSource());
      return IntellisenseDataSource.instances[twitterAccountID];
    }

    public void InitializeSampleData()
    {
      List<IntellisenseItem> list = new List<IntellisenseItem>();
      StreamReader streamReader = new StreamReader("wordlist.txt");
      string str1 = string.Empty;
      string str2;
      while ((str2 = streamReader.ReadLine()) != null)
      {
        string str3 = str2.Replace(" ", "");
        list.Add(new IntellisenseItem()
        {
          FilterValue = str3,
          DisplayValue = "@" + str3
        });
      }
      this.MentionsCollection.AddWords((IEnumerable<IntellisenseItem>) list, "@");
      this.TagsCollection.AddWords((IEnumerable<IntellisenseItem>) list, "#");
    }
  }
}
