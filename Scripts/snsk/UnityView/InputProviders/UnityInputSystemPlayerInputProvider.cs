using System;

using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

using InputProviders.Player;


namespace UnityView.InputProviders
{
    public class UnityInputSystemPlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        //必要コンポーネント
        [SerializeField] PlayerInput playerInput;


        //サブジェクト
        private Subject<MoveInputData> moveSubject = new Subject<MoveInputData>();
        private Subject<JumpInputData> jumpSubject = new Subject<JumpInputData>();
        private Subject<SprintInputData> sprintSubject = new Subject<SprintInputData>();
        private Subject<bool> interactSubject = new Subject<bool>();


        //イベント購読用
        public IObservable<MoveInputData> onMoveInput => moveSubject;
        public IObservable<JumpInputData> onJumpInput => jumpSubject;
        public IObservable<SprintInputData> onSprintInput => sprintSubject;
        public IObservable<bool> onInteractInput => interactSubject;


        //パラメータ
        [SerializeField] private bool isSprintHold;

        //移動キャッシュ
        private int lastHorizontal = 0;
        private int lastVertical = 0;





        //Destory時に破棄
        private void OnDestroy()
        {
            moveSubject.Dispose();
            jumpSubject.Dispose();
            sprintSubject.Dispose();
        }




        //Update
        private void Update()
        {
            string scheme = playerInput.currentControlScheme;
            Debug.Log(scheme);


            //ゲームパッドを優先
            if (scheme == "GamePad")
            {
                GamePadByInputSystem();
            }

            //スキームがゲームパッドでないならInputManagerのキーボマウスで判定
            else if (scheme == "MouseKeyBoard")
            {
                KeyBoardMouseByInputManager();
            }
        }




        private void GamePadByInputSystem()
        {
            //moveを発行
            Vector3 move = playerInput.currentActionMap["Move"].ReadValue<Vector2>();
            moveSubject.OnNext(new MoveInputData(move.x, move.y));

            //Sprintを発行
            bool isSprint;
            if (isSprintHold) isSprint = (playerInput.currentActionMap["Sprint"].WasPerformedThisFrame());
            else isSprint = (playerInput.currentActionMap["Sprint"].WasPressedThisFrame());
            sprintSubject.OnNext(new SprintInputData(isSprint, isSprintHold));


            //ジャンプを発行
            if (playerInput.currentActionMap["Jump"].WasPressedThisFrame()) jumpSubject.OnNext(new JumpInputData(1.0f));

            //インタラクト発行
            if (playerInput.currentActionMap["Interact"].WasPressedThisFrame()) interactSubject.OnNext(true);
        }




        private void KeyBoardMouseByInputManager()
        {
           /*
            * 移動Inputを生成
            */

            /*
             * 水平方向 完成
             */
            float x = 0;
            bool right = Input.GetKey(KeyCode.D);
            bool left = Input.GetKey(KeyCode.A);

            //同時押し
            if (right && left)
            {
                //Downに変更
                right = Input.GetKeyDown(KeyCode.D);
                left = Input.GetKeyDown(KeyCode.A);

                //downしたほうに変更
                if (right && !left) { x = 1; lastHorizontal = 1; }
                else if (left && !right) { x = -1; lastHorizontal = -1; }
                else
                {
                    //同時押しでどちらもDownじゃないときバグるので何処かにキャッシュがいる
                    x = lastHorizontal;
                }

                //完全に同じタイミングでダウンした場合 0のままにしておく
            }

            //同時押しじゃないならそのまま
            else
            {
                lastHorizontal = 0; //キャッシュを破棄
                if (right && !left) x = 1;
                else if (left && !right) x = -1;
            }


            /*
             * 垂直方向 水平と同じように修正
             */
            float y = 0;
            bool front = Input.GetKey(KeyCode.W);
            bool back = Input.GetKey(KeyCode.S);

            //同時押し
            if (front && back)
            {
                //Downに変更
                front = Input.GetKeyDown(KeyCode.W);
                back = Input.GetKeyDown(KeyCode.S);

                //downしたほうに変更
                if (front && !back) { y = 1; lastVertical = 1; }
                else if (back && !front) { y = -1; lastVertical = -1; }

                else
                {
                    y = lastVertical;
                }
                //完全に同じタイミングでダウンした場合 0のままにしておく
            }

            //同時押しじゃないならそのまま
            else
            {
                lastVertical = 0;
                if (front && !back) y = 1;
                else if (back && !front) y = -1;
            }


            //生成して発行
            MoveInputData moveInputData = new MoveInputData(x, y);
            moveSubject.OnNext(moveInputData);


            //Sprintを発行
            bool isSprint;
            if (isSprintHold) isSprint = Input.GetKey(KeyCode.LeftShift);
            else isSprint = Input.GetKeyDown(KeyCode.LeftShift);
            sprintSubject.OnNext(new SprintInputData(isSprint, isSprintHold));


            //ジャンプを発行
            if (Input.GetKeyDown(KeyCode.Space)) jumpSubject.OnNext(new JumpInputData(1.0f));

            //インタラクト発行
            if (Input.GetKeyDown(KeyCode.F)) interactSubject.OnNext(true);
        }
    }
}
