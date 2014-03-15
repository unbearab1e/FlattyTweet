
using System;

namespace PropertyChanged
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class DoNotSetChangedAttribute : Attribute
  {
  }
}
