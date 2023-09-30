using System;

namespace Animation.Triggerer
{
    public interface ILoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //�I�������邽�߂̃g���K�[
        public IObservable<T> stopTriggableObserbable { get; }
    }
}
