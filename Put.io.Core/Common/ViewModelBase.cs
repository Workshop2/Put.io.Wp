using System.ComponentModel;
using System.Runtime.CompilerServices;
using Put.io.Core.Annotations;
using Put.io.Core.InvokeSynchronising;

namespace Put.io.Core.Common
{
    /// <summary>
    /// Wraps up the MVVM Lite implementation of ViewModelBase with the ability to detect property names automatically and with added lovelynous of cross thread handling
    /// </summary>
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public bool IsDataLoaded { get; private set; }
        public IPropertyChangedInvoke Invoker { get; set; }

        protected ViewModelBase()
        {
        }

        protected ViewModelBase(IPropertyChangedInvoke invokeHandler)
        {
            Invoker = invokeHandler;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (Invoker != null)
            {
                Invoker.HandleCall(RaisePropertyChanged, propertyName);
                return;
            }

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