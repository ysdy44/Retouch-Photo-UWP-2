﻿// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public partial class GeometryPieLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPie;

        //@Content       
        public float SweepAngle = FanKit.Math.PiOver2;
        public float StartingSweepAngle { get; private set; }
        public void CacheSweepAngle() => this.StartingSweepAngle = this.SweepAngle;


        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryPieLayer
        {
            SweepAngle = this.SweepAngle,
        });

        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("SweepAngle", this.SweepAngle));
        }
        public override void Load(XElement element)
        {
            if (element.Element("SweepAngle") is XElement sweepAngle) this.SweepAngle = (float)sweepAngle;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePie(resourceCreator, transformer, this.SweepAngle);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePie(resourceCreator, transformer, matrix, this.SweepAngle);
        }

    }
}