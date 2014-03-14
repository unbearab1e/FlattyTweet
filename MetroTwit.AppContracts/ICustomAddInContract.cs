// Type: MetroTwit.AppContracts.ICustomAddInContract
// Assembly: MetroTwit.AppContracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A64DB2D0-7404-4326-9865-5F5529F23BFA
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\Contracts\MetroTwit.AppContracts.dll

using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace MetroTwit.AppContracts
{
  [AddInContract]
  public interface ICustomAddInContract : IContract
  {
    NativeHandleContractSurrogate GetUI(double width, double height);

    void ClearBrowser();

    void Navigate(string URL);
  }
}
