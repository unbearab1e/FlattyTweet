
using System.Windows;
using System.Windows.Controls;

namespace FlattyTweet.Extensions
{
  public static class PasswordBoxHelper
  {
    public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached("BoundPassword", typeof (string), typeof (PasswordBoxHelper), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(PasswordBoxHelper.OnBoundPasswordChanged)));
    public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached("BindPassword", typeof (bool), typeof (PasswordBoxHelper), new PropertyMetadata((object) false, new PropertyChangedCallback(PasswordBoxHelper.OnBindPasswordChanged)));
    private static readonly DependencyProperty UpdatingPassword = DependencyProperty.RegisterAttached("UpdatingPassword", typeof (bool), typeof (PasswordBoxHelper), new PropertyMetadata((object) false));

    static PasswordBoxHelper()
    {
    }

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PasswordBox passwordBox = d as PasswordBox;
      if (d == null || !PasswordBoxHelper.GetBindPassword(d))
        return;
      passwordBox.PasswordChanged -= new RoutedEventHandler(PasswordBoxHelper.HandlePasswordChanged);
      string str = (string) e.NewValue;
      if (!PasswordBoxHelper.GetUpdatingPassword((DependencyObject) passwordBox))
        passwordBox.Password = str;
      passwordBox.PasswordChanged += new RoutedEventHandler(PasswordBoxHelper.HandlePasswordChanged);
    }

    private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
      PasswordBox passwordBox = dp as PasswordBox;
      if (passwordBox == null)
        return;
      bool flag1 = (bool) e.OldValue;
      bool flag2 = (bool) e.NewValue;
      if (flag1)
        passwordBox.PasswordChanged -= new RoutedEventHandler(PasswordBoxHelper.HandlePasswordChanged);
      if (!flag2)
        return;
      passwordBox.PasswordChanged += new RoutedEventHandler(PasswordBoxHelper.HandlePasswordChanged);
    }

    private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
    {
      PasswordBox passwordBox = sender as PasswordBox;
      PasswordBoxHelper.SetUpdatingPassword((DependencyObject) passwordBox, true);
      PasswordBoxHelper.SetBoundPassword((DependencyObject) passwordBox, passwordBox.Password);
      PasswordBoxHelper.SetUpdatingPassword((DependencyObject) passwordBox, false);
    }

    public static void SetBindPassword(DependencyObject dp, bool value)
    {
        dp.SetValue(BindPassword, value);
    }

    public static bool GetBindPassword(DependencyObject dp)
    {
      return (bool) dp.GetValue(PasswordBoxHelper.BindPassword);
    }

    public static string GetBoundPassword(DependencyObject dp)
    {
      return (string) dp.GetValue(PasswordBoxHelper.BoundPassword);
    }

    public static void SetBoundPassword(DependencyObject dp, string value)
    {
      dp.SetValue(PasswordBoxHelper.BoundPassword, (object) value);
    }

    private static bool GetUpdatingPassword(DependencyObject dp)
    {
      return (bool) dp.GetValue(PasswordBoxHelper.UpdatingPassword);
    }

    private static void SetUpdatingPassword(DependencyObject dp, bool value)
    {
        dp.SetValue(UpdatingPassword, value);
    }
  }
}
