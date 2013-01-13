using System;
using System.Globalization;
using Put.io.Core.Common;
using Put.io.Core.Extensions;
using Put.io.Core.Models;

namespace Put.io.Core.ViewModels
{
    public class TransferViewModel : ViewModelBase
    {
        public TransferViewModel()
        {
            PreviousStatus = null;
        }

        #region DynamicUpdating

        private void SubTransferPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var name = e.PropertyName;
            if (string.IsNullOrEmpty(name))
                return;

            if (name.Equals("PercentComplete", StringComparison.InvariantCultureIgnoreCase))
            {
                UpdatePercentString();
                return;
            }

            if (name.Equals("TimeRemaining", StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateFurtherInformation();
                return;
            }

            if (name.Equals("Status", StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateAllDynamicTextFields();
                return;
            }

            if (name.Equals("DownSpeed", StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateTransferSpeed();
                return;
            }
        }

        private StatusType? PreviousStatus { get; set; }
        private void UpdateAllDynamicTextFields()
        {
            UpdatePercentString();
            UpdateFurtherInformation();
            UpdateCancelText();
            UpdateOpenState();
            UpdateTransferSpeed();
        }

        private void UpdateTransferSpeed()
        {
            if (Transfer == null || Transfer.Status != StatusType.Downloading)
            {
                TransferSpeed = string.Empty;
                return;
            }

            TransferSpeed = Transfer.DownSpeed.ToTransferSpeed();
        }

        private void UpdateOpenState()
        {
            if (Transfer == null)
                return;

            if (Transfer.Status == PreviousStatus)
                return;

            if (Transfer.Status == StatusType.Downloading)
            {
                IsOpen = true;
            }

            PreviousStatus = Transfer.Status;
        }

        private void UpdateFurtherInformation()
        {
            if (Transfer == null)
            {
                FurtherInformation = string.Empty;
                return;
            }

            var status = Transfer.Status;
            var timeRemaining = Transfer.TimeRemaining;

            if (status == StatusType.Downloading)
            {
                var timeSpan = new TimeSpan(0, 0, timeRemaining);
                FurtherInformation = timeSpan.ToReadableString();
                return;
            }

            if (status == StatusType.InQueue)
            {
                FurtherInformation = "In queue, waiting for download to start...";
                return;
            }

            if (status == StatusType.Completed)
            {
                FurtherInformation = "Download complete.";
                return;
            }

            if (status == StatusType.Seeding)
            {
                FurtherInformation = "Seeding with other users.";
                return;
            }

            if (status == StatusType.Completing)
            {
                FurtherInformation = "Completing download...";
                return;
            }

            FurtherInformation = string.Empty;
        }

        private void UpdatePercentString()
        {
            if (Transfer == null || Transfer.Status != StatusType.Downloading)
            {
                PercentCompleteString = string.Empty;
                return;
            }

            PercentCompleteString = Transfer.PercentComplete.ToString(CultureInfo.InvariantCulture) + CultureInfo.CurrentUICulture.NumberFormat.PercentSymbol;
        }

        private void UpdateCancelText()
        {
            if (Transfer == null)
            {
                CancelText = string.Empty;
                return;
            }

            switch (Transfer.Status)
            {
                case StatusType.Seeding:
                    CancelText = "Stop seeding";
                    break;
                case StatusType.Completed:
                    CancelText = "Clear finished transfer";
                    break;
                default:
                    CancelText = "Cancel transfer";
                    break;
            }
        }

        #endregion

        #region Properties

        private Transfer _transfer;
        public Transfer Transfer
        {
            get { return _transfer; }
            set
            {
                if (_transfer == value) return;

                if (value != null)
                {
                    if (value.Invoker == null)
                        value.Invoker = Invoker;

                    value.PropertyChanged += SubTransferPropertyChanged;
                }

                if (_transfer != null)
                {
                    _transfer.PropertyChanged -= SubTransferPropertyChanged;
                }

                _transfer = value;
                OnPropertyChanged();
                UpdateAllDynamicTextFields();
            }
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value) return;

                _isOpen = value;
                OnPropertyChanged();
            }
        }

        private string _percentCompleteString;
        public string PercentCompleteString
        {
            get { return _percentCompleteString; }
            set
            {
                if (_percentCompleteString == value) return;

                _percentCompleteString = value;
                OnPropertyChanged();
            }
        }

        private string _furtherInformation;
        public string FurtherInformation
        {
            get { return _furtherInformation; }
            set
            {
                if (_furtherInformation == value) return;

                _furtherInformation = value;
                OnPropertyChanged();
            }
        }

        private string _cancelText;
        public string CancelText
        {
            get { return _cancelText; }
            set
            {
                if (_cancelText == value || string.IsNullOrEmpty(value))
                    return;

                _cancelText = value;
                OnPropertyChanged();
            }
        }

        private string _transferSpeed;
        public string TransferSpeed
        {
            get { return _transferSpeed; }
            set
            {
                if (_transferSpeed == value) return;

                _transferSpeed = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}