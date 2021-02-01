﻿// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using FanKit.Transformers;
using System;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change document serup.
    /// </summary>
    public class LayersSetupTransformMultipliesHistory : LayersTransformMultipliesHistory
    {

        private int Width = 1024;
        private int Height = 1024;
        private readonly Action SizeAction;


        //@Construct
        /// <summary>
        /// Initializes a LayersSetupTransformMultipliesHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        /// <param name="canvasTransformer"> The canvas-transformer. </param>  
        public LayersSetupTransformMultipliesHistory(string title, CanvasTransformer canvasTransformer) : base(title)
        {
            this.Width = canvasTransformer.Width;
            this.Height = canvasTransformer.Height;

            this.SizeAction = () =>
            {
                canvasTransformer.Width = this.Width;
                canvasTransformer.Height = this.Height;
                canvasTransformer.ReloadMatrix();
            };
        }

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            base.Undo();

            this.SizeAction?.Invoke();
        }

    }
}