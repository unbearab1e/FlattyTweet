// Type: MetroTwit.Behaviors.MediaPreviewDialogBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Twitterizer.Models;

namespace MetroTwit.Behaviors
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
