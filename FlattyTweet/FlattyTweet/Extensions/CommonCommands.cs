using Bugsense.WPF;
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Model;
using FlattyTweet.ViewModel;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Twitterizer;
using Twitterizer.Models;

namespace FlattyTweet.Extensions
{
    public class CommonCommands
    {
        private static object DefaultAvatarLock = new object();
        private static BitmapImage defaultUserImage;

        static CommonCommands()
        {
        }

        public static void ExecuteTag(string tag, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.MainWindowShow);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)tag), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Search, TwitterAccountID));
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void ExecuteUserProfile(string username, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.MainWindowShow);
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)username), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowUserProfile, TwitterAccountID));
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void ExecuteLink(object link)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (link == null)
                    return;
                if (link.GetType() == typeof(UrlEntity) || link.GetType() == typeof(MediaEntity))
                    CommonCommands.OpenLink((link as UrlEntity).Url);
                else
                    CommonCommands.OpenLink(link.ToString());
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void ExecuteContentLink(UrlEntity link)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (link == null)
                    return;
                if (SettingsData.Instance.ShowMediaPreviews)
                    Messenger.Default.Send<GenericMessage<UrlEntity>>(new GenericMessage<UrlEntity>(link), (object)DialogType.MediaPreview);
                else
                    CommonCommands.OpenLink(link.Url);
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void Retweet(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!orig.User.IsProtected && orig.AdUrls == null)
                    Messenger.Default.Send<RetweetMessage>(new RetweetMessage((Action<MessageBoxResult, Decimal>)(async (answer, account) =>
                    {
                        if (answer == MessageBoxResult.Yes)
                        {
                            Decimal ID = orig.IsRetweet ? orig.OriginalID : orig.ID;
                            TwitterResponse<Status> r = await Tweets.RetweetAsync(App.AppState.Accounts[account].Tokens, ID, MetroTwitTwitterizer.Options);
                            if (r.Result != RequestResult.Success)
                                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.RetweetError)), new object[0]);
                        }
                        if (answer == MessageBoxResult.No)
                            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Retweet, account));
                    })), (object)DialogType.RetweetDialog);
                else
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Retweet, orig.TwitterAccountID));
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void DirectMessage(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.DirectMessage, orig.TwitterAccountID))), DispatcherPriority.Background, new object[0]);
        }

        public static void Reply(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Reply, orig.TwitterAccountID));
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void ReplyAll(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.ReplyAll, orig.TwitterAccountID));
            }), DispatcherPriority.Background, new object[0]);
        }

        public static void OpenLink(string link)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo(Uri.UnescapeDataString(link))
                    {
                        UseShellExecute = true
                    });
                }
                catch
                {
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(answer =>
                    {
                        if (answer != MessageBoxResult.Yes)
                            return;
                        CommonCommands.OpenLink(link);
                    })), (object)DialogType.LinkOpenError);
                }
            }), DispatcherPriority.Background, new object[0]);
        }

        public static bool Favourite(Decimal id, Decimal TwitterAccountID)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Favorites.CreateAsync(App.AppState.Accounts[TwitterAccountID].Tokens, id, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<Status>>>)(r =>
                {
                    if (r.Result.Result != RequestResult.Success)
                        return;
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.FavouriteTweet, TwitterAccountID));
                }))), DispatcherPriority.Background, new object[0]);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool UnFavourite(Decimal id, Decimal TwitterAccountID)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Favorites.DeleteAsync(App.AppState.Accounts[TwitterAccountID].Tokens, id, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<Status>>>)(r =>
                {
                    if (r.Result.Result != RequestResult.Success)
                        return;
                    Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.RemoveFavouriteTweet, TwitterAccountID));
                }))), DispatcherPriority.Background, new object[0]);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteTweet(Decimal id, Decimal TwitterAccountID, Action<RequestResult> callback = null, bool suppressPrompt = false)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!suppressPrompt)
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(answer =>
                    {
                        if (answer != MessageBoxResult.Yes)
                            return;
                        CommonCommands.InternalDeleteTweet(id, TwitterAccountID, callback);
                    })), (object)DialogType.DeleteTweet);
                else
                    CommonCommands.InternalDeleteTweet(id, TwitterAccountID, callback);
            }), DispatcherPriority.Background, new object[0]);
        }

        private static async void InternalDeleteTweet(Decimal id, Decimal TwitterAccountID, Action<RequestResult> callback = null)
        {
            TwitterResponse<Status> r = await Tweets.DeleteAsync(App.AppState.Accounts[TwitterAccountID].Tokens, id, MetroTwitTwitterizer.Options);
            if (r.Result == RequestResult.Success)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.DeleteTweet, TwitterAccountID));
            if (callback != null)
                callback(r.Result);
        }

        public static void DeleteDirectMessage(Decimal id, Decimal TwitterAccountID, Action<RequestResult> callback = null, bool suppressPrompt = false)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!suppressPrompt)
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(answer =>
                    {
                        if (answer != MessageBoxResult.Yes)
                            return;
                        CommonCommands.InternalDeleteDirectMessage(id, TwitterAccountID, callback);
                    })), (object)DialogType.DeleteDirectMessage);
                else
                    CommonCommands.InternalDeleteDirectMessage(id, TwitterAccountID, callback);
            }), DispatcherPriority.Background, new object[0]);
        }

        private static async void InternalDeleteDirectMessage(Decimal id, Decimal TwitterAccountID, Action<RequestResult> callback)
        {
            TwitterResponse<TwitterDirectMessage> r = await DirectMessages.DeleteAsync(App.AppState.Accounts[TwitterAccountID].Tokens, id, MetroTwitTwitterizer.Options);
            if (r.Result == RequestResult.Success)
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.DeleteTweet, TwitterAccountID));
            if (callback != null)
                callback(r.Result);
        }

        public static void ShowGeo(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)DialogType.GeoDialog)), DispatcherPriority.Background, new object[0]);
        }

        public static void ShowConversation(MetroTwitStatusBase orig)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)orig), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowConversation, orig.TwitterAccountID))), DispatcherPriority.Background, new object[0]);
        }

        public static void CheckForUpdates()
        {
            if (SettingsData.Instance.Updating)
                return;
            SettingsData.Instance.Updating = true;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
                currentDeployment.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(CommonCommands.deployment_CheckForUpdateCompleted);
                try
                {
                    currentDeployment.CheckForUpdateAsync();
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            else
            {
                if (!SettingsData.Instance.QuietUpdating)
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateNonInternet);
                SettingsData.Instance.QuietUpdating = false;
                SettingsData.Instance.Updating = false;
            }
        }

        private static void deployment_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (SettingsData.Instance.Updating)
            {
                if (e.Error != null)
                {
                    if (!SettingsData.Instance.QuietUpdating)
                    {
                        if (e.Error.GetType() == typeof(DeploymentDownloadException))
                            Messenger.Default.Send<DialogMessage>(new DialogMessage(e.Error.Message, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateDownloadError);
                        else if (e.Error.GetType() == typeof(InvalidDeploymentException))
                            Messenger.Default.Send<DialogMessage>(new DialogMessage(e.Error.Message, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateInstallError);
                        else if (e.Error.GetType() == typeof(InvalidOperationException))
                            Messenger.Default.Send<DialogMessage>(new DialogMessage(e.Error.Message, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateInvalidError);
                    }
                    SettingsData.Instance.QuietUpdating = false;
                }
                else if (e.UpdateAvailable)
                {
                    bool doUpdate = true;
                    if (!e.IsUpdateRequired)
                        Messenger.Default.Send<DialogMessage>(new DialogMessage("http://www.metrotwit.com/category/loop-releases/", (Action<MessageBoxResult>)(dr =>
                        {
                            if (MessageBoxResult.Yes == dr)
                                return;
                            doUpdate = false;
                        })), (object)DialogType.UpdateAvailable);
                    else
                        Messenger.Default.Send<DialogMessage>(new DialogMessage(((object)e.MinimumRequiredVersion).ToString(), (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateMandatory);
                    if (doUpdate)
                        Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateStart);
                }
                else
                {
                    if (!SettingsData.Instance.QuietUpdating)
                        Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.UpdateNone);
                    SettingsData.Instance.QuietUpdating = false;
                }
                SettingsData.Instance.Updating = false;
            }
            ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted -= new CheckForUpdateCompletedEventHandler(CommonCommands.deployment_CheckForUpdateCompleted);
        }

        public static void ChangeTheme(SharedResourceDictionary CustomDictionary = null)
        {
            if (System.Windows.Application.Current.MainWindow != null && System.Windows.Application.Current.MainWindow.IsLoaded)
            {
                Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, (Action<MessageBoxResult>)(o => { })), (object)DialogType.ThemeChange);
                DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render, System.Windows.Application.Current.Dispatcher);
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(250.0);
                dispatcherTimer.Tick += (EventHandler)((s, a) =>
                {
                    (s as DispatcherTimer).Stop();
                    CommonCommands.internalChangeTheme(CustomDictionary);
                });
                dispatcherTimer.Start();
            }
            else
                CommonCommands.internalChangeTheme(CustomDictionary);
        }

        private static void internalChangeTheme(SharedResourceDictionary CustomDictionary)
        {
            if (Enumerable.Count<ResourceDictionary>((IEnumerable<ResourceDictionary>)System.Windows.Application.Current.Resources.MergedDictionaries) > 0)
                (System.Windows.Application.Current.Resources.MergedDictionaries[0] as SharedResourceDictionary).ClearShared();
            System.Windows.Application.Current.Resources.MergedDictionaries.Clear();
            if (CustomDictionary != null)
                System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)CustomDictionary);
            else if (SettingsData.Instance.Accent == "Custom")
            {
                switch (SettingsData.Instance.AccentColour)
                {
                    case ColourAccent.DarkBlue:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/DarkBlue.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Blue:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Blue.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Purple:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Purple.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Pink:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Pink.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Red:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Red.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Orange:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Orange.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Green:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Green.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Lime:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Lime.xaml", UriKind.Absolute)
                        });
                        break;
                    case ColourAccent.Silver:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Silver.xaml", UriKind.Absolute)
                        });
                        break;
                    default:
                        System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                        {
                            Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/Blue.xaml", UriKind.Absolute)
                        });
                        break;
                }
            }
            else
                System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Accents/DarkBlue.xaml", UriKind.Absolute)
                });
            switch (string.IsNullOrEmpty(SettingsData.Instance.Theme) ? "Dark" : SettingsData.Instance.Theme)
            {
                case "Dark":
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Dark.xaml", UriKind.Absolute)
                    });
                    break;
                case "Light":
                    System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                    {
                        Source = new Uri("pack://application:,,,/FlattyTweet.Themes;component/Light.xaml", UriKind.Absolute)
                    });
                    break;
               
            }
            StreamReader streamReader = new StreamReader(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/FlattyTweet.Themes;component/Shared/Theme.txt", UriKind.Absolute)).Stream);
            string str;
            while ((str = streamReader.ReadLine()) != null)
                System.Windows.Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)new SharedResourceDictionary()
                {
                    Source = new Uri(string.Format("pack://application:,,,/FlattyTweet.Themes;component/Shared/{0}", (object)str), UriKind.Absolute)
                });
            streamReader.Close();
            Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.ThemeChanged);
            DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render, System.Windows.Application.Current.Dispatcher);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000.0);
            dispatcherTimer.Tick += (EventHandler)((s, a) =>
            {
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)null), (object)ViewModelMessages.ThemeChangeUI);
                (s as DispatcherTimer).Stop();
            });
            dispatcherTimer.Start();
            GC.Collect();
        }

        public static DependencyObject FindParent(object sender, System.Type ParentType)
        {
            DependencyObject dependencyObject = new DependencyObject();
            DependencyObject reference = (DependencyObject)sender;
            while (reference != null && reference.GetType() != ParentType)
                reference = VisualTreeHelper.GetParent(reference);
            return reference;
        }

        public static bool IsValidImage(Stream imageStream)
        {
            try
            {
                if (imageStream.Length <= 0L)
                    return false;
                Image image = Image.FromStream(imageStream, true, true);
                return image.RawFormat.Equals((object)ImageFormat.Jpeg) || image.RawFormat.Equals((object)ImageFormat.Bmp) || image.RawFormat.Equals((object)ImageFormat.Gif) || image.RawFormat.Equals((object)ImageFormat.Png);
            }
            catch
            {
                return false;
            }
        }

        public static void DownloadFile(string ImageURILocal, string ImageURITwitter, string ScreenName, bool CacheUser, Decimal TwitterAccountID)
        {
            WebClient webClient = new WebClient();
            if (ImageURILocal == null)
                ImageURILocal = App.AppState.Accounts[TwitterAccountID].Cache.UserImageURI(ImageURITwitter, ScreenName);
            string str = Path.IsPathRooted(ImageURILocal) ? ImageURILocal : Path.Combine(ApplicationPaths.UserSettings(TwitterAccountID, FileType.UserImages), ImageURILocal);
            if (System.IO.File.Exists(str) && new FileInfo(str).Length != 0L)
                return;
            try
            {
                webClient.DownloadFile(ImageURITwitter, str);
            }
            catch (WebException ex)
            {
            }
        }

        public static byte[] DownloadFile(string ImageURI)
        {
            WebClient webClient1 = new WebClient();
            WebClient webClient2;
            try
            {
                byte[] numArray = webClient1.DownloadData(ImageURI);
                webClient2 = (WebClient)null;
                return numArray;
            }
            catch (WebException ex)
            {
                webClient2 = (WebClient)null;
                return (byte[])null;
            }
        }

        public static void UnBlock(long id, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Twitterizer.Block.DestroyAsync(App.AppState.Accounts[TwitterAccountID].Tokens, (Decimal)id, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<User>>>)(r =>
            {
                if (r.Result.Result != RequestResult.Success && r.Result.Result != RequestResult.Unknown)
                    return;
                Messenger.Default.Send<GenericMessage<KeyValuePair<Decimal, bool>>>(new GenericMessage<KeyValuePair<Decimal, bool>>(new KeyValuePair<Decimal, bool>((Decimal)id, false)), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Blocked, TwitterAccountID));
                App.AppState.Accounts[TwitterAccountID].Cache.RemoveBlockedUser(id);
            }))), DispatcherPriority.Background, new object[0]);
        }

        public static void Block(long id, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Twitterizer.Block.CreateAsync(App.AppState.Accounts[TwitterAccountID].Tokens, (Decimal)id, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<User>>>)(r2 =>
          {
              if (r2.Result.Result != RequestResult.Success && r2.Result.Result != RequestResult.Unknown)
                  return;
              Messenger.Default.Send<GenericMessage<KeyValuePair<Decimal, bool>>>(new GenericMessage<KeyValuePair<Decimal, bool>>(new KeyValuePair<Decimal, bool>((Decimal)id, true)), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Blocked, TwitterAccountID));
              Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.DeleteUserTweets, TwitterAccountID));
              App.AppState.Accounts[TwitterAccountID].Cache.RemoveCachedUser((Decimal)id);
              App.AppState.Accounts[TwitterAccountID].Cache.AddBlockedUser(id);
          }))), DispatcherPriority.Background, new object[0]);
        }

        public static void ReportSpam(Decimal id, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Spam.ReportSpamAsync(App.AppState.Accounts[TwitterAccountID].Tokens, id, MetroTwitTwitterizer.Options).ContinueWith((Action<Task<TwitterResponse<User>>>)(r =>
            {
                if (r.Result.Result != RequestResult.Success && r.Result.Result != RequestResult.Unknown)
                    return;
                Messenger.Default.Send<GenericMessage<KeyValuePair<Decimal, bool>>>(new GenericMessage<KeyValuePair<Decimal, bool>>(new KeyValuePair<Decimal, bool>(id, true)), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.Blocked, TwitterAccountID));
                Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)id), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.DeleteUserTweets, TwitterAccountID));
            }))), DispatcherPriority.Background, new object[0]);
        }

        public static BitmapImage DefaultUserImage()
        {
            if (CommonCommands.defaultUserImage == null)
            {
                Stream stream = CommonCommands.DefaultAvatarStream();
                try
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                }
                catch
                {
                }
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                CommonCommands.defaultUserImage = bitmapImage;
            }
            return CommonCommands.defaultUserImage;
        }

        public static Stream DefaultAvatarStream()
        {
            lock (CommonCommands.DefaultAvatarLock)
                return System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/" + Assembly.GetEntryAssembly().GetName().Name + ";component/Resources/defaultavatar.png", UriKind.Absolute)).Stream;
        }

        public static BitmapImage GetImage(string RelativeImagePath)
        {
            Stream stream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/" + Assembly.GetEntryAssembly().GetName().Name + ";component/" + RelativeImagePath, UriKind.Absolute)).Stream;
            try
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }
            catch
            {
            }
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public static void CopyTweet(MetroTwitStatusBase tweet)
        {
            try
            {
                if (tweet == null || tweet.User == null)
                    return;
                System.Windows.Clipboard.SetText(string.Format("@{0}: {1}", (object)tweet.User.ScreenName, (object)WebUtility.HtmlDecode(tweet.Text)));
            }
            catch
            {
            }
        }

        public static void CopyUrl(UrlEntity url)
        {
            try
            {
                if (url == null)
                    return;
                System.Windows.Clipboard.SetText(!string.IsNullOrEmpty(url.ExpandedUrl) ? url.ExpandedUrl : url.Url);
            }
            catch
            {
            }
        }

        public static void CopyText(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return;
                System.Windows.Clipboard.SetText(text);
            }
            catch
            {
            }
        }

        public static Decimal[] GetAvailableUserAccounts()
        {
            List<Decimal> list = new List<Decimal>();
            if (Directory.Exists(ApplicationPaths.AppConfigPath))
            {
                foreach (string path in Directory.GetDirectories(ApplicationPaths.AppConfigPath))
                {
                    if (Enumerable.FirstOrDefault<string>(Directory.EnumerateFiles(path, ApplicationPaths.UserConfigFilename)) != null)
                    {
                        Decimal result = new Decimal(0);
                        if (Decimal.TryParse(Enumerable.LastOrDefault<string>((IEnumerable<string>)Path.GetFullPath(path).Split(new char[1]
            {
              Path.DirectorySeparatorChar
            })), out result))
                            list.Add(result);
                    }
                }
            }
            return list.ToArray();
        }

        public static void DeleteAccountData(Decimal twitterID)
        {
            string path = Path.Combine(ApplicationPaths.AppConfigPath, twitterID.ToString());
            if (!Directory.Exists(path))
                return;
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                BugSense.SendException(ex);
            }
        }

        internal static void ShowRetweetUsers(Decimal OriginalID, Decimal TwitterAccountID)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() => Messenger.Default.Send<GenericMessage<object>>(new GenericMessage<object>((object)OriginalID), (object)CommonCommands.MultiAccountifyToken((System.Enum)ViewModelMessages.ShowRetweetUsers, TwitterAccountID))), DispatcherPriority.Background, new object[0]);
        }



        internal static Screen CurrentScreen()
        {
            return Screen.FromHandle((PresentationSource.FromVisual((Visual)System.Windows.Application.Current.MainWindow) as HwndSource).Handle);
        }

        internal static PresentationSource CurrentSource()
        {
            return PresentationSource.FromVisual((Visual)System.Windows.Application.Current.MainWindow);
        }

        internal static string MultiAccountifyToken(System.Enum VMEnum, Decimal TwitterAccountID)
        {
            return ((object)VMEnum).ToString() + TwitterAccountID.ToString();
        }

        public static string GetResponseResultMessage(RequestResult result)
        {
            return result == RequestResult.RateLimited ? "You are now rate limited." : "Unknown error";
        }

        internal static void CheckTaskExceptions(Task t)
        {
            if (t.Exception == null)
                return;
            foreach (Exception e in t.Exception.Flatten().InnerExceptions)
                App.LogException(e);
        }

        internal static void EmailTweet(MetroTwitStatusBase metroTwitStatusBase)
        {
            try
            {
                Process.Start(string.Format("mailto:{0}?subject={1}&body={2}", (object)"", (object)("Check this tweet out from @" + metroTwitStatusBase.User.ScreenName), (object)string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", (object)metroTwitStatusBase.Text, (object)"%0D%0A", (object)"%0D%0A", (object)metroTwitStatusBase.Permalink, (object)"%0D%0A", (object)"%0D%0A", (object)"---------------------------------", (object)"%0D%0A", (object)"%0D%0A", (object)"Sent from MetroTwit")));
            }
            catch
            {
            }
        }

        internal async static Task<bool?> Follow(string AdUrl, MetroTwitUser User, decimal TwitterAccountID, string FollowText = "following...")
        {
            bool? nullable;
            try
            {
                IEnumerable<TwitterError> errors;
                if (FollowText == "following...")
                {
                    TwitterResponse<User> response = await Friendship.CreateAsync(App.AppState.Accounts[TwitterAccountID].Tokens, User.ScreenName, MetroTwitTwitterizer.CreateFriendshipOptions);
                    TwitterResponse<User> r = response;
                    if (r.Result == RequestResult.Success)
                    {
                        App.AppState.Accounts[TwitterAccountID].Cache.AddFollowedUser(User);
                        App.AppState.Accounts[TwitterAccountID].Cache.RemoveBlockedUser((long)User.Id);
                        nullable = true;
                    }
                    else
                    {
                        errors = r.Errors;
                        if (errors != null)
                        {
                            BugSense.SendException(new Exception(errors.First<TwitterError>().Message));
                        }
                        nullable = false;
                    }
                    return nullable;
                }
                if (FollowText == "it's you")
                {
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Empty, delegate(MessageBoxResult o)
                    {
                    }), DialogType.FollowSelf);
                    return null;
                }
                bool _answer = false;
                Messenger.Default.Send<DialogMessage>(new DialogMessage(User.ScreenName, delegate(MessageBoxResult answer)
                {
                    _answer = answer == MessageBoxResult.Yes;
                }), DialogType.DeleteFriendship);
                if (_answer)
                {
                    TwitterResponse<User> r = await Friendship.DeleteAsync(App.AppState.Accounts[TwitterAccountID].Tokens, User.Id, User.ScreenName, MetroTwitTwitterizer.Options);

                    if (r.Result == RequestResult.Success)
                    {
                        App.AppState.Accounts[TwitterAccountID].Cache.RemovedFollowedUser(User);
                        nullable = true;
                    }
                    else
                    {
                        errors = r.Errors;
                        if (errors != null)
                        {
                            BugSense.SendException(new Exception(errors.First<TwitterError>().Message));
                        }
                        nullable = false;
                    }
                }
                else
                {
                    nullable = false;
                }
            }
            catch
            {
                nullable = false;
            }
            return nullable;
        }
    }
}
