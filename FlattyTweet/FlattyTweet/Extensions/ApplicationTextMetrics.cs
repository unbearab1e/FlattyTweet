using GalaSoft.MvvmLight;

namespace FlattyTweet.Extensions
{
  public class ApplicationTextMetrics : ViewModelBase
  {
    private double tweetHeaderFontSize;
    private double tweetBodyFontSize;
    private double tweetFooterFontSize;
    private double tweetTextLineHeight;

    public double TweetHeaderFontSize
    {
      get
      {
        return this.tweetHeaderFontSize;
      }
      set
      {
        if (this.tweetHeaderFontSize == value)
          return;
        this.tweetHeaderFontSize = value;
        base.RaisePropertyChanged("TweetHeaderFontSize");
      }
    }

    public double TweetBodyFontSize
    {
      get
      {
        return this.tweetBodyFontSize;
      }
      set
      {
        if (this.tweetBodyFontSize == value)
          return;
        this.tweetBodyFontSize = value;
        base.RaisePropertyChanged("TweetBodyFontSize");
      }
    }

    public double TweetFooterFontSize
    {
      get
      {
        return this.tweetFooterFontSize;
      }
      set
      {
        if (this.tweetFooterFontSize == value)
          return;
        this.tweetFooterFontSize = value;
        base.RaisePropertyChanged("TweetFooterFontSize");
      }
    }

    public double TweetTextLineHeight
    {
      get
      {
        return this.tweetTextLineHeight;
      }
      set
      {
        if (this.tweetTextLineHeight == value)
          return;
        this.tweetTextLineHeight = value;
        base.RaisePropertyChanged("TweetTextLineHeight");
      }
    }

    public void SetFontSize(TweetFontSizeDisplay fontSize)
    {
      switch (fontSize)
      {
        case TweetFontSizeDisplay.Small:
          this.TweetHeaderFontSize = 13.0;
          this.TweetBodyFontSize = 12.0;
          this.TweetFooterFontSize = 12.0;
          this.TweetTextLineHeight = 15.0;
          break;
        case TweetFontSizeDisplay.Medium:
          this.TweetHeaderFontSize = 15.0;
          this.TweetBodyFontSize = 13.0;
          this.TweetFooterFontSize = 13.0;
          this.TweetTextLineHeight = 18.0;
          break;
        case TweetFontSizeDisplay.Large:
          this.TweetHeaderFontSize = 17.0;
          this.TweetBodyFontSize = 15.0;
          this.TweetFooterFontSize = 15.0;
          this.TweetTextLineHeight = 19.0;
          break;
      }
    }
  }
}
