
using System;
using System.AddIn.Contract;

namespace FlattyTweet.AppContracts
{
  [Serializable]
  public class NativeHandleContractSurrogate
  {
    private long _handle;

    public NativeHandleContractSurrogate(INativeHandleContract contract)
    {
      this._handle = contract.GetHandle().ToInt64();
    }

    public long GetHandle()
    {
      return this._handle;
    }
  }
}
