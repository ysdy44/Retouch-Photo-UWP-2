﻿using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class MainPage : Page
    {


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainPage" />'s selected count. </summary>
        public int SelectedCount
        {
            get => (int)base.GetValue(SelectedCountProperty);
            set => base.SetValue(SelectedCountProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainPage.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedCountProperty = DependencyProperty.Register(nameof(SelectedCount), typeof(int), typeof(MainPage), new PropertyMetadata(0));


        /// <summary> Gets or sets <see cref = "MainPage" />'s selected is enable. </summary>
        public bool SelectedIsEnabled
        {
            get => (bool)base.GetValue(SelectedIsEnabledProperty);
            set => base.SetValue(SelectedIsEnabledProperty, value);
        }
        /// <summary> Identifies the <see cref = "MainPage.SelectedCount" /> dependency property. </summary>
        public static readonly DependencyProperty SelectedIsEnabledProperty = DependencyProperty.Register(nameof(SelectedIsEnabled), typeof(bool), typeof(MainPage), new PropertyMetadata(false));


        /// <summary>
        /// Refresh the selected count.
        /// </summary>
        public void RefreshSelectCount()
        {
            int count = this.Items.Count(p => p.IsSelected);
            this.SelectedCount = count;

            bool isEnable = (count != 0);
            this.SelectedIsEnabled = isEnable;
        }

        /// <summary>
        /// Load all ProjectViewItems in GridView children.
        /// </summary>
        public void LoadAllProjectViewItems()
        {
            this.MainLayout.State = (this.Items.Count == 0) ? MainPageState.Initial : MainPageState.Main;
        }


        #endregion


        /// <summary>
        /// Rename the ProjectViewItem.
        /// </summary>
        /// <param name="oldName"> The old name. </param>
        /// <param name="newName"> The new name. </param>
        public async Task RenameProjectViewItem(string oldName, string newName)
        {
            //Same name. 
            if (oldName == newName)
            {
                this.TextBoxTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Name is already occupied.
            bool hasRenamed = this.Items.Any(p => p.Name == newName);
            if (hasRenamed)
            {
                this.TextBoxTipTextBlock.Visibility = Visibility.Visible;
                return;
            }

            //Rename
            IProjectViewItem item = this.Items.First(p => p.Name == oldName);
            await FileUtil.RenameZipFolder(oldName, newName, item);

            this.HideRenameDialog();
            this.MainLayout.State = MainPageState.Main;
        }


        /// <summary>
        /// Delete all selected ProjectViewItem(s).
        /// </summary>
        /// <param name="items"> The items. </param>
        public async Task DeleteProjectViewItems(IEnumerable<IProjectViewItem> items)
        {
            foreach (IProjectViewItem item in items.ToList())
            {
                //Delete
                string name = item.Name;
                await FileUtil.DeleteZipFolder(name);

                item.Visibility = Visibility.Collapsed;
                this.Items.Remove(item);//Notify

                await Task.Delay(300);
            }
        }


        /// <summary>
        /// Duplicate all selected ProjectViewItem(s).
        /// </summary>     
        /// <param name="items"> The items. </param>
        public async Task DuplicateProjectViewItems(IEnumerable<IProjectViewItem> items)
        {
            foreach (IProjectViewItem item in items.ToList())
            {
                string oldName = item.Name;
                string newName = this.UntitledRenameByRecursive(oldName);

                StorageFolder storageFolder = await FileUtil.DuplicateZipFolder(oldName, newName);
                IProjectViewItem newItem = await FileUtil.ConstructProjectViewItem(storageFolder);
                this.Items.Add(newItem);//Notify
            }
        }


        /// <summary>
        /// Get a name that doesn't have a rename.
        /// If there are, add the number.
        /// [Untitled] --> [Untitled1]   
        /// </summary>
        /// <param name="name"> The previous name. </param>
        /// <returns> The new name. </returns>
        public string UntitledRenameByRecursive(string name)
        {
            // Is there a re-named item?
            if (this.Items.All(i => i.Name != name))
                return name;

            int num = 0;
            string newName;

            do
            {
                num++;
                newName = $"{name}{num}";
            }
            // Is there a re-named item?
            while (this.Items.Any(i => i.Name == newName));

            return newName;
        }

    }
}