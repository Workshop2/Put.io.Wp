using Put.io.Core.Common;

namespace Put.io.Core.Models
{
    public class Transfer : ViewModelBase
    {
        #region Properties
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

        private int _transferID;
        public int TransferID
        {
            get { return _transferID; }
            set
            {
                if (_transferID == value) return;

                _transferID = value;
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

        private int _percentComplete;
        public int PercentComplete
        {
            get { return _percentComplete; }
            set
            {
                if (_percentComplete == value) return;

                _percentComplete = value;
                OnPropertyChanged();
            }
        }

        private StatusType _status;
        public StatusType Status
        {
            get { return _status; }
            set
            {
                if (_status == value) return;

                _status = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}