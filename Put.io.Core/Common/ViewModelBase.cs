using System.ComponentModel;
using System.Runtime.CompilerServices;
using Put.io.Core.Annotations;

namespace Put.io.Core.Common
{
    /// <summary>
    /// Wraps up the MVVM Lite implementation of ViewModelBase with the ability to detect property names automatically
    /// </summary>
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public bool IsDataLoaded { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }

        public void LoadData()
        {
            if (IsDataLoaded)
                return;

            OnLoadData();

            IsDataLoaded = true;
        }

        /// <summary>
        /// Override this method to initialise the object. This will only occur once
        /// </summary>
        protected virtual void OnLoadData()
        {
        }
    }
}