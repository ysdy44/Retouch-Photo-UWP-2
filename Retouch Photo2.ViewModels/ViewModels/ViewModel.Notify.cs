﻿using FanKit.Transformers;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the accent color. </summary>
        public Color AccentColor { get; set; } = Colors.DodgerBlue;


        /// <summary> Gets or sets the on state of the IsHitTestVisible on the canvas. </summary>
        public bool CanvasHitTestVisible
        {
            get => this.canvasHitTestVisible;
            set
            {
                this.canvasHitTestVisible = value; 
                this.OnPropertyChanged(nameof(CanvasHitTestVisible));//Notify 
            }
        }
        private bool canvasHitTestVisible = true;


        /// <summary> Gets or sets the tip text. </summary>
        public string TipText
        {
            get => this.tipText;
            set
            {
                this.tipText = value;
                this.OnPropertyChanged(nameof(TipText));//Notify 
            }
        }
        private string tipText = string.Empty;
        /// <summary> Gets or sets the visibility of tip text. </summary>
        public Visibility TipTextVisibility
        {
            get => this.tipTextVisibility;
            set
            {
                this.tipTextVisibility = value;
                this.OnPropertyChanged(nameof(TipTextVisibility));//Notify 
            }
        }
        private Visibility tipTextVisibility = Visibility.Collapsed;


        public async void TipTextBegin(string text)
        {
            this.TipText = text;
            this.TipTextVisibility = Visibility.Visible;
            await Task.Delay(2000);
            this.TipTextVisibility = Visibility.Collapsed;
        }
        public void SetTipText()
        {
            if (this.TipText.Length > 44) this.TipText = string.Empty;
            else this.TipText += "O";
        }


        int _width;
        int _height;
        public void SetTipTextWidthHeight(Transformer transformer)
        {
            Vector2 horizontal = transformer.Horizontal;
            Vector2 vertical = transformer.Vertical;

            int width = (int)horizontal.Length();
            int height = (int)vertical.Length();

            if (this._width != width || this._height != height)
            {
                this._width = width;
                this._height = height;
                this.TipText = $"W: {width} px  H:{height} px";
            }
        }

        int _x;
        int _y;
        public void SetTipTextPosition()
        {
            int x = (int)this.CanvasTransformer.Position.X;
            int y = (int)this.CanvasTransformer.Position.X;

            if (this._x != x || this._y != y)
            {
                this._x = x;
                this._y = y;
                this.TipText = $"X: {x} px  Y:{y} px";
            }
        }

        int _percent;
        public void SetTipTextScale()
        {
            int percent = (int)(this.CanvasTransformer.Scale * 100);

            if (this._percent != percent)
            {
                this._percent = percent;
                this.TipText = $"{percent} %";
            }
        }
        

    }
}