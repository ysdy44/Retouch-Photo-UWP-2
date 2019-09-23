﻿using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainPage" />. 
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel { get => App.SettingViewModel; set => App.SettingViewModel = value; }

        ObservableCollection<Photo> PhotoFileList = new ObservableCollection<Photo>();


        //Loading
        private bool IsLoading { set => this.LoadingControl.IsActive = value; }
        /// <summary> State of <see cref="MainPage"/>. </summary>
        public MainPageState State
        {
            get => this.state;
            set
            {
                if (value == MainPageState.None)
                {
                    this.InitialControl.Visibility = Visibility.Visible;

                    this.GridView.Visibility = Visibility.Collapsed;
                    this.RadiusAnimaPanel.Visibility = Visibility.Collapsed;
                    this.RadiusAnimaPanel.CenterContent = null;
                }
                else
                {
                    this.InitialControl.Visibility = Visibility.Collapsed;

                    this.GridView.Visibility = Visibility.Visible;
                    this.RadiusAnimaPanel.Visibility = Visibility.Visible;

                    switch (value)
                    {
                        case MainPageState.Main:
                            this.RadiusAnimaPanel.CenterContent = this.MainControl;
                            break;
                        //case MainPageState.Loading:
                        //break;

                        //case MainPageState.Add:
                        //break;
                        case MainPageState.Pictures:
                            this.RadiusAnimaPanel.CenterContent = this.PicturesControl;
                            break;

                        case MainPageState.Save:
                            this.RadiusAnimaPanel.CenterContent = this.SaveControl;
                            break;
                        case MainPageState.Share:
                            this.RadiusAnimaPanel.CenterContent = this.ShareControl;
                            break;

                        case MainPageState.Delete:
                            this.RadiusAnimaPanel.CenterContent = this.DeleteControl;
                            break;
                        case MainPageState.Duplicate:
                            this.RadiusAnimaPanel.CenterContent = this.DuplicateControl;
                            break;

                        //case MainPageState.Folder:
                        //break;
                        //case MainPageState.Move:
                        //break;

                        default:
                            break;
                    }
                }


                this.state = value;
            }
        }
        private MainPageState state;


        MainControl MainControl = new MainControl();

        //AddDialog AddDialog = new AddDialog();
        PicturesControl PicturesControl = new PicturesControl();

        SaveControl SaveControl = new SaveControl();
        ShareControl ShareControl = new ShareControl();

        DeleteControl DeleteControl = new DeleteControl();
        DuplicateControl DuplicateControl = new DuplicateControl();

        //FolderDialog FolderDialog = new FolderDialog();


        //@Construct
        public MainPage()
        {
            this.InitializeComponent();


            this.Loaded += async (s, e) =>
            {
                //Setting
                SettingViewModel setting = null;

                try
                {
                    setting = await SettingViewModel.CreateFromLocalFile();
                }
                catch (Exception)
                {
                }

                if (setting != null)
                {
                    this.SettingViewModel = setting;
                }

                ElementTheme theme = this.SettingViewModel.ElementTheme;
                ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);


                if (this.PhotoFileList.Count == 0)
                    this.State = MainPageState.None;//State   
                else
                    this.State = MainPageState.Main;//State      

                //Project project = new Project(1024, 1024);//Project
                //this.Frame.Navigate(typeof(DrawPage), project);//Navigate    
            };

            this.SettingButton.Tapped += (s, e) =>
            {
                this.Frame.Navigate(typeof(SettingPage));//Navigate     
            };

            this.GridView.ItemClick += (s, e) =>
            {
                if (this.State != MainPageState.None) return;

                if (e.ClickedItem is Photo photo)
                {
                    this.IsLoading = true;//Loading

                    Project project = new Project(1024, 1024);
                    this.Frame.Navigate(typeof(DrawPage), project);//Navigate     

                    this.IsLoading = false;//Loading
                }
            };

            //Initial
            this.AddButton.Tapped += async (s, e) => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);

            //Main
            this.MainControl.AddButton.Tapped += async (s, e) => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.MainControl.PicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;
            this.MainControl.SaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
            this.MainControl.ShareButton.Tapped += (s, e) => this.State = MainPageState.Share;
            this.MainControl.DeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
            this.MainControl.DuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;
            this.MainControl.FolderButton.Tapped += async (s, e) => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.MainControl.MoveButton.Tapped += (s, e) => this.State = MainPageState.Move;

            //Second
            this.MainControl.SecondAddButton.Tapped += async (s, e) => await this.AddDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.MainControl.SecondPicturesButton.Tapped += (s, e) => this.State = MainPageState.Pictures;
            this.MainControl.SecondSaveButton.Tapped += (s, e) => this.State = MainPageState.Save;
            this.MainControl.SecondShareButton.Tapped += (s, e) => this.State = MainPageState.Share;
            this.MainControl.SecondDeleteButton.Tapped += (s, e) => this.State = MainPageState.Delete;
            this.MainControl.SecondDuplicateButton.Tapped += (s, e) => this.State = MainPageState.Duplicate;
            this.MainControl.SecondFolderButton.Tapped += async (s, e) => await this.FolderDialog.ShowAsync(ContentDialogPlacement.InPlace);
            this.MainControl.SecondMoveButton.Tapped += (s, e) => this.State = MainPageState.Move;

            //Add
            this.AddDialog.PrimaryButtonClick += (sender, args) =>
            {
                this.AddDialog.Hide();

                this.IsLoading = true;//Loading

                BitmapSize pixels = this.AddDialog.Size;
                Project project = new Project((int)pixels.Width, (int)pixels.Height);//Project
                this.Frame.Navigate(typeof(DrawPage), project);//Navigate    

                this.IsLoading = false;//Loading
            };

            //Pictures
            this.PicturesControl.PhotoButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.PicturesLibrary);
            this.PicturesControl.DestopButton.Tapped += async (s, e) => await this.NewProjectFromPictures(PickerLocationId.Desktop);
            this.PicturesControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Save
            this.SaveControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Share
            this.ShareControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Delete
            this.DeleteControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Duplicate
            this.DuplicateControl.CancelButton.Tapped += (s, e) => this.State = MainPageState.Main;

            //Folder
            this.FolderDialog.PrimaryButtonClick += (s, e) =>
            {
                this.FolderDialog.Hide();

                this.IsLoading = true;//Loading

                //......

                this.IsLoading = false;//Loading
            };
        }



        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.RequestedTheme = theme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }


        private async Task NewProjectFromPictures(PickerLocationId location)
        {
            //ImageRe
            ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images
            this.ViewModel.DuplicateChecking(imageRe);

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageLayer imageLayer = new ImageLayer()
            {
                ImageRe = imageRe,
                Source = transformerSource,
                Destination = transformerSource,
            };

            //Project
            Project project = new Project(imageLayer);
            this.Frame.Navigate(typeof(DrawPage), project);//Navigate       
        }

        private async void Refresh()
        {
            this.PhotoFileList.Clear(); //Notify

            // Get files from the destination folder.
            IOrderedEnumerable<StorageFile> storageFiles = await Photo.CreatePhotoFilesFromStorageFolder(ApplicationData.Current.LocalFolder).ConfigureAwait(false);

            foreach (StorageFile storageFile in storageFiles)
            {
                // [StorageFile] --> [Photo]
                Photo photo = Photo.CreatePhoto(storageFile, ApplicationData.Current.LocalFolder.Path);

                if (photo == null) break;

                this.PhotoFileList.Add(photo); //Notify
            }
        }
    }
}