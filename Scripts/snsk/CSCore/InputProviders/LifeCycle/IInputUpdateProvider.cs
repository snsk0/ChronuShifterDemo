using System;
using RxUtils;

namespace InputProviders.LifeCycle
{
    public interface IInputUpdateProvider
    {
        public IObservable<Unit> onUpdate { get; } 
    }
}
