﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class MenuButton : UserControl
    {
        //@Content 
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        /// <summary> MenuButton's MenuState. </summary>
        public MenuState MenuState
        {
            set
            {
                this._vsMenuState = value;
                this.VisualState = this.VisualState;//State         
            }
        }


        //@VisualState
        MenuState _vsMenuState;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsMenuState == MenuState.FlyoutShow) return this.Flyout;

                if (this._vsMenuState == MenuState.FlyoutHide)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }

                if (this._vsMenuState == MenuState.OverlayExpanded || this._vsMenuState == MenuState.OverlayNotExpanded)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Overlay;
                        case ClickMode.Hover: return this.PointerOverOverlay;
                        case ClickMode.Press: return this.PressedOverlay;
                    }
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
                     

        //@Construct
        public MenuButton()
        {
            this.InitializeComponent();
            this.PointerEntered += (s, e) =>
            {
                this._vsClickMode = ClickMode.Hover;
                this.VisualState = this.VisualState;//State
            };
            this.PointerPressed += (s, e) =>
            {
                this._vsClickMode = ClickMode.Press;
                this.VisualState = this.VisualState;//State
            };
            this.PointerExited += (s, e) =>
            {
                this._vsClickMode = ClickMode.Release;
                this.VisualState = this.VisualState;//State
            };
        }        
    }
}