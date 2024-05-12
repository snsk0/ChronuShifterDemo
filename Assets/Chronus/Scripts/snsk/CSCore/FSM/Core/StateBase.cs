using System;

using FSM.Parameter;

namespace FSM.Core
{
    public abstract class StateBase<TOwner, TEvent>
    {
        //�t�B�[���h
        protected TOwner owner { get; private set; }                    //�I�[�i�[      

        //�p�����[�^�R���e�i
        protected IReadOnlyParameterContainer globalParameterContainer;  //�O���t�O����A�N�Z�X�\
        protected IParameterContainer localParameterContainer;           //�O���t���ł̂ݕύX�\

        //�R�[���o�b�N
        protected Func<TEvent, bool> sendEvent { get; private set; }



        //���������\�b�h(�K��StateMachine����Ăяo�����s��)
        internal virtual void Initialize(TOwner owner, Func<TEvent, bool> sendEvent, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            this.owner = owner;
            this.sendEvent = sendEvent;
            this.globalParameterContainer = globalParameterContainer;
            this.localParameterContainer = localParameterContainer;

            OnInitialize();
        }

        //�C�x���g�󂯎�菈��
        internal virtual bool ReceiveEvent(TEvent triggerEvent) { return false; }



        //���ۃ��\�b�h
        protected virtual void OnInitialize() { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnStart() { }
        protected internal virtual void OnEnd() { } 
        protected internal virtual bool GuardEvent(TEvent triggerEvent) { return true; }
    }
}