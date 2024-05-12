using System;

namespace Animation.Triggerer
{
    public interface IAnimationTriggable<T>
    {
        //アニメーショントリガー
        public IObservable<AnimationParameter<T>> triggableObservable { get; }
    }
}
