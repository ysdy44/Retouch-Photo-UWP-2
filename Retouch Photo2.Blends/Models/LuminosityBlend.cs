﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends.Models
{
    public class LuminosityBlend : Blend
    {
        public LuminosityBlend()
        {
            base.Type = BlendType.Luminosity;
        }

        protected override FrameworkElement GetIcon() => new LuminosityControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Luminosity
            };
        }
    }
}