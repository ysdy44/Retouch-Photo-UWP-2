﻿// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="IMoveTool"/>'s MoveTool.
    /// </summary>
    public partial class MoveTool : IMoveTool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        
        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;
        BorderBorderSnap Snap => this.ViewModel.BorderBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;

            
        public bool Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            bool isMove = this.GetIsSelectedLayer(canvasStartingPoint);
            if (isMove == false) return false;

            //Snap
            if (this.IsSnap) this.ViewModel.BorderBorderSnapInitiate(this.SelectionViewModel.GetFirstSelectedLayerage());

            //Selection
            if (this.IsSnap) this.Snap.StartingSource = new TransformerBorder(this.Transformer);

            //Method
            this.MethodViewModel.MethodTransformAddStarted();
            return true;
        }
        public bool Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;
            
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasMove = canvasPoint - canvasStartingPoint;

            //Snap
            if (this.IsSnap) canvasMove = this.Snap.Snap(canvasMove);

            //Method
            this.MethodViewModel.MethodTransformAddDelta(canvasMove);
            return true;
        }
        public bool Complete(Vector2 startingPoint, Vector2 point)
        {
            if (this.Mode == ListViewSelectionMode.None) return false;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasMove = canvasPoint - canvasStartingPoint;

            //Snap
            if (this.IsSnap)
            {
                canvasMove = this.Snap.Snap(canvasMove);
                this.Snap.Default();
            }

            //Method
            this.MethodViewModel.MethodTransformAddComplete(canvasMove);
            return true;
        }

        public bool Clicke(Vector2 point)
        {
            //SelectedLayer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Layerage selectedLayerage = this.GetClickSelectedLayerage(canvasPoint);

            if (selectedLayerage == null)
            {
                this.MethodViewModel.MethodSelectedNone();//Method
                return false;
            }


            switch (this.MarqueeCompositeMode)
            {
                //Method
                case MarqueeCompositeMode.New: this.MethodViewModel.MethodSelectedNew(selectedLayerage); return true;
                case MarqueeCompositeMode.Add: this.MethodViewModel.MethodSelectedAdd(selectedLayerage); return true;
                case MarqueeCompositeMode.Subtract: this.MethodViewModel.MethodSelectedSubtract(selectedLayerage); return true;
                //case MarqueeCompositeMode.Intersect: this.MethodViewModel.MethodSelectedIntersect(selectedLayerage); return true;
                default: return false;
            }
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.Mode == ListViewSelectionMode.None) return;

            //Transformer
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            drawingSession.DrawBoundNodes(this.Transformer, matrix, this.ViewModel.AccentColor);

            //Snapping
            if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);
        }



        private Layerage GetClickSelectedLayerage(Vector2 canvasPoint)
        {
            //Select a layer of the same depth
            Layerage selectedLayerage = this.SelectionViewModel.GetFirstSelectedLayerage();
            Layerage parents = LayerageCollection.GetParentsChildren(selectedLayerage);

            bool FillContainsPoint(Layerage layerage)
            {
                ILayer layer = layerage.Self;

                return layer.FillContainsPoint(layerage, canvasPoint);
            };

            return parents.Children.FirstOrDefault(layerage=> FillContainsPoint(layerage));
        }

        private bool GetIsSelectedLayer(Vector2 canvasStartingPoint)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        bool isFillContains = this.Transformer.FillContainsPoint(canvasStartingPoint);
                        if (isFillContains) return true;
                    }
                    break;
            }

            //SelectedLayerage
            Layerage selectedLayerage = this.GetClickSelectedLayerage(canvasStartingPoint);
            
            if (selectedLayerage == null)
            {
                this.MethodViewModel.MethodSelectedNone();//Method
                return false;
            }
            else
            {
                this.MethodViewModel.MethodSelectedNew(selectedLayerage);//Method
                return true;
            }
        }

    }
}