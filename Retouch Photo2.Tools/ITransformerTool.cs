﻿using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Interface of <see cref = "TransformerTool" />.
    /// </summary>
    public abstract class ITransformerTool
    {
        //Cursor
        /// <summary> <see cref = "Tool.Starting" />'s method. </summary>
        public abstract bool Starting(Vector2 point);

        /// <summary> <see cref = "Tool.Started" />'s method. </summary>
        public abstract bool Started(Vector2 startingPoint, bool isSetTransformerMode = true);

        /// <summary> <see cref = "Tool.Delta" />'s method. </summary>
        public abstract bool Delta(Vector2 startingPoint, Vector2 point);

        /// <summary> <see cref = "Tool.Complete" />'s method. </summary>
        public abstract bool Complete(bool isSingleStarted);
     
        /// <summary> <see cref = "Tool.Draw" />'s method. </summary>
        public abstract void Draw(CanvasDrawingSession ds);
    }
}