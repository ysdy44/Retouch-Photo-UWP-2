﻿// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Segmented of <see cref="CanvasBitmapFileFormat"/>.
    /// </summary>
    public sealed partial class FileFormatSegmented : UserControl
    {
        
        //@VisualState
        CanvasBitmapFileFormat _vsFileFormat;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsFileFormat)
                {
                    case CanvasBitmapFileFormat.Jpeg: return this.Jpeg;
                    case CanvasBitmapFileFormat.Png: return this.Png;
                    case CanvasBitmapFileFormat.Bmp: return this.Bmp;
                    case CanvasBitmapFileFormat.Gif: return this.Gif;
                    case CanvasBitmapFileFormat.Tiff: return this.Tiff;
                    case CanvasBitmapFileFormat.JpegXR: return this.JpegXR;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        
        #region DependencyProperty


        /// <summary> Gets or sets the format type. </summary>
        public CanvasBitmapFileFormat FileFormat
        {
            get => (CanvasBitmapFileFormat)base.GetValue(FileFormatProperty);
            set => base.SetValue(FileFormatProperty, value);
        }
        /// <summary> Identifies the <see cref = "FileFormatSegmented.FileFormat" /> dependency property. </summary>
        public static readonly DependencyProperty FileFormatProperty = DependencyProperty.Register(nameof(FileFormat), typeof(CanvasBitmapFileFormat), typeof(FileFormatSegmented), new PropertyMetadata(CanvasBitmapFileFormat.Jpeg, (sender, e) =>
        {
            FileFormatSegmented control = (FileFormatSegmented)sender;

            if (e.NewValue is CanvasBitmapFileFormat value)
            {
                control._vsFileFormat = value;
                control.VisualState = control.VisualState; // State
            }
        }));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a FileFormatComboBox. 
        /// </summary>
        public FileFormatSegmented()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState; // State
            this.JpegButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Jpeg;
            this.PngButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Png;
            this.BmpButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Bmp;
            this.GifButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Gif;
            this.TiffButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Tiff;
            this.JpegXRButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.JpegXR;
        }


        /// <summary> File Choices. </summary>
        public string FileChoices
        {
            get
            {
                switch (this.FileFormat)
                {
                    case CanvasBitmapFileFormat.Jpeg: return ".Jpeg";
                    case CanvasBitmapFileFormat.Png: return ".Png";
                    case CanvasBitmapFileFormat.Bmp: return ".Bmp";
                    case CanvasBitmapFileFormat.Gif: return ".Gif";
                    case CanvasBitmapFileFormat.Tiff: return ".Tiff";
                    case CanvasBitmapFileFormat.JpegXR: return ".JpegXR";
                    default: return ".Jpeg";
                }
            }
        }

        /// <summary> Clears to the white color. </summary>
        public bool IsClearWhite
        {
            get
            {
                switch (this.FileFormat)
                {
                    case CanvasBitmapFileFormat.Jpeg: return true;
                    case CanvasBitmapFileFormat.Png: return false;
                    case CanvasBitmapFileFormat.Bmp: return false;
                    case CanvasBitmapFileFormat.Gif: return false;
                    case CanvasBitmapFileFormat.Tiff: return false;
                    case CanvasBitmapFileFormat.JpegXR: return true;
                    default: return true;
                }
            }
        }

    }
}