
using System.Windows.Controls;

namespace FlattyTweet.ViewModel
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
