// Type: MetroTwit.Behaviors.RetweetDialogBehavior
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using GalaSoft.MvvmLight.Messaging;
using MetroTwit.Extensions;
using MetroTwit.Model;
using MetroTwit.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace MetroTwit.Behaviors
{
  internal class RetweetDialogBehavior : Behavior<FrameworkElement>
  {
    private RetweetMessage dialogmessage;
    private RetweetQuestionView messageBox;

    protected override void OnAttached()
    {
      base.OnAttached();
      Messenger.Default.Register<RetweetMessage>((object) this, (object) DialogType.RetweetDialog, new Action<RetweetMessage>(this.ShowDialog));
    }

    private void ShowDialog(RetweetMessage dm)
    {
      this.dialogmessage = dm;
      this.messageBox = new RetweetQuestionView();
      this.messageBox.Closing += new CancelEventHandler(this.messageBox_Closing);
      this.messageBox.ShowAnimated(PlacementMode.Bottom, SettingsData.Instance.DialogActiveControl, new Point?());
    }

    private void messageBox_Closing(object sender, CancelEventArgs e)
    {
      this.messageBox.Closing -= new CancelEventHandler(this.messageBox_Closing);
      this.dialogmessage.Callback(this.messageBox.MessageBoxResult, this.messageBox.Account);
    }
  }
}
