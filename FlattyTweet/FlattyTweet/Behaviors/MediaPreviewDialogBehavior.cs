
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Twitterizer.Models;

namespace FlattyTweet.Behaviors
{
  internal class MediaPreviewDialogBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<GenericMessage<UrlEntity>>((object) this, (object) DialogType.MediaPreview, new Action<GenericMessage<UrlEntity>>(this.ShowDialog));
    }

    private void ShowDialog(GenericMessage<UrlEntity> dm)
    {
      if (!App.LastURLClickMousePosition.HasValue)
        return;
      MediaPreviewViewModel VM = new MediaPreviewViewModel(dm.Content);
      MediaPreviewView mediaPreviewView = new MediaPreviewView(VM);
      if (!VM.Closed)
        mediaPreviewView.ShowAnimated(PlacementMode.Bottom, SettingsData.Instance.DialogActiveControl, new Point?(App.LastURLClickMousePosition.GetValueOrDefault()));
    }
  }
}
