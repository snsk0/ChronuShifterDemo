using System;

using FSM.Parameter;

namespace FSM.Core.Hierarchical
{
    public abstract class ParentStateBase<TOwner, TEvent> : StateBase<TOwner, TEvent>
    {
        //�����X�e�[�g�}�V��
        public FiniteStateMachine<TOwner, TEvent> innerFSM { get; private set; }



        //����������
        internal override void Initialize(TOwner owner, Func<TEvent, bool> sendEvent, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            //BaseState�̏������Ăяo��
            base.Initialize(owner, sendEvent, globalParameterContainer, localParameterContainer);

            //�X�e�[�g�}�V���̃C���X�^���X��
            innerFSM = new FiniteStateMachine<TOwner, TEvent>(owner, globalParameterContainer, localParameterContainer);
        }

        //�C�x���g�̓]��
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
            //���g�̃K�[�h���`�F�b�N
            bool guard = SelfGuardEvent(triggerEvent);

            //guard���ꂽ��Ԃ��A����Ȃ�������q��Guard��Ԃ��A�ŏI�I�ɉ�������������Ȃ��Ȃ�true��Ԃ�
            if (!guard) return guard;
            else if (innerFSM.isRunning) return innerFSM.currentState.GuardEvent(triggerEvent);
            else return true;
        }



        //���ۃ��\�b�h
        protected virtual void OnSelfUpdate() { }
        protected virtual void OnSelfStart() { }
        protected virtual void OnSelfEnd() { }
        protected virtual bool SelfGuardEvent(TEvent triggerEvent) { return true; }
    }
}
