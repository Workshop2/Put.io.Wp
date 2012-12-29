﻿using System.Collections.ObjectModel;
using Put.io.Core.Common;
using Put.io.Core.Models;
using Put.io.Core.Extensions;
using Put.io.Core.ProgressTracking;
using Put.io.Core.Storage;

namespace Put.io.Core.ViewModels
{
    public class FileCollectionViewModel : ViewModelBase
    {
        public FileCollectionViewModel()
        {
            AllFiles = new ObservableCollection<FileViewModel>();

            if (IsInDesignMode)
            {
                _currentFileList = new ObservableCollection<FileViewModel>
                {
                    new FileViewModel{ File = new File{FileID = 12345, ContentType = ContentType.Directory, Mp4Available = false, Name = "Test Item", ParentID = 1, ScreenShot = null, Size = 96349382}},
                    new FileViewModel{ File = new File{FileID = 5324, ContentType = ContentType.Video, Mp4Available = true, Name = "Test Item2", ParentID = 1, ScreenShot = null, Size = 5235}},
                    new FileViewModel{ File = new File{FileID = 2342, ContentType = ContentType.Music, Mp4Available = false, Name = "Test Item3", ParentID = 1, ScreenShot = null, Size = 234245}},
                    new FileViewModel{ File = new File{FileID = 6234, ContentType = ContentType.Other, Mp4Available = true, Name = "Test Item4", ParentID = 1, ScreenShot = null, Size = 6542}},
                    new FileViewModel{ File = new File{FileID = 12531, ContentType = ContentType.Directory, Mp4Available = false, Name = "Test Item5", ParentID = 1, ScreenShot = null, Size = 123677543}}
                };
            }
        }

        public FileCollectionViewModel(ProgressTracker tracker, ISettingsRepository settings)
            : this()
        {
            ProgressTracker = tracker;
            Settings = settings;
        }

        #region Methods

        protected override void OnLoadData()
        {
            if (IsInDesignMode || string.IsNullOrEmpty(Settings.ApiKey))
                return;

            var transactionID = ProgressTracker.StartNewTransaction();

            RestApi.ListFiles(null, response =>
            {
                //Store these down as our root items
                AllFiles = response.Data.ToModelList().ToObservableCollection();

                //These root items will then be displayed as default
                CurrentFileList = AllFiles;

                ProgressTracker.CompleteTransaction(transactionID);
            });
        }

        public void ExpandFile(FileViewModel file)
        {
            if (file.File.ContentType != ContentType.Directory)
                return;

            if (file.Children != null)
            {
                CurrentFileList = file.Children;
                return;
            }

            var transactionID = ProgressTracker.StartNewTransaction();

            RestApi.ListFiles(file.File.FileID, response =>
            {
                file.Children = response.Data.ToModelList().ToObservableCollection();
                CurrentFileList = file.Children;

                ProgressTracker.CompleteTransaction(transactionID);
            });
        }

        public bool NavigateUp()
        {
            if (SelectedFile != null)
            {
                SelectedFile = SelectedFile.Parent;
                CurrentFileList = SelectedFile == null ? AllFiles : SelectedFile.Children;

                return true;
            }

            return false;
        }

        public void Refresh()
        {
            Refresh(false);
        }

        public void Refresh(bool allFiles)
        {
            if (CurrentFileList != null)
                CurrentFileList.Clear();

            if (allFiles)
            {
                if (AllFiles != null)
                    AllFiles.Clear();

                SelectedFile = null;
            }

            if (SelectedFile == null)
            {
                OnLoadData();
            }
            else
            {
                SelectedFile.Children = null;

                ExpandFile(SelectedFile);
            }
        }

        #endregion

        #region Properties
        private ObservableCollection<FileViewModel> AllFiles { get; set; }
        private ProgressTracker ProgressTracker { get; set; }
        private ISettingsRepository Settings { get; set; }

        private ObservableCollection<FileViewModel> _currentFileList;
        public ObservableCollection<FileViewModel> CurrentFileList
        {
            get { return _currentFileList; }
            set
            {
                if (_currentFileList == value) return;

                _currentFileList = value;
                OnPropertyChanged();
            }
        }

        private FileViewModel _selectedFile;
        public FileViewModel SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                if (_selectedFile == value) return;

                _selectedFile = value;
                OnPropertyChanged();
            }
        }

        private Api.Rest.Files _restApi;
        private Api.Rest.Files RestApi
        {
            //TODO: Use proper API key
            get { return _restApi ?? (_restApi = new Api.Rest.Files(Settings.ApiKey)); }
        }
        #endregion
    }
}