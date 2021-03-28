﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int FontSizeConverter(float fontSize) => (int)fontSize;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "TextMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "TextMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(TextMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a TextMenu. 
        /// </summary>
        public TextMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructAlign();
            this.ConstructFontStyle();
            this.ConstructFontWeight();
            this.ConstructFontFamily();
            this.ConstructFontSize();
        }
    }

    public sealed partial class TextMenu : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FontAlignmentTextBlock.Text = resource.GetString("Texts_FontAlignment");

            this.FontStyleTextBlock.Text = resource.GetString("Texts_FontStyle");
            this.BoldToolTip.Content = resource.GetString("Texts_FontStyle_Bold");
            this.ItalicToolTip.Content = resource.GetString("Texts_FontStyle_Italic");
            this.UnderLineToolTip.Content = resource.GetString("Texts_FontStyle_UnderLine");

            this.FontWeightTextBlock.Text = resource.GetString("Texts_FontWeight");

            this.FontFamilyTextBlock.Text = resource.GetString("Texts_FontFamily");

            this.FontSizeTextBlock.Text = resource.GetString("Texts_FontSize");
        }

    }

    public sealed partial class TextMenu : UserControl
    {

        //FontAlignment
        private void ConstructAlign()
        {
            this.FontAlignmentSegmented.AlignmentChanged += (s, fontAlignment) =>
            {
                this.SetFontAlignment(fontAlignment);
            };
        }


        //FontStyle
        private void ConstructFontStyle()
        {
            this.BoldButton.Click += (s, e) =>
            {
                //Whether the judgment is small or large.
                bool isBold = this.SelectionViewModel.FontWeight.Weight == FontWeights.Bold.Weight;
                // isBold ? ""Normal"" : ""Bold""
                FontWeight fontWeight = isBold ? FontWeights.Normal : FontWeights.Bold;

                this.SetFontWeight(fontWeight);
            };

            this.ItalicButton.Click += (s, e) =>
            {
                //Whether the judgment is Normal or Italic.
                bool isNormal = this.SelectionViewModel.FontStyle == FontStyle.Normal;
                // isNormal ? ""Italic"" : ""Normal""
                FontStyle fontStyle = isNormal ? FontStyle.Italic : FontStyle.Normal;

                this.SetFontStyle(fontStyle);
            };

            this.UnderLineButton.Click += (s, e) =>
            {
            };
        }


        //FontWeight
        private void ConstructFontWeight()
        {
            this.FontWeightComboBox.WeightChanged += (s, fontWeight) => this.SetFontWeight(fontWeight);
        }


        //FontFamily
        private void ConstructFontFamily()
        {
            // Get all FontFamilys in your device.
            this.FontFamilyListView.ItemsSource = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);

            this.FontFamilyButton.Click += (s, e) => this.FontFamilyFlyout.ShowAt(this.FontFamilyButton);

            this.FontFamilyListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is string value)
                {
                    this.SetFontFamily(value);
                }
            };
        }


        //FontSize
        private void ConstructFontSize()
        {
            // Get all fontSizes in your device.
            this.FontSizeListView.ItemsSource = new List<int>
            {
                5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 24, 30, 36, 48, 64, 72, 96, 144, 288,
            };

            this.FontSizeButton.Click += (s, e) => this.FontSizeFlyout.ShowAt(this.FontSizeButton);

            this.FontSizePicker.ValueChanged += (s, value) => this.SetFontSize(value);
            this.FontSizeListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is int value)
                {
                    this.FontSizePicker.Value = value;

                    this.SetFontSize(value);
                }
            };
        }
    }

    public sealed partial class TextMenu : UserControl
    {

        private void SetFontAlignment(CanvasHorizontalAlignment fontAlignment)
        {
            this.SelectionViewModel.FontAlignment = fontAlignment;
            this.MethodViewModel.ITextLayerChanged<CanvasHorizontalAlignment>
            (
                set: (textLayer) => textLayer.FontAlignment = fontAlignment,

                type: HistoryType.LayersProperty_SetFontAlignment,
                getUndo: (textLayer) => textLayer.FontAlignment,
                setUndo: (textLayer, previous) => textLayer.FontAlignment = previous
           );
        }


        private void SetFontWeight(FontWeight fontWeight)
        {
            this.SelectionViewModel.FontWeight = fontWeight;
            this.MethodViewModel.ITextLayerChanged<FontWeight>
            (
                set: (textLayer) => textLayer.FontWeight = fontWeight,

                type: HistoryType.LayersProperty_SetFontWeight,
                getUndo: (textLayer) => textLayer.FontWeight,
                setUndo: (textLayer, previous) => textLayer.FontWeight = previous
           );
        }


        private void SetFontStyle(FontStyle fontStyle)
        {
            this.SelectionViewModel.FontStyle = fontStyle;
            this.MethodViewModel.ITextLayerChanged<FontStyle>
            (
                set: (textLayer) => textLayer.FontStyle = fontStyle,

                type: HistoryType.LayersProperty_SetFontStyle,
                getUndo: (textLayer) => textLayer.FontStyle,
                setUndo: (textLayer, previous) => textLayer.FontStyle = previous
           );
        }


        private void SetFontFamily(string fontFamily)
        {
            this.SelectionViewModel.FontFamily = fontFamily;
            this.MethodViewModel.ITextLayerChanged<string>
            (
                set: (textLayer) => textLayer.FontFamily = fontFamily,

                type: HistoryType.LayersProperty_SetFontFamily,
                getUndo: (textLayer) => textLayer.FontFamily,
                setUndo: (textLayer, previous) => textLayer.FontFamily = previous
           );
        }


        private void SetFontSize(float fontSize)
        {
            this.SelectionViewModel.FontSize = fontSize;
            this.MethodViewModel.ITextLayerChanged<float>
            (
                set: (textLayer) => textLayer.FontSize = fontSize,

                type: HistoryType.LayersProperty_SetFontSize,
                getUndo: (textLayer) => textLayer.FontSize,
                setUndo: (textLayer, previous) => textLayer.FontSize = previous
           );

            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }

    }
}