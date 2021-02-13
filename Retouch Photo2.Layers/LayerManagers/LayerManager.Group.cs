﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Remove a layerage from a parents's children,
        /// and insert to parents's parents's children
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        public static bool ReleaseGroupLayer(Layerage layerage)
        {
            Layerage parents = layerage.Parents;

            if (parents != LayerManager.Layerage)
            {
                ILayer parentsLayer = parents.Self;

                Layerage parentsParents = LayerManager.GetParentsChildren(parents);
                int parentsIndex = parents.Children.IndexOf(parents);
                if (parentsIndex < 0) parentsIndex = 0;
                if (parentsIndex > parentsParents.Children.Count - 1) parentsIndex = parentsParents.Children.Count - 1;

                parents.Children.Remove(layerage);
                parentsParents.Children.Insert(parentsIndex, layerage);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Un group all group layerage
        /// </summary>
        public static void UnGroupAllSelectedLayer()
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();
            Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayerages);
            if (outermost == null) return;
            Layerage parents = LayerManager.GetParentsChildren(outermost);
            int index = parents.Children.IndexOf(outermost);
            if (index < 0) index = 0;


            do
            {
                Layerage groupLayerage = selectedLayerages.FirstOrDefault(l => l.Self.Type == LayerType.Group);
                if (groupLayerage == null) break;
                ILayer groupLayer = groupLayerage.Self;

                //Insert
                foreach (Layerage layerage in groupLayerage.Children)
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = true;
                    parents.Children.Insert(index, layerage);
                }
                groupLayerage.Children.Clear();

                //Remove
                {
                    Layerage groupLayerageParents = LayerManager.GetParentsChildren(groupLayerage);
                    groupLayerageParents.Children.Remove(groupLayerage);
                }

            } while (selectedLayerages.Any(l => l.Self.Type == LayerType.Group) == false);
        }


        /// <summary>
        /// Group all selected layerages.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        public static void GroupAllSelectedLayers(CanvasDevice customDevice)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();
            Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayerages);
            if (outermost == null) return;
            Layerage parents = LayerManager.GetParentsChildren(outermost);
            int index = parents.Children.IndexOf(outermost);
            if (index < 0) index = 0;


            //GroupLayer
            GroupLayer groupLayer = new GroupLayer(customDevice)
            {
                IsSelected = true,
                IsExpand = false,
                //Refactoring
                IsRefactoringTransformer = true,
            };
            Layerage groupLayerage = groupLayer.ToLayerage();
            LayerBase.Instances.Add(groupLayer);


            //Temp
            foreach (Layerage layerage in selectedLayerages)
            {
                ILayer layer = layerage.Self;

                Layerage childParents = LayerManager.GetParentsChildren(layerage);
                childParents.Children.Remove(layerage);
                layer.IsSelected = false;

                groupLayerage.Children.Add(layerage);
            }

            //Insert
            parents.Children.Insert(index, groupLayerage);
        }


    }
}