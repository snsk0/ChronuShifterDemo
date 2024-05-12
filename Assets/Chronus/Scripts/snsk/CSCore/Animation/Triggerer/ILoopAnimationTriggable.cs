using System;

namespace Animation.Triggerer
{
    public interface ILoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //I—¹‚³‚¹‚é‚½‚ß‚ÌƒgƒŠƒK[
        public IObservable<T> stopTriggableObserbable { get; }
    }
}
