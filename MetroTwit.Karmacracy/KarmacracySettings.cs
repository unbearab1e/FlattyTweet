// Type: MetroTwit.Karmacracy.KarmacracySettings
// Assembly: MetroTwit.Karmacracy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D8758AA4-2444-4BFE-8204-2AC009585A92
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Karmacracy.dll

using System.ComponentModel;
using System.Reflection;

namespace MetroTwit.Karmacracy
{
  public class KarmacracySettings : IDataErrorInfo, INotifyPropertyChanged
  {
    public string Username { get; set; }

    public string APIKey { get; set; }

    public bool IsValid
    {
      get
      {
        bool flag = true;
        foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        {
          if (!string.IsNullOrEmpty(this[propertyInfo.Name]))
          {
            this.OnPropertyChanged(propertyInfo.Name);
            flag = false;
            break;
          }
        }
        return flag;
      }
    }

    public string Error
    {
      get
      {
        return (string) null;
      }
    }

    public string this[string name]
    {
      get
      {
        string str = (string) null;
        if (name == "Username" && string.IsNullOrEmpty(this.Username) && string.IsNullOrWhiteSpace(this.Username))
          str = "Username required.";
        if (name == "APIKey" && string.IsNullOrEmpty(this.APIKey) && string.IsNullOrWhiteSpace(this.APIKey))
          str = "API key required.";
        return str;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
