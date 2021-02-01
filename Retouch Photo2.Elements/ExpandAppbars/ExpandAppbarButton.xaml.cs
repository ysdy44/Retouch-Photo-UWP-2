﻿// Core:              
// Referenced:   ★
// Difficult:         
// Only:              
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button of <see cref="ExpandAppbar"/>.
    /// </summary>
    public sealed partial class ExpandAppbarButton : UserControl, IExpandAppbarElement
    {
        //@Content
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> ContentPresenter's Content </summary>
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }
        /// <summary> Gets element width. </summary>
        public double ExpandWidth => 40.0d;   
        /// <summary> Gets it yourself. </summary>
        public FrameworkElement Self => this;


        //@VisualState
        bool _vsIsEnabled = true;
        ClickMode _vsClickMode;
        bool _vsIsSecondPage;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsSecondPage==false)
                {
                    if (this._vsIsEnabled == false) return this.Disabled;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }
                else
                {
                    if (this._vsIsEnabled == false) return this.SecondDisabled;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Second;
                        case ClickMode.Hover: return this.SecondPointerOver;
                        case ClickMode.Press: return this.SecondPressed;
                    }
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ClickMode. </summary>
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }
        public bool IsSecondPage
        {
            set
            {
                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState;//State
            }
        }
        public bool IsEnabledChange
        {
            set
            {
                this._vsIsEnabled = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ExpandAppbarButton.
        /// </summary>
        public ExpandAppbarButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}