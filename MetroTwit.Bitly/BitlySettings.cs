// Type: MetroTwit.Bitly.BitlySettings
// Assembly: MetroTwit.Bitly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B2DBF1B-8845-4660-8620-D7CA34A41F2D
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bitly.dll

using System.ComponentModel;
using System.Reflection;

namespace MetroTwit.Bitly
{
  public class BitlySettings : IDataErrorInfo, INotifyPropertyChanged
  {
    private bool usePersonalLogin;
    private bool usejmp;

    public bool UsePersonalLogin
    {
      get
      {
        return this.usePersonalLogin;
      }
      set
      {
        this.usePersonalLogin = value;
        if (!value)
        {
          this.Username = string.Empty;
          this.APIKey = string.Empty;
          this.OnPropertyChanged("Username");
          this.OnPropertyChanged("APIKey");
        }
        this.OnPropertyChanged("UsePersonalLogin");
      }
    }

    public bool Usejmp
    {
      get
      {
        return this.usejmp;
      }
      set
      {
        this.usejmp = value;
        this.OnPropertyChanged("Usejmp");
      }
    }

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
        if (this.UsePersonalLogin)
        {
          if (name == "Username" && string.IsNullOrEmpty(this.Username) && string.IsNullOrWhiteSpace(this.Username))
            str = "Username required.";
          if (name == "APIKey" && string.IsNullOrEmpty(this.APIKey) && string.IsNullOrWhiteSpace(this.APIKey))
            str = "API key required.";
        }
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
