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
    /// <see cref="ILayer"/>'s GeometryCogLayer .
    /// </summary>
    public class GeometryCogLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCog;

        //@Content
        public int Count = 8;
        public float InnerRadius = 0.7f;

        public float Tooth = 0.3f;
        public float Notch = 0.6f;

        //@Construct
        /// <summary>
        /// Initializes a cog-layer.
        /// </summary>
        public GeometryCogLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCogIcon(),
                Text = this.ConstructStrings(),
            };
        }
              

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCogLayer cogLayer = new GeometryCogLayer
            {
                Count = this.Count,
                InnerRadius = this.InnerRadius,
                Tooth = this.Tooth,
                Notch = this.Notch,
            };

            LayerBase.CopyWith(resourceCreator, cogLayer, this);
            return cogLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Count", this.Count));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("Tooth", this.Tooth));
            element.Add(new XElement("Notch", this.Notch));
        }
        public override void Load(XElement element)
        {
            this.Count = (int)element.Element("Count");
            this.InnerRadius = (float)element.Element("InnerRadius");
            this.Tooth = (float)element.Element("Tooth");
            this.Notch = (float)element.Element("Notch");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreateCog(resourceCreator, transformer, canvasToVirtualMatrix,
                this.Count, this.InnerRadius,
                this.Tooth, this.Notch);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromCog(transformer,
                this.Count, this.InnerRadius,
                this.Tooth, this.Notch);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryCog");
        }

    }
}