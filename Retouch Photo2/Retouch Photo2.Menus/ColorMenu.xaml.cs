﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref="Windows.UI.Color"/>.
    /// </summary>
    public sealed partial class ColorMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Construct
        /// <summary>
        /// Initializes a ColorMainPage. 
        /// </summary>
        public ColorMenu()
        {
            this.InitializeComponent();

            this.ConstructColor1();
            this.ConstructColor2();
        }


        private void ConstructColor1()
        {
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.ColorPicker.HexPicker is TextBox textBox)
            {
                //textBox.IsEnabled = false;
                //this.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                //this.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChanged(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChanged(value);
                        break;
                }
            };
        }

        private void ConstructColor2()
        {
            //Color
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeStarted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeStarted(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeDelta(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeDelta(value);
                        break;
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.MethodViewModel.MethodFillColorChangeCompleted(value);
                        break;
                    case FillOrStroke.Stroke:
                        this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
                        break;
                }
            };
        }

    }
}