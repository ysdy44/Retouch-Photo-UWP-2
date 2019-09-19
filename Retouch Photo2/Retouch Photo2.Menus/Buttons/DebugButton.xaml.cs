﻿using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Buttons
{
    public sealed partial class DebugButton : UserControl, IMenuButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        public MenuState State { set => this.Button.SetMenuState(value); }
        public FrameworkElement Self=> this;
        public MenuButtonType Type => MenuButtonType.None;

        //@Construct
        public DebugButton()
        {
            this.InitializeComponent();
        }
    }
}