
using GalaSoft.MvvmLight.Messaging;
using FlattyTweet.Extensions;
using FlattyTweet.Model;
using FlattyTweet.View;
using FlattyTweet.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace FlattyTweet.Behaviors
{
  internal class GeoDialogBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<GenericMessage<object>>((object) this, (object) DialogType.GeoDialog, new Action<GenericMessage<object>>(this.ShowDialog));
    }

    private void ShowDialog(GenericMessage<object> dm)
    {
      GeoView geoView = new GeoView();
      geoView.DataContext = (object) new GeoViewModel(dm.Content as MetroTwitStatusBase);
      geoView.ShowAnimated(PlacementMode.Bottom, SettingsData.Instance.DialogActiveControl, new Point?());
    }
  }
}
