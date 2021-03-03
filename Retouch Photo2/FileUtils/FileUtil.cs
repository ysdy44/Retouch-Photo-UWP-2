﻿// Core:              ★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★★★★
// Complete:      ★★★
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    /// <summary>
    /// A collection of file-processing util methods.
    /// </summary>
    public static partial class FileUtil
    {

        /// <summary>
        /// Delet all files in temporary folder.
        /// </summary>
        public static async Task DeleteInTemporaryFolder()
        {
            try
            {
                IReadOnlyList<StorageFile> items = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
                foreach (StorageFile item in items)
                {
                    await item.DeleteAsync();
                }
            }
            catch (Exception) { }
        }


        #region ProjectViewItem


        /// <summary>
        /// Initializes ProjectViewItem by zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder. </param>
        /// <returns> The product ProjectViewItem. </returns>
        public static IProjectViewItem ConstructProjectViewItem(StorageFolder zipFolder)
        {
            string name = zipFolder.DisplayName.Replace(".photo2pk", "");

            return FileUtil.ConstructProjectViewItem(name, zipFolder);
        }
        /// <summary>
        /// Initializes a ProjectViewItem by zip folder.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="zipFolder"> The zip folder. </param>
        /// <returns> The product ProjectViewItem. </returns>
        public static IProjectViewItem ConstructProjectViewItem(string name, StorageFolder zipFolder)
        {
            return new ProjectViewItem
            {
                Name = name,
                ImageSource = new Uri($"{zipFolder.Path}\\Thumbnail.png", UriKind.Relative)
            };
        }


        #endregion



        #region Save


        /// <summary>
        /// Save thumbnail file to zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder.</param>
        /// <param name="renderTarget"> The render target.</param>
        /// <returns> Return Image Path. </returns>
        public static async Task<Uri> SaveThumbnailFile(StorageFolder zipFolder, CanvasRenderTarget renderTarget)
        {
            StorageFile thumbnailFile = await zipFolder.CreateFileAsync("Thumbnail.png");

            using (IRandomAccessStream fileStream = await thumbnailFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png);
            }

            return new Uri(thumbnailFile.Path, UriKind.Relative);
        }


        /// <summary>
        /// Saves the entire bitmap to the specified stream
        /// with the specified file format and quality level.
        /// </summary>
        /// <param name="renderTarget"> The render target.</param>
        /// <param name="fileChoices"> The file choices. </param>
        /// <param name="suggestedFileName"> The suggested name of file. </param>
        /// <param name="fileFormat"> The file format. </param>
        /// <param name="quality"> The file quality. </param>
        /// <returns> Saved successful? </returns>
        public static async Task<bool> SaveCanvasBitmapFile(CanvasRenderTarget renderTarget, string fileChoices = ".Jpeg", string suggestedFileName = "Untitled", CanvasBitmapFileFormat fileFormat = CanvasBitmapFileFormat.Jpeg, float quality = 1.0f)
        {
            //FileSavePicker
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = suggestedFileName,
            };
            savePicker.FileTypeChoices.Add("DB", new[] { fileChoices });


            //PickSaveFileAsync
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file == null) return false;

            try
            {
                using (IRandomAccessStream accessStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await renderTarget.SaveAsync(accessStream, fileFormat, quality);
                }

                renderTarget.Dispose();
                return true;
            }
            catch (Exception)
            {
                renderTarget.Dispose();
                return false;
            }
        }


        #endregion


        #region Pick and Copy


        /// <summary>
        /// Copy file to the temporary folder, and return the copy.
        /// </summary>
        /// <param name="file"> The destination file. </param>
        /// <returns> The copied  file. </returns>
        public async static Task<StorageFile> CopySingleImageFileAsync(StorageFile file)
        { 
            if (file == null) return null;

            StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
            return copyFile;
        }
        /// <summary>
        /// The file picker is displayed so that the user can select a file.
        /// </summary>
        /// <param name="location"> The destination locationId. </param>
        /// <returns> The product file. </returns>
        public async static Task<StorageFile> PickSingleImageFileAsync(PickerLocationId location)
        {
            //Picker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = location,
                FileTypeFilter =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".bmp"
                }
            };

            //File
            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
        }
        /// <summary>
        /// The files picker is displayed so that the user can select a files.
        /// </summary>
        /// <param name="location"> The destination locationId. </param>
        /// <returns> The product files. </returns>
        public async static Task<IReadOnlyList<StorageFile>> PickMultipleImageFilesAsync(PickerLocationId location)
        {
            //Picker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = location,
                FileTypeFilter =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".bmp"
                }
            };

            //File
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            return files;
        }

        /// <summary>
        /// Filter files that are not pictures.
        /// Then copy to the temporary folder, and return the copy.
        /// </summary>
        /// <param name="item"> The destination item. </param>
        /// <returns> The product file. </returns>
        public async static Task<StorageFile> CopySingleImageFileAsync(IStorageItem item)
        {
            if (item is StorageFile file)
            {
                if (file == null) return null;

                string fileType = file.FileType.ToUpper();
                switch (fileType)
                {
                    case ".JPG":
                    case ".JPEG":
                    case ".PNG":
                    case ".GIF":
                    case ".BMP":
                        StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
                        return copyFile;
                }
            }
            return null;
        }
      
        
        #endregion


        #region Exists


        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExistsInLocalFolder(string fileName) => await FileUtil.IsFileExists(fileName, ApplicationData.Current.LocalFolder);

        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExistsInTemporaryFolder(string fileName) => await FileUtil.IsFileExists(fileName, ApplicationData.Current.TemporaryFolder);

        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <param name="folder"> The folder. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExists(string fileName, StorageFolder folder)
        {
            IStorageItem item = await folder.TryGetItemAsync(fileName);
            return item != null;
        }


        #endregion

    }
}