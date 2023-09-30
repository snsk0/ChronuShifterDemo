using System;

using UnityEngine;
using UniRx;

using UnityView.Player.Animation;

using Player.Structure;
using Player.Behaviours;
using Animation;
using Animation.Triggerer;



namespace UnityView.Player.Behaviours
{
    //仮インターフェース
    public interface IPlayerVelocityChanger
    {
        public void SetVelocityMultiply(float multiply);
    }


    public class PlayerMover : MonoBehaviour, IPlayerMove, ILoopAnimationTriggable<PlayerAnimationType>, IPlayerVelocityChanger
    {
        //コンポーネント
        [SerializeField] private new Rigidbody rigidbody;
        private IGroundNormalProvider slopeAngleProvider;

        
        //調整用パラメータ
        [SerializeField] private float maxVelocity;
        [SerializeField] private float moveForceMultiplier;
        [SerializeField] private float maxAirVelocity;
        [SerializeField] private float airMoveStrength;
        [SerializeField] private float angleBlend;
        [SerializeField] private float sprintMultiply;
        [SerializeField] private float maxGroundedSlopeAngle;
        [SerializeField] private bool isSlopeNormalize;


        //フィールド
        private float velocityMultiply = 1.0f;      //デフォルト等倍


        //キャッシュ
        private Vector3 direction;
        private Vector3 lastDirection;
        private bool isAirborne;
        private bool isSprint;



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
                direction = Vector3.zero;
            }
            else direction = new Vector3(lookDirection.x * angleBlend + moveDirection.x, 0, lookDirection.y * angleBlend + moveDirection.y).normalized;
            this.isSprint = isSprint;
            isAirborne = false;
        }
        //空中移動関数
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection)
        {
            if (moveDirection.x == 0 && moveDirection.y == 0)
            {
                direction = Vector3.zero;
            }
            else direction = new Vector3(lookDirection.x * angleBlend + moveDirection.x, 0, lookDirection.y * angleBlend + moveDirection.y).normalized;
            isAirborne = true;
        }

        //移動速度変更
        public void SetVelocityMultiply(float multiply)
        {
            velocityMultiply = multiply;
        }

        //移動取得関数
        public bool isMoving()
        {
            return !(lastDirection == Vector3.zero);    //最後に処理した移動が静止だった場合
        }


        /*
         * Unity関数
         */
            //初期化
            private void Awake()
        {
            slopeAngleProvider = GetComponent<IGroundNormalProvider>();
        }


        //移動計算
        private void FixedUpdate()
        {
            //入力がなかった場合
            if (direction == Vector3.zero)
            {
                if(isSprint) onStopAnimationSubject.OnNext(PlayerAnimationType.Run);
                else onStopAnimationSubject.OnNext(PlayerAnimationType.Walk);

                lastDirection = direction;
                return;
            }


            //移動種別で処理
            if (isAirborne)
            {
                Vector3 vec = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                float maxVelocity = maxAirVelocity * velocityMultiply;
                //if (isSprint) maxVelocity *= sprintMultiply;

                if (vec.magnitude > maxVelocity)
                {
                    vec = vec.normalized * maxVelocity;                                         //最高速に修正
                    rigidbody.velocity = new Vector3(vec.x, rigidbody.velocity.y, vec.z);       //y軸をvelocityに代入
                }
                else rigidbody.AddForce(direction * airMoveStrength, ForceMode.Acceleration);
            }

            else
            {
                /*
                * アニメーションを発行(実際に動いてから発行する)
                */
                PlayerAnimationType type = PlayerAnimationType.Walk;
                if (isSprint) type = PlayerAnimationType.Run;

                //発行
                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(type, velocityMultiply));




                //移動方向を水平のみ算出
                Vector3 velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

                //坂を考よ
                direction = Vector3.ProjectOnPlane(direction, slopeAngleProvider.slopeNormal);
                if (isSlopeNormalize) direction = direction.normalized;

                //移動量計算
                float magnitude;
                if (isSprint) magnitude = maxVelocity * sprintMultiply * velocityMultiply - velocity.magnitude;
                else magnitude = maxVelocity * velocityMultiply - velocity.magnitude;

                rigidbody.AddForce(moveForceMultiplier * (direction * magnitude), ForceMode.Acceleration);
            }

            //移動方向キャッシュを破棄 これはダメ Fixedが連続する可能性もある。
            //direction = Vector3.zero;
            lastDirection = direction;
        }
    }
}
