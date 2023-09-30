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
        //�R���|�[�l���g
        [SerializeField] private PlayerHealth health;
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded playerOnGrounded;
        private IDropper dropper;


        //�_���[�W�����ǂ���
        public bool isDamaging { get; private set; }


        //���G����
        public bool isInvincible { get; private set; }


        //�A�j���[�V�����C�x���g���s
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;



        //�_���[�W�C�x���g���s
        public Subject<Damage> onDamageSubject = new Subject<Damage>();
        public IObservable<Damage> onDamageObservable => onDamageSubject;





        //���[�V�����I�����֐�
        void INonLoopAnimationTriggable<PlayerAnimationType>.OnEndAnimation()
        {
            isDamaging = false;
        }


        //�_���[�W�֐�
        public void Damage(Damage damage)
        {
            isDamaging = true;
            health.Damage(damage.damage);

            //���R�[�h slip�悤�ɃA�j���[�V�������Đ����Ȃ��悤�ɂ���
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



        //������
        private void Awake()
        {
            playerOnGrounded = GetComponent<IPlayerOnGrounded>();
            dropper = GetComponent<IDropper>();
        }

        //Destory�Ŕj��
        private void OnDestroy()
        {
            onDamageSubject.Dispose();
        }
    }
}
