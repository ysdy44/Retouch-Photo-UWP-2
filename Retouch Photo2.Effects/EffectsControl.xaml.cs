﻿// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Represents a effects control, that containing some buttons.
    /// </summary>
    public partial class EffectsControl : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "EffectsControl" />'s EffectPages. </summary>
        public IList<IEffectPage> EffectPages { get; set; } = new List<IEffectPage>
        {
        };

        /// <summary> Gets or sets <see cref = "EffectsControl" />'s Effect. </summary>
        public Effect Effect
        {
            set
            {
                if (value != null)
                {
                    foreach (IEffectPage effectPage in this.EffectPages)
                    {
                        if (effectPage == null) continue;

                        effectPage.CheckControl.IsEnabled = true;
                        effectPage.Button.IsEnabled = effectPage.CheckControl.IsChecked = effectPage.FollowButton(value);
                    }
                }
                else
                {
                    foreach (IEffectPage effectPage in this.EffectPages)
                    {
                        if (effectPage == null) continue;

                        effectPage.CheckControl.IsEnabled = false;
                        effectPage.Button.IsEnabled = effectPage.CheckControl.IsChecked = false;
                    }
                }
            }
        }

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a EffectsControl. 
        /// </summary>
        public EffectsControl()
        {
            this.InitializeComponent();
        }


        //String
        public void ConstructString(IList<IEffectPage> effectPages)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            foreach (IEffectPage effectPage in effectPages)
            {
                if (effectPage == null) continue;

                EffectType type = effectPage.Type;
                Button button = effectPage.Button;
                CheckControl checkControl = effectPage.CheckControl;

                button.IsEnabled = false;
                button.Style = this.IconButton;
                button.Content = resource.GetString($"Effects_{type}");
                button.Resources = new ResourceDictionary
                {
                    //@Template
                    Source = new Uri($@"ms-appx:///Retouch Photo2.Effects\Icons\{type}Icon.xaml")
                };
                button.Tag = new ContentControl
                {
                    //@Template
                    Template = button.Resources[$"{type}Icon"] as ControlTemplate
                };

                checkControl.Height = button.Height;
            }
        }

        public void ConstructButton(IList<IEffectPage> effectPages)
        {
            foreach (IEffectPage effectPage in effectPages)
            {
                if (effectPage == null)
                    this.ButtonsStackPanel.Children.Add(new Rectangle
                    {
                        Style = this.SeparatorRectangle
                    });
                else
                    this.ButtonsStackPanel.Children.Add(effectPage.Button);
            }
        }

        public void ConstructToggleButton(IList<IEffectPage> effectPages)
        {
            foreach (IEffectPage effectPage in effectPages)
            {
                if (effectPage == null)
                    this.CheckControlsStackPanel.Children.Add(new Rectangle
                    {
                        Style = this.SeparatorRectangle2
                    });
                else
                    this.CheckControlsStackPanel.Children.Add(effectPage.CheckControl);
            }
        }

    }
}