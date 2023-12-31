using FSM.Core;

namespace FSMUtils
{
    public interface IFSMUpdator
    {
        public void RegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);        //登録関数
        public void UnRegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);      //解除関数
    }
}
