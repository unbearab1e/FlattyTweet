
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace FlattyTweet.AppContracts
{
  [AddInContract]
  public interface ICustomAddInContract : IContract
  {
    NativeHandleContractSurrogate GetUI(double width, double height);

    void ClearBrowser();

    void Navigate(string URL);
  }
}
