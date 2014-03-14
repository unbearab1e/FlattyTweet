// Type: MetroTwit.View.UpdateView
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;

namespace MetroTwit.View
{
    public partial class UpdateView : Window, IComponentConnector
  {
    public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof (string), typeof (UpdateView), (PropertyMetadata) new UIPropertyMetadata((object) string.Empty));
    private double progressValue;
    private static UpdateView Updater;

    public string Caption
    {
      get
      {
        return this.Title;
      }
      set
      {
        this.Title = value;
      }
    }

    public double ProgressValue
    {
      get
      {
        return this.progressValue;
      }
      set
      {
        this.progressValue = value;
      }
    }

    public string DisplayText
    {
      get
      {
        return (string) this.GetValue(UpdateView.DisplayTextProperty);
      }
      set
      {
        this.SetValue(UpdateView.DisplayTextProperty, (object) value);
      }
    }

    static UpdateView()
    {
    }

    public UpdateView()
    {
      this.InitializeComponent();
    }

    public static void DoUpdate()
    {
      UpdateView.Updater = new UpdateView();
      UpdateView.Updater.Title = "downloading update";
      UpdateView.Updater._Restart.IsEnabled = false;
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
      UpdateView.Updater.Show();
      Task task = new Task((Action) (() => UpdateView.rssWorker()));
      task.ContinueWith((Action<Task>) (t => CommonCommands.CheckTaskExceptions(t)));
      task.Start();
      try
      {
        ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
        Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Visible), (object) ViewModelMessages.OverlayVisible);
        currentDeployment.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(UpdateView.Updater.ad_UpdateProgressChanged);
        currentDeployment.UpdateCompleted += new AsyncCompletedEventHandler(UpdateView.Updater.ad_UpdateCompleted);
        currentDeployment.UpdateAsync();
      }
      catch
      {
        UpdateView.Updater.ResultMessage.Text = "MetroTwit is not installed correctly, please reinstall MetroTwit.";
        UpdateView.Updater.ProgressDock.Visibility = Visibility.Collapsed;
        UpdateView.Updater._Restart.IsEnabled = true;
      }
    }

    private static void rssWorker()
    {
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(new Uri("http://www.metrotwit.com/category/loop-releases/feed/"));
        httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        string text = string.Empty;
        using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
          text = streamReader.ReadToEnd();
        IEnumerable<MetroTwitSiteFeed> items = Enumerable.Select<XElement, MetroTwitSiteFeed>(XDocument.Parse(text).Descendants((XName) "item"), (Func<XElement, MetroTwitSiteFeed>) (item => new MetroTwitSiteFeed()
        {
          Title = item.Element((XName) "title").Value,
          ReleaseDateString = item.Element((XName) "pubDate").Value,
          Text = item.Element((XName) "{http://purl.org/rss/1.0/modules/content/}encoded").Value
        }));
        Application.Current.Dispatcher.Invoke((Action) (() =>
        {
          MetroTwitSiteFeed local_0 = Enumerable.First<MetroTwitSiteFeed>(items);
          DateTime local_1;
          if (DateTime.TryParse(local_0.ReleaseDateString, out local_1))
            local_0.ReleaseDate = local_1.ToLocalTime();
          else
            local_0.ReleaseDateVisibility = Visibility.Collapsed;
          string local_2 = WebUtility.HtmlDecode(local_0.Text).Replace("\n", "\r\n");
          local_0.Text = Regex.Replace(local_2, "<(.)*?>", string.Empty);
          UpdateView.Updater.DataContext = (object) local_0;
        }));
      }
      catch
      {
        Application.Current.Dispatcher.Invoke((Action) (() => UpdateView.Updater._Restart.IsEnabled = true));
      }
    }

    private void cancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
      Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object) Visibility.Collapsed), (object) ViewModelMessages.OverlayVisible);
      App.RestartApplication();
    }

    private void ad_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        if (e.Error.GetType() == typeof (DeploymentDownloadException))
        {
          this.ResultMessage.Text = "Cannot install the latest version of MetroTwit. \n\nPlease check your network connection, and try again later. Error: " + e.Error.Message;
          UpdateView.Updater.ProgressDock.Visibility = Visibility.Collapsed;
        }
      }
      else
      {
        UpdateView.Updater.Title = "update downloaded";
        this.ResultMessage.Text = "Restart app to complete updating.";
        UpdateView.Updater.ProgressDock.Visibility = Visibility.Collapsed;
      }
      SettingsData.Instance.QuietUpdating = false;
      this._Restart.IsEnabled = true;
      this._Restart.IsDefault = true;
    }

    private void ad_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
    {
      this.ProgressBar.Value = (double) e.ProgressPercentage;
    }

    
  }
}
