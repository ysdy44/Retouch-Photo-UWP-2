﻿using FanKit.Transformers;
using HSVColorPickers;
using Retouch_Photo2.Photos;
using System.Linq;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {

        /// <summary>
        /// Changed the brush's type.
        /// </summary>
        /// <param name="type"> The new type. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photo"> The photo. </param>
        public void TypeChanged(BrushType type, Transformer transformer, Photo photo = null)
        {
            switch (type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    this.ChangeColorCore();
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    this.ChangingStopsCore();
                    this.ChangingLinearGradientPointsCore(transformer);
                    break;
                case BrushType.EllipticalGradient:
                    this.ChangingStopsCore();
                    this.ChangingEllipticalGradientPointsCore(transformer);
                    break;

                case BrushType.Image:
                    {
                        // Photo
                        if (photo is null) return;

                        Photocopier photocopier = photo.ToPhotocopier();
                        float width = photo.Width;
                        float height = photo.Height;

                        this.Photocopier = photocopier;

                        Vector2 center = this.Center;
                        Vector2 yPoint = this.YPoint;

                        //this.Center = center;
                        this.XPoint = BrushBase.YToX(yPoint, center, width, height);
                        //this.YPoint = yPoint;
                        this.ChangingEllipticalGradientPointsCore(transformer);
                    }
                    break;

                default:
                    break;
            }

            this.Type = type;
        }
        /// <summary>
        /// Change the brush's type.
        /// </summary>
        /// <param name="type"> The new type. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="color"> The color. </param>
        /// <param name="photo"> The photo. </param>
        public void TypeChanged(BrushType type, Transformer transformer, Color color, Photo photo = null)
        {
            switch (type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    this.ChangeColorCore(color);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    this.ChangingStopsCore(color);
                    this.ChangingLinearGradientPointsCore(transformer);
                    break;
                case BrushType.EllipticalGradient:
                    this.ChangingStopsCore(color);
                    this.ChangingEllipticalGradientPointsCore(transformer);
                    break;

                case BrushType.Image:
                    {
                        // Photo
                        if (photo is null) return;

                        Photocopier photocopier = photo.ToPhotocopier();
                        float width = photo.Width;
                        float height = photo.Height;

                        this.Photocopier = photocopier;

                        Vector2 center = this.Center;
                        Vector2 yPoint = this.YPoint;

                        //this.Center = center;
                        this.XPoint = BrushBase.YToX(yPoint, center, width, height);
                        //this.YPoint = yPoint;
                        this.ChangingEllipticalGradientPointsCore(transformer);
                    }
                    break;

                default:
                    break;
            }

            this.Type = type;
        }


        private void ChangeColorCore()
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.Color = this.Stops.Last().Color;
                    break;

                default:
                    this.Color = Colors.LightGray;
                    break;
            }
        }
        private void ChangeColorCore(Color color)
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.Color = this.Stops.Last().Color;
                    break;

                default:
                    this.Color = color;
                    break;
            }
        }

        private void ChangingStopsCore()
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray(this.Color);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    break;

                default:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray();
                    break;
            }
        }
        private void ChangingStopsCore(Color color)
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray(this.Color);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    break;

                default:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray(color);
                    break;
            }
        }


        private void ChangingLinearGradientPointsCore(Transformer transformer)
        {
            switch (this.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    this.Center = transformer.Center;
                    this.YPoint = transformer.CenterBottom;
                    break;
            }
        }

        private void ChangingEllipticalGradientPointsCore(Transformer transformer)
        {
            switch (this.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    this.Center = transformer.Center;
                    this.XPoint = transformer.CenterRight;
                    this.YPoint = transformer.CenterBottom;
                    break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    this.XPoint = BrushBase.YToX(this.YPoint, this.Center);
                    break;
            }
        }

    }
}