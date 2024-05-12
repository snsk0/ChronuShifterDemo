using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerJumpState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�t�B�[���h
        private IPlayerJump _jump;
        private IPlayerMove _move;
        private IPlayerLook _look;

        //������
        protected override void OnInitialize()
        {
            _jump = owner.objectContainer.GetObject<IPlayerJump>();
            _move = owner.objectContainer.GetObject<IPlayerMove>();
            _look = owner.objectContainer.GetObject<IPlayerLook>();
        }

        protected override void OnStart()
        {
            //�W�����v����
            _jump.Jump(globalParameterContainer.GetParameter<float>(PlayerParameterKey.jumpInput));
        }

        protected override void OnUpdate()
        {
            //�W�����v�����ǂ����`�F�b�N����
            if (!_jump.isJumping())
            {
                sendEvent(PlayerEvent.Fall);
                return;
            }

            //���͎󂯎��
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0)
            {
                _look.Look(lookToDirection);
            }

            //�����Ă���Ɠ��͕�����n��
            LookDirection lookDirection = _look.GetDirection();
            _move.AirMove(lookDirection, lookToDirection, localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));
        }

        //�I������Ƃ��Ɏ~�߂�悤�ɂ���
        protected override void OnEnd()
        {
            _move.Move(_look.GetDirection(), new LookDirection(0, 0), false);
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
