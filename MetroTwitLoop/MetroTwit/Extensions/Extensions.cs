// Type: MetroTwit.Extensions.Extensions
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace MetroTwit.Extensions
{
  public static class Extensions
  {
    private static byte[] entropy = Encoding.Unicode.GetBytes("Windows Twitter Client you'll love to use.");
    internal static string mtpass = "mtpass-";

    static Extensions()
    {
    }

    public static void AddRange<T>(this IList<T> oc, IEnumerable<T> nc)
    {
      if (nc == null)
        throw new ArgumentNullException("nc");
      foreach (T obj in nc)
        oc.Add(obj);
    }

    public static void AddTwitterRange<T>(this Collection<T> oc, IEnumerable<T> nc)
    {
      if (nc == null)
        return;
      foreach (T obj in nc)
        oc.Add(obj);
    }

    public static void InsertRange<T>(this IList<T> oc, IEnumerable<T> nc)
    {
      if (nc == null)
        return;
      int index = 0;
      foreach (T obj in nc)
      {
        oc.Insert(index, obj);
        ++index;
      }
    }

    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
    {
      T[] array = (T[]) null;
      int count = 0;
      foreach (T obj in source)
      {
        if (array == null)
          array = new T[size];
        array[count] = obj;
        ++count;
        if (count == size)
        {
          yield return (IEnumerable<T>) new ReadOnlyCollection<T>((IList<T>) array);
          array = (T[]) null;
          count = 0;
        }
      }
      if (array != null)
      {
        Array.Resize<T>(ref array, count);
        yield return (IEnumerable<T>) new ReadOnlyCollection<T>((IList<T>) array);
      }
    }

    public static string EncryptString(this string input)
    {
      byte[] inArray = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), Extensions.entropy, DataProtectionScope.CurrentUser);
      return Extensions.mtpass + Convert.ToBase64String(inArray);
    }

    public static string DecryptStringEx(this string encryptedData)
    {
      try
      {
        encryptedData = encryptedData.Replace(Extensions.mtpass, "");
        return Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), Extensions.entropy, DataProtectionScope.CurrentUser));
      }
      catch
      {
        return string.Empty;
      }
    }

    public static string MD5String(this string Orig)
    {
      if (Orig == null)
        return string.Empty;
      MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
      UTF8Encoding utF8Encoding = new UTF8Encoding();
      byte[] hash = cryptoServiceProvider.ComputeHash(utF8Encoding.GetBytes(Orig));
      cryptoServiceProvider.Clear();
      return Convert.ToBase64String(hash);
    }

    internal static string Signature(this string str)
    {
      byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(str));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return ((object) stringBuilder).ToString();
    }

    public static string DecryptString(this string Message)
    {
      UTF8Encoding utF8Encoding = new UTF8Encoding();
      string s = WindowsIdentity.GetCurrent().User.AccountDomainSid.ToString();
      MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
      byte[] hash = cryptoServiceProvider1.ComputeHash(utF8Encoding.GetBytes(s));
      TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
      cryptoServiceProvider2.Key = hash;
      cryptoServiceProvider2.Mode = CipherMode.ECB;
      cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
      byte[] inputBuffer = Convert.FromBase64String(Message);
      byte[] bytes;
      try
      {
        bytes = cryptoServiceProvider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
      }
      finally
      {
        cryptoServiceProvider2.Clear();
        cryptoServiceProvider1.Clear();
      }
      return utF8Encoding.GetString(bytes);
    }

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
      return source != null && source.IndexOf(toCheck, comp) >= 0;
    }
  }
}
