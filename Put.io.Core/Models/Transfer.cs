using System;
using System.Globalization;
using Put.io.Core.Common;
using Put.io.Core.Extensions;

namespace Put.io.Core.Models
{
    public class Transfer : ViewModelBase
    {
        private void UpdateFurtherInformation()
        {
            if (Status == StatusType.Downloading)
            {
                var timeSpan = new TimeSpan(0, 0, TimeRemaining);
                FurtherInformation = timeSpan.ToReadableString();
                return;
            }

            if (Status == StatusType.InQueue)
            {
                FurtherInformation = "In queue, waiting for download to start...";
                return;
            }

            if (Status == StatusType.Completed)
            {
                FurtherInformation = "Download complete.";
                return;
            }

            if (Status == StatusType.Seeding)
            {
                FurtherInformation = "Seeding with other users.";
                return;
            }

            if (Status == StatusType.Completing)
            {
                FurtherInformation = "Completing download...";
                return;
            }

            FurtherInformation = string.Empty;
        }

        private void UpdateCancelText()
        {
            switch (Status)
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

        public void UpdateAllDynamicFields()
        {
            UpdateFurtherInformation();
            UpdateCancelText();
        }

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

                PercentCompleteString = _percentComplete.ToString(CultureInfo.InvariantCulture) + CultureInfo.CurrentUICulture.NumberFormat.PercentSymbol;
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
                UpdateCancelText();
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

        private int _timeRemaining;
        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set
            {
                UpdateFurtherInformation();

                if (_timeRemaining == value) return;

                _timeRemaining = value;
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
        #endregion
    }
}