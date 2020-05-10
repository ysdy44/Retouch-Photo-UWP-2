﻿using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreTransformControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        
        //@Construct
        public MoreTransformControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RatioTextBlock.Text = resource.GetString("/ToolElements/MoreTransform_Ratio ");

            this.CenterTextBlock.Text = resource.GetString("/ToolElements/MoreTransform_Center");
        }

    }
}