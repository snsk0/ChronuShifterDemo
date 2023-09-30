using System;

using FSM.Core;
using FSM.Core.Hierarchical;
using FSM.Parameter;
using FSMUtils;


namespace Player
{
    public class PlayerFSMOwner : IPlayerEventReceiver, IPlayerCurrentLeafStateProvider, IPlayerParameterContainerAccessable
    {
        //フィールド
        private readonly FiniteStateMachine<PlayerFSMOwner, PlayerEvent> fsm;    //ステートマシン
        private readonly ObjectContainer objectContainerInstance;                //ロジックの集約オブジェクト
        private readonly IFSMUpdator fsmUpdator;                                 //ステートマシンの更新機


        //プロパティ
        public StateBase<PlayerFSMOwner, PlayerEvent> currentLeafState => fsm.GetCurrentLeafState();    //ステート読み取り
        public IReadOnlyObjectContainer objectContainer => objectContainerInstance;     //ロジックコンテナの読み取り

        //コンテナアクセス
        public IParameterContainer playerGlobalParameterContainer => fsm.globalParameterContainer;
        public IReadOnlyParameterContainer playerLocalParameterContainer => fsm.readLocalParameterContainer;


        //コンストラクタ
        public PlayerFSMOwner(ObjectContainer objectContainerInstance, IFSMUpdator fsmUpdator, Action<FiniteStateMachine<PlayerFSMOwner, PlayerEvent>> initialize,
            IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            //フィールドを初期化
            fsm = new FiniteStateMachine<PlayerFSMOwner, PlayerEvent>(this, globalParameterContainer, localParameterContainer);
            this.fsmUpdator = fsmUpdator;
            this.objectContainerInstance = objectContainerInstance;

            //ステートマシンの初期化呼び出し
            initialize.Invoke(fsm);
        }


        //デストラクタ(保険)
        ~PlayerFSMOwner()
        {
            //登録解除
            fsmUpdator.UnRegisterFSM(fsm);
        }






        //Updateを開始する
        public void StartFSM()
        {
            fsmUpdator.RegisterFSM(fsm);
        }

        //中断
        public void AbortFSM()
        {
            fsmUpdator.UnRegisterFSM(fsm);
            fsm.Abort();
        } 

        //イベント受け取り
        public bool SendEvent(PlayerEvent triggerEvent)
        {
            if (triggerEvent == PlayerEvent.End) return false;  //Endはアクションの終了イベントとして用いられるので外部は拒否しておく
            return fsm.SendEvent(triggerEvent);
        }

        //ステート内タイプ検索
        public bool ContainsTypeCurrentState<TType>()
        {
            return fsm.ContainsTypeCurrentState<PlayerFSMOwner, PlayerEvent, TType>();
        }
    }
}
