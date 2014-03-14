// Type: MetroTwit.ViewModel.SearchTerm
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Windows.Controls;

namespace MetroTwit.ViewModel
{
  public class SearchTerm
  {
    public RelationalOperator Operator { get; set; }

    public string SearchText { get; set; }

    public ComboBoxItem SearchOperator { get; set; }

    public bool IsExact { get; set; }

    public SearchTerm()
    {
      this.Operator = RelationalOperator.AND;
    }
  }
}
