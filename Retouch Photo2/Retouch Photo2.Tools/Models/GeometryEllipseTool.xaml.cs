﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryEllipseTool.
    /// </summary>
    public partial class GeometryEllipseTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;


        //@Content 
        public ToolType Type => ToolType.GeometryEllipse;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryEllipseTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryEllipseTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryEllipseTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryEllipseTool. 
        /// </summary>
        public GeometryEllipseTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //Flyout
            this.FillBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowFillColorFlyout?.Invoke(this, this.FillBrushButton);
            this.StrokeBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowStrokeColorFlyout?.Invoke(this, this.StrokeBrushButton);
            this.StrokeShowControl.Tapped += (s, e) => Expander.ShowAt(MenuType.Stroke, this.StrokeShowControl);

            this.ConvertToCurvesButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.ViewModel.ToolType = ToolType.Node;
            };

            this.MoreButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryEllipseLayer
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.CreateTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillTextBlock.Text = resource.GetString("Tools_Fill");
            this.StrokeTextBlock.Text = resource.GetString("Tools_Stroke");

            this.StrokeShowToolTip.Content = resource.GetString("Menus_Stroke");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }
    }
}