﻿// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Straighten_IsOn"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;

        
        //@Content
        private float Angle
        {
            set
            {
                this.AnglePicker.Value = (int)(value * 180.0f / FanKit.Math.Pi);
                this.AnglePicker2.Radians = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a StraightenEffectPage. 
        /// </summary>
        public StraightenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructStraighten_Angle1();
            this.ConstructStraighten_Angle2();
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffectPage"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("Effects_Straighten");

            this.AngleTextBlock.Text = resource.GetString("Effects_Straighten_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Straighten;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new StraightenIcon()
        };
        
        public void Reset()
        {
            this.Angle = 0.0f;

            this.MethodViewModel.EffectChanged<float>
            (
                set: (effect) => effect.Straighten_Angle = 0.0f,

                historyTitle: "Set effect straighten",
                getHistory: (effect) => effect.Straighten_Angle,
                setHistory: (effect, previous) => effect.Straighten_Angle = previous
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Straighten_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Angle = effect.Straighten_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "StraightenEffectPage"/>.
    /// </summary>
    public sealed partial class StraightenEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.Straighten_IsOn = isOn,

                historyTitle: "Set effect straighten is on",
                getHistory: (effect) => effect.Straighten_IsOn,
                setHistory: (effect, previous) => effect.Straighten_IsOn = previous
            );            
        }


        //Straighten_Angle
        private void ConstructStraighten_Angle1()
        {
            this.AnglePicker.Unit = "º";
            this.AnglePicker.Minimum = -360;
            this.AnglePicker.Maximum = 360;
            this.AnglePicker.ValueChanged += (s, value) =>
            {
                float radians = (float)value * 180 / FanKit.Math.Pi;
                this.Angle = radians;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.Straighten_Angle = radians,

                    historyTitle: "Set effect straighten angle",
                    getHistory: (effect) => effect.Straighten_Angle,
                    setHistory: (effect, previous) => effect.Straighten_Angle = previous
               );
            };
        }

        private void ConstructStraighten_Angle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheStraighten());
            this.AnglePicker2.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Straighten_Angle = radians);
            };
            this.AnglePicker2.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Straighten_Angle = radians,

                    historyTitle: "Set effect straighten angle",
                    getHistory: (effect) => effect.StartingStraighten_Angle,
                    setHistory: (effect, previous) => effect.Straighten_Angle = previous
               );
            };
        }

    }
}