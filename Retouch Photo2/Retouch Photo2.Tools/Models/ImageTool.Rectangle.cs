﻿using FanKit.Transformers;
using System;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s ImageTool.
    /// </summary>
    public partial class ImageTool : ITool
    {

        public Transformer CreateTransformer(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer canvasTransformer = this.GetAspectRatioRectangle(startingPoint, point, sizeWidth, sizeHeight);
            return canvasTransformer * inverseMatrix;
        }


        /// <summary>
        /// Get a rectangle with the same size scale.
        /// </summary>
        /// <param name="startingPoint"> starting-point </param>
        /// <param name="point"> point </param>
        /// <param name="sizeWidth"> The source size width. </param>
        /// <param name="sizeHeight"> The source size height. </param>
        /// <returns> Transformer </returns>
        private Transformer GetAspectRatioRectangle(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            float lengthSquared = Vector2.DistanceSquared(startingPoint, point);

            //Height not less than 10
            if (sizeWidth > sizeHeight)
            {
                float heightSquared = lengthSquared / (1 + (sizeWidth * sizeWidth) / (sizeHeight * sizeHeight));
                float height = (float)Math.Sqrt(heightSquared) / 1.4142135623730950488016887242097f;

                if (height < 10) height = 10;
                float width = height * sizeWidth / sizeHeight;

                return this.GetRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width not less than 10
            else if (sizeWidth < sizeHeight)
            {
                float widthSquared = lengthSquared / (1 + (sizeHeight * sizeHeight) / (sizeWidth * sizeWidth));
                float width = (float)Math.Sqrt(widthSquared) / 1.4142135623730950488016887242097f; ;

                if (width < 10) width = 10;
                float height = width * sizeHeight / sizeWidth;

                return this.GetRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width equals height
            else
            {
                float spare = (float)Math.Sqrt(lengthSquared) / 1.4142135623730950488016887242097f; ;

                return this.GetRectangleInQuadrant(startingPoint, point, spare, spare);
            }
        }


        /// <summary>
        /// Get a rectangle corresponding to the 1, 2, 3 or 4 quadrant.
        /// </summary>
        /// <param name="startingPoint"> starting-point </param>
        /// <param name="point"> point </param>
        /// <param name="sizeWidth"> The source size width. </param>
        /// <param name="sizeHeight"> The source size height. </param>
        /// <returns> Transformer </returns>
        private Transformer GetRectangleInQuadrant(Vector2 startingPoint, Vector2 point, float width, float height)
        {
            bool xAxis = (point.X >= startingPoint.X);
            bool yAxis = (point.Y >= startingPoint.Y);

            //Fourth Quadrant  
            if (xAxis && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }
            //Third Quadrant  
            else if (xAxis == false && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }
            //First Quantity
            else if (xAxis && yAxis == false)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }
            //Second Quadrant
            else
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }
        }

    }
}