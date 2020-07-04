﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
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
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public partial class GeometryDiamondTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        private bool TouchBarMode
        {
            set
            {
                if (value == false)
                {
                    this.MidTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.MidTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.MidTouchbarSlider;
                }
            }
        }
        
        //@Converter
        private int MidNumberConverter(float mid) => (int)(mid * 100.0f);
        private double MidValueConverter(float mid) => mid * 100d;

        //@Construct
        /// <summary>
        /// Initializes a GeometryDiamondTool. 
        /// </summary>
        public GeometryDiamondTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructMid1();
            this.ConstructMid2();
            this.ConstructMirror();
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public sealed partial class GeometryDiamondTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = 
                this.Title = resource.GetString("/ToolsSecond/GeometryDiamond");
            this._button.Style = this.IconSelectedButtonStyle;

            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryDiamond_Mirror");
            this.MidTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDiamond_Mid");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDiamond;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryDiamondIcon();
        readonly Button _button = new Button { Tag = new GeometryDiamondIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryDiamondLayer(customDevice)
            {
                Mid = this.SelectionViewModel.GeometryDiamondMid,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
       
    /// <summary>
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public sealed partial class GeometryDiamondTool : Page, ITool
    {

        //Mid
        private void ConstructMid1()
        {
            //Button
            this.MidTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.MidTouchbarSlider.Unit = "%";
            this.MidTouchbarSlider.NumberMinimum = 0;
            this.MidTouchbarSlider.NumberMaximum = 100;
            this.MidTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                if (mid < 0.0f) mid = 0.0f;
                if (mid > 1.0f) mid = 1.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryDiamondMid = mid,
                    set: (tLayer) => tLayer.Mid = mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.Mid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }
        private void ConstructMid2()
        {
            //Value
            this.MidTouchbarSlider.Value = 0;
            this.MidTouchbarSlider.Minimum = 0;
            this.MidTouchbarSlider.Maximum = 100;
            this.MidTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                this.MethodViewModel.TLayerChangeStarted<GeometryDiamondLayer>
                (
                    LayerType.GeometryDiamond,
                    (tLayer) => tLayer.CacheMid()
                );
            };
            this.MidTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                if (mid < 0.0f) mid = 0.0f;
                if (mid > 1.0f) mid = 1.0f;

                this.MethodViewModel.TLayerChangeDelta<GeometryDiamondLayer>
                (
                    LayerType.GeometryDiamond,
                    (tLayer) => tLayer.Mid = mid
                );


                //Selection
                this.SelectionViewModel.GeometryDiamondMid = mid;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryDiamond)
                    {
                        GeometryDiamondLayer geometryDiamondLayer = (GeometryDiamondLayer)layer;
                  
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryDiamondLayer.Mid = mid;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.MidTouchbarSlider.ValueChangeCompleted += (sender, value2) =>
            {
                float mid = (float)value2 / 100.0f;
                if (mid < 0.0f) mid = 0.0f;
                if (mid > 1.0f) mid = 1.0f;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryDiamondMid = mid,
                    set: (tLayer) => tLayer.Mid = mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.StartingMid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float mid= 1.0f - this.SelectionViewModel.GeometryDiamondMid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryDiamondMid = mid,
                    set: (tLayer) => tLayer.Mid = 1.0f - tLayer.Mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.Mid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

    }
}