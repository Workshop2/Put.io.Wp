using Put.io.Api.UrlHelper;
using Put.io.Core.Common;
using Put.io.Core.ProgressTracking;

namespace Put.io.Core.ViewModels
{
    public delegate void WorkingStatusChangedHandler(bool isWorking);

    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                _fileCollection = new FileCollectionViewModel();
                _transferCollection = new TransferCollectionViewModel();
            }
            else
            {
                Tracker = new ProgressTracker();
                Tracker.OnProgressChanged += Tracker_OnProgressChanged;

                _fileCollection = new FileCollectionViewModel(Tracker);
                _transferCollection = new TransferCollectionViewModel(Tracker);
            }
        }

        protected override void OnLoadData()
        {
            _fileCollection.LoadData();
            _transferCollection.LoadData();
        }

        public void SelectFile(FileViewModel selected)
        {
            if (selected.IsExpandable)
            {
                FileCollection.SelectedFile = selected;
                FileCollection.ExpandFile(selected);
                return;
            }

            //TODO: IsOpenable

        }

        #region Properties
        private ProgressTracker Tracker { get; set; }

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

        private IUrlHelper _urlSetup;
        private IUrlHelper UrlSetup
        {
            get { return _urlSetup ?? (_urlSetup = new UrlHelperFactory().GetUrlDetails()); }
        }

        public string AuthenticateUrl()
        {
            return UrlSetup.AuthenticateUrl();
        }
        #endregion

        #region Events

        public event WorkingStatusChangedHandler OnWorkingStatusChanged;

        private void WorkingStatusChanged(bool isWorking)
        {
            if (OnWorkingStatusChanged != null)
                OnWorkingStatusChanged(isWorking);
        }

        #endregion

        #region ProgressMonitor
        
        private void Tracker_OnProgressChanged(bool isWorking)
        {
            WorkingStatusChanged(isWorking);
        }


        #endregion
    }
}