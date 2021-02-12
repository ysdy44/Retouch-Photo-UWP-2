﻿using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Gets all photocopiers, which in ( layer and children )'s style manager.
        /// </summary>
        /// <param name="layerages"> The layerages. </param>
        /// <returns> The yield photocopiers. </returns>
        public static IEnumerable<Photocopier> GetPhotocopiers(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                foreach (Photocopier photocopier in LayerManager.GetPhotocopiers(layerage.Children))
                {
                    yield return photocopier;
                }

                //ImageLayer
                if (layer.Type == LayerType.Image)
                {
                    ImageLayer imageLayer = (ImageLayer)layer;
                    yield return imageLayer.Photocopier;
                }

                //Fill
                if (layer.Style.Fill.Type == BrushType.Image)
                {
                    yield return layer.Style.Fill.Photocopier;
                }
                //Stroke
                if (layer.Style.Stroke.Type == BrushType.Image)
                {
                    yield return layer.Style.Stroke.Photocopier;
                }
            }
        }


        /// <summary>
        /// Gets un-uesting layerages
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The yield layerages. </returns>
        public static IEnumerable<Layerage> GetUnUestingLayerages(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                yield return child;

                foreach (Layerage unUesting in LayerManager.GetUnUestingLayerages(child))
                {
                    yield return unUesting;
                }
            }
        }


    }
}