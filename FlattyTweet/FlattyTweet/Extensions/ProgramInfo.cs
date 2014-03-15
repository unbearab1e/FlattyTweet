
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FlattyTweet.Extensions
{
  public static class ProgramInfo
  {
    public static string AssemblyGuid
    {
      get
      {
        object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (GuidAttribute), false);
        if (customAttributes.Length == 0)
          return string.Empty;
        else
          return ((GuidAttribute) customAttributes[0]).Value;
      }
    }

    public static string AssemblyTitle
    {
      get
      {
        object[] customAttributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
        if (customAttributes.Length > 0)
        {
          AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute) customAttributes[0];
          if (assemblyTitleAttribute.Title != "")
            return assemblyTitleAttribute.Title;
        }
        return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
      }
    }
  }
}
