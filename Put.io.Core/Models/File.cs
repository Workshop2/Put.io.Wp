using Put.io.Core.Common;

namespace Put.io.Core.Models
{
    public class File : ViewModelBase
    {
        #region Properties

        private int _fileID;
        public int FileID
        {
            get { return _fileID; }

            set
            {
                if (_fileID == value) return;

                _fileID = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged();
            }
        }

        private string _screenshot;
        public string ScreenShot
        {
            get { return _screenshot; }
            set
            {
                if (_screenshot == value) return;

                _screenshot = value;
                OnPropertyChanged();
            }
        }

        private int _parentID;
        public int ParentID
        {
            get { return _parentID; }
            set
            {
                if (_parentID == value) return;

                _parentID = value;
                OnPropertyChanged();
            }
        }  

        private bool _mp4Available;
        public bool Mp4Available
        {
            get { return _mp4Available; }
            set
            {
                if (_mp4Available == value) return;

                _mp4Available = value;
                OnPropertyChanged();
            }
        }

        private ContentType _contentType;
        public ContentType ContentType
        {
            get { return _contentType; }
            set
            {
                if (_contentType == value) return;

                _contentType = value;
                OnPropertyChanged();
            }
        }

        private long _size;
        public long Size
        {
            get { return _size; }
            set
            {
                if (_size == value) return;

                _size = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}