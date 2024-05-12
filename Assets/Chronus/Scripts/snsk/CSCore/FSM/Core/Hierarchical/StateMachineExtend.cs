namespace FSM.Core.Hierarchical
{
    public static class StateMachineExtend
    {
        //��Ԏq�̃X�e�[�g���擾����
        public static StateBase<TOwner, TEvent> GetCurrentLeafState<TOwner, TEvent>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //���[�t�X�e�[�g
            StateBase<TOwner, TEvent> leafState;

            //���݃X�e�[�g���e���ǂ���
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;

            //�e�������ꍇ,�e�̃X�e�[�g�}�V������ċA�Ăяo��,�e����Ȃ��Ȃ猻�݃X�e�[�g��Ԃ�
            if (parentState != null) leafState = parentState.innerFSM.GetCurrentLeafState();
            else leafState = fsm.currentState;

            return leafState;
        }


        //�ŏI�X�e�[�g��
        public static StateBase<TOwner, TEvent> GetLastLeafState<TOwner, TEvent>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //���[�t�X�e�[�g
            StateBase<TOwner, TEvent> leafState;

            //���݃X�e�[�g���e���ǂ���
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;

            //�e�������ꍇ,�e�̃X�e�[�g�}�V������ċA�Ăяo��,�e����Ȃ��Ȃ猻�݃X�e�[�g��Ԃ�
            if (parentState != null) leafState = parentState.innerFSM.GetLastLeafState();
            else leafState = fsm.lastState;

            return leafState;
        }



        //�Ώی^�����s���X�e�[�g�Ɋ܂܂�邩
        public static bool ContainsTypeCurrentState<TOwner, TEvent, TType>(this FiniteStateMachine<TOwner, TEvent> fsm)
        {
            //���g�������A��������������return
            bool isContain = fsm.currentState is TType;
            if (isContain) return true;


            //���݃X�e�[�g���e���ǂ���
            ParentStateBase<TOwner, TEvent> parentState = fsm.currentState as ParentStateBase<TOwner, TEvent>;


            //�e�������ꍇ�A�q�𒲂ׂ�
            if (parentState != null) isContain = parentState.innerFSM.ContainsTypeCurrentState<TOwner, TEvent, TType>();

            return isContain;
        }
    }
}
