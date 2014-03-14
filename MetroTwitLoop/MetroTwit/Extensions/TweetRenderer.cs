// Type: MetroTwit.Extensions.TweetRenderer
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit;
using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Twitterizer.Models;

namespace MetroTwit.Extensions
{
  public class TweetRenderer : DependencyObject
  {
    public static readonly DependencyProperty TweetProperty;
    public static readonly DependencyProperty NotificationProperty;
    public static readonly DependencyProperty ForceFullnameForTweetHeaderProperty;

    static TweetRenderer()
    {
      string name1 = "Tweet";
      Type propertyType1 = typeof (object);
      Type ownerType1 = typeof (TweetRenderer);
      FrameworkPropertyMetadata propertyMetadata1 = new FrameworkPropertyMetadata();
      propertyMetadata1.BindsTwoWayByDefault = true;
      propertyMetadata1.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        TextBlock local_0 = (TextBlock) obj;
        if (DesignerProperties.GetIsInDesignMode((DependencyObject) local_0))
          return;
        object local_1 = TweetRenderer.GetTweet((DependencyObject) local_0);
        if (local_1 != null)
        {
          MetroTwitStatusBase local_2 = local_1 as MetroTwitStatusBase;
          TweetRenderer.PrepareTweetDocument(local_0, local_2, false);
        }
      });
      FrameworkPropertyMetadata propertyMetadata2 = propertyMetadata1;
      TweetRenderer.TweetProperty = DependencyProperty.RegisterAttached(name1, propertyType1, ownerType1, (PropertyMetadata) propertyMetadata2);
      string name2 = "Notification";
      Type propertyType2 = typeof (object);
      Type ownerType2 = typeof (TweetRenderer);
      FrameworkPropertyMetadata propertyMetadata3 = new FrameworkPropertyMetadata();
      propertyMetadata3.BindsTwoWayByDefault = true;
      propertyMetadata3.PropertyChangedCallback = (PropertyChangedCallback) ((obj, e) =>
      {
        TextBlock local_0 = (TextBlock) obj;
        if (DesignerProperties.GetIsInDesignMode((DependencyObject) local_0))
          return;
        object local_1 = TweetRenderer.GetNotification((DependencyObject) local_0);
        if (local_1 != null)
        {
          MetroTwitStatusBase local_2 = local_1 as MetroTwitStatusBase;
          TweetRenderer.PrepareTweetDocument(local_0, local_2, true);
        }
      });
      FrameworkPropertyMetadata propertyMetadata4 = propertyMetadata3;
      TweetRenderer.NotificationProperty = DependencyProperty.RegisterAttached(name2, propertyType2, ownerType2, (PropertyMetadata) propertyMetadata4);
      TweetRenderer.ForceFullnameForTweetHeaderProperty = DependencyProperty.RegisterAttached("ForceFullnameForTweetHeader", typeof (bool), typeof (TweetRenderer), (PropertyMetadata) new FrameworkPropertyMetadata()
      {
        BindsTwoWayByDefault = true
      });
    }

    public static object GetTweet(DependencyObject obj)
    {
      return obj.GetValue(TweetRenderer.TweetProperty);
    }

    public static void SetTweet(DependencyObject obj, object value)
    {
      obj.SetValue(TweetRenderer.TweetProperty, value);
    }

    public static object GetNotification(DependencyObject obj)
    {
      return obj.GetValue(TweetRenderer.NotificationProperty);
    }

    public static void SetNotification(DependencyObject obj, object value)
    {
      obj.SetValue(TweetRenderer.NotificationProperty, value);
    }

    public static bool GetForceFullnameForTweetHeader(DependencyObject obj)
    {
      return (bool) obj.GetValue(TweetRenderer.ForceFullnameForTweetHeaderProperty);
    }

    public static void SetForceFullnameForTweetHeader(DependencyObject obj, bool value)
    {
        obj.SetValue(ForceFullnameForTweetHeaderProperty, value);
    }

    private static void PrepareTweetDocument(TextBlock textBlock, MetroTwitStatusBase tweet, bool Notification = false)
    {
      if (DesignerProperties.GetIsInDesignMode((DependencyObject) textBlock))
        return;
      try
      {
        if (textBlock != null && textBlock.Inlines != null && textBlock.Inlines.Count > 0)
          textBlock.Inlines.Clear();
      }
      catch
      {
        foreach (Inline inline in Enumerable.ToArray<Inline>((IEnumerable<Inline>) textBlock.Inlines))
          textBlock.Inlines.Remove(inline);
      }
      if (Notification)
      {
        Span span1 = new Span();
        span1.FontWeight = FontWeights.SemiBold;
        Span span2 = span1;
        Hyperlink hyperlink = new Hyperlink((Inline) new Run("@" + tweet.User.ScreenName));
        hyperlink.SetResourceReference(TextElement.ForegroundProperty, (object)"ModernTextDarkerBrush");
        hyperlink.SetBinding(Hyperlink.CommandProperty, (BindingBase) new Binding("UserProfileCommand")
        {
          Mode = BindingMode.OneWay
        });
        hyperlink.SetBinding(Hyperlink.CommandParameterProperty, (BindingBase) new Binding("CurrentTweet.User.ScreenName")
        {
          Mode = BindingMode.OneWay
        });
        ((TextElementCollection<Inline>) span2.Inlines).Add((Inline) hyperlink);
        ((TextElementCollection<Inline>) span2.Inlines).Add((Inline) new Run()
        {
          Text = ": "
        });
        ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) span2);
      }
      if (tweet != null && tweet.Entities != null && tweet.Entities.Count > 0)
      {
        try
        {
          int[] numArray = StringInfo.ParseCombiningCharacters(tweet.RawText);
          IOrderedEnumerable<Entity> orderedEnumerable = Enumerable.OrderBy<Entity, int>((IEnumerable<Entity>) tweet.Entities, (Func<Entity, int>) (x => x.StartIndex));
          int num1 = 0;
          foreach (Entity entity in (IEnumerable<Entity>) orderedEnumerable)
          {
            int num2 = 0;
            if (numArray.Length < tweet.RawText.Length && entity.StartIndex < numArray.Length)
              num2 = numArray[entity.StartIndex] - entity.StartIndex;
            int num3 = entity.StartIndex + num2;
            int num4 = entity.EndIndex + num2;
            int startIndex1 = num1;
            if (num3 > startIndex1 && startIndex1 < numArray.Length && startIndex1 + (num3 - startIndex1) < numArray.Length)
            {
              string input = WebUtility.HtmlDecode(tweet.RawText.Substring(startIndex1, num3 - startIndex1));
              if (!string.IsNullOrEmpty(input))
              {
                Run run = new Run(Regex.Replace(input, "\n{2,}|(\r\n){2,}", "\n\n"));
                ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) run);
              }
            }
            if (entity is MentionEntity)
            {
              MentionEntity mentionEntity = entity as MentionEntity;
              if (!string.IsNullOrEmpty(mentionEntity.ScreenName))
              {
                Hyperlink hyperlink = new Hyperlink((Inline) new TagMentionLinkRun("@" + mentionEntity.ScreenName))
                {
                  CommandParameter = (object) ("@" + mentionEntity.ScreenName)
                };
                hyperlink.SetResourceReference(TextElement.ForegroundProperty, (object) "HyperlinkUsername");
                hyperlink.SetBinding(Hyperlink.CommandProperty, (BindingBase) new Binding("UserProfileCommand")
                {
                  Mode = BindingMode.OneTime
                });
                ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) hyperlink);
                hyperlink.ContextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                menuItem1.Header = (object) "Filter tweets from this user";
                menuItem1.CommandParameter = hyperlink.CommandParameter;
                MenuItem menuItem2 = menuItem1;
                menuItem2.SetBinding(MenuItem.CommandProperty, (BindingBase) new Binding("FilterCommand")
                {
                  Mode = BindingMode.OneTime
                });
                hyperlink.ContextMenu.Items.Add((object) menuItem2);
              }
            }
            else if (entity is HashTagEntity)
            {
              HashTagEntity hashTagEntity = entity as HashTagEntity;
              if (!string.IsNullOrEmpty(hashTagEntity.Text))
              {
                Hyperlink hyperlink = new Hyperlink((Inline) new TagMentionLinkRun("#" + hashTagEntity.Text))
                {
                  CommandParameter = (object) ("#" + hashTagEntity.Text)
                };
                hyperlink.SetResourceReference(TextElement.ForegroundProperty, (object) "HyperlinkHashtag");
                hyperlink.SetBinding(Hyperlink.CommandProperty, (BindingBase) new Binding("TagsCommand")
                {
                  Mode = BindingMode.OneTime
                });
                ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) hyperlink);
                hyperlink.ContextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                menuItem1.Header = (object) "Filter tweets that contain this hashtag";
                menuItem1.CommandParameter = hyperlink.CommandParameter;
                MenuItem menuItem2 = menuItem1;
                menuItem2.SetBinding(MenuItem.CommandProperty, (BindingBase) new Binding("FilterCommand")
                {
                  Mode = BindingMode.OneTime
                });
                hyperlink.ContextMenu.Items.Add((object) menuItem2);
              }
            }
            else if (entity is UrlEntity)
            {
              UrlEntity url = entity as UrlEntity;
              if (!string.IsNullOrEmpty(url.Url))
              {
                if (tweet.AdUrls != null && !string.IsNullOrEmpty(tweet.AdUrls.click_url))
                  url.ExpandedUrl = tweet.AdUrls.click_url;
                string str = string.IsNullOrEmpty(url.DisplayUrl) || tweet.AdUrls != null ? url.Url : (url.DisplayUrl.Length > 50 ? url.DisplayUrl.Substring(0, 50) + "..." : url.DisplayUrl);
                if (tweet.AdUrls != null & !string.IsNullOrEmpty(url.DisplayUrl))
                  str = url.DisplayUrl;
                if (url.DisplayUrl == null && tweet.AdUrls == null)
                {
                  try
                  {
                    MatchCollection matchCollection = RegularExpressions.VALID_URL.Matches(url.Url);
                    if (matchCollection.Count == 0 || matchCollection.Count > 0 && !matchCollection[0].Groups[4].Success)
                      url.Url = "http://" + url.Url;
                  }
                  catch
                  {
                  }
                }
                if (tweet.AdUrls != null)
                {
                  if (string.IsNullOrEmpty(url.DisplayUrl))
                    url.DisplayUrl = url.Url;
                  url.Url = url.ExpandedUrl;
                }
                Hyperlink hyperlink = new Hyperlink((Inline) new TagMentionLinkRun(WebUtility.HtmlDecode(str)))
                {
                  CommandParameter = (object) url
                };
                hyperlink.SetResourceReference(TextElement.ForegroundProperty, (object) "HyperlinkURL");
                hyperlink.SetBinding(Hyperlink.CommandProperty, (BindingBase) new Binding(Notification ? "LinkCommand" : "ContentLinkCommand")
                {
                  Mode = BindingMode.OneTime
                });
                hyperlink.ToolTip = (object) url.ExpandedUrl;
                hyperlink.ContextMenu = new ContextMenu();
                MenuItem menuItem1 = new MenuItem();
                menuItem1.Header = (object) "Copy URL";
                menuItem1.CommandParameter = (object) url;
                MenuItem menuItem2 = menuItem1;
                menuItem2.SetBinding(MenuItem.CommandProperty, (BindingBase) new Binding("CopyUrlCommand")
                {
                  Mode = BindingMode.OneTime
                });
                hyperlink.ContextMenu.Items.Add((object) menuItem2);
                hyperlink.PreviewMouseDown += (MouseButtonEventHandler) ((clickSender, clickArgs) => Application.Current.Dispatcher.BeginInvoke((Action) (() =>
                {
                  string local_0 = url.Url;
                  if (clickArgs.MiddleButton == MouseButtonState.Pressed || Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                  {
                    App.LastURLClickMousePosition = new Point?();
                    CommonCommands.OpenLink(local_0);
                    clickArgs.Handled = true;
                  }
                  else
                    App.LastURLClickMousePosition = new Point?(Application.Current.MainWindow.PointToScreen(Mouse.GetPosition((IInputElement) Application.Current.MainWindow)));
                }), new object[0]));
                ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) hyperlink);
              }
            }
            num1 = num4;
            int startIndex2 = num4;
            if (Enumerable.Count<Entity>((IEnumerable<Entity>) orderedEnumerable) > 0 && Enumerable.Last<Entity>((IEnumerable<Entity>) orderedEnumerable) == entity && startIndex2 < numArray.Length)
            {
              string text = WebUtility.HtmlDecode(tweet.RawText.Substring(startIndex2));
              if (!string.IsNullOrEmpty(text))
              {
                Run run = new Run(text);
                ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) run);
              }
            }
          }
        }
        catch
        {
          ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) new Run(Regex.Replace(WebUtility.HtmlDecode(tweet.RawText), "\n{2,}|(\r\n){2,}", "\n\n")));
        }
      }
      else
        ((TextElementCollection<Inline>) textBlock.Inlines).Add((Inline) new Run(Regex.Replace(WebUtility.HtmlDecode(tweet.RawText), "\n{2,}|(\r\n){2,}", "\n\n")));
    }
  }
}
