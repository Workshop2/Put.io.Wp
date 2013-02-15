using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Put.io.Core.Common;
using Put.io.Core.InvokeSynchronising;
using Put.io.Core.Models;
using Put.io.Core.Extensions;
using Put.io.Core.ProgressTracking;
using Put.io.Core.Storage;
using System.Linq;
using RestSharp;

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
                    new FileViewModel{ File = new File{FileID = 6234, ContentType = ContentType.Image, Mp4Available = true, Name = "Test Item4", ParentID = 1, ScreenShot = null, Size = 6542}},
                    new FileViewModel{ File = new File{FileID = 12531, ContentType = ContentType.Other, Mp4Available = false, Name = "Test Item5", ParentID = 1, ScreenShot = null, Size = 123677543}}
                };
            }
        }

        public FileCollectionViewModel(ProgressTracker tracker, ISettingsRepository settings, IPropertyChangedInvoke invoker)
            : base(invoker)
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
                AllFiles = OrderCollection(response.Data.ToModelList(Invoker, Settings, ProgressTracker));

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
                file.Children = OrderCollection(response.Data.ToModelList(Invoker, Settings, ProgressTracker));
                CurrentFileList = file.Children;

                ProgressTracker.CompleteTransaction(transactionID);
            });
        }

        /// <summary>
        /// Orders a collection by directories first, then by name
        /// </summary>
        private static ObservableCollection<FileViewModel> OrderCollection(IEnumerable<FileViewModel> collection)
        {
            var result = collection
                .OrderBy(x => x.File.ContentType != ContentType.Directory)
                .ThenBy(x => x.File.Name)
                .ToObservableCollection();

            return result;
        }

        /// <summary>
        /// This will usually hit when pressing the back button
        /// </summary>
        /// <returns>Has the event been handled?</returns>
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

        /// <summary>
        /// Refreshes the current view of files
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        /// Refreshes the current view of files
        /// </summary>
        /// <param name="allFiles">Clears down all files to be refreshed</param>
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

        private void UpdateCurrentPath()
        {
            if (CurrentFileList == null || !CurrentFileList.Any())
            {
                CurrentPath = string.Empty;
                return;
            }

            var first = CurrentFileList.First();

            CurrentPath = first.Path();
        }

        public void GetMp4Status(FileViewModel context, Action<Mp4Status, int> action)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);
            var transaction = ProgressTracker.StartNewTransaction();

            rester.Mp4Status(context.File.FileID, response =>
            {
                ProgressTracker.CompleteTransaction(transaction);
                action(response.Data.ToMp4Status(), response.Data.mp4.percent_done);
            });
        }

        public void GetMp4Url(FileViewModel context, Action<Uri> action)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);
            var transaction = ProgressTracker.StartNewTransaction();

            rester.StreamMp4(context.File.FileID, response =>
            {
                ProgressTracker.CompleteTransaction(transaction);
                action(response.ResponseUri);
            });
        }

        public void ConvertToMp4(FileViewModel context, Action action)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);
            var transaction = ProgressTracker.StartNewTransaction();

            rester.FileToMp4(context.File.FileID, response =>
            {
                ProgressTracker.CompleteTransaction(transaction);
                action();
            });
        }

        public void ConvertToMp4(List<FileViewModel> selected)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);

            foreach (var file in selected.Where(x => x.File.ContentType == ContentType.Video && !x.File.Name.EndsWith("mp4", StringComparison.InvariantCultureIgnoreCase)))
            {
                var fileID = file.File.FileID;
                var transaction = ProgressTracker.StartNewTransaction();

                rester.Mp4Status(fileID, response =>
                {
                    if (response.Data.ToMp4Status() == Mp4Status.NotAvailable)
                    {
                        rester.FileToMp4(fileID, response2 => ProgressTracker.CompleteTransaction(transaction));
                    }
                    else
                        ProgressTracker.CompleteTransaction(transaction);
                });
            }
        }

        public void GetStreamUrl(FileViewModel context, Action<Uri> action)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);
            var transaction = ProgressTracker.StartNewTransaction();

            rester.StreamFile(context.File.FileID, response =>
            {
                ProgressTracker.CompleteTransaction(transaction);
                action(response.ResponseUri);
            });
        }

        public void DeleteFiles(List<FileViewModel> selected)
        {
            var rester = new Api.Rest.Files(Settings.ApiKey);
            var transaction = ProgressTracker.StartNewTransaction();
            var ids = selected.Select(x => x.File.FileID);

            rester.DeleteFiles(ids, response =>
            {
                ProgressTracker.CompleteTransaction(transaction);

                if (response.ResponseStatus == ResponseStatus.Completed)
                    Refresh();
            });
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

                UpdateCurrentPath();
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
            get { return _restApi ?? (_restApi = new Api.Rest.Files(Settings.ApiKey)); }
        }

        private string _currentPath;
        public string CurrentPath
        {
            get { return _currentPath; }
            set
            {
                if (_currentPath == value) return;

                _currentPath = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}