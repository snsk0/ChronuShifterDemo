using System;

namespace Animation.Triggerer
{
    public interface ILoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //終了させるためのトリガー
        public IObservable<T> stopTriggableObserbable { get; }
    }
}
