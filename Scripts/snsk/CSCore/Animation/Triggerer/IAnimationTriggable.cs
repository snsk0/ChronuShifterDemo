using System;

namespace Animation.Triggerer
{
    public interface IAnimationTriggable<T>
    {
        //�A�j���[�V�����g���K�[
        public IObservable<AnimationParameter<T>> triggableObservable { get; }
    }
}
