﻿// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change layer properties.
    /// </summary>
    public class LayersPropertyHistory : HistoryBase, IHistory
    {
        /// <summary>
        /// Undo action
        /// </summary>
        public Action UndoAction { get; set; }

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="type"> The type. </param>  
        public LayersPropertyHistory(HistoryType type)
        {
            base.Type = type;
        }

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            this.UndoAction?.Invoke();
        }

        public void Dispose()
        {
            this.UndoAction = null;
        }
    }
}