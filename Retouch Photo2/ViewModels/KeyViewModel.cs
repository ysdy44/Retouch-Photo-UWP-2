﻿using Retouch_Photo2.Library;
using Retouch_Photo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Retouch_Photo2.ViewModels
{
    public class KeyViewModel
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        
        public void KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Control:
                    this.ViewModel.KeyCtrl = true;
                    break;

                case VirtualKey.Shift:
                    this.ViewModel.KeyShift = true;
                    break;

                case VirtualKey.Delete:
                    Layer layer = this.ViewModel.Layer;
                    if (layer != null)
                    {
                        this.ViewModel.RenderLayer.Remove(layer);
                        this.ViewModel.Tool.ToolOnNavigatedTo();
                    }
                    this.ViewModel.SetLayer(null);
                    break;

                default:
                    break;
            }
            this.KeyUpAndDown(sender, args);
        }


        public void KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Control:
                    this.ViewModel.KeyCtrl = false;
                    break;

                case VirtualKey.Shift:
                    this.ViewModel.KeyShift = false;
                    break;

                default:
                    break;
            }
            this.KeyUpAndDown(sender, args);
        }


        public void KeyUpAndDown(CoreWindow sender, KeyEventArgs args)
        {
            if (this.ViewModel.KeyCtrl == false && this.ViewModel.KeyShift == false)
                this.ViewModel.MarqueeMode = MarqueeMode.None;
            else if (this.ViewModel.KeyCtrl == false && this.ViewModel.KeyShift)
                this.ViewModel.MarqueeMode = MarqueeMode.Square;
            else if (this.ViewModel.KeyCtrl && this.ViewModel.KeyShift == false)
                this.ViewModel.MarqueeMode = MarqueeMode.Center;
            else //if (this.ViewModel.KeyCtrl && this.ViewModel.KeyShift)
                this.ViewModel.MarqueeMode = MarqueeMode.SquareAndCenter;
        }
        
    }
}
