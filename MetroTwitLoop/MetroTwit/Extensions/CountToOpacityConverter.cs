﻿// Type: MetroTwit.Extensions.CountToOpacityConverter
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace MetroTwit.Extensions
{
  [ValueConversion(typeof (int), typeof (double))]
  public class CountToOpacityConverter : IValueConverter
  {
    public double TargetOpacity { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int result = -1;
      if (value != null && (value is int || int.TryParse(value.ToString(), out result) || value.ToString() == "+") && (result > 0 || value.ToString() == "+"))
        return (object) 1.0;
      else
        return (object) this.TargetOpacity;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) null;
    }
  }
}