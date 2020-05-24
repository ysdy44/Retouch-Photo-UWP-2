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
    /// <see cref="Layer"/>'s CurveLayer .
    /// </summary>
    public class CurveLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Curve;

        //@Content 
        public NodeCollection Nodes { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>
        public CurveLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new CurveIcon(),
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(IEnumerable<Node> nodes) : this() => this.Nodes = new NodeCollection(nodes);
        /// <summary>
        /// Initializes a curve-layer from a line.
        /// </summary>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(Vector2 left, Vector2 right) : this() => this.Nodes = new NodeCollection(left, right);


        public override Transformer GetActualDestinationWithRefactoringTransformer(Layerage layerage)
        {
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                TransformerBorder border = new TransformerBorder(this.Nodes);
                Transformer transformer = border.ToTransformer();
                this.Transform.Destination = transformer;
                return this.Transform.IsCrop ? this.Transform.CropDestination : transformer;
            }

            return this.Transform.IsCrop ? this.Transform.CropDestination : this.Transform.Destination;
        }

        public override void CacheTransform()
        {
            base.CacheTransform();
            this.Nodes.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
            this.Nodes.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
            this.Nodes.TransformAdd(vector);
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            CurveLayer curveLayer = new CurveLayer
            {
                Nodes = this.Nodes.Clone()
            };

            Layer.CopyWith(resourceCreator, curveLayer, this);
            return curveLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveNodeCollection("Nodes", "Node", this.Nodes));
        }
        public override void Load(XElement element)
        {
            this.Nodes = FanKit.Transformers.XML.LoadNodeCollection("Node", element.Element("Nodes"));
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return this.Nodes.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Curve");
        }

    }
}