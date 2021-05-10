﻿// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections.Generic;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Category of <see cref = "IStyle" />.
    /// </summary>
    public class StyleCategory
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The source data.
        /// </summary>
        public IEnumerable<IStyle> Styles { get; set; }
    }
}