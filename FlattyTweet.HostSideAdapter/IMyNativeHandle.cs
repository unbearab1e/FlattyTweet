
using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace FlattyTweet.HostSideAdapter
{
  public class IMyNativeHandle : ContractBase, INativeHandleContract, IContract
  {
    private IntPtr _handle;

    public IMyNativeHandle(long handle)
    {
      this._handle = (IntPtr) handle;
    }

    public IntPtr GetHandle()
    {
      return this._handle;
    }
  }
}
