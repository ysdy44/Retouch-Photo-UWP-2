﻿using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Photos;
using Retouch_Photo2.Tools.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary>
    /// Mode of <see cref="PhotosPage"/>
    /// </summary>
    public enum GalleryMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Add a <see cref="ImageLayer"/>. </summary>
        AddImage,

        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        FillImage,
        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        StrokeImage,

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        SelectImage,
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        ReplaceImage
    }

    public sealed partial class DrawPage : Page
    {

        //@Static       
        /// <summary> Add a <see cref="ImageLayer"/>. </summary>
        public static Action<Photo> AddImageCallBack;

        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Fill"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> FillImageCallBack;
        /// <summary> Make <see cref="Retouch_Photo2.Styles.IStyle.Stroke"/> to <see cref="IBrush"/> in <see cref="BrushTool"/>. </summary>
        public static Action<Photo> StrokeImageCallBack;

        /// <summary> Select a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> SelectImageCallBack;
        /// <summary> Replace a image in <see cref= "ImageTool" />. </summary>
        public static Action<Photo> ReplaceImageCallBack;


        private GalleryMode GalleryMode { get; set; } = GalleryMode.None;


        private void ConstructGallery()
        {
            this.GalleryDialog.GridView.ItemsSource = Photo.Instances;

            this.GalleryDialog.CloseButton.Click += (s, e) => this.GalleryDialog.Hide();
            this.GalleryDialog.PrimaryButton.Click += async (s, e) =>
            {
                //Files
                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop);
                await this.CopyMultipleImageFilesAsync(files);
            };

            Photo.FlyoutShow += this.BillboardCanvas.Show;
            Photo.ItemClick += (sender, photo) =>
            {
                GalleryMode mode = this.GalleryMode;

                switch (mode)
                {
                    case GalleryMode.None:
                        return;
                    case GalleryMode.AddImage:
                        Retouch_Photo2.DrawPage.AddImageCallBack?.Invoke(photo);//Delegate
                        break;

                    case GalleryMode.FillImage:
                        Retouch_Photo2.DrawPage.FillImageCallBack?.Invoke(photo);//Delegate
                        break;
                    case GalleryMode.StrokeImage:
                        Retouch_Photo2.DrawPage.StrokeImageCallBack?.Invoke(photo);//Delegate
                        break;

                    case GalleryMode.SelectImage:
                        Retouch_Photo2.DrawPage.SelectImageCallBack?.Invoke(photo);//Delegate
                        break;
                    case GalleryMode.ReplaceImage:
                        Retouch_Photo2.DrawPage.ReplaceImageCallBack?.Invoke(photo);//Delegate
                        break;
                    default:
                        return;
                }

                this.GalleryDialog.Hide();
            };
        }
        private void ShowGalleryDialog(GalleryMode mode)
        {
            this.GalleryMode = mode;
            this.GalleryDialog.Show();
        }


        private void ConstructDragAndDrop()
        {
            this.AllowDrop = true;
            this.Drop += async (s, e) =>
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                    await this.CopyMultipleImageFilesAsync(items);
                }
            };
            this.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                //e.DragUIOverride.Caption = App.resourceLoader.GetString("DropAcceptable_");//可以接受的图片
                e.DragUIOverride.IsCaptionVisible = e.DragUIOverride.IsContentVisible = e.DragUIOverride.IsGlyphVisible = true;
            };
        }


         
        private async Task CopyMultipleImageFilesAsync(IReadOnlyList<StorageFile> files)
        {
            if (files == null) return;

            foreach (StorageFile file in files)
            {
                StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(file);
                if (copyFile == null) return;
                Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                Photo.DuplicateChecking(photo);
            }
        }
        private async Task CopyMultipleImageFilesAsync(IReadOnlyList<IStorageItem> items)
        {
            if (items == null) return;

            foreach (IStorageItem item in items)
            {
                //Photo
                StorageFile copyFile = await FileUtil.CopySingleImageFileAsync(item);
                if (copyFile == null) return;
                Photo photo = await Photo.CreatePhotoFromCopyFileAsync(LayerManager.CanvasDevice, copyFile);
                Photo.DuplicateChecking(photo);
            }
        }

    }
}