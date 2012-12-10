using Put.io.Core.Common;
using Put.io.Core.Models;

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
    }
}