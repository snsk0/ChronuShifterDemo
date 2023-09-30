using System;

using RxUtils;

namespace Animation.Player
{
    public interface IAnimationPlayer<T>
    {
        //�A�j���[�V�������Đ�
        public void PlayAnimation(AnimationParameter<T> animationParameter);


        //�A�j���[�V�����̍Đ��I��
        public void StopAnimation(T type);


        //�A�j���[�V�����I���ʒm
        public IObservable<Unit> endAnimationObservable { get; }
    }
}
