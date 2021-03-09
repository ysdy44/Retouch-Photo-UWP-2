﻿// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★

using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;
        

        //@Content
        private int Size
        {
            set
            {
                this.SizePicker.Value = value;
                this.SizeSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a MorphologyEffectPage. 
        /// </summary>
        public MorphologyEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructSize1();
            this.ConstructSize2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SizeTextBlock.Text = resource.GetString("Effects_Morphology_Size");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Morphology;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public Button Button { get; } = new Button();
        /// <summary> Gets the CheckControl. </summary>
        public CheckControl CheckControl { get; } = new CheckControl();


        public void Reset()
        {
            this.Size = 1;

            this.MethodViewModel.EffectChanged<int>
            (
                set: (effect) => effect.Morphology_Size = 1,

                type: HistoryType.LayersProperty_ResetEffect_Mmorphology,
                getUndo: (effect) => effect.Morphology_Size,
                setUndo: (effect, previous) => effect.Morphology_Size = previous
            );
        }
        public bool FollowButton(Effect effect) => effect.Morphology_IsOn;
        public void FollowPage(Effect effect)
        {
            this.SizeSlider.Value = effect.Morphology_Size;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Morphology_IsOn"/>.
    /// </summary>
    public sealed partial class MorphologyEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.CheckControl.Tapped += (s, e) =>
            {
                bool isOn = !this.CheckControl.IsChecked;

                this.Button.IsEnabled = isOn;
                this.CheckControl.IsChecked = isOn;
                this.MethodViewModel.EffectChanged<bool>
                (
                    set: (effect) => effect.Morphology_IsOn = isOn,

                    type: HistoryType.LayersProperty_SwitchEffect_Mmorphology,
                    getUndo: (effect) => effect.Morphology_IsOn,
                    setUndo: (effect, previous) => effect.Morphology_IsOn = previous
                );
            };
        }


        //Size
        private void ConstructSize1()
        {
            this.SizePicker.Unit = null;
            this.SizePicker.Minimum = -100;
            this.SizePicker.Maximum = 100;
            this.SizePicker.ValueChanged += (s, value) =>
            {
                int size = value;
                this.Size = size;

                this.MethodViewModel.EffectChanged<int>
                (
                    set: (effect) => effect.Morphology_Size = size,

                    type: HistoryType.LayersProperty_SetEffect_Mmorphology_Size,
                    getUndo: (effect) => effect.Morphology_Size,
                    setUndo: (effect, previous) => effect.Morphology_Size = previous
                );
            };
        }

        private void ConstructSize2()
        {
            this.SizeSlider.Minimum = -100.0d;
            this.SizeSlider.Maximum = 100.0d;
            this.SizeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheMorphology());
            this.SizeSlider.ValueChangeDelta += (s, value) =>
            {
                int size = (int)value;
                this.Size = size;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Morphology_Size = size);
            };
            this.SizeSlider.ValueChangeCompleted += (s, value) =>
            {
                int size = (int)value;
                this.Size = size;

                this.MethodViewModel.EffectChangeCompleted<int>
                (
                    set: (effect) => effect.Morphology_Size = size,

                    type: HistoryType.LayersProperty_SetEffect_Mmorphology_Size,
                    getUndo: (effect) => effect.StartingMorphology_Size,
                    setUndo: (effect, previous) => effect.Morphology_Size = previous
                );
            };
        }

    }
}