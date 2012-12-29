using Put.io.Core.Common;
using Put.io.Core.Models;

namespace Put.io.Core.ViewModels
{
    public class TransferViewModel : ViewModelBase
    {
        private Transfer _transfer;
        public Transfer Transfer
        {
            get { return _transfer; }
            set
            {
                if (_transfer == value) return;

                if (_transfer != null)
                    _transfer.Invoker = Invoker;

                _transfer = value;
                OnPropertyChanged();
            }
        }
    }
}