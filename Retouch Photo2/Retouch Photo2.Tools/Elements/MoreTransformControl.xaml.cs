﻿// Core:              
// Referenced:   ★★
// Difficult:         ★
// Only:              
// Complete:      ★
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "SettingViewModel.IsSquare" /> and <see cref = "SettingViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreTransformControl : UserControl
    {

        //@ViewModel
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Construct
        /// <summary>
        /// Initializes a MoreTransformControl. 
        /// </summary>
        public MoreTransformControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RatioTextBlock.Text = resource.GetString("Tools_MoreTransform_Ratio ");

            this.CenterTextBlock.Text = resource.GetString("Tools_MoreTransform_Center");
        }

    }
}