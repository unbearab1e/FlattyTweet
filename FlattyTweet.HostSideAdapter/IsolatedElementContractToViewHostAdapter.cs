namespace FlattyTweet.HostSideAdapter
{
    using FlattyTweet.AppContracts;
    using FlattyTweet.HostView;
    using System;
    using System.AddIn.Pipeline;
    using System.Windows;

    [HostAdapter]
    public class IsolatedElementContractToViewHostAdapter : CustomAddIn
    {
        private ICustomAddInContract _contract;
        private ContractHandle _tokenHandle;

        public IsolatedElementContractToViewHostAdapter(ICustomAddInContract contract)
        {
            this._contract = contract;
            this._tokenHandle = new ContractHandle(contract);
        }

        public override void ClearBrowser()
        {
            try
            {
                this._contract.ClearBrowser();
            }
            catch
            {
            }
        }

        public override FrameworkElement GetUI(double width, double height)
        {
            IMyNativeHandle nativeHandleContract = new IMyNativeHandle(this._contract.GetUI(width, height).GetHandle());
            return FrameworkElementAdapters.ContractToViewAdapter(nativeHandleContract);
        }

        public override void Navigate(string URL)
        {
            try
            {
                this._contract.Navigate(URL);
            }
            catch
            {
            }
        }
    }
}

