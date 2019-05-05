﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    public delegate void AdjustmentHandler(Adjustment adjustment);
    public delegate void AdjustmentsHandler(IEnumerable<Adjustment> adjustments);

    /// <summary> Adjust Layers. </summary>
    public abstract class Adjustment
    {
        //@static
        public static Action Invalidate;

        public AdjustmentType Type;
        public FrameworkElement Icon;

        public AdjustmentItem Item;
        public bool HasPage;
    }
}