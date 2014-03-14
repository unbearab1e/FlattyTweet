// Type: MetroTwit.AppContracts.NativeHandleContractSurrogate
// Assembly: MetroTwit.AppContracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A64DB2D0-7404-4326-9865-5F5529F23BFA
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\Contracts\MetroTwit.AppContracts.dll

using System;
using System.AddIn.Contract;

namespace MetroTwit.AppContracts
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
