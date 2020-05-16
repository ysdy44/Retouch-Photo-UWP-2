﻿using Retouch_Photo2.Menus;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a <see cref="MenuType"/> from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="MenuType"/>. </returns>
        public static MenuType CreateMenuType(string type)
        {
            switch (type)
            {
                case "Debug": return MenuType.Keyboard;

                case "Selection": return MenuType.Selection;
                case "Operate": return MenuType.Operate;

                case "Adjustment": return MenuType.Adjustment;
                case "Effect": return MenuType.Effect;

                case "Character": return MenuType.Character;
                case "Paragraph": return MenuType.Paragraph;

                case "Stroke": return MenuType.Stroke;
                case "Style": return MenuType.Style;

                case "History": return MenuType.History;
                case "Transformer": return MenuType.Transformer;

                case "Layer": return MenuType.Layer;
                case "Color": return MenuType.Color;
                default: return MenuType.Keyboard;
            }
        }

    }
}