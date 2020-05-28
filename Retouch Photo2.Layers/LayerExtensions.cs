﻿using FanKit.Transformers;

namespace Retouch_Photo2.Layers
{
    public static class LayerExtensions
    {

        public static bool IsText(this LayerType  layerType)
        {
            switch (layerType)
            {
                case LayerType.TextArtistic:
                case LayerType.TextFrame:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsScale(this TransformerMode transformerMode)
        {
            switch (transformerMode)
            {
                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    return true;
                default:
                    return false;
            }
        }


    }
}