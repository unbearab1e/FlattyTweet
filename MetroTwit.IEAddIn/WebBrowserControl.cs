// Type: MetroTwit.IEAddIn.WebBrowserControl
// Assembly: MetroTwit.IEAddIn, Version=1.0.5168.42598, Culture=neutral, PublicKeyToken=null
// MVID: A02BE231-FC8A-4301-9B33-194F130CD844
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\AddIns\IeAddIn\MetroTwit.IEAddIn.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MetroTwit.IEAddIn
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
