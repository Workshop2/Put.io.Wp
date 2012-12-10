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
            if (IsInDesignMode)
            {
                _files = new ObservableCollection<FileViewModel>
                {
                    new FileViewModel{ File = new File{FileID = 12345, ContentType = ContentType.Directory, Mp4Available = false, Name = "Test Item", ParentID = 1, ScreenShot = null, Size = 96349382}},
                    new FileViewModel{ File = new File{FileID = 5324, ContentType = ContentType.Video, Mp4Available = true, Name = "Test Item2", ParentID = 1, ScreenShot = null, Size = 5235}},
                    new FileViewModel{ File = new File{FileID = 2342, ContentType = ContentType.Music, Mp4Available = false, Name = "Test Item3", ParentID = 1, ScreenShot = null, Size = 234245}},
                    new FileViewModel{ File = new File{FileID = 6234, ContentType = ContentType.Other, Mp4Available = true, Name = "Test Item4", ParentID = 1, ScreenShot = null, Size = 6542}},
                    new FileViewModel{ File = new File{FileID = 12531, ContentType = ContentType.Directory, Mp4Available = false, Name = "Test Item5", ParentID = 1, ScreenShot = null, Size = 123677543}}
                };
            }
            else
            {
                var rester = new Api.Rest.Files("PUTIO_KEY");

                rester.ListFiles(null, response =>
                {
                    Files = response.Data.ToModelList().ToObservableCollection();
                });
            }
        }


        private ObservableCollection<FileViewModel> _files;
        public ObservableCollection<FileViewModel> Files
        {
            get { return _files; }
            set
            {
                if (_files == value) return;

                _files = value;
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
    }
}