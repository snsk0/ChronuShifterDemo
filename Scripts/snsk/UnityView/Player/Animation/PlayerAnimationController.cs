using System;

using UnityEngine;
using UniRx;

using Animation;
using Animation.Player;

namespace UnityView.Player.Animation
{
    public class PlayerAnimationController : MonoBehaviour, IAnimationPlayer<PlayerAnimationType>
    {
        //�A�j���[�^�[
        [SerializeField] private Animator animator;


        //�A�j���[�V�����I���ʒm�p
        private Subject<RxUtils.Unit> onEndAnimationSubject = new Subject<RxUtils.Unit>();
        public IObservable<RxUtils.Unit> endAnimationObservable => onEndAnimationSubject;


        //�I���\��
        private string endState;
        private int animeHash;

        //�ړ�
        [SerializeField] private AnimationCurve moveAnimationCurve;
        [SerializeField] private float moveAnimationBlendTime;
        private int sprintSpeedDir = 0;
        private float blendXCount = 0;


        //�A�j���[�V�����Đ��֐�
        public void PlayAnimation(AnimationParameter<PlayerAnimationType> parameter)
        {
            switch (parameter.type)
            {
                case PlayerAnimationType.Walk:
                    animator.SetBool("move", true);
                    animator.SetFloat("moveSpeed", parameter.speed);
                    sprintSpeedDir = -1;
                    break;

                case PlayerAnimationType.Run:
                    animator.SetBool("move", true);
                    animator.SetFloat("moveSpeed", parameter.speed);
                    sprintSpeedDir = 1;
                    break;

                case PlayerAnimationType.Airborne:
                    animator.SetBool("fall", true);
                    animator.SetFloat("FallSpeed", parameter.blend.x);
                    break;
                case PlayerAnimationType.Damage:
                    animator.SetTrigger("Damage");
                    endState = "Damage";
                    break;

                case PlayerAnimationType.HoldUp:
                    animator.SetTrigger("PickUp");
                    endState = "PickUp";
                    break;

                case PlayerAnimationType.HoldDown:
                    animator.SetTrigger("PickDown");
                    endState = "PickDown";
                    break;
            }
        }





        //�A�j���[�V�����I��
        public void StopAnimation(PlayerAnimationType type)
        {
            switch (type)
            {
                case PlayerAnimationType.Walk:
                    animator.SetBool("move", false);
                    sprintSpeedDir = 0;
                    break;

                case PlayerAnimationType.Run:
                    animator.SetBool("move", false);
                    sprintSpeedDir = 0;
                    break;

                case PlayerAnimationType.Airborne:
                    animator.SetBool("fall", false);
                    break;
            }
        }



        //Update
        public void Update()
        {
            MoveAnimationUpdate();

            //�\�񂵂Ă������̂����ۂɍĐ����ꂽ��
            if (endState != null && animator.GetCurrentAnimatorStateInfo(0).IsName(endState))
            {
                endState = null;
                animeHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                return;
            }


            //�ʃA�j���[�V�����ɂȂ�����I��
            if(animeHash != 0 && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != animeHash)
            {
                animeHash = 0;
                onEndAnimationSubject.OnNext(new RxUtils.Unit());   //�I���ʒm
            }
        }


        private void MoveAnimationUpdate()
        {
            if (sprintSpeedDir > 0)
            {
                float blend = animator.GetFloat("sprintBlend");
                if (blend >= 1) return;
                else
                {
                    blendXCount += Time.deltaTime;
                    if (blendXCount > moveAnimationBlendTime) blendXCount = moveAnimationBlendTime;
                    blend = moveAnimationCurve.Evaluate(blendXCount / moveAnimationBlendTime);
                    animator.SetFloat("sprintBlend", blend);
                }
            }
            else if(sprintSpeedDir < 0)
            {
                float blend = animator.GetFloat("sprintBlend");
                if (blend <= 0) return;
                else
                {
                    blendXCount -= Time.deltaTime;
                    if (blendXCount < 0) blendXCount = 0;
                    blend = moveAnimationCurve.Evaluate(blendXCount / moveAnimationBlendTime);
                    animator.SetFloat("sprintBlend", blend);
                }
            }
            else
            {
                blendXCount = 0;
                animator.SetFloat("sprintBlend", 0);
            }
        }
    }
}