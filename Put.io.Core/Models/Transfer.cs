using System;
using System.Globalization;
using Put.io.Core.Common;
using Put.io.Core.Extensions;

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
                if (_timeRemaining == value) return;

                _timeRemaining = value;
                OnPropertyChanged();

                SetTimeRemainingString();
            }
        }

        private string _timeRemainingString;
        public string TimeRemainingString
        {
            get { return _timeRemainingString; }
            set
            {
                if (_timeRemainingString == value) return;

                _timeRemainingString = value;
                OnPropertyChanged();
            }
        }

        private void SetTimeRemainingString()
        {
            var timeSpan = new TimeSpan(0, 0, TimeRemaining);
            TimeRemainingString = timeSpan.ToReadableString();
        }
        #endregion
    }
}