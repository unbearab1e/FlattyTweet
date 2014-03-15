
using System;
using System.Collections.Generic;
using System.IO;

namespace FlattyTweet.Extensions
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
