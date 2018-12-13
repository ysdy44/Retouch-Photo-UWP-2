﻿using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Models
{
    public struct VectorRect
    {
        public Vector2 Start;
        public Vector2 End;
        
        public VectorRect(Vector2 start, Vector2 end)
        {
            this.Start = start;
            this.End = end;
        }
        public VectorRect(Vector2 start, Vector2 end, MarqueeMode mode)
        {
            this.Start = start;
            this.End = end;

            switch (mode)
            {
                case MarqueeMode.Square:
                    float square = (Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)) / 2;
                    this.Start = start;
                    this.End.X = start.X < end.X ? start.X + square : start.X - square;
                    this.End.Y = start.Y < end.Y ? start.Y + square : start.Y - square;
                    break;

                case MarqueeMode.Center:
                    this.Start = start + start - end;
                    this.End = end;
                    break;

                case MarqueeMode.SquareAndCenter:
                    float square2 = (Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y)) / 2;
                    this.Start.X = start.X - square2;
                    this.Start.Y = start.Y - square2;
                    this.End.X = start.X + square2;
                    this.End.Y = start.Y + square2;
                    break;

                default:
                    break;
            }

        }

        public VectorRect(Rect rect)
        {
            this.Start.X = (float)rect.Left;
            this.Start.Y = (float)rect.Top;
            this.End.X = (float)rect.Right;
            this.End.Y = (float)rect.Bottom;
        }
        public Rect ToRect() => new Rect(this.X, this.Y, this.Width, this.Height);

        public float X => Math.Min(this.Start.X, this.End.X);
        public float Y => Math.Min(this.Start.Y, this.End.Y);
        public float Width => Math.Abs(this.Start.X - this.End.X);
        public float Height => Math.Abs(this.Start.Y - this.End.Y);
        
        public float Left => this.Start.X;
        public float Top => this.Start.Y;
        public float Right => this.End.X;
        public float Bottom => this.End.Y;

        public Vector2 LeftTop => this.Start;
        public Vector2 RightTop => new Vector2(this.End.X, this.Start.Y);
        public Vector2 RightBottom => this.End;
        public Vector2 LeftBottom => new Vector2(this.Start.X, this.End.Y);

        /// <summary>变换矩形</summary>
        public VectorRect Transform(Matrix3x2 matrix) => new VectorRect(Vector2.Transform(this.End, matrix), Vector2.Transform(this.Start, matrix));
                
        /// <summary>Draw nodes and lines ，just like【由】</summary>
        public static void DrawNodeLine(CanvasDrawingSession ds, VectorRect rect, Matrix3x2 canvasToVirtualMatrix, bool isDrawNode=false)
        {
            Vector2 leftTop = Vector2.Transform(rect.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(rect.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(rect.RightBottom, canvasToVirtualMatrix); 
            Vector2 leftBottom = Vector2.Transform(rect.LeftBottom, canvasToVirtualMatrix);

            ds.DrawLine(leftTop, rightTop, Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Colors.DodgerBlue);

            if (isDrawNode)
            {
                VectorRect.DrawNodeVector2(ds, leftTop);
                VectorRect.DrawNodeVector2(ds, rightTop);
                VectorRect.DrawNodeVector2(ds, rightBottom);
                VectorRect.DrawNodeVector2(ds, leftBottom);
                

                Vector2 centerLeft = (leftTop+ leftBottom) / 2;
                Vector2 centerTop = (leftTop + rightTop) / 2;
                Vector2 centerRight = (rightTop + rightBottom) / 2;
                Vector2 centerBottom = (leftBottom + rightBottom) / 2;

                VectorRect.DrawNodeVector2(ds, centerLeft);
                VectorRect.DrawNodeVector2(ds, centerTop);
                VectorRect.DrawNodeVector2(ds, centerRight);
                VectorRect.DrawNodeVector2(ds, centerBottom);
            }
        }

        /// <summary>draw a ⊙ </summary>
        private static void DrawNodeVector(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Colors.White);
        }
        /// <summary> draw a ●</summary>
        private static void DrawNodeVector2(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.White);
            ds.FillCircle(vector, 6, Colors.DodgerBlue);
        }
               
    }
}
