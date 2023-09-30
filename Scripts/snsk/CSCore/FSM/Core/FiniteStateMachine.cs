using System.Collections.Generic;

using FSM.Parameter;

namespace FSM.Core
{
    public class FiniteStateMachine<TOwner, TEvent>
    {
        /*
         * フィールド
         */
        //フラグ
        public bool isRunning => currentState != null;  //ステートマシンが実行されているかどうか
        private bool isAcceptableEvent;                 //イベントを受け入れられるかどうかのフラグ

        //コンテキスト,ステート
        public readonly TOwner owner;                                       //ステートマシンのオーナー
        private StateBase<TOwner, TEvent> initialState;                     //初期ステート
        public StateBase<TOwner, TEvent> currentState { get; private set; } //現在ステート
        public StateBase<TOwner, TEvent> lastState { get; private set; }    //実行時最終ステート

        //コレクション
        private readonly List<StateBase<TOwner, TEvent>> stateList;                                                                     //ステートリスト
        private readonly Dictionary<StateBase<TOwner, TEvent>, Dictionary<TEvent, StateBase<TOwner, TEvent>>> transitonTableDictionary; //遷移リスト

        //イベント処理
        private readonly List<TEvent> processableEventList;                         //処理可能なイベントリスト
        private readonly Queue<TEvent> stateEventQueue;                             //ステートによって送信された処理待ちイベント
        private readonly Queue<TEvent> eventQueue;                                  //処理待ちイベント

        //パラメータ
        public readonly IParameterContainer globalParameterContainer;   //ゲーム内共有変数等用
        private readonly IParameterContainer localParameterContainer;   //グラフ内でのみ有効,読み込みは外部から可能
        public IParameterContainer readLocalParameterContainer => localParameterContainer;  //読み取り専用


        //コンストラクタ
        public FiniteStateMachine(TOwner owner, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            this.owner = owner;
            this.globalParameterContainer = globalParameterContainer;
            this.localParameterContainer = localParameterContainer;

            //フラグの初期化
            isAcceptableEvent = true;

            //各コレクションの初期化
            processableEventList = new List<TEvent>();
            stateList = new List<StateBase<TOwner, TEvent>>();
            transitonTableDictionary = new Dictionary<StateBase<TOwner, TEvent>, Dictionary<TEvent, StateBase<TOwner, TEvent>>>();
            stateEventQueue = new Queue<TEvent>();
            eventQueue = new Queue<TEvent>();
        }





        //初期化ステートの設定
        public bool SetInitialState(StateBase<TOwner, TEvent> state)
        {
            //実行中でないとき初期ステートの設定可能
            if (!isRunning)
            {
                initialState = state;
                return true;
            }
            return false;
        }




        //強制起動メソッド
        public void StartUp()
        {
            //初期化してstart呼び出し
            lastState = null;
            currentState = initialState;
            currentState.OnStart();
        }


        //更新メソッド
        public void Tick()
        {
            //現在ステートがnullの場合
            if (!isRunning)
            {
                StartUp();
            }


            //Updateの呼び出す
            currentState.OnUpdate();


            //イベント処理をし続ける
            while (eventQueue.Count > 0　|| stateEventQueue.Count > 0)
            {
                //次ステートを取得する
                StateBase<TOwner, TEvent> nextState = GetNextState(stateEventQueue);    //ステートからのイベントを先に処理

                if (nextState == null) nextState = GetNextState(eventQueue);            //ステートからのイベントで遷移できなければ外部イベントを処理
                else stateEventQueue.Clear();                                           //ステートイベントによって遷移が決まった場合残りのステートイベントキューをクリア

                if (nextState != null) ChangeState(nextState);                          //どちらかでステートを取得できていればステートを変更する
            }
        }


        //中断メソッド
        public void Abort()
        {
            //現在ステートの終了を呼び出す
            isAcceptableEvent = false;
            currentState.OnEnd();
            isAcceptableEvent = true;

            //現在ステートをnullにする
            lastState = currentState;
            currentState = null;
        }



        //イベント送信(外部)
        public bool SendEvent(TEvent triggerEvent)
        {
            //イベントが受け入れ可能かどうか
            if (!isAcceptableEvent || !isRunning) return false;

            //イベントの処理判定(処理できていたらそのままtrueを返す)
            if (currentState.ReceiveEvent(triggerEvent)) return true;


            //イベントが処理可能か判定する
            if (!processableEventList.Contains(triggerEvent)) return false;

            //キューに積む
            eventQueue.Enqueue(triggerEvent);
            return true;
        }

        
        //ステート側によるイベント送信
        private bool SendEventState(TEvent triggerEvent)
        {
            //イベントが受け入れ可能かどうか
            if (!isAcceptableEvent || !isRunning) return false;

            //キューに追加
            stateEventQueue.Enqueue(triggerEvent);
            return true;
        }




        //ステート追加
        public void AddState(StateBase<TOwner, TEvent> state)
        {
            if (!stateList.Contains(state))
            {
                //リストに追加
                stateList.Add(state);

                //初期化
                state.Initialize(owner, SendEventState, globalParameterContainer, localParameterContainer);

                //新しくテーブルを生成する
                Dictionary<TEvent, StateBase<TOwner, TEvent>> transitionTable = new Dictionary<TEvent, StateBase<TOwner, TEvent>>();

                //テーブルの登録処理
                transitonTableDictionary.Add(state, transitionTable);
            }
        }


        //遷移追加
        public bool AddTransition(StateBase<TOwner, TEvent> state, StateBase<TOwner, TEvent> nextState, TEvent triggerEvent)
        {
            //遷移元と先が管理済みか
            if (!stateList.Contains(state) || !stateList.Contains(nextState)) return false;


            //Transition構築
            Dictionary<TEvent, StateBase<TOwner, TEvent>> transitionTable = transitonTableDictionary[state];

            //イベントがすでに登録されている場合失敗
            if (transitionTable.ContainsKey(triggerEvent)) return false;

            //イベントを登録する
            transitionTable.Add(triggerEvent, nextState);
            processableEventList.Add(triggerEvent);
            return true;
        }

        
        //現在ステートの変更
        private void ChangeState(StateBase<TOwner, TEvent> nextState)
        {
            //終了を呼び出し
            isAcceptableEvent = false;
            currentState.OnEnd();
            isAcceptableEvent = true;

            //ステートを変更する
            currentState = nextState;

            //スタートとUpdateを呼び出す
            currentState.OnStart();
            currentState.OnUpdate();
        }


        //次の遷移ステートを取得(ない場合はnullを返す)
        private StateBase<TOwner, TEvent> GetNextState(Queue<TEvent> events)
        {
            //判定テーブルを取得
            Dictionary<TEvent, StateBase<TOwner, TEvent>> currentStateTable = transitonTableDictionary[currentState];

            //スタックされたイベントを処理する
            while (events.Count > 0)
            {
                //イベントをポップする
                TEvent triggerEvent = events.Dequeue();

                //遷移チェック
                if (currentStateTable.ContainsKey(triggerEvent))
                {
                    //遷移テーブルにある場合現在ステートのガードをチェック
                    if (!currentState.GuardEvent(triggerEvent)) continue;        //ガードされたら飛ばす

                    //イベントから取得する
                    StateBase<TOwner, TEvent> nextState = currentStateTable[triggerEvent];
                    return nextState;
                }
            }
            //遷移先が見つからなかった場合
            return null;
        }
    }
}

