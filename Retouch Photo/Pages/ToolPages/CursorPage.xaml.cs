﻿using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pages.ToolPages
{
    public sealed partial class CursorPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        public CursorPage()
        {
            this.InitializeComponent();
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        private void StepFrequencyButton_Tapped(object sender, TappedRoutedEventArgs e) => this.ViewModel.Invalidate();
        private void SkewButton_Tapped(object sender, TappedRoutedEventArgs e) => this.ViewModel.Invalidate();
                     


    }
}
