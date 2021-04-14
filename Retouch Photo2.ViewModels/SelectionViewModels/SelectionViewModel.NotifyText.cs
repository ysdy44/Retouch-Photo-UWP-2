﻿using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Texts;
using System.ComponentModel;
using Windows.UI.Text;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged, ITextLayer
    {

        /// <summary> <see cref="ITextLayer"/>'s font text. </summary>     
        public string FontText
        {
            get => this.fontText;
            set
            {
                if (this.fontText == value) return;
                this.fontText = value;
                this.OnPropertyChanged(nameof(FontText));//Notify 
            }
        }
        private string fontText = null;

        /// <summary> <see cref="ITextLayer"/>'s font family.. </summary>     
        public string FontFamily
        {
            get => this.fontFamily;
            set
            {
                this.fontFamily = value;
                this.OnPropertyChanged(nameof(FontFamily));//Notify 
            }
        }
        private string fontFamily = "Arial";

        /// <summary> <see cref="ITextLayer"/>'s font size. </summary>
        public float FontSize
        {
            get => this.fontSize;
            set
            {
                this.fontSize = value;
                this.OnPropertyChanged(nameof(FontSize));//Notify 
            }
        }
        private float fontSize = 22;

        /// <summary> <see cref="ITextLayer"/>'s horizontal alignment. </summary>
        public CanvasHorizontalAlignment HorizontalAlignment
        {
            get => this.horizontalAlignment;
            set
            {
                this.horizontalAlignment = value; 
                this.OnPropertyChanged(nameof(HorizontalAlignment));//Notify 
            }
        }
        private CanvasHorizontalAlignment horizontalAlignment = CanvasHorizontalAlignment.Left;

        /// <summary> <see cref="ITextLayer"/>'s font style. </summary>
        public FontStyle FontStyle
        {
            get => this.fontStyle;
            set
            {
                this.fontStyle = value;
                this.OnPropertyChanged(nameof(FontStyle));//Notify 
            }
        }
        private FontStyle fontStyle = FontStyle.Normal;

        /// <summary> <see cref="ITextLayer"/>'s font weight. </summary>
        public FontWeight2 FontWeight
        {
            get => this.fontWeight;
            set
            {
                this.fontWeight = value;
                this.OnPropertyChanged(nameof(FontWeight));//Notify 
            }
        }
        private FontWeight2 fontWeight = FontWeight2.Normal;


        /// <summary> Sets the <see cref="ITextLayer"/>. </summary>     
        private void SetTextLayer(ILayer layer)
        {
            if (layer == null) return;

            ITextLayer fextLayer = this.GetTextLayer(layer);
            if (fextLayer == null) return;

            TextLayer.FontCopyWith(fextLayer, this);
        }
        /// <summary> Gets the <see cref="ITextLayer"/>. </summary>     
        private ITextLayer GetTextLayer(ILayer layer)
        {
            if (layer == null) return null;

            if (layer.Type == LayerType.TextArtistic)
            {
                return (TextArtisticLayer)layer;
            }
            else if (layer.Type == LayerType.TextFrame)
            {
                return (TextFrameLayer)layer;
            }

            return null;
        }

    }
}