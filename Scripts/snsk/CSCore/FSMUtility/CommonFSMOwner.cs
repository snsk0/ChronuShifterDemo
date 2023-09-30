using System;

using FSM.Core;
using FSM.Parameter;

namespace FSMUtils
{
    //�ꉞ�����������A�X�e�[�g�N���X�̌p����Generic�w�肪���Ȃ��₱�����Ȃ邽�ߋ��炭�p�~
    //�ėp�I�[�i�[���쐬�������Ȃ�Event���w�肵�Ȃ��Ǝg���Â炢�����H
    //�����A�ėp�R���e�L�X�g��p�ӂ���Əd���R�[�h��������镔�����傫�����ߗv����
    //�������A�ėp�R���e�L�X�g�͓����ɔėp�X�e�[�g���������ƂɂȂ�(�Ⴆ�΃Q�[���}�l�[�W���ƃL���������Ŏg���Ɨ����ɓ����X�e�[�g�������ł���A����͗��΂ɔ�������)
    //���̂��߂�����x�̏d���R�[�h�͋��e���ׂ����Ɗ�����
    [Obsolete]
    public class CommonFSMOwner<TEvent>
    {
        //�t�B�[���h
        protected readonly FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent> fsm;    //�X�e�[�g�}�V��
        private readonly ObjectContainer objectContainer;                             //�X�e�[�g����A�N�Z�X����I�u�W�F�N�g�R���e�i


        //�v���p�e�B
        public Type currentStateType => fsm.currentState.GetType();                     //���݃X�e�[�g�̌^
        public IReadOnlyObjectContainer readonlyObjectContainer => objectContainer;     //���W�b�N�R���e�i�̓ǂݎ��





        //�R���X�g���N�^
        public CommonFSMOwner(ObjectContainer objectContainer, Action<FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent>> initialize)
        {
            //�t�B�[���h������
            fsm = new FiniteStateMachine<CommonFSMOwner<TEvent>, TEvent>(this, new DefaultParameterContainer(), new DefaultParameterContainer());
            this.objectContainer = objectContainer;

            //�X�e�[�g�}�V���̏������Ăяo��
            initialize.Invoke(fsm);
        }


        //�f�X�g���N�^
        ~CommonFSMOwner()
        {
            //�X�e�[�g�}�V���̃I�[�g�X�V����
            FSMUpdateManager.instance.UnRegisterFSM(fsm);
        }





        //�X�e�[�g�}�V���̃C�x���g���M�@�\
        public bool SendEvent(TEvent triggerEvent)
        {
            return fsm.SendEvent(triggerEvent);
        }

        //�J�n
        public void StartFSM()
        {
            FSMUpdateManager.instance.RegisterFSM(fsm);
        }

        //���f�@�\
        public void AbortFSM()
        {
            //�����X�V������
            FSMUpdateManager.instance.UnRegisterFSM(fsm);

            //���f�Ăяo��
            fsm.Abort();
        }
    }
}
