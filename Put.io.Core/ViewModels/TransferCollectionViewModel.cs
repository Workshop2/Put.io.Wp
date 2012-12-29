﻿using System.Collections.ObjectModel;
using Put.io.Api.Rest;
using Put.io.Core.Common;
using Put.io.Core.Models;
using Put.io.Core.Extensions;
using Put.io.Core.ProgressTracking;
using Put.io.Core.Storage;
using Put.io.Core.Transfers;

namespace Put.io.Core.ViewModels
{
    public class TransferCollectionViewModel : ViewModelBase
    {
        public TransferCollectionViewModel()
        {
            if (IsInDesignMode)
            {
                _transfers = new ObservableCollection<TransferViewModel>
                {
                    new TransferViewModel{Transfer = new Transfer{Name = "Transfer 1", PercentComplete = 55, Size = 4324233, TransferID = 1}},
                    new TransferViewModel{Transfer = new Transfer{Name = "Transfer 2", PercentComplete = 64, Size = 3432423, TransferID = 2}},
                    new TransferViewModel{Transfer = new Transfer{Name = "Transfer 3", PercentComplete = 24, Size = 6422, TransferID = 3}},
                    new TransferViewModel{Transfer = new Transfer{Name = "Transfer 4", PercentComplete = 99, Size = 22453, TransferID = 4}},
                    new TransferViewModel{Transfer = new Transfer{Name = "Transfer 5", PercentComplete = 23, Size = 3333443, TransferID = 5}}
                };
            }
        }

        public TransferCollectionViewModel(ProgressTracker tracker, ISettingsRepository settings)
            : this()
        {
            ProgressTracker = tracker;
            Settings = settings;
        }

        protected override void OnLoadData()
        {
            if (IsInDesignMode || string.IsNullOrEmpty(Settings.ApiKey))
                return;

            var rester = new Api.Rest.Transfers(Settings.ApiKey);
            
            rester.ListTransfers(response =>
            {
                Transfers = response.Data.ToModelList().ToObservableCollection();

                if (Updater != null)
                    Updater.Dispose();

                Updater = new AutonomousUpdater(Transfers, Settings);
            });
        }

        public void Refresh()
        {
            if (Transfers != null)
                Transfers.Clear();

            SelectedTransfer = null;
            OnLoadData();
        }

        #region Properties
        private ProgressTracker ProgressTracker { get; set; }
        private ISettingsRepository Settings { get; set; }
        private AutonomousUpdater Updater { get; set; }

        private ObservableCollection<TransferViewModel> _transfers;
        public ObservableCollection<TransferViewModel> Transfers
        {
            get { return _transfers; }
            set
            {
                if (_transfers == value) return;

                _transfers = value;
                OnPropertyChanged();
            }
        }

        private TransferViewModel _selectedTransfer;
        public TransferViewModel SelectedTransfer
        {
            get { return _selectedTransfer; }
            set
            {
                if (_selectedTransfer == value) return;

                _selectedTransfer = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}