using FSM.Core;

using Player.Behaviours;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerJumpState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�t�B�[���h
        private IPlayerJump jump;


        //������
        protected override void OnInitialize()
        {
            jump = owner.objectContainer.GetObject<IPlayerJump>();
        }




        protected override void OnStart()
        {
            //�W�����v����
            jump.Jump(globalParameterContainer.GetParameter<float>(PlayerParameterKey.jumpInput));
        }


        protected override void OnUpdate()
        {
            //�W�����v�����ǂ����`�F�b�N����
            if (!jump.isJumping()) sendEvent(PlayerEvent.Fall);  //�������������C�x���g�𑗐M
        }




        //�K�[�h
        protected override bool GuardEvent(PlayerEvent triggerEvent)
        {
            //�C���^���N�g�������ꍇ�K�[�h����
            if (triggerEvent == PlayerEvent.Interact) return false;
            return true;
        }
    }
}
