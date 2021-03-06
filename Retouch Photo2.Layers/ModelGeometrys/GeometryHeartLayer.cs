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
    /// <see cref="GeometryLayer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryHeart;

        //@Content
        public float Spread = 0.8f;
        public float StartingSpread { get; private set; }
        public void CacheSpread() => this.StartingSpread = this.Spread;


        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryHeartLayer
        {
            Spread = this.Spread
        });

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Spread", this.Spread));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Spread") is XElement spread) this.Spread = (float)spread;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, this.Spread);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, matrix, this.Spread);
        }

    }
}