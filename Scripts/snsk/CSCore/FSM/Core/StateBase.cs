using System;

using FSM.Parameter;

namespace FSM.Core
{
    public abstract class StateBase<TOwner, TEvent>
    {
        //フィールド
        protected TOwner owner { get; private set; }                    //オーナー      

        //パラメータコンテナ
        protected IReadOnlyParameterContainer globalParameterContainer;  //グラフ外からアクセス可能
        protected IParameterContainer localParameterContainer;           //グラフ内でのみ変更可能

        //コールバック
        protected Func<TEvent, bool> sendEvent { get; private set; }



        //初期化メソッド(必ずStateMachineから呼び出しを行う)
        internal virtual void Initialize(TOwner owner, Func<TEvent, bool> sendEvent, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            this.owner = owner;
            this.sendEvent = sendEvent;
            this.globalParameterContainer = globalParameterContainer;
            this.localParameterContainer = localParameterContainer;

            OnInitialize();
        }

        //イベント受け取り処理
        internal virtual bool ReceiveEvent(TEvent triggerEvent) { return false; }



        //抽象メソッド
        protected virtual void OnInitialize() { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnEnd() { } 
        protected internal virtual bool GuardEvent(TEvent triggerEvent) { return true; }
    }
}