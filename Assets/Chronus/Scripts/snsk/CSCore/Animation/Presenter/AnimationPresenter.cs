using System;
using System.Collections.Generic;

using RxUtils;

using Animation.Player;
using Animation.Triggerer;

namespace Animation.Presenter
{
    public class AnimationPresenter<T>
    {
        //フィールド
        private readonly IAnimationPlayer<T> animationPlayer;   //アニメーション再生機
        private readonly List<IDisposable> disposableList;      //Dispose対象


        //キャッシュ
        private INonLoopAnimationTriggable<T> lastTrigger;


        //コンストラクタ
        public AnimationPresenter(IAnimationPlayer<T> animationPlayer)
        {
            this.animationPlayer = animationPlayer;

            disposableList = new List<IDisposable>();


            //アニメーションプレイヤーの終了通知を送信
            disposableList.Add(animationPlayer.endAnimationObservable.Subscribe(_ =>
            {
                lastTrigger.OnEndAnimation();       //終了を通知
                lastTrigger = null;                 //キャッシュを破棄
            }));
        }


        //デストラクタ
        ~AnimationPresenter()
        {
            disposableList.Clear();
        }




        //アニメーショントリガーを受け取って再生機に渡す
        public void RegisterAnimationTriggable(IAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.triggableObservable.Subscribe(inputData => animationPlayer.PlayAnimation(inputData)));
        }


        //再生の終了をトリガー側に通知する
        public void RegisterAnimationTriggable(INonLoopAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.triggableObservable.Subscribe(_ => lastTrigger = animationTriggable));

            RegisterAnimationTriggable((IAnimationTriggable<T>)animationTriggable);
        }


        //終了をPlayerに通知
        public void RegisterAnimationTriggable(ILoopAnimationTriggable<T> animationTriggable)
        {
            disposableList.Add(animationTriggable.stopTriggableObserbable.Subscribe(type => animationPlayer.StopAnimation(type)));

            RegisterAnimationTriggable((IAnimationTriggable<T>)animationTriggable);
        }
    }
}
