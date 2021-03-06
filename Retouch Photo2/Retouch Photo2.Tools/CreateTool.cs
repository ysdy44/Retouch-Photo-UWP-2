﻿// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ICreateTool"/>'s CreateTool.
    /// </summary>
    public class CreateTool : ICreateTool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsSquare => this.SettingViewModel.IsSquare;

        Layerage MezzanineLayerage;


        public void Started(Func<Transformer, ILayer> createLayer, Vector2 startingPoint, Vector2 point)
        {
            if (this.ViewModel.TransformerTool.Started(startingPoint, point)) return;// TransformerTool

            // Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Snap         
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.GetFirstSelectedLayerage());

            // History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);

            // Selection
            Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);

            // Mezzanine
            ILayer layer = createLayer(transformer);
            Layerage layerage = Layerage.CreateByGuid();
            layer.Id = layerage.Id;
            LayerBase.Instances.Add(layerage.Id, layer);

            // Mezzanine
            this.MezzanineLayerage = layerage;
            LayerManager.Mezzanine(this.MezzanineLayerage);

            // History
            this.ViewModel.MethodSelectedNone();

            // Tip
            this.ViewModel.SetTipTextWidthHeight(transformer);
            this.ViewModel.TipTextVisibility = Visibility.Visible;

            // Selection
            this.Transformer = transformer;
            this.SelectionViewModel.SetModeExtended();

            // Cursor
            CoreCursorExtension.IsManipulationStarted = true;
            CoreCursorExtension.Cross();

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail); // Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.Extended)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                // Snap
                if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

                // Selection
                Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);
                this.Transformer = transformer;

                // Mezzanine
                ILayer mezzanineLayer = this.MezzanineLayerage.Self;
                mezzanineLayer.Transform = new Transform(transformer);
                mezzanineLayer.Style.DeliverBrushPoints(transformer);
                // Refactoring
                mezzanineLayer.IsRefactoringRender = true;
                this.MezzanineLayerage.RefactoringParentsRender();

                this.ViewModel.SetTipTextWidthHeight(transformer); // Tip
                this.ViewModel.Invalidate(); // Invalidate
                return;
            }

            if (this.ViewModel.TransformerTool.Delta(startingPoint, point)) return;// TransformerTool
        }
        public bool Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            bool result = false;
            if (this.Mode == ListViewSelectionMode.Extended)
            {
                if (isOutNodeDistance)
                {
                    Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                    Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                    Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                    // Snap
                    if (this.IsSnap)
                    {
                        canvasPoint = this.Snap.Snap(canvasPoint);
                        this.Snap.Default();
                    }

                    // Transformer
                    Transformer transformer = new Transformer(canvasStartingPoint, canvasPoint, this.IsCenter, this.IsSquare);
                    this.Transformer = transformer;

                    // Mezzanine
                    ILayer mezzanineLayer = this.MezzanineLayerage.Self;
                    mezzanineLayer.Transform = new Transform(transformer);
                    mezzanineLayer.IsSelected = true;
                    // Refactoring
                    mezzanineLayer.IsRefactoringRender = true;
                    mezzanineLayer.IsRefactoringIconRender = true;

                    // Selection
                    this.SelectionViewModel.SetModeSingle(this.MezzanineLayerage);
                    LayerManager.ArrangeLayers();
                    LayerManager.ArrangeLayersBackground();
                    result = true;
                }
                else
                {
                    LayerManager.RemoveMezzanine(this.MezzanineLayerage); // Mezzanine

                    // Selection
                    this.SelectionViewModel.SetModeNone();
                    LayerManager.ArrangeLayers();
                    LayerManager.ArrangeLayersBackground();
                }

                this.MezzanineLayerage = null;
                this.ViewModel.TipTextVisibility = Visibility.Collapsed;// Tip

                // Cursor
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.Cross();

                this.ViewModel.Invalidate(InvalidateMode.HD); // Invalidate
            }

            if (this.ViewModel.TransformerTool.Complete(startingPoint, point)) return false;// TransformerTool
            return result;
        }

        public void Cursor(Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.Extended) return;

            this.ViewModel.ClickeTool.Cursor(point); // TransformerTool
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawWireframe(layer2, matrix, this.ViewModel.AccentColor);

                    this.ViewModel.TransformerTool.Draw(drawingSession); // TransformerTool
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawWireframe(layer, matrix, this.ViewModel.AccentColor);
                    }

                    this.ViewModel.TransformerTool.Draw(drawingSession); // TransformerTool
                    break;
                case ListViewSelectionMode.Extended:
                    drawingSession.DrawBound(this.Transformer, matrix, this.ViewModel.AccentColor);

                    // Snapping
                    if (this.IsSnap)
                    {
                        this.Snap.Draw(drawingSession, matrix);
                        this.Snap.DrawNode2(drawingSession, matrix);
                    }
                    break;
            }
        }

    }
}