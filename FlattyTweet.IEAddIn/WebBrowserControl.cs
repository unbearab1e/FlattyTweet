
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace FlattyTweet.IEAddIn
{
  public partial class WebBrowserControl : UserControl, IComponentConnector
  {

    public WebBrowserControl()
    {
      this.InitializeComponent();
      this.browser.Navigated += new NavigatedEventHandler(this.browser_Navigated);
    }

    private void browser_Navigated(object sender, NavigationEventArgs e)
    {
      this.SuppressScriptErrors(this.browser, true);
    }

    public void SuppressScriptErrors(WebBrowser wb, bool Hide)
    {
      try
      {
        FieldInfo field = typeof (WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
        if (!(field != (FieldInfo) null))
          return;
        object target = field.GetValue((object) wb);
        if (target != null)
          target.GetType().InvokeMember("Silent", BindingFlags.SetProperty, (Binder) null, target, new object[1]
          {
            (object) Hide
          });
      }
      catch
      {
      }
    }

    
  }
}
