using System;

namespace Put.io.Core.InvokeSynchronising
{
    public interface IPropertyChangedInvoke
    {
        bool RequiresInvoke();
        void HandleCall(Action<string> raisePropertyChanged, string propertyName);
        void HandleCall(Action toCall);
    }
}