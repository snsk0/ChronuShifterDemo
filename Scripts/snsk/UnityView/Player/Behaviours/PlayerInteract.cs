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
        public Transform rightHandTransform { get; }        //右手位置
        public Transform leftHandTransform { get; }         //左手位置
        public Transform defaultParentTransform { get; }    //初期の親オブジェクト           
        public Transform transform { get; }                 //オブジェクトTransform
        public void OnHandle(bool isHoldUp);                //持った時に呼び出される(flagはもち上げかもち下げか)
    }


    //仮インターフェース
    public interface IDropper
    {
        //アイテムを持っている場合ドロップする
        public void Drop();
    }



    public class PlayerInteract : MonoBehaviour, IPlayerInteract, IDropper, IAnimationEventHandler, INonLoopAnimationTriggable<PlayerAnimationType>
    {
        //実行している処理がどれかの判別enum
        private enum HoldMode
        {
            None,
            HoldingUp,
            HoldingDown,
            Cancelled
        }



        //必要パラメータ,コンポーネント
        [SerializeField] private Transform itemHandlingPosition;
        [SerializeField] private Transform releaseTransform;
        [SerializeField] private float rayRange;



        //インタラクト実行中か
        public bool isInteracting => mode != HoldMode.None;


        //アニメーション用

        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;


        //フィールド
        private HoldMode mode;                       //行っている処理が持ちあげかもち下げか
        private IHandableObject canHandingObject;    //もち上げ可能なアイテム
        private IHandableObject handlingObject;      //持っているアイテム



        public void Interact(bool isSprint)
        {
            //インタラクト実行中は受け付けない
            if (isInteracting) return;

            //アイテムを持っているかどうか
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
            //isInteracting中なら中断処理を行い、handingをnullに
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


        
        //終了通知
        public void OnEndAnimation()
        {
            //アニメーション終了を受け取ったらNoneに戻す
            mode = HoldMode.None;
        }


        //アニメーション通知
        public void OnAnimationEvent(string code)
        {
            if (code == PlayerAnimationCode.pickUp) OnHoldUpProcess();
            else if (code == PlayerAnimationCode.pickDown) OnHoldDownProcess();
        }



        //HoldDown系
        private void HoldDown()
        {
            //モード変更
            mode = HoldMode.HoldingDown;

            //HoldDownを再生
            onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.HoldDown, 1));

        }
        public void OnHoldDownProcess()
        {
            //そのまま置く
            handlingObject.transform.parent = handlingObject.defaultParentTransform;
            handlingObject.transform.rotation = releaseTransform.rotation;
            handlingObject.transform.position = releaseTransform.position;
            handlingObject.OnHandle(false);
            handlingObject = null;
        }



        //HoldUp系
        private void HoldUp()
        {
            //canhandingオブジェクトnullならダメ
            if (canHandingObject == null) canHandingObject = GetHoldableItemOnWorld();
            if (canHandingObject == null) return;
            handlingObject = canHandingObject;
            canHandingObject = null;

            //モード変更
            mode = HoldMode.HoldingUp;

            //アニメーションを再生
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
                //持てるアイテムを取得
                IHandableObject objects = hit.collider.GetComponent<IHandableObject>();
                return objects;
            }
            return null;
        }




        private void LateUpdate()
        {
            //キャッシュを削除
            canHandingObject = null;
        }
    }
}
