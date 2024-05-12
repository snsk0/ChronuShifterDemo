using System;


namespace InputProviders.Player
{
    public interface IPlayerInputProvider
    {
        //���͂̉�
        public void SetInputActive(bool active);

        //�ړ�����
        public IObservable<MoveInputData> onMoveInput { get; }

        //�_�b�V��
        public IObservable<SprintInputData> onSprintInput { get; }

        //�W�����v����
        public IObservable<JumpInputData> onJumpInput { get; }

        //�C���^���N�g����
        public IObservable<bool> onInteractInput { get; }


        //�^�C���V�t�g����


        //�G�X�P�[�v����

    }
}
