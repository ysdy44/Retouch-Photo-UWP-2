﻿using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public abstract partial class Expander : UserControl
    {

        //Width
        private ExpanderWidth WidthMode
        {
            set
            {
                this.WidthFlyoutItem222.IsChecked = false;
                this.WidthFlyoutItem272.IsChecked = false;
                this.WidthFlyoutItem322.IsChecked = false;
                this.WidthFlyoutItem372.IsChecked = false;

                switch (value)
                {
                    case ExpanderWidth.Width222:
                        this.WidthFlyoutItem222.IsChecked = true;
                        this.WidthStoryboard222.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width272:
                        this.WidthFlyoutItem272.IsChecked = true;
                        this.WidthStoryboard272.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width322:
                        this.WidthFlyoutItem322.IsChecked = true;
                        this.WidthStoryboard322.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width372:
                        this.WidthFlyoutItem372.IsChecked = true;
                        this.WidthStoryboard372.Begin();//Storyboard
                        break;
                }
            }
        }

        private void ConstructWidthStoryboard()
        {
            this.WidthFlyoutItem322.IsChecked = true;
            this.WidthFlyoutItem222.Click += (s, e) => this.WidthMode = ExpanderWidth.Width222;
            this.WidthFlyoutItem272.Click += (s, e) => this.WidthMode = ExpanderWidth.Width272;
            this.WidthFlyoutItem322.Click += (s, e) => this.WidthMode = ExpanderWidth.Width322;
            this.WidthFlyoutItem372.Click += (s, e) => this.WidthMode = ExpanderWidth.Width372;

            this.TitleGrid.RightTapped += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
            this.TitleGrid.Holding += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        }


        //Height        
        private void ConstructHeightStoryboard()
        {
            this.HeightKeyFrames.Completed += (s, e) =>
            {
                this.HeightKeyFrames.From = this.HeightKeyFrames.To;
            };
            
            this.PageBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                double height = e.NewSize.Height;
                this.HeightKeyFrames.To = height + 40;
               this.HeightStoryboard.Begin();//Storyboard
            };

            {
                double height = this.PageBorder.Width;
                this.HeightRectangle.Height = height + 40;
                this.HeightKeyFrames.From = 200;
            }
        }

    }
}