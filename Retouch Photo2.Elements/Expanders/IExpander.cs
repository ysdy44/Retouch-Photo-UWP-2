﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Interface of <see cref="Expander">.
    /// </summary>
    public interface IExpander
    {
        /// <summary> Occurs when the position changes. </summary>
        Action Move { get; set; }
        /// <summary> Occurs when the flyout closed. </summary>
        Action Closed { get; set; }
        /// <summary> Occurs when the flyout opened. </summary>
        Action Opened { get; set; }
        /// <summary> Occurs when the flyout overlaid. </summary>
        Action Overlaid { get; set; }

        /// <summary> Gets or sets the title. </summary>
        String Title { get; set; }
        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { get; set; }        
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }
        /// <summary> Gets or sets the layout. </summary>
        FrameworkElement Layout { get; }
        /// <summary> Gets or sets the button. </summary>
        IExpanderButton Button { get; }

        /// <summary>
        /// Calculate postion relative to the placement target.
        /// </summary>
        /// <param name="placementTarget"> The placement target.</param>
        /// <param name="placementMode"> The placement mode.</param>
        void CalculatePostion(FrameworkElement placementTarget, FlyoutPlacementMode placementMode);
        /// <summary> 
        /// Hide the layout. 
        /// </summary>
        void HideLayout();
        /// <summary> 
        /// Hide the layout, and crop the limiting bound.  
        /// </summary>
        void CropLayout();
    }
}