using System;
using System.Collections.Generic;

using RxUtils;

using InputProviders.Player;

using Player.Structure;
using Player.States;


namespace Player.Presenter
{

    //ここに書くべきでなさそう
    //後日Adaptorに移動することを検討
    public class PlayerEventPresenter
    {
        //フィールド
        private readonly IPlayerEventReceiver eventReceiver;                                //イベント送信先
        private readonly IPlayerParameterContainerAccessable containerAccessable;           //コンテナへのアクセス
        private readonly IPlayerInputProvider inputProvider;                                //プレイヤーInput発行元
        private readonly IDamagableEventSender damagableEventSender;                        //ダメージイベント
        private readonly IMoveDirectionConverter moveDirectionConverter;                    //カメラ方向取得

        private readonly List<IDisposable> disposableList;                                  //イベント破棄用



        //コンストラクタ
        public PlayerEventPresenter(IPlayerEventReceiver eventReceiver, IPlayerParameterContainerAccessable containerAccessable, 
            IPlayerInputProvider inputProvider, IDamagableEventSender damagableEventSender, IMoveDirectionConverter moveDirectionConverter)
        {
            //フィールド初期化
            this.eventReceiver = eventReceiver;
            this.containerAccessable = containerAccessable;
            this.inputProvider = inputProvider;
            this.damagableEventSender = damagableEventSender;
            this.moveDirectionConverter = moveDirectionConverter;

            disposableList = new List<IDisposable>();


            //イベント購読開始
            RegisterPlayerEvent();
        }

        //デストラクタ
        ~PlayerEventPresenter()
        {
            UnRegisterPlayerEvent();
        }





        //イベント登録関数
        private void RegisterPlayerEvent()
        {
            //移動
            disposableList.Add(inputProvider.onMoveInput.Subscribe(inputData =>
            {
                //入力があった場合イベントを送信
                if(!inputData.isNone()) eventReceiver.SendEvent(PlayerEvent.Move);

                //方向はコンテナに数値を格納
                LookDirection direction = moveDirectionConverter.ConvertMoveInputData(inputData);
                containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.moveInput, direction);
            }));




            //ダッシュ
            disposableList.Add(inputProvider.onSprintInput.Subscribe(inputData =>
            {
                //holdか
                if (inputData.isHolding)
                {
                    //holdならそのまま入力を伝える
                    containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, inputData.isSprintInput);
                }

                //トグル
                else
                {

                    //Inputがある場合
                    if (inputData.isSprintInput) 
                    {
                        //現在のsprintを取得する
                        bool isSprint = containerAccessable.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);

                        //それとは逆にするように入力
                        containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, !isSprint);
                    }


                    //Inputがない場合
                    else
                    {
                        //現在のsprintを取得する
                        bool isSprint = containerAccessable.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);

                        //それと「同じ」になるように書き換える
                        containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, isSprint);
                    }
                }
            }));




            //ジャンプ
            disposableList.Add(inputProvider.onJumpInput.Subscribe(inputData =>
            {
                //イベントをステートマシンに送信
                eventReceiver.SendEvent(PlayerEvent.Jump);

                //コンテナに数値を格納
                containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.jumpInput, inputData.magnitude);
            }));




            //インタラクト
            disposableList.Add(inputProvider.onInteractInput.Subscribe(inputData =>
            {
                //ダメージイベントを送信
                eventReceiver.SendEvent(PlayerEvent.Interact);
            }));


            //ダメージ
            disposableList.Add(damagableEventSender.onDamageObservable.Subscribe(inputData =>
            {
                //ダメージイベントを送信(もしknockが0なら再生しない)
                if (inputData.knock == 0) return;
                eventReceiver.SendEvent(PlayerEvent.Damage);
            }));
            //その他etc
        }










        //イベント破棄関数
        private void UnRegisterPlayerEvent()
        {
            foreach (IDisposable disposable in disposableList)
            {
                disposable.Dispose();
            }
        }
    }
}
