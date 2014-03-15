
using System.ComponentModel;

namespace FlattyTweet.TwitLonger
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
