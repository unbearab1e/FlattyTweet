// Type: MetroTwit.Extensions.MTColumnCollection
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace MetroTwit.Extensions
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
