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
    //���C���^�[�t�F�[�X
    public interface IPlayerVelocityChanger
    {
        public void SetVelocityMultiply(float multiply);
    }


    public class PlayerMover : MonoBehaviour, IPlayerMove, ILoopAnimationTriggable<PlayerAnimationType>, IPlayerVelocityChanger
    {
        //�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;
        private IGroundNormalProvider slopeAngleProvider;

        
        //�����p�p�����[�^
        [SerializeField] private float maxVelocity;
        [SerializeField] private float moveForceMultiplier;
        [SerializeField] private float maxAirVelocity;
        [SerializeField] private float airMoveStrength;
        [SerializeField] private float angleBlend;
        [SerializeField] private float sprintMultiply;
        [SerializeField] private float maxGroundedSlopeAngle;
        [SerializeField] private bool isSlopeNormalize;


        //�t�B�[���h
        private float velocityMultiply = 1.0f;      //�f�t�H���g���{


        //�L���b�V��
        private Vector3 direction;
        private Vector3 lastDirection;
        private bool isAirborne;
        private bool isSprint;



        //�A�j���[�V�����C�x���g���s
        private Subject<PlayerAnimationType> onStopAnimationSubject = new Subject<PlayerAnimationType>();
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<PlayerAnimationType> stopTriggableObserbable => onStopAnimationSubject;
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;




        //�ړ��֐�
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
        //�󒆈ړ��֐�
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection)
        {
            if (moveDirection.x == 0 && moveDirection.y == 0)
            {
                direction = Vector3.zero;
            }
            else direction = new Vector3(lookDirection.x * angleBlend + moveDirection.x, 0, lookDirection.y * angleBlend + moveDirection.y).normalized;
            isAirborne = true;
        }

        //�ړ����x�ύX
        public void SetVelocityMultiply(float multiply)
        {
            velocityMultiply = multiply;
        }

        //�ړ��擾�֐�
        public bool isMoving()
        {
            return !(lastDirection == Vector3.zero);    //�Ō�ɏ��������ړ����Î~�������ꍇ
        }


        /*
         * Unity�֐�
         */
            //������
            private void Awake()
        {
            slopeAngleProvider = GetComponent<IGroundNormalProvider>();
        }


        //�ړ��v�Z
        private void FixedUpdate()
        {
            //���͂��Ȃ������ꍇ
            if (direction == Vector3.zero)
            {
                if(isSprint) onStopAnimationSubject.OnNext(PlayerAnimationType.Run);
                else onStopAnimationSubject.OnNext(PlayerAnimationType.Walk);

                lastDirection = direction;
                return;
            }


            //�ړ���ʂŏ���
            if (isAirborne)
            {
                Vector3 vec = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                float maxVelocity = maxAirVelocity * velocityMultiply;
                //if (isSprint) maxVelocity *= sprintMultiply;

                if (vec.magnitude > maxVelocity)
                {
                    vec = vec.normalized * maxVelocity;                                         //�ō����ɏC��
                    rigidbody.velocity = new Vector3(vec.x, rigidbody.velocity.y, vec.z);       //y����velocity�ɑ��
                }
                else rigidbody.AddForce(direction * airMoveStrength, ForceMode.Acceleration);
            }

            else
            {
                /*
                * �A�j���[�V�����𔭍s(���ۂɓ����Ă��甭�s����)
                */
                PlayerAnimationType type = PlayerAnimationType.Walk;
                if (isSprint) type = PlayerAnimationType.Run;

                //���s
                onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(type, velocityMultiply));




                //�ړ������𐅕��̂ݎZ�o
                Vector3 velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

                //����l��
                direction = Vector3.ProjectOnPlane(direction, slopeAngleProvider.slopeNormal);
                if (isSlopeNormalize) direction = direction.normalized;

                //�ړ��ʌv�Z
                float magnitude;
                if (isSprint) magnitude = maxVelocity * sprintMultiply * velocityMultiply - velocity.magnitude;
                else magnitude = maxVelocity * velocityMultiply - velocity.magnitude;

                rigidbody.AddForce(moveForceMultiplier * (direction * magnitude), ForceMode.Acceleration);
            }

            //�ړ������L���b�V����j�� ����̓_�� Fixed���A������\��������B
            //direction = Vector3.zero;
            lastDirection = direction;
        }
    }
}
