using System.ComponentModel;
using System.Runtime.CompilerServices;
using Put.io.Core.Annotations;

namespace Put.io.Core.Models
{
    public class File : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private int _fileID;
        public int FileID
        {
            get { return _fileID; }

            set
            {
                if (_fileID == value) return;

                _fileID = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}