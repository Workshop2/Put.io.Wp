using Put.io.Core.Common;

namespace Put.io.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _fileCollection = new FileCollectionViewModel();
            _transferCollection = new TransferCollectionViewModel();
        }

        protected override void OnLoadData()
        {
            _fileCollection.LoadData();
            _transferCollection.LoadData();
        }

        #region Properties
        private FileCollectionViewModel _fileCollection;
        public FileCollectionViewModel FileCollection
        {
            get { return _fileCollection; }
            set
            {
                if (_fileCollection == value) return;

                _fileCollection = value;
                OnPropertyChanged();
            }
        }

        private TransferCollectionViewModel _transferCollection;
        public TransferCollectionViewModel TransferCollection
        {
            get { return _transferCollection; }
            set
            {
                if (_transferCollection == value) return;

                _transferCollection = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}