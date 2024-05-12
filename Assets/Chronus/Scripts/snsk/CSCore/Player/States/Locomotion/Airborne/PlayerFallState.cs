using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerFallState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�t�B�[���h
        private IPlayerOnGrounded grounded;
        private IPlayerLook look;
        private IPlayerMove move;

        //������
        protected override void OnInitialize()
        {
            grounded = owner.objectContainer.GetObject<IPlayerOnGrounded>();
            look = owner.objectContainer.GetObject<IPlayerLook>();
            move = owner.objectContainer.GetObject<IPlayerMove>();
        }

        //Update
        protected override void OnUpdate()
        {
            //�ڒn����
            if (grounded.IsOnGrounded())
            {
                sendEvent(PlayerEvent.End);
                return;
            }

            //���͎󂯎��
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0)
            {
                look.Look(lookToDirection);
            }

            //�����Ă���Ɠ��͕�����n��
            LookDirection lookDirection = look.GetDirection();
            move.AirMove(lookDirection, lookToDirection, localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));
        }

        //�I������Ƃ��Ɏ~�߂�悤�ɂ���
        protected override void OnEnd()
        {
            move.Move(look.GetDirection(), new LookDirection(0, 0), false);
        }

        //�K�[�h
        protected override bool GuardEvent(PlayerEvent triggerEvent)
        {
            //�C���^���N�g�������ꍇ�K�[�h����
            if (triggerEvent == PlayerEvent.Interact)
            {
                return false;
            }
            return true;
        }
    }
}
