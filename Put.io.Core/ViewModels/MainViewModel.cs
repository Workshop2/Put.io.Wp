using System;
using System.IO.IsolatedStorage;
using GalaSoft.MvvmLight.Ioc;
using Put.io.Api.UrlHelper;
using Put.io.Core.Common;
using Put.io.Core.InvokeSynchronising;
using Put.io.Core.ProgressTracking;
using Put.io.Core.Storage;

namespace Put.io.Core.ViewModels
{
    public delegate void WorkingStatusChangedHandler(bool isWorking);
    public delegate void OpenFilePopupHandler(FileViewModel file, ProgressTracker tracker);

    public class MainViewModel : ViewModelBase
    {
        #region Constructors
        [PreferredConstructor]
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                _fileCollection = new FileCollectionViewModel();
                _transferCollection = new TransferCollectionViewModel();
                InvalidApiKey = true;
            }
        }

        public MainViewModel(IPropertyChangedInvoke invokeDelegate)
            : this()
        {
            Invoker = invokeDelegate;

            Tracker = new ProgressTracker();
            Tracker.OnProgressChanged += Tracker_OnProgressChanged;

            Settings = new SettingsRepository(IsolatedStorageSettings.ApplicationSettings);

            _fileCollection = new FileCollectionViewModel(Tracker, Settings, Invoker);
            _transferCollection = new TransferCollectionViewModel(Tracker, Settings, Invoker);

            ValidateKey();
        }
        #endregion

        private void ValidateKey()
        {
            InvalidApiKey = string.IsNullOrEmpty(Settings.ApiKey);
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

            if (selected.IsOpenable)
            {
                FileCollection.SelectedFile = selected;
                OpenFilePopup(selected);
                return;
            }

        }

        public void SelectTransfer(TransferViewModel selected)
        {
            selected.IsOpen = !selected.IsOpen;
        }

        #region Properties
        private ProgressTracker Tracker { get; set; }
        private ISettingsRepository Settings { get; set; }

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

        public Uri AuthenticateUrl
        {
            get { return new Uri(UrlSetup.AuthenticateUrl(), UriKind.Absolute); }
        }

        private bool _invalidApiKey;
        public bool InvalidApiKey
        {
            get { return _invalidApiKey; }
            set
            {
                if (_invalidApiKey == value) return;

                _invalidApiKey = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events

        public event WorkingStatusChangedHandler OnWorkingStatusChanged;

        private void WorkingStatusChanged(bool isWorking)
        {
            if (OnWorkingStatusChanged != null)
                OnWorkingStatusChanged(isWorking);
        }

        public event OpenFilePopupHandler OnOpenFilePopup;

        private void OpenFilePopup(FileViewModel file)
        {
            if (OnOpenFilePopup != null)
                OnOpenFilePopup(file, Tracker);
        }
        #endregion

        #region ProgressMonitor

        private void Tracker_OnProgressChanged(bool isWorking)
        {
            WorkingStatusChanged(isWorking);
        }


        #endregion

        #region SettingsManagement
        public void ChangeKey(string apikey)
        {
            Settings.ApiKey = apikey;
            ValidateKey();

            FileCollection.Refresh(true);
            TransferCollection.Refresh();
        }
        #endregion

    }
}