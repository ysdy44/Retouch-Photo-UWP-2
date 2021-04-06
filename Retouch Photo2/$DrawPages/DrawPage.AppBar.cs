﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        // Menu
        private void ConstructMenuTypes(IList<string> menuTypes)
        {
            foreach (FrameworkElement button in this.MenuButtonsStackPanel.Children)
            {
                string key = button.Name;
                bool isContains = menuTypes.Contains(key);
                button.Visibility = isContains ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        // AppBar. 
        private void ConstructAppBar()
        {
            //AppBarGrid
            this.AppBarGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize.IsEmpty) return;
                if (e.NewSize == e.PreviousSize) return;

                //Document
                this.DocumentColumnDefinition.Width = new GridLength(e.NewSize.Width / 8);
                //Overflow
                this.AppBarOverflow(e.NewSize.Width);
            };

            //Right
            this.RightScrollViewer.ViewChanged += (s, e) => this.ShadowRectangle.Visibility = (40 < this.RightScrollViewer.HorizontalOffset) ? Visibility.Visible : Visibility.Collapsed;


            //Document
            this.DocumentButton.Holding += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
            this.DocumentButton.RightTapped += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);


            this.DocumentUnSaveButton.Click += (s, e) => this.DocumentUnSave();
            this.DocumentButton.Click += (s, e) =>
            {
                int countHistorys = HistoryBase.Instances.Count;
                int countLayerages = LayerManager.RootLayerage.Children.Count;

                if (countHistorys == 0 && countLayerages > 1)
                {
                    this.DocumentUnSave();
                }
                else
                {
                    this.Document();
                }
            };


            //Appbar
            this.ExportButton.Tapped += (s, e) => this.ShowExportDialog();
            this.OverflowExportButton.Tapped += (s, e) =>
            {
                this.ShowExportDialog();
                this.OverflowFlyout.Hide();
            };

            this.UndoButton.Tapped += (s, e) => this.MethodViewModel.MethodEditUndo();
            this.OverflowUndoButton.Tapped += (s, e) => this.MethodViewModel.MethodEditUndo();

            //this.RedoButton.Click += (s, e) => { };
            //this.OverflowRedoButton.Click += (s, e) => { };

            this.SetupButton.Tapped += (s, e) => this.ShowSetupDialog();
            this.OverflowSetupButton.Tapped += (s, e) =>
            {
                this.ShowSetupDialog();
                this.OverflowFlyout.Hide();
            };

            this.RulerButton.Tapped += (s, e) => this.ViewModel.Invalidate();//Invalidate
            this.OverflowRulerButton.Tapped += (s, e) =>
            {
                this.ViewModel.Invalidate();//Invalidate
                this.OverflowFlyout.Hide();
            };

            this.FullScreenButton.Tapped += (s, e) => this.DrawLayout.IsFullScreen = true;
            this.OverflowFullScreenButton.Tapped += (s, e) =>
            {
                this.DrawLayout.IsFullScreen = true;
                this.OverflowFlyout.Hide();
            };

            this.UnFullScreenButton.Click += (s, e) => this.DrawLayout.IsFullScreen = false;

            this.ConstructAppBar_TipButton(this.TipButton);
            this.ConstructAppBar_TipButton(this.OverflowTipButton);
        }
        private void ConstructAppBar_TipButton(ButtonBase tipButton)
        {
            tipButton.PointerPressed += (s, e) => this.TipViewModel.IsOpen = true;
            tipButton.PointerReleased += (s, e) => this.TipViewModel.IsOpen = false;
            tipButton.PointerCanceled += (s, e) => this.TipViewModel.IsOpen = false;
            tipButton.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.TipViewModel.IsOpen = true;
                }
            };
            tipButton.PointerExited += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.TipViewModel.IsOpen = false;
                }
            };
        }


        /// <summary>
        /// Overflow buttons by width.
        /// </summary>
        private void AppBarOverflow(double width)
        {
            double overflowWidth = this.OverflowButton.ActualWidth;
            double rightWidth = this.MenuButtonsStackPanel.ActualWidth;
            double leftWidth = width - overflowWidth - rightWidth;
            int count = (int)(leftWidth / 40.0d) - 1;

            //Overflow
            this.OverflowButton.Visibility = (count < this.LeftStackPanel.Children.Count) ? Visibility.Visible : Visibility.Collapsed;

            //Left
            for (int i = 0; i < this.LeftStackPanel.Children.Count; i++)
            {
                UIElement leftButton = this.LeftStackPanel.Children[i];
                leftButton.Visibility = (i < count) ? Visibility.Visible : Visibility.Collapsed;
            }

            //Overflow
            for (int i = 0; i < this.OverflowStackPanel.Children.Count; i++)
            {
                UIElement overflowButton = this.OverflowStackPanel.Children[i];
                overflowButton.Visibility = (i < count) ? Visibility.Collapsed : Visibility.Visible;
            }
        }


        /// <summary>
        /// Save, exit and back.
        /// </summary>
        private async void Document()
        {
            this.LoadingControl.State = LoadingState.Saving;
            this.LoadingControl.IsActive = true;

            await this.Save();

            await this.Exit();
            this.DrawLayout.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.GoBack();
        }

        /// <summary>
        /// Un save, exit and back.
        /// </summary>
        private async void DocumentUnSave()
        {
            this.LoadingControl.State = LoadingState.Saving;
            this.LoadingControl.IsActive = true;

            await this.Exit();
            this.DrawLayout.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.GoBack();
        }

    }
}