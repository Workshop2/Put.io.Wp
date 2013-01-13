using System.Collections.ObjectModel;
using Put.io.Core.Common;
using Put.io.Core.Models;
using System.Linq;

namespace Put.io.Core.ViewModels
{
    public class FileViewModel : ViewModelBase
    {
        public FileViewModel()
        {
            
        }

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

                if (value != null)
                {
                    foreach (var fileViewModel in value)
                    {
                        fileViewModel.Parent = this;
                    }
                }

                _children = value;
                OnPropertyChanged();
            }
        }

        public FileViewModel Parent { get; set; }

        public bool IsExpandable
        {
            get
            {
                return File.ContentType == ContentType.Directory;
            }
        }

        public string Path()
        {
            if (Parent == null)
                return " / ";

            var path = Parent.Path();
            path += string.Format("{0} / ", ShrinkFileName(Parent.File.Name));

            return path;
        }

        private const int MaxFileName = 20;
        private string ShrinkFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = string.Empty;

            if (name.Length > MaxFileName + 3)
            {
                name = name.Substring(0, name.Length - (name.Length - MaxFileName)) + "...";
            }

            return name;
        }
    }
}