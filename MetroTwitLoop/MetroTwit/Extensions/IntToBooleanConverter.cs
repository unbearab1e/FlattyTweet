namespace MetroTwit.Extensions
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((int)value) == int.Parse(parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value != null) && (parameter != null))
            {
                bool flag = (bool)value;
                string s = parameter.ToString();
                if (flag)
                {
                    return int.Parse(s);
                }
            }
            return null;
        }
    }
}

