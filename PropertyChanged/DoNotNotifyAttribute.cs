
using System;

namespace PropertyChanged
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class DoNotNotifyAttribute : Attribute
  {
  }
}
