﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo.Library
{
    /// <summary>  the Dotted Line of photoshop  </summary>
    public class DottedLine
    {
        //Brush  
        Vector2 space;
        readonly CanvasLinearGradientBrush brush;
        readonly CanvasGradientStop[] stops = new CanvasGradientStop[2] { new CanvasGradientStop { Color = Windows.UI.Colors.White, Position = 0 }, new CanvasGradientStop { Color = Windows.UI.Colors.Black, Position = 1 } };

        //Image
        public ICanvasImage OutPut;

        /// <summary> Initialize DottedLine</summary>
        /// <param name="distance">Distance between black and white</param>
        /// <param name="space">Refresh, change the position of the gradient</param>
        public DottedLine(ICanvasResourceCreator creator, float distance = 6, float space = 1)
        {
            this.space = new Vector2(space, space);

            this.brush = new CanvasLinearGradientBrush(creator, stops, CanvasEdgeBehavior.Mirror, CanvasAlphaMode.Premultiplied)
            {
                StartPoint = new Vector2(0, 0),
                EndPoint = new Vector2(distance, distance)
            };
        }
        

        /// <summary>Update</summary>
        public void Update()
        {
            this.brush.StartPoint -= this.space;
            this.brush.EndPoint -= this.space;
        }

        
        /// <summary>Draw</summary>
        /// <param name="canvasBounds">the bounds of this CanvasCOntrol.</param>
        public void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Rect canvasBounds)
        {
            if (this.OutPut == null) return;

            CanvasCommandList commandList = new CanvasCommandList(creator);
            using (var dds = commandList.CreateDrawingSession())
            {
                dds.FillRectangle(canvasBounds, this.brush);
                dds.DrawImage(this.OutPut, 0, 0, canvasBounds, 1, CanvasImageInterpolation.NearestNeighbor, CanvasComposite.DestinationIn);
            }
            ds.DrawImage(commandList);
        }
                

        /// <summary>
        /// Render
        /// When your image has changed, call it
        /// </summary>
        /// <param name="image">Marquee selection</param>
        public void Render(ICanvasResourceCreator creator, ICanvasImage image)
        {
            //Crop:So that it does not exceed the canvas boundary
            CanvasCommandList commandList = new CanvasCommandList(creator);
            using (var ds = commandList.CreateDrawingSession())
            {
                ds.Clear(Windows.UI.Colors.Transparent);
                ds.DrawImage(new CropEffect
                {
                    SourceRectangle = image.GetBounds(creator),
                    Source = image,
                });
            }

            //DottedLine
            this.OutPut = new LuminanceToAlphaEffect//Alpha
            {
                Source = new EdgeDetectionEffect//Edge
                {
                    Amount = 1,
                    Source = commandList
                }
            };
        }

        public void Render(ICanvasResourceCreator creator, ICanvasImage image, Matrix3x2 matrix)
        {
            //Transform by matrix
            Transform2DEffect effect = new Transform2DEffect
            {
                Source = image,
                TransformMatrix = matrix
            };

            //Crop:So that it does not exceed the canvas boundary
            CanvasCommandList commandList = new CanvasCommandList(creator);
            using (var ds = commandList.CreateDrawingSession())
            {
                ds.Clear(Windows.UI.Colors.Transparent);
                ds.DrawImage(new CropEffect
                {
                    SourceRectangle = this.GetMatrixRect(image.GetBounds(creator), matrix),
                    Source = effect,
                });
            }

            //DottedLine
            this.OutPut = new LuminanceToAlphaEffect//Alpha
            {
                Source = new EdgeDetectionEffect//Edge
                {
                    Amount = 1,
                    Source = commandList
                }
            };
        }

        private Rect GetMatrixRect(Rect rect, Matrix3x2 matrix)
        {
            /*
              Vector2 a = new Vector2((float)rect.Left, (float)rect.Top);
              Vector2 b = new Vector2((float)rect.Right, (float)rect.Bottom);
              Vector2 aa = Vector2.Transform(a, matrix);
              Vector2 bb = Vector2.Transform(b, matrix);

              return new Rect(aa.X + 2, aa.Y + 2, bb.X - aa.X - 4, bb.Y - aa.Y - 4);
           */
            return new Rect
            (
               x: rect.X * matrix.M11 + matrix.M31,
               y: rect.Y * matrix.M22 + matrix.M32,
               width: rect.Width * matrix.M11,
               height: rect.Width * matrix.M22
            );
        }
                
    }
}



