using System;
using System.Collections.Generic;

using RxUtils;

using Animation.Player;
using Animation.Triggerer;

namespace Animation.Presenter
{
    public class AnimationPresenter<T>
    {
        //�t�B�[���h
        private readonly IAnimationPlayer<T> animationPlayer;   //�A�j���[�V�����Đ��@
        private readonly List<IDisposable> disposableList;      //Dispose�Ώ�


        //�L���b�V��
        private INonLoopAnimationTriggable<T> lastTrigger;


        //�R���X�g���N�^
        public AnimationPresenter(IAnimationPlayer<T> animationPlayer)
        {
            this.animationPlayer = animationPlayer;

            disposableList = new List<IDisposable>();


            //�A�j���[�V�����v���C���[�̏I���ʒm�𑗐M
            disposableList.Add(animationPlayer.endAnimationObservable.Subscribe(_ =>
            {
                lastTrigger.OnEndAnimation();       //�I����ʒm
                lastTrigger = null;                 //�L���b�V����j��
            }));
        }


        //�f�X�g���N�^
        ~AnimationPresenter()
        {
            disposableList.Clear();
        }




        //�A�j���[�V�����g���K�[���󂯎���čĐ��@�ɓn��
        public void RegisterAnimationTriggable(IAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.triggableObservable.Subscribe(inputData => animationPlayer.PlayAnimation(inputData)));
        }


        //�Đ��̏I�����g���K�[���ɒʒm����
        public void RegisterAnimationTriggable(INonLoopAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.triggableObservable.Subscribe(_ => lastTrigger = animationTriggable));

            RegisterAnimationTriggable((IAnimationTriggable<T>)animationTriggable);
        }


        //�I����Player�ɒʒm
        public void RegisterAnimationTriggable(ILoopAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.stopTriggableObserbable.Subscribe(type => animationPlayer.StopAnimation(type)));

            RegisterAnimationTriggable((IAnimationTriggable<T>)animationTriggable);
        }
    }
}
