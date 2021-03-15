﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Operates;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu
    {

        //@Content
        public bool IsOpen { set => this.OperateMainPage.IsOpen = value; }
        public override UIElement MainPage => this.OperateMainPage;

        readonly OperateMainPage OperateMainPage = new OperateMainPage();


        //@Construct
        /// <summary>
        /// Initializes a OperateMenu. 
        /// </summary>
        public OperateMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("Menus_Operate");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Operate;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            Content = new Retouch_Photo2.Operates.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        Transformer Transformer { get => this.SelectionViewModel.Transformer; set => this.SelectionViewModel.Transformer = value; }
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;


        /// <summary> Gets or sets <see cref = "OperateMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            set
            {
                foreach (UIElement child in this.Grid.Children)
                {
                    if (child is Button button)
                    {
                        if (ToolTipService.GetToolTip(button) is ToolTip toolTip)
                        {
                            toolTip.IsOpen = value;
                        }
                    }
                }
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a OperateMainPage. 
        /// </summary>
        public OperateMainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructTransform();
            this.ConstructArrange();
            this.ConstructHorizontally();
            this.ConstructVertically();
        }
    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            //@Group
            void constructGroup(Button button, string folder)
            {
                string key = button.Name;

                ToolTipService.SetToolTip(button, new ToolTip
                {
                    Placement = PlacementMode.Top,
                    Content = resource.GetString($"Operates_{folder}_{key}"),
                    Style = this.ToolTipStyle
                });
                button.Content = new OperateControl(key, folder);
            }

            string transform = "Transform"; 
            this.TransformTextBlock.Text = resource.GetString($"Operates_{transform}");
            constructGroup(this.FlipHorizontal, transform);
            constructGroup(this.FlipVertical, transform);
            constructGroup(this.RotateLeft, transform);
            constructGroup(this.RotateRight, transform);

            string arrange = "Arrange";
            this.ArrangeTextBlock.Text = resource.GetString($"Operates_{arrange}");
            constructGroup(this.MoveBack, arrange);
            constructGroup(this.BackOne, arrange);
            constructGroup(this.ForwardOne, arrange);
            constructGroup(this.MoveFront, arrange);


            string horizontally = "Horizontally"; 
            this.HorizontallyTextBlock.Text = resource.GetString($"Operates_{horizontally}");
            constructGroup(this.Left, horizontally);
            constructGroup(this.Center, horizontally);
            constructGroup(this.Right, horizontally);
            constructGroup(this.HorizontallySpace, horizontally);

            string vertically = "Vertically";
            this.VerticallyTextBlock.Text = resource.GetString($"Operates_{vertically}");
            constructGroup(this.Top, vertically);
            constructGroup(this.Middle, vertically);
            constructGroup(this.Bottom, vertically);
            constructGroup(this.VerticallySpace, vertically);
        }

    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        //Transform
        private void ConstructTransform()
        {

            this.FlipHorizontal.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(-1, 1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.FlipVertical.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateScale(1, -1, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateLeft.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(-FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

            this.RotateRight.Click += (s, e) =>
            {
                Transformer transformer = this.Transformer;
                Matrix3x2 matrix = Matrix3x2.CreateRotation(FanKit.Math.PiOver2, transformer.Center);
                this.MethodViewModel.MethodTransformMultiplies(matrix);//Method
            };

        }

        //Arrange
        private void ConstructArrange()
        {

            this.MoveBack.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                Layerage parents = LayerManager.GetParentsChildren(destination);
                if (parents.Children.Count < 2) return;

                parents.Children.Remove(destination);
                parents.Children.Add(destination);

                LayerManager.ArrangeLayers();
                this.ViewModel.Invalidate();//Invalidate
            };

            this.BackOne.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                Layerage parents = LayerManager.GetParentsChildren(destination);
                if (parents.Children.Count < 2) return;

                int index = parents.Children.IndexOf(destination);
                index++;

                if (index < 0) index = 0;
                if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;

                parents.Children.Remove(destination);
                parents.Children.Insert(index, destination);

                LayerManager.ArrangeLayers();
                this.ViewModel.Invalidate();//Invalidate
            };

            this.ForwardOne.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                Layerage parents = LayerManager.GetParentsChildren(destination);
                if (parents.Children.Count < 2) return;

                int index = parents.Children.IndexOf(destination);
                index--;

                if (index < 0) index = 0;
                if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;

                parents.Children.Remove(destination);
                parents.Children.Insert(index, destination);

                LayerManager.ArrangeLayers();
                this.ViewModel.Invalidate();//Invalidate
            };

            this.MoveFront.Click += (s, e) =>
            {
                if (this.Mode != ListViewSelectionMode.Single) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
                this.ViewModel.HistoryPush(history);

                Layerage destination = this.SelectionViewModel.SelectionLayerage;
                Layerage parents = LayerManager.GetParentsChildren(destination);
                if (parents.Children.Count < 2) return;

                parents.Children.Remove(destination);
                parents.Children.Insert(0, destination);

                LayerManager.ArrangeLayers();
                this.ViewModel.Invalidate();//Invalidate
            };

        }

        //Horizontally
        private void ConstructHorizontally()
        {
            this.Left.Click += (s, e) => this.TransformAlign(BorderMode.MinX, Orientation.Horizontal);
            this.Center.Click += (s, e) => this.TransformAlign(BorderMode.CenterX, Orientation.Horizontal);
            this.Right.Click += (s, e) => this.TransformAlign(BorderMode.MaxX, Orientation.Horizontal);
            this.HorizontallySpace.Click += (s, e) => this.TransformSapce(Orientation.Horizontal);
        }

        //Vertical
        private void ConstructVertically()
        {
            this.Top.Click += (s, e) => this.TransformAlign(BorderMode.MinY, Orientation.Vertical);
            this.Middle.Click += (s, e) => this.TransformAlign(BorderMode.CenterY, Orientation.Vertical);
            this.Bottom.Click += (s, e) => this.TransformAlign(BorderMode.MaxY, Orientation.Vertical);
            this.VerticallySpace.Click += (s, e) => this.TransformSapce(Orientation.Vertical);
        }

    }

    /// <summary>
    /// MainPag of <see cref = "OperateMenu"/>.
    /// </summary>
    public sealed partial class OperateMainPage : UserControl
    {

        private void TransformAlign(BorderMode borderMode, Orientation orientation)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.Single:
                    {
                        float positionValue = this.ViewModel.CanvasTransformer.GetBorderValue(borderMode);
                        this.TransformAlign(positionValue, borderMode, orientation);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        Transformer transformer = this.Transformer;
                        float positionValue = transformer.GetBorderValue(borderMode);
                        this.TransformAlign(positionValue, borderMode, orientation);
                    }
                    break;
            }
        }

        private void TransformAlign(float positionValue, BorderMode borderMode, Orientation orientation)
        {
            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory(HistoryType.LayersTransformAdd_Move);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                Transformer transformer = layerage.GetActualTransformer();
                float value = transformer.GetBorderValue(borderMode);

                float distance = positionValue - value;
                if (distance == 0) return;
                Vector2 vector = orientation == Orientation.Horizontal ?
                    new Vector2(distance, 0) :
                    new Vector2(0, distance);

                layerage.SetValueWithChildren((layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer, vector);

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsTransformer();
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });

            });
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate}
        }


        ///////////////////////////////


        private void TransformSapce(Orientation orientation)
        {
            if (this.Mode != ListViewSelectionMode.Multiple) return;

            IEnumerable<Layerage> layerages = this.SelectionViewModel.SelectionLayerages;
            int count = layerages.Count();
            if (count < 3) return;

            this.TransformSapce(layerages, count, orientation);
        }

        /// <summary>
        /// Border: 
        ///  Between previous and current
        /// 
        ///  Min            Center           Max             Min            Center           Max             Min            Center           Max
        ///    |-------------o-------------|                  |-------------o-------------|                  |-------------o-------------|
        ///    |__________Length_________|     space    |__________Length_________|     space    |__________Length_________|
        /// 
        /// </summary>
        private void TransformSapce(IEnumerable<Layerage> layerages, int count, Orientation orientation)
        {
            //Layerage, Min, Center, Max, Length
            var borders = orientation == Orientation.Horizontal ?
                from layerage in layerages select this._getBorderX(layerage) :
                from layerage in layerages select this._getBorderY(layerage);

            float min = borders.Min(border => border.Min);//Min
            float max = borders.Max(border => border.Max);//Max

            float lengthSum = borders.Sum(border => border.Length);//Sum of Length
            float space = ((max - min) - lengthSum) / (count - 1);//Between [ previous.Max ] and [ current.Min ].


            //History
            LayersTransformAddHistory history = new LayersTransformAddHistory(HistoryType.LayersTransformAdd_Move);


            float postionMin = min;//[ previous.Min ] + [ previous.Length ] + space.
            var orderedBorders = borders.OrderBy(border => border.Min);

            foreach (var border in orderedBorders)
            {
                Layerage layerage = border.Layerage;

                float distance = postionMin - border.Min;
                postionMin += border.Length + space;//Sum

                if (distance == 0) continue;
                Vector2 vector = orientation == Orientation.Horizontal ?
                    new Vector2(distance, 0) :
                    new Vector2(0, distance);

                //Selection
                layerage.SetValueWithChildren((layerage2) =>
                {
                    ILayer layer = layerage2.Self;

                    //History
                    history.PushTransform(layer, vector);

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layer.CacheTransform();
                    layer.TransformAdd(vector);
                });
            }

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

        private (Layerage Layerage, float Min, float Center, float Max, float Length) _getBorderX(Layerage layerage)
        {
            Transformer transformer = layerage.GetActualTransformer();

            float min = transformer.MinX;
            float center = transformer.Center.X;
            float max = transformer.MaxX;

            return (layerage, min, center, max, max - min);
        }
        private (Layerage Layerage, float Min, float Center, float Max, float Length) _getBorderY(Layerage layerage)
        {
            Transformer transformer = layerage.GetActualTransformer();

            float min = transformer.MinY;
            float center = transformer.Center.Y;
            float max = transformer.MaxY;

            return (layerage, min, center, max, max - min);
        }

    }
}