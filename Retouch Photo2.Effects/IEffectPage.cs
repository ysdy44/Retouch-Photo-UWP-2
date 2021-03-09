﻿// Core:              ★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★★★
using Retouch_Photo2.Elements;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Page of <see cref="Effect"/>.
    /// </summary>
    public interface IEffectPage
    {
        /// <summary> Gets the type. </summary>
        EffectType Type { get; }
        /// <summary> Gets the page. </summary>
        FrameworkElement Page { get; }
        /// <summary> Gets the button. </summary>
        Button Button { get; }
        /// <summary> Gets the CheckControl. </summary>
        CheckControl CheckControl { get; }

        /// <summary>
        /// Reset the <see cref="Effect"/> and <see cref="IEffectPage"/>'s data.
        /// </summary>
        void Reset();
        /// <summary>
        /// IsChecked follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        bool FollowButton(Effect effect);
        /// <summary>
        /// <see cref="IEffectPage"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void FollowPage(Effect effect);
    }
}