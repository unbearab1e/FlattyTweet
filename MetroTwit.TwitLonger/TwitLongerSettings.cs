// Type: MetroTwit.TwitLonger.TwitLongerSettings
// Assembly: MetroTwit.TwitLonger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1724F65-D568-4C76-942B-059AE183E337
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.TwitLonger.dll

using System.ComponentModel;

namespace MetroTwit.TwitLonger
{
  public class TwitLongerSettings : INotifyPropertyChanged
  {
    private bool useTwitLonger = true;

    public bool UseTwitLonger
    {
      get
      {
        return this.useTwitLonger;
      }
      set
      {
        this.useTwitLonger = value;
        this.OnPropertyChanged("UseTwitLonger");
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
