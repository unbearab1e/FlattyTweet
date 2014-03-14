// Type: PropertyChanged.AlsoNotifyForAttribute
// Assembly: PropertyChanged, Version=1.45.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 42DCC808-0503-408B-B4B4-FF00128C77AB
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\PropertyChanged.dll

using System;

namespace PropertyChanged
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class AlsoNotifyForAttribute : Attribute
  {
    public AlsoNotifyForAttribute(string property)
    {
    }

    public AlsoNotifyForAttribute(string property, params string[] otherProperties)
    {
    }
  }
}
