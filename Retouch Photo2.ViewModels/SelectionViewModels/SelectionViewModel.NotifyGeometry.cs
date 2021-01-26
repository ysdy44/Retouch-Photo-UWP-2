﻿using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {


        /// <summary> Sets the PatternLayer. </summary>     
        private void SetPatternLayer(ILayer layer)
        {
            if (layer == null) return;

            switch (layer.Type)
            {
                case LayerType.PatternGrid:
                    PatternGridLayer gridLayer = (PatternGridLayer)layer;
                    this.PatternGridType = gridLayer.GridType;
                    this.PatternGridHorizontalStep = gridLayer.HorizontalStep;
                    this.PatternGridVerticalStep = gridLayer.VerticalStep;
                    break;

                case LayerType.PatternDiagonal:
                    PatternDiagonalLayer diagonalLayer = (PatternDiagonalLayer)layer;
                    this.PatternDiagonalOffset = diagonalLayer.Offset;
                    this.PatternDiagonalHorizontalStep = diagonalLayer.HorizontalStep;
                    break;

                case LayerType.PatternSpotted:
                    PatternSpottedLayer spottedLayer = (PatternSpottedLayer)layer;
                    this.PatternSpottedRadius = spottedLayer.Radius;
                    this.PatternSpottedStep = spottedLayer.Step;
                    break;
            }
        }


        #region Pattern


        /// <summary> PatternGridLayer's Type. </summary>     
        public PatternGridType PatternGridType
        {
            get => this.patternGridType;
            set
            {
                if (this.patternGridType == value) return;
                this.patternGridType = value;
                this.OnPropertyChanged(nameof(PatternGridType));//Notify 
            }
        }
        private PatternGridType patternGridType = PatternGridType.Grid;
                
        /// <summary> PatternGridLayer's HorizontalStep. </summary>     
        public float PatternGridHorizontalStep
        {
            get => this.patternGridHorizontalStep;
            set
            {
                if (this.patternGridHorizontalStep == value) return;
                this.patternGridHorizontalStep = value;
                this.OnPropertyChanged(nameof(PatternGridHorizontalStep));//Notify 
            }
        }
        private float patternGridHorizontalStep = 30.0f;

        /// <summary> PatternGridLayer's VerticalStep. </summary>     
        public float PatternGridVerticalStep
        {
            get => this.patternGridVerticalStep;
            set
            {
                if (this.patternGridVerticalStep == value) return;
                this.patternGridVerticalStep = value;
                this.OnPropertyChanged(nameof(PatternGridVerticalStep));//Notify 
            }
        }
        private float patternGridVerticalStep = 30.0f;



        /// <summary> PatternSpottedLayer's offset. </summary>     
        public float PatternDiagonalOffset
        {
            get => this.patternDiagonalOffset;
            set
            {
                if (this.patternDiagonalOffset == value) return;
                this.patternDiagonalOffset = value;
                this.OnPropertyChanged(nameof(PatternDiagonalOffset));//Notify 
            }
        }
        private float patternDiagonalOffset = 30.0f;

        /// <summary> PatternDiagonalLayer's HorizontalStep. </summary>     
        public float PatternDiagonalHorizontalStep
        {
            get => this.patternDiagonalHorizontalStep;
            set
            {
                if (this.patternDiagonalHorizontalStep == value) return;
                this.patternDiagonalHorizontalStep = value;
                this.OnPropertyChanged(nameof(PatternDiagonalHorizontalStep));//Notify 
            }
        }
        private float patternDiagonalHorizontalStep = 30.0f;



        /// <summary> PatternSpottedLayer's radius. </summary>     
        public float PatternSpottedRadius
        {
            get => this.patternSpottedRadius;
            set
            {
                if (this.patternSpottedRadius == value) return;
                this.patternSpottedRadius = value;
                this.OnPropertyChanged(nameof(PatternSpottedRadius));//Notify 
            }
        }
        private float patternSpottedRadius = 8.0f;

        /// <summary> PatternSpottedLayer's step. </summary>     
        public float PatternSpottedStep
        {
            get => this.patternSpottedStep;
            set
            {
                if (this.patternSpottedStep == value) return;
                this.patternSpottedStep = value;
                this.OnPropertyChanged(nameof(PatternSpottedStep));//Notify 
            }
        }
        private float patternSpottedStep = 30.0f;


        #endregion



        /// <summary> Sets the GeometryLayer. </summary>     
        private void SetGeometryLayer(ILayer layer)
        {
            if (layer == null) return;

            switch (layer.Type)
            {
                //Geometry0
                case LayerType.GeometryRectangle: break;
                case LayerType.GeometryEllipse: break;

                //Geometry1
                case LayerType.GeometryRoundRect: this.GeometryRoundRectCorner = ((GeometryRoundRectLayer)layer).Corner; break;
                case LayerType.GeometryTriangle: this.GeometryTriangleCenter = ((GeometryTriangleLayer)layer).Center; break;
                case LayerType.GeometryDiamond: this.GeometryDiamondMid = ((GeometryDiamondLayer)layer).Mid; break;

                //Geometry12
                case LayerType.GeometryPentagon: this.GeometryPentagonPoints = ((GeometryPentagonLayer)layer).Points; break;
                case LayerType.GeometryStar:
                    GeometryStarLayer starLayer = (GeometryStarLayer)layer;
                    this.GeometryStarPoints = starLayer.Points;
                    this.GeometryStarInnerRadius = starLayer.InnerRadius;
                    break;
                case LayerType.GeometryCog:
                    GeometryCogLayer cogLayer = (GeometryCogLayer)layer;
                    this.GeometryCogCount = cogLayer.Count;
                    this.GeometryCogInnerRadius = cogLayer.InnerRadius;
                    this.GeometryCogTooth = cogLayer.Tooth;
                    this.GeometryCogNotch = cogLayer.Notch;
                    break;

                //Geometry3
                case LayerType.GeometryDount: this.GeometryDountHoleRadius = ((GeometryDountLayer)layer).HoleRadius; break;
                case LayerType.GeometryPie: this.GeometryPieSweepAngle = ((GeometryPieLayer)layer).SweepAngle; break;
                case LayerType.GeometryCookie:
                    GeometryCookieLayer cookieLayer = (GeometryCookieLayer)layer;
                    this.GeometryCookieInnerRadius = cookieLayer.InnerRadius;
                    this.GeometryCookieSweepAngle = cookieLayer.SweepAngle;
                    break;

                //Geometry4
                case LayerType.GeometryArrow:
                    GeometryArrowLayer arrowLayer = (GeometryArrowLayer)layer;
                    this.GeometryArrowValue = arrowLayer.Value;
                    this.GeometryArrowLeftTail = arrowLayer.LeftTail;
                    this.GeometryArrowRightTail = arrowLayer.RightTail;
                    break;
                case LayerType.GeometryCapsule: break;
                case LayerType.GeometryHeart: this.GeometryHeartSpread = ((GeometryHeartLayer)layer).Spread; break;
            }
        }


        #region Geometry1


        /// <summary> GeometryRoundRectLayer's corner. </summary>     
        public float GeometryRoundRectCorner
        {
            get => this.geometryRoundRectCorner;
            set
            {
                if (this.geometryRoundRectCorner == value) return;
                this.geometryRoundRectCorner = value;
                this.OnPropertyChanged(nameof(GeometryRoundRectCorner));//Notify 
            }
        }
        private float geometryRoundRectCorner = 0.12f;


        /// <summary> GeometryTriangle's center-point. </summary>     
        public float GeometryTriangleCenter
        {
            get => this.geometryTriangleCenter;
            set
            {
                if (this.geometryTriangleCenter == value) return;
                this.geometryTriangleCenter = value;
                this.OnPropertyChanged(nameof(GeometryTriangleCenter));//Notify 
            }
        }
        private float geometryTriangleCenter = 0.5f;


        /// <summary> GeometryDiamond's mid-point. </summary>     
        public float GeometryDiamondMid
        {
            get => this.geometryDiamondMid;
            set
            {
                if (this.geometryDiamondMid == value) return;
                this.geometryDiamondMid = value;
                this.OnPropertyChanged(nameof(GeometryDiamondMid));//Notify 
            }
        }
        private float geometryDiamondMid = 0.5f;


        #endregion


        #region Geometry2


        /// <summary> GeometryPentagon's points. </summary>     
        public int GeometryPentagonPoints
        {
            get => this.geometryPentagonPoints;
            set
            {
                if (this.geometryPentagonPoints == value) return;
                this.geometryPentagonPoints = value;
                this.OnPropertyChanged(nameof(GeometryPentagonPoints));//Notify 
            }
        }
        private int geometryPentagonPoints = 5;


        /// <summary> GeometryStar's points. </summary>     
        public int GeometryStarPoints
        {
            get => this.geometryStarPoints;
            set
            {
                if (this.geometryStarPoints == value) return;
                this.geometryStarPoints = value;
                this.OnPropertyChanged(nameof(GeometryStarPoints));//Notify 
            }
        }
        private int geometryStarPoints = 5;
        /// <summary> GeometryStar's inner-radius. </summary>     
        public float GeometryStarInnerRadius
        {
            get => this.geometryStarInnerRadius;
            set
            {
                if (this.geometryStarInnerRadius == value) return;
                this.geometryStarInnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryStarInnerRadius));//Notify 
            }
        }
        private float geometryStarInnerRadius = 0.4f;


        /// <summary> GeometryCog's count. </summary>     
        public int GeometryCogCount
        {
            get => this.geometryCogCount;
            set
            {
                if (this.geometryCogCount == value) return;
                this.geometryCogCount = value;
                this.OnPropertyChanged(nameof(GeometryCogCount));//Notify 
            }
        }
        private int geometryCogCount = 8;
        /// <summary> GeometryCog's inner-radius. </summary>     
        public float GeometryCogInnerRadius
        {
            get => this.geometryCogInnerRadius;
            set
            {
                if (this.geometryCogInnerRadius == value) return;
                this.geometryCogInnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryCogInnerRadius));//Notify 
            }
        }
        private float geometryCogInnerRadius = 0.7f;
        /// <summary> GeometryCog's tooth. </summary>     
        public float GeometryCogTooth
        {
            get => this.geometryCogTooth;
            set
            {
                if (this.geometryCogTooth == value) return;
                this.geometryCogTooth = value;
                this.OnPropertyChanged(nameof(GeometryCogTooth));//Notify 
            }
        }
        private float geometryCogTooth = 0.3f;
        /// <summary> GeometryCog's notch. </summary>     
        public float GeometryCogNotch
        {
            get => this.geometryCogNotch;
            set
            {
                if (this.geometryCogNotch == value) return;
                this.geometryCogNotch = value;
                this.OnPropertyChanged(nameof(GeometryCogNotch));//Notify 
            }
        }
        private float geometryCogNotch = 0.6f;


        #endregion


        #region Geometry3


        /// <summary> GeometryPie's sweep-angle. </summary>     
        public float GeometryPieSweepAngle
        {
            get => this.geometryPieSweepAngle;
            set
            {
                if (this.geometryPieSweepAngle == value) return;
                this.geometryPieSweepAngle = value;
                this.OnPropertyChanged(nameof(GeometryPieSweepAngle));//Notify 
            }
        }
        private float geometryPieSweepAngle = FanKit.Math.Pi / 2f;
        

        /// <summary> GeometryDount's hole-radius. </summary>     
        public float GeometryDountHoleRadius
        {
            get => this.geometryDountHoleRadius;
            set
            {
                if (this.geometryDountHoleRadius == value) return;
                this.geometryDountHoleRadius = value;
                this.OnPropertyChanged(nameof(GeometryDountHoleRadius));//Notify 
            }
        }
        private float geometryDountHoleRadius = 0.5f;


        /// <summary> GeometryCookie's inner-radius. </summary>     
        public float GeometryCookieInnerRadius
        {
            get => this.geometryCookieInnerRadius;
            set
            {
                if (this.geometryCookieInnerRadius == value) return;
                this.geometryCookieInnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryCookieInnerRadius));//Notify 
            }
        }
        private float geometryCookieInnerRadius = 0.5f;
        /// <summary> GeometryCookie's sweep-angle. </summary>     
        public float GeometryCookieSweepAngle
        {
            get => this.geometryCookieSweepAngle;
            set
            {
                if (this.geometryCookieSweepAngle == value) return;
                this.geometryCookieSweepAngle = value;
                this.OnPropertyChanged(nameof(GeometryCookieSweepAngle));//Notify 
            }
        }
        private float geometryCookieSweepAngle = FanKit.Math.PiOver2;


        #endregion


        #region Geometry4


        /// <summary> GeometryArrow's value. </summary>     
        public float GeometryArrowValue
        {
            get => this.geometryArrowValue;
            set
            {
                if (this.geometryArrowValue == value) return;
                this.geometryArrowValue = value;
                this.OnPropertyChanged(nameof(GeometryArrowValue));//Notify 
            }
        }
        private float geometryArrowValue = 0.5f;
        /// <summary> GeometryArrow's left-tail. </summary>     
        public GeometryArrowTailType GeometryArrowLeftTail
        {
            get => this.geometryArrowLeftTail;
            set
            {
                if (this.geometryArrowLeftTail == value) return;
                this.geometryArrowLeftTail = value;
                this.OnPropertyChanged(nameof(GeometryArrowLeftTail));//Notify 
            }
        }
        private GeometryArrowTailType geometryArrowLeftTail = GeometryArrowTailType.None;
        /// <summary> GeometryArrow's right-tail. </summary>     
        public GeometryArrowTailType GeometryArrowRightTail
        {
            get => this.geometryArrowRightTail;
            set
            {
                if (this.geometryArrowRightTail == value) return;
                this.geometryArrowRightTail = value;
                this.OnPropertyChanged(nameof(GeometryArrowRightTail));//Notify 
            }
        }
        private GeometryArrowTailType geometryArrowRightTail = GeometryArrowTailType.Arrow;


        /// <summary> GeometryArrow's spread. </summary>     
        public float GeometryHeartSpread
        {
            get => this.geometryHeartSpread;
            set
            {
                if (this.geometryHeartSpread == value) return;
                this.geometryHeartSpread = value;
                this.OnPropertyChanged(nameof(GeometryHeartSpread));//Notify 
            }
        }
        private float geometryHeartSpread = 0.8f;


        #endregion

    }
}