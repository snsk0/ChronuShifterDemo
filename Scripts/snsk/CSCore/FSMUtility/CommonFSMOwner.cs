using System;

using FSM.Core;
using FSM.Parameter;

namespace FSMUtils
{
    //一応実装したが、ステートクラスの継承のGeneric指定がかなりややこしくなるため恐らく廃止
    //汎用オーナーを作成したいならEventも指定しないと使いづらいかも？
    //ただ、汎用コンテキストを用意すると重複コードを避けられる部分が大きいため要件等
    //ただし、汎用コンテキストは同時に汎用ステートを許すことになる(例えばゲームマネージャとキャラ両方で使うと両方に入れるステートを実装できる、それは流石に避けたい)
    //そのためある程度の重複コードは許容すべきだと感じる
    [Obsolete]
    public class CommonFSMOwner<TEvent>
    {
        //フィールド
        protected readonly FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent> fsm;    //ステートマシン
        private readonly ObjectContainer objectContainer;                             //ステートからアクセスするオブジェクトコンテナ


        //プロパティ
        public Type currentStateType => fsm.currentState.GetType();                     //現在ステートの型
        public IReadOnlyObjectContainer readonlyObjectContainer => objectContainer;     //ロジックコンテナの読み取り





        //コンストラクタ
        public CommonFSMOwner(ObjectContainer objectContainer, Action<FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent>> initialize)
        {
            //フィールド初期化
            fsm = new FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent>(this, new DefaultParameterContainer(), new DefaultParameterContainer());
            this.objectContainer = objectContainer;

            //ステートマシンの初期化呼び出し
            initialize.Invoke(fsm);
        }


        //デストラクタ
        ~CommonFSMOwner()
        {
            //ステートマシンのオート更新解除
            FSMUpdateManager.instance.UnRegisterFSM(fsm);
        }





        //ステートマシンのイベント送信機能
        public bool SendEvent(TEvent triggerEvent)
        {
            return fsm.SendEvent(triggerEvent);
        }

        //開始
        public void StartFSM()
        {
            FSMUpdateManager.instance.RegisterFSM(fsm);
        }

        //中断機能
        public void AbortFSM()
        {
            //自動更新を解除
            FSMUpdateManager.instance.UnRegisterFSM(fsm);

            //中断呼び出し
            fsm.Abort();
        }
    }
}
