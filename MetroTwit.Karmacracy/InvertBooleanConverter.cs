// Type: MetroTwit.Karmacracy.InvertBooleanConverter
// Assembly: MetroTwit.Karmacracy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D8758AA4-2444-4BFE-8204-2AC009585A92
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwit.Karmacracy.dll

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroTwit.Karmacracy
{
  [ValueConversion(typeof (bool), typeof (bool))]
  public class InvertBooleanConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (bool) (!bool.Parse(value.ToString()) ? 1 : 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (bool) (!bool.Parse(value.ToString()) ? 1 : 0);
    }
  }
}
