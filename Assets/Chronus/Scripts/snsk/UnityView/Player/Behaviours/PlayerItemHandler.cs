using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Player.Behaviours;
using Animation;
using Animation.Triggerer;
using UnityView.Player.Animation;
using Player;
using Player.Structure;
using UnityView.Player.Utils;
using Chronus.KinematicPhysics;

namespace UnityView.Player.Behaviours
{
    //仮インターフェース
    public interface IDropper
    {
        public void Drop();
    }

    public class PlayerItemHandler : MonoBehaviour, IPlayerItemHandler, IDropper, IAnimationEventHandler, INonLoopAnimationTriggable<PlayerAnimationType>
    {
        [SerializeField] private CharacterBody _characterBody;
        [SerializeField] private Transform _itemPosition;

        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;

        private GameObject _itemGameObject;
        private IPlayerItemObject _item;
        private IChronusItemController _controller;
        
        private bool _isExecution = false;
        private string _animationCode = ""; 

        public bool IsExecution => _isExecution;
 
        private void Awake()
        {
            onTriggableAnimationSubject.AddTo(this);
        }

        public void OnAnimationEvent(string code)
        {
            _animationCode = code;
        }

        public void OnEndAnimation()
        {
            _isExecution = false;
            _animationCode = "";
        }

        /// <summary>
        /// 持てるアイテムのGameObject本体を返す
        /// </summary>
        public GameObject GetHandlingObject()
        {
            if (HasItem())
            {
                return _itemGameObject;
            }
            return null;
        }

        /// <summary>
        /// アイテムを拾う
        /// </summary>
        public async void PickUp(IPlayerItemObject targetItem)
        {
            if (_isExecution || HasItem())
            {
                return;
            }

            _characterBody.Stop();

            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.PickUp, 1));
            _isExecution = true;

            await UniTask.WaitUntil(() => _animationCode == PlayerAnimationCode.PickUp);

            _item = targetItem;

            ItemTemp temp = (_item as ItemTemp);
            _itemGameObject = temp.GameObject;
            _controller = temp.Controller;

            _controller.OnHandle(true);
            await _controller.CallItemAnimation(true);

            //アニメーションより後になるとバグる
            _itemGameObject.transform.position = _itemPosition.position;
            _itemGameObject.transform.rotation = _itemPosition.rotation;
            _itemGameObject.transform.parent = _itemPosition;
            _itemGameObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// アイテムを置く
        /// </summary>
        public async void DropOff(Position position, bool isSprint)
        {
            if (_isExecution || !HasItem())
            {
                return;
            }

            _characterBody.Stop();

            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.DropOff, 1));
            _isExecution = true;

            await UniTask.WaitUntil(() => _animationCode == PlayerAnimationCode.DropOff);

            _itemGameObject.transform.position = new Vector3(position.X, position.Y + _controller.DropYOffset, position.Z);
            _itemGameObject.transform.forward = transform.forward;
            _itemGameObject.transform.parent = null;
            _itemGameObject.gameObject.SetActive(true);
            await _controller.CallItemAnimation(false);

            //アニメーションより後になるとバグる
            _controller.OnHandle(false);
            _item = null;
            _itemGameObject = null;
            _controller = null;
        }

        public bool HasItem()
        {
            return _item != null;
        }

        public IPlayerItemObject GetItem()
        {
            return _item;
        }

        public void Abort()
        {
        }

        public void Drop()
        {
        }
    }
}
