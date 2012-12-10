using System.Collections.ObjectModel;
using Put.io.Api.Rest;
using Put.io.Core.Common;
using Put.io.Core.Models;
using Put.io.Core.Extensions;

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
            else
            {
                var rester = new Api.Rest.Transfers("PUTIO_KEY");

                rester.ListTransfers(response =>
                {
                    Transfers = response.Data.ToModelList().ToObservableCollection();
                });
            }
        }

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
    }
}