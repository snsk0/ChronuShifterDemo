using System;
using UnityEngine;
using UniRx;
using UnityView.Player.Animation;
using Player.Structure;
using Player.Behaviours;
using Animation;
using Animation.Triggerer;
using Chronus.KinematicPhysics;
using System.Runtime.CompilerServices;

namespace UnityView.Player.Behaviours
{
    //仮インターフェース
    public interface IPlayerVelocityChanger
    {
        public void SetVelocityMultiply(float multiply);
    }

    public class PlayerMover : MonoBehaviour, IPlayerMove, ILoopAnimationTriggable<PlayerAnimationType>, IPlayerVelocityChanger, IPlayerOnGrounded
    {
        [SerializeField] private CharacterBody _characterBody;

        [SerializeField] private float _speed;
        [SerializeField] private float _sprintSpeed;
        [SerializeField] private float _groundMoveForceMultiply;
        [SerializeField] private float _airborneMoveForceMultiply;
        [SerializeField] private float _angleBlendRate;

        private float _speedMultiply = 1.0f;

        private Vector3 _direction;
        private Vector3 _lastDirection;
        private bool _isAirborne;
        private bool _isSprint;

        //アニメーションイベント発行
        private Subject<PlayerAnimationType> onStopAnimationSubject = new Subject<PlayerAnimationType>();
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<PlayerAnimationType> stopTriggableObserbable => onStopAnimationSubject;
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;

        //移動関数
        public void Move(LookDirection lookDirection, LookDirection moveDirection, bool isSprint)
        {
            if (moveDirection.x == 0 && moveDirection.y == 0)
            {
                _direction = Vector3.zero;
            }
            else
            {
                _direction = new Vector3(lookDirection.x * _angleBlendRate + moveDirection.x, 0, lookDirection.y * _angleBlendRate + moveDirection.y).normalized;
            }

            _isSprint = isSprint;
            _isAirborne = false;
        }

        //空中移動関数
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection, bool isSprint)
        {
            if (moveDirection.x == 0 && moveDirection.y == 0)
            {
                _direction = Vector3.zero;
            }
            else
            {
                _direction = new Vector3(lookDirection.x * _angleBlendRate + moveDirection.x, 0, lookDirection.y * _angleBlendRate + moveDirection.y).normalized;
            }

            _isSprint = isSprint;
            _isAirborne = true;
        }

        //移動速度変更
        public void SetVelocityMultiply(float multiply)
        {
            _speedMultiply = multiply;
        }

        //移動取得関数
        public bool isMoving()
        {
            //最後に処理した移動が静止だった場合
            return !(_lastDirection == Vector3.zero);
        }

        //移動計算
        private void FixedUpdate()
        {
            if (_isAirborne)
            {
                _characterBody.AddForce(Physics.gravity);

                if(_direction != Vector3.zero)
                {
                    Vector3 move = _isSprint ? _direction * _sprintSpeed : _direction * _speed;
                    Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), _characterBody.Velocity);
                    
                    _characterBody.AddForce(_airborneMoveForceMultiply * (move - horizontalVelocity));
                }

                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Airborne, 1, new BlendParameter(_characterBody.Velocity.y, 0)));
            }
            else
            {
                onStopAnimationSubject.OnNext(PlayerAnimationType.Airborne);

                if (_direction == Vector3.zero)
                {
                    if (_isSprint)
                    {
                        onStopAnimationSubject.OnNext(PlayerAnimationType.Run);
                    }
                    else
                    {
                        onStopAnimationSubject.OnNext(PlayerAnimationType.Walk);
                    }
                }
                else
                {
                    //アニメーションを発行(実際に動いてから発行する)
                    PlayerAnimationType type;
                    if (_isSprint)
                    {
                        type = PlayerAnimationType.Run;
                    }
                    else
                    {
                        type = PlayerAnimationType.Walk;
                    }
                    onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(type, _speedMultiply));
                }

                Vector3 move = _isSprint ? _direction * _sprintSpeed : _direction * _speed;
                _characterBody.AddForce(_groundMoveForceMultiply * (move - _characterBody.Velocity));
            }

            _lastDirection = _direction;
        }

        public bool IsOnGrounded()
        {
            return _characterBody.IsGround;
        }
    }
}
