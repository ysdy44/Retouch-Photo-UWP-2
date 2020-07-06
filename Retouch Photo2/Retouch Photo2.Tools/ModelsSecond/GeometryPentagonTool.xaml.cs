﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public partial class GeometryPentagonTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private double PointsValueConverter(float points) => points;


        //@Construct
        /// <summary>
        /// Initializes a GeometryPentagonTool. 
        /// </summary>
        public GeometryPentagonTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructPoints1();
            this.ConstructPoints2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public sealed partial class GeometryPentagonTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryPentagon");

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPentagon_Points");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPentagon;
        public FrameworkElement Icon { get; } = new GeometryPentagonIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryPentagonIcon()
        };
        public FrameworkElement Page => this;

        
        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPentagonLayer(customDevice)
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public sealed partial class GeometryPentagonTool : Page, ITool
    {

        //Points
        private void ConstructPoints1()
        {
            this.PointsTouchbarPicker.Minimum = 3;
            this.PointsTouchbarPicker.Maximum = 36;
            this.PointsTouchbarPicker.ValueChange += (sender, value) =>
            {
                int points = (int)value;

                this.MethodViewModel.TLayerChanged<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPentagonPoints = points,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set pentagon layer points",
                    getHistory: (tLayer) => tLayer.Points = points,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

        private void ConstructPoints2()
        {
            this.PointsTouchbarSlider.Minimum = 3;
            this.PointsTouchbarSlider.Maximum = 36;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPentagonLayer>
            (
                layerType: LayerType.GeometryPentagon,
                cache: (tLayer) => tLayer.CachePoints()
            );
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryPentagonLayer>
            (
                layerType: LayerType.GeometryPentagon,
                set: (tLayer) => tLayer.Points = (int)value
            );
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPentagonPoints = points,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set pentagon layer points",
                    getHistory: (tLayer) => tLayer.StartingPoints,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

    }
}