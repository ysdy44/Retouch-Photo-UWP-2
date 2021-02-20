﻿// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s PatternDiagonalLayer .
    /// </summary>
    public class PatternDiagonalLayer : PatternLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.PatternDiagonal;

        //@Content   
        public float Offset = 30.0f;
        public float StartingOffset { get; private set; }
        public void CacheOffset() => this.StartingOffset = this.Offset;

        public float HorizontalStep = 30.0f;
        public float StartingHorizontalStep { get; private set; }
        public void CacheHorizontalStep() => this.StartingHorizontalStep = this.HorizontalStep;


        public override ILayer Clone() => LayerBase.CopyWith(this, new PatternDiagonalLayer
        {
            Offset = this.Offset,
            HorizontalStep = this.HorizontalStep,
        });

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Offset", this.Offset));
            element.Add(new XElement("HorizontalStep", this.HorizontalStep));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Offset") is XElement offset) this.Offset = (float)offset;
            if (element.Element("HorizontalStep") is XElement horizontalStep) this.HorizontalStep = (float)horizontalStep;
        }


        public override void GetPatternRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry)
        {
            ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
            float strokeWidth = this.Style.StrokeWidth;
            CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;

            Transformer transformer = base.Transform.GetActualTransformer();
            TransformerBorder border = new TransformerBorder(transformer);

            for (float i = border.Left; i < border.Right; i += this.HorizontalStep)
            {
                drawingSession.DrawLine(i, border.Top, i + this.Offset, border.Bottom, canvasBrush, strokeWidth, strokeStyle);
            }
        }

    }
}