﻿using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //@Content
        private float BlueOffset
        {
            set
            {
                this.BlueOffsetPicker.Value = (int)(value * 100.0f);
                this.BlueOffsetSlider.Value = value;
            }
        }
        private float BlueExponent
        {
            set
            {
                this.BlueExponentPicker.Value = (int)(value * 100.0f);
                this.BlueExponentSlider.Value = value;
            }
        }
        private float BlueAmplitude
        {
            set
            {
                this.BlueAmplitudePicker.Value = (int)(value * 100.0f);
                this.BlueAmplitudeSlider.Value = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s blue IsEnabled. </summary>
        public bool BlueIsEnabled
        {
            get => (bool)base.GetValue(BlueIsEnabledProperty);
            set => base.SetValue(BlueIsEnabledProperty, value);
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.BlueIsEnabled" /> dependency property. </summary>
        public static readonly DependencyProperty BlueIsEnabledProperty = DependencyProperty.Register(nameof(BlueIsEnabled), typeof(bool), typeof(GammaTransferPage), new PropertyMetadata(false));


        #endregion


        private void ResetBlue()
        {
            this.BlueIsEnabled = false;
            this.BlueOffset = 0.0f;
            this.BlueExponent = 1.0f;
            this.BlueAmplitude = 1.0f;
        }
        private void FollowBlue(GammaTransferAdjustment adjustment)
        {
            this.BlueIsEnabled = !adjustment.BlueDisable;
            this.BlueOffset = adjustment.BlueOffset;
            this.BlueExponent = adjustment.BlueExponent;
            this.BlueAmplitude = adjustment.BlueAmplitude;
        }

        private void ConstructStringsBlue(string title, string offset, string exponent, string amplitude)
        {
            this.BlueCheckControl.Content = title;
            this.BlueOffsetTextBlock.Text = offset;
            this.BlueExponentTextBlock.Text = exponent;
            this.BlueAmplitudeTextBlock.Text = amplitude;
        }


        // BlueDisable
        private void ConstructBlueDisable()
        {
            this.BlueCheckControl.Tapped += (s, e) =>
            {
                bool disable = this.BlueIsEnabled;
                this.BlueIsEnabled = !disable;

                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueDisable = disable,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueDisable,
                    getUndo: (tAdjustment) => tAdjustment.BlueDisable,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueDisable = previous
                );
            };
        }


        // BlueOffset
        private void ConstructBlueOffset1()
        {
            this.BlueOffsetPicker.Unit = "%";
            this.BlueOffsetPicker.Minimum = 0;
            this.BlueOffsetPicker.Maximum = 100;
            this.BlueOffsetPicker.ValueChanged += (s, value) =>
            {
                float blueOffset = (float)value / 100.0f;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueOffset,
                    getUndo: (tAdjustment) => tAdjustment.BlueOffset,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueOffset = previous
                );
            };
        }

        private void ConstructBlueOffset2()
        {
            this.BlueOffsetSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueOffsetSlider.Minimum = 0.0d;
            this.BlueOffsetSlider.Maximum = 1.0d;
            this.BlueOffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueOffset());
            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float blueOffset = (float)value;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset);
            };
            this.BlueOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueOffset = (float)value;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueOffset,
                    getUndo: (tAdjustment) => tAdjustment.StartingBlueOffset,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueOffset = previous
                );
            };
        }


        // BlueExponent
        private void ConstructBlueExponent1()
        {
            this.BlueExponentPicker.Unit = "%";
            this.BlueExponentPicker.Minimum = 0;
            this.BlueExponentPicker.Maximum = 100;
            this.BlueExponentPicker.ValueChanged += (s, value) =>
            {
                float blueExponent = (float)value / 100.0f;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueExponent,
                    getUndo: (tAdjustment) => tAdjustment.BlueExponent,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueExponent = previous
                );
            };
        }

        private void ConstructBlueExponent2()
        {
            this.BlueExponentSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueExponentSlider.Minimum = 0.0d;
            this.BlueExponentSlider.Maximum = 1.0d;
            this.BlueExponentSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueExponent());
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                float blueExponent = (float)value;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent);
            };
            this.BlueExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueExponent = (float)value;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueExponent,
                    getUndo: (tAdjustment) => tAdjustment.StartingBlueExponent,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueExponent = previous
                );
            };
        }


        // BlueAmplitude
        private void ConstructBlueAmplitude1()
        {
            this.BlueAmplitudePicker.Unit = "%";
            this.BlueAmplitudePicker.Minimum = 0;
            this.BlueAmplitudePicker.Maximum = 100;
            this.BlueAmplitudePicker.ValueChanged += (s, value) =>
            {
                float blueAmplitude = (float)value / 100.0f;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueAmplitude,
                    getUndo: (tAdjustment) => tAdjustment.BlueAmplitude,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueAmplitude = previous
                );
            };
        }

        private void ConstructBlueAmplitude2()
        {
            this.BlueAmplitudeSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueAmplitudeSlider.Minimum = 0.0d;
            this.BlueAmplitudeSlider.Maximum = 1.0d;
            this.BlueAmplitudeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueAmplitude());
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                float blueAmplitude = (float)value;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude);
            };
            this.BlueAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueAmplitude = (float)value;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_BlueAmplitude,
                    getUndo: (tAdjustment) => tAdjustment.StartingBlueAmplitude,
                    setUndo: (tAdjustment, previous) => tAdjustment.BlueAmplitude = previous
                );
            };
        }

    }
}