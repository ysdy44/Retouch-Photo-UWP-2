﻿// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Microsoft.Graphics.Canvas;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a Extend from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="CanvasEdgeBehavior"/>. </returns>
        public static CanvasEdgeBehavior CreateExtend(string type)
        {
            switch (type)
            {
                case "Clamp": return CanvasEdgeBehavior.Clamp;
                case "Wrap": return CanvasEdgeBehavior.Wrap;
                case "Mirror": return CanvasEdgeBehavior.Mirror;

                default: return CanvasEdgeBehavior.Clamp;
            }
        }

    }
}