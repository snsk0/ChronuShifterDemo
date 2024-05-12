using System;

using RxUtils;

namespace Animation.Player
{
    public interface IAnimationPlayer<T>
    {
        //アニメーションを再生
        public void PlayAnimation(AnimationParameter<T> animationParameter);


        //アニメーションの再生終了
        public void StopAnimation(T type);


        //アニメーション終了通知
        public IObservable<Unit> endAnimationObservable { get; }
    }
}
