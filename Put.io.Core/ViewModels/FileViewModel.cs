using System.Collections.ObjectModel;
using Put.io.Core.Common;
using Put.io.Core.Models;
using System.Linq;

namespace Put.io.Core.ViewModels
{
    public class FileViewModel : ViewModelBase
    {
        private File _file;
        public File File
        {
            get { return _file; }
            set
            {
                if (_file == value) return;

                _file = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FileViewModel> _children;
        public ObservableCollection<FileViewModel> Children
        {
            get { return _children; }
            set
            {
                if (_children == value) return;

                foreach (var fileViewModel in value)
                {
                    fileViewModel.Parent = this;
                }

                _children = value;
                OnPropertyChanged();
            }
        }

        public FileViewModel Parent { get; set; } 
    }
}