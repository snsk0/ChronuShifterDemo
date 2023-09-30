using System;

using UnityEngine;
using UniRx;

using Player.Behaviours;
using Player.Presenter;
using Damagable;
using Animation;
using Animation.Triggerer;

using UnityView.Player.Animation;
using UnityView.Player.Parameter;


namespace UnityView.Player.Behaviours
{
    public class PlayerDamagable : MonoBehaviour, IDamagable, IPlayerDamageReaction, IDamagableEventSender, INonLoopAnimationTriggable<PlayerAnimationType>
    {
        //コンポーネント
        [SerializeField] private PlayerHealth health;
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded playerOnGrounded;
        private IDropper dropper;


        //ダメージ中かどうか
        public bool isDamaging { get; private set; }


        //無敵中か
        public bool isInvincible { get; private set; }


        //アニメーションイベント発行
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;



        //ダメージイベント発行
        public Subject<Damage> onDamageSubject = new Subject<Damage>();
        public IObservable<Damage> onDamageObservable => onDamageSubject;





        //モーション終了時関数
        void INonLoopAnimationTriggable<PlayerAnimationType>.OnEndAnimation()
        {
            isDamaging = false;
        }


        //ダメージ関数
        public void Damage(Damage damage)
        {
            isDamaging = true;
            health.Damage(damage.damage);

            //仮コード slipようにアニメーションを再生しないようにする
            if (damage.knock != 0)
            {
                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Damage, 0));
                rigidbody.velocity = Vector3.zero;
                dropper.Drop();
                //rigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
            }

            /*
            if (playerOnGrounded.IsOnGrounded())
            {
                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Damage, 0));
            }
            else
            {
                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.DamageFall, 0));
            }*/

            onDamageSubject.OnNext(damage);
        }



        //初期化
        private void Awake()
        {
            playerOnGrounded = GetComponent<IPlayerOnGrounded>();
            dropper = GetComponent<IDropper>();
        }

        //Destoryで破棄
        private void OnDestroy()
        {
            onDamageSubject.Dispose();
        }
    }
}
