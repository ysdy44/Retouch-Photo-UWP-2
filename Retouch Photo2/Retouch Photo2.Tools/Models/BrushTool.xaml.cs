﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public partial class BrushTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        FillOrStroke FillOrStroke { get => this.SelectionViewModel.FillOrStroke; set => this.SelectionViewModel.FillOrStroke = value; }

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;
        public Visibility FillOrStrokeToVisibilityConverter(FillOrStroke fillOrStroke) => fillOrStroke == FillOrStroke.Fill ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FillOrStrokeToCollapsedConverter(FillOrStroke fillOrStroke) => fillOrStroke == FillOrStroke.Fill ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ColorToVisibilityConverter(BrushType type) => type == BrushType.Color ? Visibility.Visible : Visibility.Collapsed;
        public bool GradientToTrueConverter(BrushType type)
        {
            switch (type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    return true;
                default:
                    return false;
            }
        }
        public Visibility NoneToCollapsedConverter(BrushType type)
        {
            switch (type)
            {
                case BrushType.None:
                case BrushType.Color:
                    return Visibility.Collapsed;
                default:
                    return Visibility.Visible;
            }
        }


        //@Content
        public ToolType Type => ToolType.Brush;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a BrushTool. 
        /// </summary>
        public BrushTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructStopsPicker();
            this.ConstructFill();
            this.ConstructStroke();
            this.TypeComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.TypeComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting
            this.StrokeShowControl.Tapped += (s, e) => Expander.ShowAt(MenuType.Stroke, this.StrokeShowControl);

            this.FillOrStrokeComboBox.FillOrStrokeChanged += (s, fillOrStroke) =>
            {
                this.FillOrStroke = fillOrStroke;
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.FillOrStrokeComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.FillOrStrokeComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting

            this.ExtendComboBox.ExtendChanged += (s, extend) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillExtendChanged(extend);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeExtendChanged(extend);
                        break;
                }
            };
            this.ExtendComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.ExtendComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting
        }


        BrushHandleMode HandleMode = BrushHandleMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            // Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            // Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.Transformer);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillStarted(startingPoint, point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeStarted(startingPoint, point);
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail); // Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            // Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Snap
            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillDelta(canvasStartingPoint, canvasPoint);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeDelta(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.ViewModel.Invalidate(); // Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            // Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Snap
            if (this.IsSnap)
            {
                canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillComplete(canvasStartingPoint, canvasPoint);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeComplete(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.HandleMode = BrushHandleMode.None;
            this.ViewModel.Invalidate(InvalidateMode.HD); // Invalidate
        }
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point)
        {
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillCursor(point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeCursor(point);
                    break;
            }
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawWireframe(layer2, matrix, this.ViewModel.AccentColor);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawWireframe(layer, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }


            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        // Snapping
                        if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);

                        switch (this.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                this.Fill.Draw(drawingSession, matrix, this.ViewModel.AccentColor);
                                break;
                            case FillOrStroke.Stroke:
                                this.Stroke.Draw(drawingSession, matrix, this.ViewModel.AccentColor);
                                break;
                        }
                    }
                    break;
            }
        }


        public void OnNavigatedTo()
        {
            Layerage layerage = this.SelectionViewModel.GetFirstSelectedLayerage();
            if (layerage != null)
            {
                ILayer layer = layerage.Self;
                this.SelectionViewModel.SetStyle(layer.Style);
            }
            this.ViewModel.Invalidate(); // Invalidate
        }
        public void OnNavigatedFrom() { }

    }


    public partial class BrushTool : Page, ITool
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillOrStrokeTextBlock.Text = resource.GetString("Tools_Brush_FillOrStroke");
            this.TypeTextBlock.Text = resource.GetString("Tools_Brush_Type");

            this.ExtendTextBlock.Text = resource.GetString("Tools_Brush_Extend");
        }

        private void ConstructStopsPicker()
        {
            //@Focus
            this.StopsPicker.ColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.StopsPicker.ColorPicker.Focus(FocusState.Programmatic); };
            this.StopsPicker.ColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StopsPicker.ColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.StopsPicker.ColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StopsPicker.ColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.StopsPicker.StopsChanged += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChanged(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChanged(array);
                        break;
                }
            };

            this.StopsPicker.StopsChangeStarted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeStarted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeStarted(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeDelta += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeDelta(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeDelta(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeCompleted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeCompleted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeCompleted(array);
                        break;
                }
            };
        }

    }
}