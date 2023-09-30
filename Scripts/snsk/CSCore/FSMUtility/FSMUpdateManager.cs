using System;
using System.Collections.Generic;

using FSM.Core;
using InputProviders.LifeCycle;
using RxUtils;


namespace FSMUtils
{
    //�C���^�[�t�F�[�X���[���l�[���X�y�[�X�w�ɒu�����ق����ǂ���������Ȃ�(FSMUtility.Manager�̂悤��)
    public class FSMUpdateManager : IFSMUpdator
    {
        //���g�̃C���X�^���X
        public static FSMUpdateManager instance { get; private set; }

        //���������\�b�h
        public static void Initialize(IInputUpdateProvider updateProvider)
        {
            
            //if (instance != null) return; �V�[�����܂��������Ɍ����ɂȂ�
            //���Ȃ�updateProvider��null���ǂ����Ƃ�
            instance = new FSMUpdateManager(updateProvider);
        }




        //UpdateInput
        private IInputUpdateProvider updateProvider;
        private IDisposable providerDisposable;

        //�X�e�[�g�}�V���̃A�b�v�f�[�g���\�b�h���X�g
        private List<Action> fsmTickList;




        //�R���X�g���N�^�����J(updateProvider��DI
        private FSMUpdateManager(IInputUpdateProvider updateProvider)
        {
            this.updateProvider = updateProvider;
            providerDisposable  = this.updateProvider.onUpdate.Subscribe(_ => { UpdateAll(); });

            fsmTickList = new List<Action>();
        }

        //�f�R���X�g���N�^��Update����
        ~FSMUpdateManager()
        {
            providerDisposable.Dispose();
        }



        //�ꊇUpdate
        private void UpdateAll()
        {
            foreach(Action fsmTick in fsmTickList)
            {
                fsmTick.Invoke();
            }
        }

        //�o�^�֐�
        public void RegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm)
        {
            if (fsmTickList.Contains(fsm.Tick)) return;
            fsmTickList.Add(fsm.Tick);
        } 

        
        //�����֐�
        public void UnRegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm)
        {
            if (fsmTickList.Contains(fsm.Tick)) fsmTickList.Remove(fsm.Tick);
        }
    }
}
