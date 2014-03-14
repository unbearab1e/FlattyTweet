// Type: MetroTwit.Behaviors.GeoDialogBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.View;
using MetroTwit.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace MetroTwit.Behaviors
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
