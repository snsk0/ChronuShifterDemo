using System;

using FSM.Core;
using FSM.Core.Hierarchical;
using FSM.Parameter;
using FSMUtils;


namespace Player
{
    public class PlayerFSMOwner : IPlayerEventReceiver, IPlayerCurrentLeafStateProvider, IPlayerParameterContainerAccessable
    {
        //�t�B�[���h
        private readonly FiniteStateMachine<PlayerFSMOwner, PlayerEvent> fsm;    //�X�e�[�g�}�V��
        private readonly ObjectContainer objectContainerInstance;                //���W�b�N�̏W��I�u�W�F�N�g
        private readonly IFSMUpdator fsmUpdator;                                 //�X�e�[�g�}�V���̍X�V�@


        //�v���p�e�B
        public StateBase<PlayerFSMOwner, PlayerEvent> currentLeafState => fsm.GetCurrentLeafState();    //�X�e�[�g�ǂݎ��
        public IReadOnlyObjectContainer objectContainer => objectContainerInstance;     //���W�b�N�R���e�i�̓ǂݎ��

        //�R���e�i�A�N�Z�X
        public IParameterContainer playerGlobalParameterContainer => fsm.globalParameterContainer;
        public IReadOnlyParameterContainer playerLocalParameterContainer => fsm.readLocalParameterContainer;


        //�R���X�g���N�^
        public PlayerFSMOwner(ObjectContainer objectContainerInstance, IFSMUpdator fsmUpdator, Action<FiniteStateMachine<PlayerFSMOwner, PlayerEvent>> initialize,
            IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            //�t�B�[���h��������
            fsm = new FiniteStateMachine<PlayerFSMOwner, PlayerEvent>(this, globalParameterContainer, localParameterContainer);
            this.fsmUpdator = fsmUpdator;
            this.objectContainerInstance = objectContainerInstance;

            //�X�e�[�g�}�V���̏������Ăяo��
            initialize.Invoke(fsm);
        }


        //�f�X�g���N�^(�ی�)
        ~PlayerFSMOwner()
        {
            //�o�^����
            fsmUpdator.UnRegisterFSM(fsm);
        }






        //Update���J�n����
        public void StartFSM()
        {
            fsmUpdator.RegisterFSM(fsm);
        }

        //���f
        public void AbortFSM()
        {
            fsmUpdator.UnRegisterFSM(fsm);
            fsm.Abort();
        } 

        //�C�x���g�󂯎��
        public bool SendEvent(PlayerEvent triggerEvent)
        {
            if (triggerEvent == PlayerEvent.End) return false;  //End�̓A�N�V�����̏I���C�x���g�Ƃ��ėp������̂ŊO���͋��ۂ��Ă���
            return fsm.SendEvent(triggerEvent);
        }

        //�X�e�[�g���^�C�v����
        public bool ContainsTypeCurrentState<TType>()
        {
            return fsm.ContainsTypeCurrentState<PlayerFSMOwner, PlayerEvent, TType>();
        }
    }
}
