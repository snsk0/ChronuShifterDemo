using System;

using FSM.Parameter;

namespace FSM.Core.Hierarchical
{
    public abstract class ParentStateBase<TOwner, TEvent> : StateBase<TOwner, TEvent>
    {
        //内部ステートマシン
        public FiniteStateMachine<TOwner, TEvent> innerFSM { get; private set; }



        //初期化処理
        internal override void Initialize(TOwner owner, Func<TEvent, bool> sendEvent, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            //BaseStateの初期化呼び出し
            base.Initialize(owner, sendEvent, globalParameterContainer, localParameterContainer);

            //ステートマシンのインスタンス化
            innerFSM = new FiniteStateMachine<TOwner, TEvent>(owner, globalParameterContainer, localParameterContainer);
        }

        //イベントの転送
        internal override bool ReceiveEvent(TEvent triggerEvent)
        {
            return innerFSM.SendEvent(triggerEvent);
        }



        //Update
        protected internal override sealed void OnUpdate()
        {
            innerFSM.Tick();
            OnSelfUpdate();
        }

        //Start
        protected internal override sealed void OnStart()
        {
            OnSelfStart();
            innerFSM.StartUp();
        }

        //End
        protected internal sealed override void OnEnd()
        {
            innerFSM.Abort();
            OnSelfEnd();
        }

        //GuradEvent
        protected internal sealed override bool GuardEvent(TEvent triggerEvent)
        {
            //自身のガードをチェック
            bool guard = SelfGuardEvent(triggerEvent);

            //guardされたら返す、されなかったら子のGuardを返す、最終的に何も引っかからないならtrueを返す
            if (!guard) return guard;
            else if (innerFSM.isRunning) return innerFSM.currentState.GuardEvent(triggerEvent);
            else return true;
        }



        //抽象メソッド
        protected virtual void OnSelfUpdate() { }
        protected virtual void OnSelfStart() { }
        protected virtual void OnSelfEnd() { }
        protected virtual bool SelfGuardEvent(TEvent triggerEvent) { return true; }
    }
}
