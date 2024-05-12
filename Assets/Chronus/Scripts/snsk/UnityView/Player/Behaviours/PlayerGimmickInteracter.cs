using Cysharp.Threading.Tasks;
using UnityEngine;
using Player.Behaviours;
using UnityView.Player.Utils;
using Chronus.KinematicPhysics;
using Animation.Triggerer;
using UnityView.Player.Animation;
using Animation;
using System;
using UniRx;

namespace UnityView.Player.Behaviours
{
    public class PlayerGimmickInteracter : MonoBehaviour, IPlayerGimmickInteracter, IAnimationEventHandler, INonLoopAnimationTriggable<PlayerAnimationType>
    {
        [SerializeField] private CharacterBody _characterBody;

        private bool _watingAnimationEvent = false;
        private bool _playingAnimation = false;
        private bool _isInteracting = false;

        public bool IsInteracting => _isInteracting || _watingAnimationEvent;
        
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;

        public async void Interact(IPlayerGimmickObject gimmickObject)
        {
            _characterBody.Stop();
            _watingAnimationEvent = true;
            _playingAnimation = true;

            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Interact, 1f));

            await UniTask.WaitWhile(() => _watingAnimationEvent);

            GimmickTemp gimmick = gimmickObject as GimmickTemp;

            gimmick.Controller.OnInteract(this);
            _isInteracting = true;

            await UniTask.WaitWhile(() => gimmick.Controller.IsInteracting || _playingAnimation);

            _isInteracting = false;
        }

        public void OnAnimationEvent(string code)
        {
            if(code == PlayerAnimationCode.Interact)
            {
                _watingAnimationEvent = false;
            }
        }

        public void OnEndAnimation()
        {
            _playingAnimation = false;
        }
    }
}
