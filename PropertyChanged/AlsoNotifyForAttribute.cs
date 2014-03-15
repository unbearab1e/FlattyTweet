
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
