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

        //�p�[�e�B�N��
        [SerializeField] private ParticleSystem _sprintParticle;

        [SerializeField] private float _pickUpThreshold;
        [SerializeField] private float _dropDownThreshold;
        [SerializeField] private float _interactThreshold;

        //�A�j���[�V�����Đ��֐�
        public void PlayAnimation(AnimationParameter<PlayerAnimationType> parameter)
        {
            var emmision = _sprintParticle.emission;
            emmision.enabled = false;

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
                    emmision.enabled = true;
                    break;

                case PlayerAnimationType.Airborne:
                    animator.SetBool("fall", true);
                    animator.SetFloat("FallSpeed", parameter.blend.x);
                    break;
                case PlayerAnimationType.Damage:
                    animator.SetTrigger("Damage");
                    endState = "Damage";
                    break;

                case PlayerAnimationType.PickUp:
                    animator.SetTrigger("PickUp");
                    endState = "PickUp";
                    break;

                case PlayerAnimationType.DropOff:
                    animator.SetTrigger("PickDown");
                    endState = "PickDown";
                    break;
                case PlayerAnimationType.Interact:
                    animator.SetTrigger("Interact");
                    endState = "Interact";
                    break;
            }
        }

        //�A�j���[�V�����I��
        public void StopAnimation(PlayerAnimationType type)
        {
            var emmision = _sprintParticle.emission;
            emmision.enabled = false;

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
            /*
            if(animeHash != 0 && animator.GetCurrentAnimatorStateInfo(0).fullPathHash != animeHash)
            {
                animeHash = 0;
                onEndAnimationSubject.OnNext(new RxUtils.Unit());   //�I���ʒm
            }
            */
            //�Đ����ŏI���ʒm
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float threshold = 1;

            if (stateInfo.IsName("PickUp"))
            {
                threshold = _pickUpThreshold;
            }
            else if (stateInfo.IsName("PickDown"))
            {
                threshold = _dropDownThreshold;
            }
            else if (stateInfo.IsName("Interact"))
            {
                threshold = _interactThreshold;
            }

            if (animeHash != 0 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > threshold)
            {
                animeHash = 0;
                onEndAnimationSubject.OnNext(new RxUtils.Unit());   //�I���ʒm
                return;
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