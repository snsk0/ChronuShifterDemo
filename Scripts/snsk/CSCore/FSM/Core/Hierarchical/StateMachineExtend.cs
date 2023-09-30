namespace FSM.Core.Hierarchical
{
    public static class StateMachineExtend
    {
        //一番子のステートを取得する
        public static StateBase<TOwner, TEvent> GetCurrentLeafState<TOwner, TEvent>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //リーフステート
            StateBase<TOwner, TEvent> leafState;

            //現在ステートが親かどうか
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;

            //親だった場合,親のステートマシンから再帰呼び出し,親じゃないなら現在ステートを返す
            if (parentState != null) leafState = parentState.innerFSM.GetCurrentLeafState();
            else leafState = fsm.currentState;

            return leafState;
        }


        //最終ステート版
        public static StateBase<TOwner, TEvent> GetLastLeafState<TOwner, TEvent>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //リーフステート
            StateBase<TOwner, TEvent> leafState;

            //現在ステートが親かどうか
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;

            //親だった場合,親のステートマシンから再帰呼び出し,親じゃないなら現在ステートを返す
            if (parentState != null) leafState = parentState.innerFSM.GetLastLeafState();
            else leafState = fsm.lastState;

            return leafState;
        }



        //対象型が実行中ステートに含まれるか
        public static bool ContainsTypeCurrentState<TOwner, TEvent, TType>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //自身を検索、引っかかったらreturn
            bool isContain = fsm.currentState is TType;
            if (isContain) return true;


            //現在ステートが親かどうか
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;


            //親だった場合、子を調べる
            if (parentState != null) isContain = parentState.innerFSM.ContainsTypeCurrentState<TOwner, TEvent, TType>();

            return isContain;
        }
    }
}
