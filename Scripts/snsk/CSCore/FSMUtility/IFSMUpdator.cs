using FSM.Core;

namespace FSMUtils
{
    public interface IFSMUpdator
    {
        public void RegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);        //�o�^�֐�
        public void UnRegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm);      //�����֐�
    }
}
