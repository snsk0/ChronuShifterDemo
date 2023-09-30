using System;

using UnityEngine;
using UniRx;

using Animation;
using Animation.Player;

namespace UnityView.Player.Animation
{
    public class PlayerAnimationController : MonoBehaviour, IAnimationPlayer<PlayerAnimationType>
    {
        //アニメーター
        [SerializeField] private Animator animator;


        //アニメーション終了通知用
        private Subject<RxUtils.Unit> onEndAnimationSubject = new Subject<RxUtils.Unit>();
        public IObservable<RxUtils.Unit> endAnimationObservable => onEndAnimationSubject;


        //終了予約
        private string endState;
        private int animeHash;

        //移動
        [SerializeField] private AnimationCurve moveAnimationCurve;
        [SerializeField] private float moveAnimationBlendTime;
        private int sprintSpeedDir = 0;
        private float blendXCount = 0;


        //アニメーション再生関数
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





        //アニメーション終了
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

            //予約していたものが実際に再生されたら
            if (endState != null && animator.GetCurrentAnimatorStateInfo(0).IsName(endState))
            {
                endState = null;
                animeHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
                return;
            }


            //別アニメーションになったら終了
            if(animeHash != 0 && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != animeHash)
            {
                animeHash = 0;
                onEndAnimationSubject.OnNext(new RxUtils.Unit());   //終了通知
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