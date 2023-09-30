using FSM.Core.Hierarchical;

using Player.Behaviours;


namespace Player.States.Locomotion
{
    public class PlayerLocomotionParentState : ParentStateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�R���|�[�l���g
        private IPlayerInteract interact;


        //������
        protected override void OnInitialize()
        {
            interact = owner.objectContainer.GetObject<IPlayerInteract>();
        }


        protected override void OnSelfStart()
        {
            //sprint�𖳌���
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }


        protected override void OnSelfUpdate()
        {
            //Lock���Ȃ�ݒ肵�Ȃ�
            bool sprintLock = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprintLock);
            if (sprintLock) return;


            //SprintInput���������g��
            bool sprintInput = globalParameterContainer.GetParameter<bool>(PlayerParameterKey.sprintInput);
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, sprintInput);
        }


        protected override void OnSelfEnd()
        {
            //sprinted���X�V
            bool isSprint = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);
            localParameterContainer.SetParameter(PlayerParameterKey.isLastSprinted, isSprint);

            //sprint�𖳌���
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }



        //�C���^���N�g�����s�ł���Ȃ�J�ڂ�������
        protected override bool SelfGuardEvent(PlayerEvent triggerEvent)
        {
            if (triggerEvent != PlayerEvent.Interact) return true;

            if (interact.CanInteract()) return true;
            else return false;
        }
    }
}
