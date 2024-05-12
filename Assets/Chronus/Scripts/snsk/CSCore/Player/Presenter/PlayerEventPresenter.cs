using System;
using System.Collections.Generic;

using RxUtils;

using InputProviders.Player;

using Player.Structure;
using Player.States;


namespace Player.Presenter
{

    //�����ɏ����ׂ��łȂ�����
    //���Adaptor�Ɉړ����邱�Ƃ�����
    public class PlayerEventPresenter
    {
        //�t�B�[���h
        private readonly IPlayerEventReceiver eventReceiver;                                //�C�x���g���M��
        private readonly IPlayerParameterContainerAccessable containerAccessable;           //�R���e�i�ւ̃A�N�Z�X
        private readonly IPlayerInputProvider inputProvider;                                //�v���C���[Input���s��
        private readonly IDamagableEventSender damagableEventSender;                        //�_���[�W�C�x���g
        private readonly IMoveDirectionConverter moveDirectionConverter;                    //�J���������擾

        private readonly List<IDisposable> disposableList;                                  //�C�x���g�j���p



        //�R���X�g���N�^
        public PlayerEventPresenter(IPlayerEventReceiver eventReceiver, IPlayerParameterContainerAccessable containerAccessable, 
            IPlayerInputProvider inputProvider, IDamagableEventSender damagableEventSender, IMoveDirectionConverter moveDirectionConverter)
        {
            //�t�B�[���h������
            this.eventReceiver = eventReceiver;
            this.containerAccessable = containerAccessable;
            this.inputProvider = inputProvider;
            this.damagableEventSender = damagableEventSender;
            this.moveDirectionConverter = moveDirectionConverter;

            disposableList = new List<IDisposable>();


            //�C�x���g�w�ǊJ�n
            RegisterPlayerEvent();
        }

        //�f�X�g���N�^
        ~PlayerEventPresenter()
        {
            UnRegisterPlayerEvent();
        }





        //�C�x���g�o�^�֐�
        private void RegisterPlayerEvent()
        {
            //�ړ�
            disposableList.Add(inputProvider.onMoveInput.Subscribe(inputData =>
            {
                //���͂��������ꍇ�C�x���g�𑗐M
                if(!inputData.isNone()) eventReceiver.SendEvent(PlayerEvent.Move);

                //�����̓R���e�i�ɐ��l���i�[
                LookDirection direction = moveDirectionConverter.ConvertMoveInputData(inputData);
                containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.moveInput, direction);
            }));




            //�_�b�V��
            disposableList.Add(inputProvider.onSprintInput.Subscribe(inputData =>
            {
                //hold��
                if (inputData.isHolding)
                {
                    //hold�Ȃ炻�̂܂ܓ��͂�`����
                    containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, inputData.isSprintInput);
                }

                //�g�O��
                else
                {

                    //Input������ꍇ
                    if (inputData.isSprintInput) 
                    {
                        //���݂�sprint���擾����
                        bool isSprint = containerAccessable.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);

                        //����Ƃ͋t�ɂ���悤�ɓ���
                        containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, !isSprint);
                    }


                    //Input���Ȃ��ꍇ
                    else
                    {
                        //���݂�sprint���擾����
                        bool isSprint = containerAccessable.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);

                        //����Ɓu�����v�ɂȂ�悤�ɏ���������
                        containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.sprintInput, isSprint);
                    }
                }
            }));




            //�W�����v
            disposableList.Add(inputProvider.onJumpInput.Subscribe(inputData =>
            {
                //�C�x���g���X�e�[�g�}�V���ɑ��M
                eventReceiver.SendEvent(PlayerEvent.Jump);

                //�R���e�i�ɐ��l���i�[
                containerAccessable.playerGlobalParameterContainer.SetParameter(PlayerParameterKey.jumpInput, inputData.magnitude);
            }));




            //�C���^���N�g
            disposableList.Add(inputProvider.onInteractInput.Subscribe(inputData =>
            {
                //�_���[�W�C�x���g�𑗐M
                eventReceiver.SendEvent(PlayerEvent.Interact);
            }));


            //�_���[�W
            disposableList.Add(damagableEventSender.onDamageObservable.Subscribe(inputData =>
            {
                //�_���[�W�C�x���g�𑗐M(����knock��0�Ȃ�Đ����Ȃ�)
                if (inputData.knock == 0) return;
                eventReceiver.SendEvent(PlayerEvent.Damage);
            }));
            //���̑�etc
        }










        //�C�x���g�j���֐�
        private void UnRegisterPlayerEvent()
        {
            foreach (IDisposable disposable in disposableList)
            {
                disposable.Dispose();
            }
        }
    }
}
