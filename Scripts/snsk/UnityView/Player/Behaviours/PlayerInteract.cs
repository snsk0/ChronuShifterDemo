using System;

using UnityEngine;
using UniRx;

using Player.Behaviours;
using Animation;
using Animation.Triggerer;

using UnityView.Player.Animation;


namespace UnityView.Player.Behaviours
{
    public interface IHandableObject
    {
        public Transform rightHandTransform { get; }        //�E��ʒu
        public Transform leftHandTransform { get; }         //����ʒu
        public Transform defaultParentTransform { get; }    //�����̐e�I�u�W�F�N�g           
        public Transform transform { get; }                 //�I�u�W�F�N�gTransform
        public void OnHandle(bool isHoldUp);                //���������ɌĂяo�����(flag�͂����グ������������)
    }


    //���C���^�[�t�F�[�X
    public interface IDropper
    {
        //�A�C�e���������Ă���ꍇ�h���b�v����
        public void Drop();
    }



    public class PlayerInteract : MonoBehaviour, IPlayerInteract, IDropper, IAnimationEventHandler, INonLoopAnimationTriggable<PlayerAnimationType>
    {
        //���s���Ă��鏈�����ǂꂩ�̔���enum
        private enum HoldMode
        {
            None,
            HoldingUp,
            HoldingDown,
            Cancelled
        }



        //�K�v�p�����[�^,�R���|�[�l���g
        [SerializeField] private Transform itemHandlingPosition;
        [SerializeField] private Transform releaseTransform;
        [SerializeField] private float rayRange;



        //�C���^���N�g���s����
        public bool isInteracting => mode != HoldMode.None;


        //�A�j���[�V�����p

        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;


        //�t�B�[���h
        private HoldMode mode;                       //�s���Ă��鏈������������������������
        private IHandableObject canHandingObject;    //�����グ�\�ȃA�C�e��
        private IHandableObject handlingObject;      //�����Ă���A�C�e��



        public void Interact(bool isSprint)
        {
            //�C���^���N�g���s���͎󂯕t���Ȃ�
            if (isInteracting) return;

            //�A�C�e���������Ă��邩�ǂ���
            if (handlingObject == null) HoldUp();
            else HoldDown();
        }


        public bool CanInteract()
        {
            if (isInteracting) return false;

            if (handlingObject != null) return true;

            canHandingObject = GetHoldableItemOnWorld();
            if (canHandingObject != null) return true;
            return false;
        }


        public void Abort()
        {
            //isInteracting���Ȃ璆�f�������s���Ahanding��null��
            if (isInteracting)
            {
                mode = HoldMode.Cancelled;
                if (handlingObject != null) OnHoldDownProcess();
            }
        }


        public void Drop()
        {
            if (handlingObject != null) OnHoldDownProcess();
        }


        
        //�I���ʒm
        public void OnEndAnimation()
        {
            //�A�j���[�V�����I�����󂯎������None�ɖ߂�
            mode = HoldMode.None;
        }


        //�A�j���[�V�����ʒm
        public void OnAnimationEvent(string code)
        {
            if (code == PlayerAnimationCode.pickUp) OnHoldUpProcess();
            else if (code == PlayerAnimationCode.pickDown) OnHoldDownProcess();
        }



        //HoldDown�n
        private void HoldDown()
        {
            //���[�h�ύX
            mode = HoldMode.HoldingDown;

            //HoldDown���Đ�
            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.HoldDown, 1));

        }
        public void OnHoldDownProcess()
        {
            //���̂܂ܒu��
            handlingObject.transform.parent = handlingObject.defaultParentTransform;
            handlingObject.transform.rotation = releaseTransform.rotation;
            handlingObject.transform.position = releaseTransform.position;
            handlingObject.OnHandle(false);
            handlingObject = null;
        }



        //HoldUp�n
        private void HoldUp()
        {
            //canhanding�I�u�W�F�N�gnull�Ȃ�_��
            if (canHandingObject == null) canHandingObject = GetHoldableItemOnWorld();
            if (canHandingObject == null) return;
            handlingObject = canHandingObject;
            canHandingObject = null;

            //���[�h�ύX
            mode = HoldMode.HoldingUp;

            //�A�j���[�V�������Đ�
            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.HoldUp, 1));
        }
        public void OnHoldUpProcess()
        {
            handlingObject.transform.parent = itemHandlingPosition;
            handlingObject.OnHandle(true);
        }




        private IHandableObject GetHoldableItemOnWorld()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * rayRange, Color.red, 1f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayRange))
            {
                //���Ă�A�C�e�����擾
                IHandableObject objects = hit.collider.GetComponent<IHandableObject>();
                return objects;
            }
            return null;
        }




        private void LateUpdate()
        {
            //�L���b�V�����폜
            canHandingObject = null;
        }
    }
}
