// Type: MetroTwit.Behaviors.AnimatedTweetListBox
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using MetroTwit.Extensions;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace MetroTwit.Behaviors
{
  public class AnimatedTweetListBox : Behavior<BaseTweetListView>
  {
    private Storyboard TweetSlide = (Storyboard) Application.Current.FindResource((object) "TweetSlide");
    private double animationDelay = 100.0;
    private ListBox tweetlist;
    private Queue<object> itemsToAnimate;

    protected override void OnAttached()
    {
      base.OnAttached();
      this.itemsToAnimate = new Queue<object>();
      this.AssociatedObject.ApplyTemplate();
      if (this.AssociatedObject.TweetListBox == null)
        return;
      ListBox tweetListBox = this.AssociatedObject.TweetListBox;
      if (tweetListBox != null && tweetListBox != null)
      {
        this.tweetlist = tweetListBox;
        this.tweetlist.ItemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(this.ItemContainerGenerator_ItemsChanged);
        this.tweetlist.ItemContainerGenerator.StatusChanged += new EventHandler(this.ItemContainerGenerator_StatusChanged);
      }
    }

    private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
    {
      if (this.tweetlist.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        return;
      int currentCount = 0;
      for (int count = this.itemsToAnimate.Count; currentCount < count; ++currentCount)
      {
        FrameworkElement child = this.tweetlist.ItemContainerGenerator.ContainerFromItem(this.itemsToAnimate.Dequeue()) as FrameworkElement;
        this.AnimateChildItem(currentCount, child);
      }
    }

    protected override void OnDetaching()
    {
      if (this.tweetlist != null)
      {
        this.tweetlist.ItemContainerGenerator.ItemsChanged -= new ItemsChangedEventHandler(this.ItemContainerGenerator_ItemsChanged);
        this.tweetlist.ItemContainerGenerator.StatusChanged -= new EventHandler(this.ItemContainerGenerator_StatusChanged);
      }
      this.tweetlist = (ListBox) null;
      this.itemsToAnimate.Clear();
      this.itemsToAnimate = (Queue<object>) null;
      base.OnDetaching();
    }

    private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
    {
      if ((this.AssociatedObject.DataContext as TweetListViewModel).LastCollectionState == RefreshTypes.InitialLoadForeverScroll || e.Action != NotifyCollectionChangedAction.Add)
        return;
      int currentCount = 0;
      while (currentCount < e.ItemCount)
      {
        GeneratorPosition position = e.Position;
        int index1 = position.Index;
        position = e.Position;
        int offset = position.Offset;
        int index2 = index1 + offset + currentCount;
        FrameworkElement child = this.tweetlist.ItemContainerGenerator.ContainerFromIndex(index2) as FrameworkElement;
        if (child == null)
        {
          this.itemsToAnimate.Enqueue(this.tweetlist.Items[index2]);
          ++currentCount;
        }
        else
          currentCount = this.AnimateChildItem(currentCount, child);
      }
    }

    private int AnimateChildItem(int currentCount, FrameworkElement child)
    {
      if (child != null && child.Opacity != 0.0)
      {
        child.Opacity = 0.0;
        Storyboard storyboard = this.TweetSlide.Clone();
        storyboard.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds((double) currentCount * this.animationDelay));
        child.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace);
      }
      ++currentCount;
      return currentCount;
    }
  }
}
