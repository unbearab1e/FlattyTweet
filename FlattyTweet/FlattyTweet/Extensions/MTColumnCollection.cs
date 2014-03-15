
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace FlattyTweet.Extensions
{
  [XmlRoot("Columns")]
  [Serializable]
  public class MTColumnCollection : ObservableCollection<MetroTwitColumn>
  {
    public MetroTwitColumn this[Guid ID]
    {
      get
      {
        IEnumerable<MetroTwitColumn> source = Enumerable.Where<MetroTwitColumn>((IEnumerable<MetroTwitColumn>) this, (Func<MetroTwitColumn, bool>) (x => x.ID == ID));
        if (source != null && Enumerable.Count<MetroTwitColumn>(source) > 0)
          return Enumerable.FirstOrDefault<MetroTwitColumn>(source);
        else
          return (MetroTwitColumn) null;
      }
    }

    public MTColumnCollection()
      : base(new List<MetroTwitColumn>())
    {
    }

    public MTColumnCollection(List<MetroTwitColumn> columns)
      : base(columns)
    {
    }
  }
}
