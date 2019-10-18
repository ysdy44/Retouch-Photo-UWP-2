﻿using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public class GeometryHeartTool : IGeometryTool
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Override
        public override IGeometryLayer CreateGeometryLayer(Transformer transformer)
        {
            return new GeometryHeartLayer
            {
                Spread = this.SelectionViewModel.GeometryHeartSpread,
            };
        }
        public override ToolType Type => ToolType.GeometryHeart;
        public override FrameworkElement Icon { get; } = new GeometryHeartIcon();
        public override IToolButton Button { get; } = new GeometryHeartButton();
        public override IToolPage Page { get; } = new GeometryHeartPage();
    }
}