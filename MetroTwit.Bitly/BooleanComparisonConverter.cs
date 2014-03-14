// Type: MetroTwit.Bitly.BooleanComparisonConverter
// Assembly: MetroTwit.Bitly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B2DBF1B-8845-4660-8620-D7CA34A41F2D
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Bitly.dll

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroTwit.Bitly
{
  [ValueConversion(typeof (bool), typeof (bool))]
  public class BooleanComparisonConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (bool) (bool.Parse(parameter.ToString()) == bool.Parse(value.ToString()) ? 1 : 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (bool) (bool.Parse(parameter.ToString()) == bool.Parse(value.ToString()) ? 1 : 0);
    }
  }
}
