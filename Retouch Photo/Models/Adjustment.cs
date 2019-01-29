﻿using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Adjustments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Models
{

    /// <summary>
    /// 传递Adjustment的委托。
    /// </summary>
    public delegate void AdjustmentHandler(Adjustment adjustment);

    /// <summary>
    /// Adjustment: 调整。
    /// 给图层提供调整。
    /// </summary>
    public abstract class Adjustment
    {
        public AdjustmentType Type;
        public FrameworkElement Icon;
        /// <summary> 有无参数页面 </summary>
         public bool HasPage;

        /// <summary> 重置参数 </summary>
        public abstract void Reset();
        public abstract ICanvasImage GetRender(ICanvasImage image);

        //@static
        public static ICanvasImage Render(List<Adjustment> adjustments, ICanvasImage image)
        {
            if (adjustments == null) return image;
            if (adjustments.Count == 0) return image;
            if (adjustments.Count == 1) return adjustments.Single().GetRender(image);

            foreach (var item in adjustments)
            {
                image = item.GetRender(image);
            }
            return image;
        }
    }

    /// <summary>
    /// AdjustmentCandidate: 调整候选人。
    /// 用于生成Adjustment类，和Adjustment类一一对应，并给它提供帮助。
    /// 避免性能浪费，所以不会多次实例化同类型的AdjustmentCandidate
    /// </summary>
    public abstract class AdjustmentCandidate
    {
        public AdjustmentType Type;
        public FrameworkElement Icon;
        public FrameworkElement Page;

        /// <summary> 生成一个新的Adjustment实例 </summary>
        public abstract Adjustment GetNewAdjustment();
        /// <summary> 给当前类的页面来赋值 </summary>
        public abstract void SetPage(Adjustment adjustment);

        //@static
        public static AdjustmentCandidate GetAdjustmentCandidate(AdjustmentType type) => AdjustmentCandidate.AdjustmentCandidateList.First(e => e.Type == type);
        public static List<AdjustmentCandidate> AdjustmentCandidateList = new List<AdjustmentCandidate>()
        {
            new GrayAdjustmentCandidate(),
            new InvertAdjustmentCandidate(),
            new ExposureAdjustmentCandidate(),
            new SaturationAdjustmentCandidate(),
            new HueRotationAdjustmentCandidate(),
            new ContrastAdjustmentCandidate(),
            new TemperatureAdjustmentCandidate(),
            new HighlightsAndShadowsAdjustmentCandidate(),
        };
    }

    public enum AdjustmentType
    {
        /// <summary> 灰度 </summary>
        Gray,
        /// <summary> 反色 </summary>
        Invert,

        /// <summary> 曝光 </summary>
        Exposure,
        /// <summary> 明度 </summary>
        Brightness,
        /// <summary> 饱和度 </summary>
        Saturation,
        /// <summary> 色相旋转 </summary>
        HueRotation,
        /// <summary> 对比度 </summary>
        Contrast,
        /// <summary> 冷暖 </summary>
        Temperature,

        /// <summary> 高亮/阴影 </summary>
        HighlightsAndShadows,
    }
}