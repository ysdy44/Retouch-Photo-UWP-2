﻿// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryCogLayer .
    /// </summary>
    public class GeometryCogLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCog;

        //@Content
        public int Count = 8;
        public int StartingCount { get; private set; }
        public void CacheCount() => this.StartingCount = this.Count;

        public float InnerRadius = 0.7f;
        public float StartingInnerRadius { get; private set; }
        public void CacheInnerRadius() => this.StartingInnerRadius = this.InnerRadius;


        public float Tooth = 0.3f;
        public float StartingTooth { get; private set; }
        public void CacheTooth() => this.StartingTooth = this.Tooth;

        public float Notch = 0.6f;
        public float StartingNotch { get; private set; }
        public void CacheNotch() => this.StartingNotch = this.Notch;


        //@Construct
        /// <summary>
        /// Initializes a cog-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryCogLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
              

        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryCogLayer cogLayer = new GeometryCogLayer(customDevice)
            {
                Count = this.Count,
                InnerRadius = this.InnerRadius,
                Tooth = this.Tooth,
                Notch = this.Notch,
            };

            LayerBase.CopyWith(customDevice, cogLayer, this);
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
            if (element.Element("Count") is XElement count) this.Count = (int)count;
            if (element.Element("InnerRadius") is XElement innerRadius) this.InnerRadius = (float)innerRadius;
            if (element.Element("Tooth") is XElement tooth) this.Tooth = (float)tooth;
            if (element.Element("Notch") is XElement notch) this.Notch = (float)notch;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateCog(resourceCreator, transformer,
                this.Count, this.InnerRadius,
                this.Tooth, this.Notch);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;
            
            return TransformerGeometry.CreateCog(resourceCreator, transformer, matrix,
                this.Count, this.InnerRadius,
                this.Tooth, this.Notch);
        }
        

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_GeometryCog");
        }

    }
}