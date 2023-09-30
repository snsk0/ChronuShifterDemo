using FSM.Core;

namespace FSMUtils
{
    public interface IFSMUpdator
    {
        public void RegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);        //“o˜^ŠÖ”
        public void UnRegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);      //‰ğœŠÖ”
    }
}
