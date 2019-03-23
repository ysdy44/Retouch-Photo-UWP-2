﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Blends.Models
{
    public class DifferenceBlend : Blend
    {
        public DifferenceBlend()
        {
            base.Type = BlendType.Difference;
        }

        protected override FrameworkElement GetIcon() => new DifferenceControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Difference
            };
        }
    }
}
