
using FlattyTweet.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FlattyTweet.View
{
    public partial class AddAccountView : UserControl, IComponentConnector
  {
    private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

    public AddAccountView(Decimal twitterAccountID, bool isOOBE = false)
    {
      this.InitializeComponent();
      this.DataContext = (object) new AccountManagementViewModel(twitterAccountID)
      {
        IsOOBE = isOOBE
      };
      (this.DataContext as AccountManagementViewModel).PropertyChanged += new PropertyChangedEventHandler(this.SignInView_PropertyChanged);
      this.Unloaded += new RoutedEventHandler(this.SignInView_AddAccount_Unloaded);
      AddAccountView.InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);
    }

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

    private void SignInView_AddAccount_Unloaded(object sender, RoutedEventArgs e)
    {
      (this.DataContext as AccountManagementViewModel).PropertyChanged -= new PropertyChangedEventHandler(this.SignInView_PropertyChanged);
      Mouse.OverrideCursor = (Cursor) null;
    }

    private void SignInView_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "URL") || (this.DataContext as AccountManagementViewModel).URL.Contains("FlattyTweet"))
        return;
      (this.DataContext as AccountManagementViewModel).ProgressVisible = false;
      this.browser.Source = new Uri((this.DataContext as AccountManagementViewModel).URL);
    }

    private void browser_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      this.loadingPageThrobber.Visibility = Visibility.Visible;
      if (e.Uri.AbsoluteUri.Contains("FlattyTweet"))
        (this.DataContext as AccountManagementViewModel).URL = e.Uri.AbsoluteUri;
      else
        this.AddressText.Text = e.Uri.AbsoluteUri;
    }

    private void browser_Navigated(object sender, NavigationEventArgs e)
    {
      Mouse.OverrideCursor = (Cursor) null;
      this.loadingPageThrobber.Visibility = Visibility.Collapsed;
      this.BackButton.IsEnabled = this.browser.CanGoBack;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      if (!this.browser.CanGoBack)
        return;
      this.browser.GoBack();
    }

    
  }
}
