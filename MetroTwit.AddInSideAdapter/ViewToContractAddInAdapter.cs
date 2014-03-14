// Type: MetroTwit.AddInSideAdapter.ViewToContractAddInAdapter
// Assembly: MetroTwit.AddInSideAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0266BFFC-EF84-41CB-9612-44F86DC41AAC
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\AddInSideAdapters\MetroTwit.AddInSideAdapter.dll

using MetroTwit.AppContracts;
using MetroTwit.CustomAddInView;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace MetroTwit.AddInSideAdapter
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
