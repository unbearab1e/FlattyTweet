// Type: MetroTwit.Extensions.CacheUser
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MetroTwit.Extensions
{
  public class CacheUser
  {
    private bool isDownloading = false;
    private BitmapImage userImage170;
    private BitmapImage userImage54;
    private BitmapImage userImage16;

    public Decimal TwitterID { get; set; }

    public string ScreenName { get; set; }

    public string FullName { get; set; }

    public string ImageURITwitterSecure { get; set; }

    public string ImageURILocal { get; set; }

    public string ImageURILocalOrig { get; set; }

    public DateTime Expiry { get; set; }

    public override bool Equals(object obj)
    {
      if (obj.GetType() != typeof (CacheUser))
        return false;
      else
        return (obj as CacheUser).TwitterID == this.TwitterID;
    }

    public Decimal GetHashCode()
    {
      return this.TwitterID;
    }


    internal async Task<object> UserImage(int size, bool CachedUser, decimal TwitterAccountID)
    {
        byte[] numArray;
        object obj;
        string empty;
        bool flag;
        bool flag1;
        bool flag2;
        bool flag3;
        while (this.isDownloading)
        {
            await Task.Delay(100);
        }
        int num = size;
        if (num != 16)
        {
            if (num == 54)
            {
                if (this.userImage54 != null)
                {
                    obj = this.userImage54;
                    return obj;
                }
            }
        }
        else if (this.userImage16 != null)
        {
            obj = this.userImage16;
            return obj;
        }
        Thread.CurrentThread.Name = "User Image";
        if (!string.IsNullOrEmpty(this.ImageURILocal))
        {
            empty = (Path.IsPathRooted(this.ImageURILocal) ? this.ImageURILocal : Path.Combine(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserImages), this.ImageURILocal));
        }
        else
        {
            empty = string.Empty;
        }
        string str = empty;
        flag = (str == null ? false : File.Exists(str));
        bool flag4 = flag;
        try
        {
            Stream memoryStream = new MemoryStream();
            bool flag5 = false;
            int num1 = 1;
            do
            {
                if (flag4)
                {
                    flag1 = false;
                }
                else
                {
                    flag1 = (flag4 ? true : !CachedUser);
                }
                if (flag1)
                {
                    numArray = CommonCommands.DownloadFile(this.ImageURITwitterSecure.Replace("_normal", "_bigger"));
                    if (numArray != null)
                    {
                        memoryStream = new MemoryStream(numArray);
                    }
                    else
                    {
                        obj = null;
                        return obj;
                    }
                }
                else
                {
                    flag3 = (!CachedUser || flag4 ? (new FileInfo(str)).Length != (long)0 : false);
                    if (!flag3)
                    {
                        try
                        {
                            this.isDownloading = true;
                            string str1 = this.ImageURITwitterSecure.Replace("_normal", "_bigger");
                            CommonCommands.DownloadFile(this.ImageURILocal, str1, this.ScreenName, CachedUser, TwitterAccountID);
                        }
                        finally
                        {
                            this.isDownloading = false;
                        }
                    }
                    numArray = File.ReadAllBytes(str);
                    memoryStream = new MemoryStream(numArray);
                }
                num1++;
                flag5 = CommonCommands.IsValidImage(memoryStream);
                flag2 = (flag5 ? false : num1 <= 2);
            }
            while (flag2);
            if (flag5)
            {
                try
                {
                    memoryStream.Seek((long)0, SeekOrigin.Begin);
                }
                catch
                {
                }
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.DecodePixelWidth = size;
                bitmapImage.DecodePixelHeight = size;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                num = size;
                if (num == 16)
                {
                    this.userImage16 = bitmapImage;
                    obj = this.userImage16;
                    return obj;
                }
                else if (num == 54)
                {
                    this.userImage54 = bitmapImage;
                    obj = this.userImage54;
                    return obj;
                }
            }
            else
            {
                obj = null;
                return obj;
            }
        }
        catch
        {
            obj = null;
            return obj;
        }
        obj = null;
        return obj;
    }

    internal void CloseImages()
    {
      this.userImage16 = (BitmapImage) null;
      this.userImage54 = (BitmapImage) null;
      this.userImage170 = (BitmapImage) null;
    }

    internal void DeleteImage()
    {
      this.CloseImages();
      if (!File.Exists(this.ImageURILocal))
        return;
      File.Delete(this.ImageURILocal);
    }
  }
}
