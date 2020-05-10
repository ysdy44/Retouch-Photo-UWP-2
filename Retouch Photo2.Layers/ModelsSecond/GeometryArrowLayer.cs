﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="ILayer"/>'s ArrowLayer .
    /// </summary>
    public class GeometryArrowLayer : LayerBase, ILayer
    {

        //@Override
        public override LayerType Type => LayerType.GeometryArrow;

        //@Content       
        public bool IsAbsolute = false;
        public float Width = 10;
        public float Value = 0.5f;

        public GeometryArrowTailType LeftTail = GeometryArrowTailType.None;
        public GeometryArrowTailType RightTail = GeometryArrowTailType.Arrow;

        //@Construct
        /// <summary>
        /// Construct a arrow-layer.
        /// </summary>
        public GeometryArrowLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryArrowIcon(),
                Text = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryArrowLayer ArrowLayer = new GeometryArrowLayer
            {
                IsAbsolute = this.IsAbsolute,
                Width = this.Width,
                Value = this.Value,

                LeftTail = this.LeftTail,
                RightTail = this.RightTail,
            };

            LayerBase.CopyWith(resourceCreator, ArrowLayer, this);
            return ArrowLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("IsAbsolute", this.IsAbsolute));
            element.Add(new XElement("Width", this.Width));
            element.Add(new XElement("Value", this.Value));

            element.Add(new XElement("LeftTail", this.LeftTail));
            element.Add(new XElement("RightTail", this.RightTail));
        }
        public override void Load(XElement element)
        {
            this.IsAbsolute = (bool)element.Element("IsAbsolute");
            this.Width = (float)element.Element("Width");
            this.Value = (float)element.Element("Value");

            this.LeftTail = FanKit.Transformers.XML.CreateGeometryArrowTailType(element.Element("LeftTail").Value);
            this.RightTail = FanKit.Transformers.XML.CreateGeometryArrowTailType(element.Element("RightTail").Value);
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateArrow(resourceCreator, transformer, canvasToVirtualMatrix,
                this.IsAbsolute, this.Width, this.Value,
                this.LeftTail, this.RightTail);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromArrow(transformer,
                this.IsAbsolute, this.Width, this.Value,
                this.LeftTail, this.RightTail);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryArrow");
        }

    }
}