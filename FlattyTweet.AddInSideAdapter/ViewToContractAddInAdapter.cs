
using FlattyTweet.AppContracts;
using FlattyTweet.CustomAddInView;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace FlattyTweet.AddInSideAdapter
{
  [AddInAdapter]
  public class ViewToContractAddInAdapter : ContractBase, ICustomAddInContract, IContract
  {
    private CustomAddIn _view;
    private static AppDispatcher ad;

    public ViewToContractAddInAdapter(CustomAddIn view)
    {
      this._view = view;
      ViewToContractAddInAdapter.ad = AppDispatcher.Current;
    }

    public NativeHandleContractSurrogate GetUI(double width, double height)
    {
      NativeHandleContractSurrogate value = (NativeHandleContractSurrogate) null;
      ViewToContractAddInAdapter.ad.DoWork((Worker) (() => value = new NativeHandleContractSurrogate(FrameworkElementAdapters.ViewToContractAdapter(this._view.GetUI(width, height)))));
      return value;
    }

    public void ClearBrowser()
    {
      ViewToContractAddInAdapter.ad.DoWork((Worker) (() => this._view.ClearBrowser()));
    }

    public void Navigate(string URL)
    {
      ViewToContractAddInAdapter.ad.DoWork((Worker) (() => this._view.Navigate(URL)));
    }
  }
}
