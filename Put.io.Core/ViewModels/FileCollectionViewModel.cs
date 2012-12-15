using System.Collections.ObjectModel;
using Put.io.Core.Common;
using Put.io.Core.Models;
using Put.io.Core.Extensions;

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

        #region Methods

        private Api.Rest.Files _restApi;
        private Api.Rest.Files RestApi
        {
            //TODO: Use proper API key
            get { return _restApi ?? (_restApi = new Api.Rest.Files("PUTIO_KEY")); }
        }

        protected override void OnLoadData()
        {
            if (IsInDesignMode)
                return;

            RestApi.ListFiles(null, response =>
            {
                //Store these down as our root items
                AllFiles = response.Data.ToModelList().ToObservableCollection();

                //These root items will then be displayed as default
                CurrentFileList = AllFiles;
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

            RestApi.ListFiles(file.File.FileID, response =>
            {
                file.Children = response.Data.ToModelList().ToObservableCollection();
                CurrentFileList = file.Children;                           
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

        #endregion

        #region Properties
        private ObservableCollection<FileViewModel> AllFiles { get; set; }

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
        #endregion
    }
}