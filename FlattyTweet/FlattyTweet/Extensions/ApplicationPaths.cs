using System;
using System.Collections.Generic;
using System.IO;

namespace FlattyTweet.Extensions
{
  public static class ApplicationPaths
  {
    private static string[,] tree = new string[1, 3]
    {
      {
        "AppConfigPath",
        "user_app_data",
        "FlattyTweet"
      }
    };
    private static Dictionary<string, string> pathMap = new Dictionary<string, string>();
    private static string profileOptimisationsPath;

    public static string UserConfigFilename
    {
      get
      {
        return "User.config";
      }
    }

    public static string AppConfigFilename
    {
      get
      {
        return "FlattyTweet.config";
      }
    }

    public static string AppConfigPath
    {
      get
      {
        return ApplicationPaths.pathMap["AppConfigPath"];
      }
    }

    public static string ProfileOptimisationsPath
    {
      get
      {
        return ApplicationPaths.profileOptimisationsPath;
      }
    }

    public static string ConfigFile
    {
      get
      {
        string appConfigPath = ApplicationPaths.AppConfigPath;
        if (!Directory.Exists(appConfigPath))
          Directory.CreateDirectory(appConfigPath);
        return Path.Combine(appConfigPath, ApplicationPaths.AppConfigFilename);
      }
    }

    public static string ConfigBackup
    {
      get
      {
        string appConfigPath = ApplicationPaths.AppConfigPath;
        if (!Directory.Exists(appConfigPath))
          Directory.CreateDirectory(appConfigPath);
        return Path.Combine(appConfigPath, "FlattyTweet.backup");
      }
    }

    public static string CacheFile
    {
      get
      {
        string appConfigPath = ApplicationPaths.AppConfigPath;
        if (!Directory.Exists(appConfigPath))
          Directory.CreateDirectory(appConfigPath);
        return Path.Combine(appConfigPath, "FlattyTweet.cache");
      }
    }

    static ApplicationPaths()
    {
      ApplicationPaths.pathMap["user_app_data"] = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      ApplicationPaths.BuildTree();
    }

    private static void BuildTree()
    {
      for (int index = 0; index <= ApplicationPaths.tree.GetUpperBound(0); ++index)
      {
        string path = Path.Combine(ApplicationPaths.pathMap[ApplicationPaths.tree[index, 1]], ApplicationPaths.tree[index, 2]);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        ApplicationPaths.pathMap[ApplicationPaths.tree[index, 0]] = path;
      }
      ApplicationPaths.profileOptimisationsPath = Path.Combine(ApplicationPaths.pathMap["AppConfigPath"], "ProfileOptimisations");
      if (Directory.Exists(ApplicationPaths.profileOptimisationsPath))
        return;
      Directory.CreateDirectory(ApplicationPaths.profileOptimisationsPath);
    }

    public static string UserSettings(Decimal TwitterAccountID, FileType UserFileType)
    {
      string str = Path.Combine(ApplicationPaths.AppConfigPath, TwitterAccountID.ToString() + "\\");
      if (!Directory.Exists(str))
        Directory.CreateDirectory(str);
      switch (UserFileType)
      {
        case FileType.Cache:
          return Path.Combine(str, "User.cache");
        case FileType.UserSettings:
          return Path.Combine(str, ApplicationPaths.UserConfigFilename);
        case FileType.UserSettingsBackup:
          return Path.Combine(str, "User.backup");
        case FileType.UserImages:
          string path = Path.Combine(str, "User_Images\\");
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
          return path;
        default:
          return string.Empty;
      }
    }
  }
}
