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
    /// <see cref="ILayer"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPentagon;

        //@Content
        public int Points = 5;

        //@Construct
        /// <summary>
        /// Construct a pentagon-layer.
        /// </summary>
        public GeometryPentagonLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryPentagonIcon(),
                Text = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPentagonLayer PentagonLayer = new GeometryPentagonLayer
            {
                Points = this.Points,
            };

            LayerBase.CopyWith(resourceCreator, PentagonLayer, this);
            return PentagonLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Points", this.Points));
        }
        public override void Load(XElement element)
        {
            this.Points = (int)element.Element("Points");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, canvasToVirtualMatrix, this.Points);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromPentagon(transformer, this.Points);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryPentagon");
        }

    }
}