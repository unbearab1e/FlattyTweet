
using System;

namespace PropertyChanged
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
  public class DependsOnAttribute : Attribute
  {
    public DependsOnAttribute(string dependency)
    {
    }

    public DependsOnAttribute(string dependency, params string[] otherDependencies)
    {
    }
  }
}
