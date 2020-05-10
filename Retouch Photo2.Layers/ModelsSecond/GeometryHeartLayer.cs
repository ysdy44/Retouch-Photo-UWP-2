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
    /// <see cref="ILayer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryHeart;

        //@Content
        public float Spread = 0.8f;

        //@Construct
        /// <summary>
        /// Construct a heart-layer.
        /// </summary>
        public GeometryHeartLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryHeartIcon(),
                Text = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryHeartLayer HeartLayer = new GeometryHeartLayer
            {
                Spread=this.Spread
            };

            LayerBase.CopyWith(resourceCreator, HeartLayer, this);
            return HeartLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Spread", this.Spread));
        }
        public override void Load(XElement element)
        {
            this.Spread = (float)element.Element("Spread");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, canvasToVirtualMatrix, this.Spread);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromHeart(transformer, this.Spread);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryHeart");
        }

    }
}