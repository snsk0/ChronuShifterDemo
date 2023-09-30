using System.Collections.Generic;

using UnityEngine;

using Player;
using Player.Presenter;
using Player.States;
using Animation.Player;
using Animation.Triggerer;
using Animation.Presenter;
using InputProviders.Player;
using InputProviders.LifeCycle;
using FSM.Parameter;
using FSMUtils;

using UnityView.Player.Animation;


namespace UnityView.Player
{
    public class PlayerCore : MonoBehaviour
    {
        //コンポーネントリスト(オブジェクトコンテナ格納する
        [SerializeField] private List<MonoBehaviour> playerBehaviourList;

        //ステートマシンコア
        private PlayerFSMOwner fsmCore;



        //初期化
        private void Awake()
        {
            //アニメーションのPresenter紐づけ
            AnimationPresenter<PlayerAnimationType> presenter = new AnimationPresenter<PlayerAnimationType>(GetComponent<IAnimationPlayer<PlayerAnimationType>>());
            foreach (INonLoopAnimationTriggable<PlayerAnimationType> triggable in GetComponents<INonLoopAnimationTriggable<PlayerAnimationType>>()) presenter.RegisterAnimationTriggable(triggable);
            foreach (ILoopAnimationTriggable<PlayerAnimationType> triggable in GetComponents<ILoopAnimationTriggable<PlayerAnimationType>>()) presenter.RegisterAnimationTriggable(triggable);


            //FSMUpdatorを初期化
            FSMUpdateManager.Initialize(GetComponent<IInputUpdateProvider>());


            //コンテナを生成
            ObjectContainer objectContainer = new ObjectContainer();
            foreach (object obj in playerBehaviourList) objectContainer.AddObject(obj);


            //パラメータコンテナの生成
            IParameterContainer globalParameterContainer = new DefaultParameterContainer();
            globalParameterContainer.InitializePlayerGlobalParameter();
            IParameterContainer localParameterContainer = new DefaultParameterContainer();
            localParameterContainer.InitializePlayerLocalParameter();



            //fsmコアを生成
            fsmCore = new PlayerFSMOwner(objectContainer, FSMUpdateManager.instance, PlayerFSMBuilder.builder, globalParameterContainer, localParameterContainer);


            //プレイヤーのプレゼンターを生成
            new PlayerEventPresenter(fsmCore, fsmCore, GetComponent<IPlayerInputProvider>(), GetComponent<IDamagableEventSender>(), GetComponent<IMoveDirectionConverter>());
        }


        //開始
        private void Start()
        {
            fsmCore.StartFSM();
        }


        //更新処理(Debug)
        void Update()
        {
            //Debug.Log(fsmCore.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));
            //Debug.Log(fsmCore.currentLeafState.GetType().Name);
        }
    }
}
