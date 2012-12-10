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
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}