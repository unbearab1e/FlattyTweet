// Type: MetroTwit.HostSideAdapter.IMyNativeHandle
// Assembly: MetroTwit.HostSideAdapter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE03F259-053C-430F-8311-6D202E97837B
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\HostSideAdapters\MetroTwit.HostSideAdapter.dll

using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace MetroTwit.HostSideAdapter
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
