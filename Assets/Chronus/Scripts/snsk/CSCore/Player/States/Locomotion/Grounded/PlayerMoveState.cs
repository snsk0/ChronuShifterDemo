using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Grounded
{
    public class PlayerMoveState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�t�B�[���h
        private IPlayerLook look;
        private IPlayerMove move;

        //������
        protected override void OnInitialize()
        {
            look = owner.objectContainer.GetObject<IPlayerLook>();
            move = owner.objectContainer.GetObject<IPlayerMove>();
        }

        //Update����
        protected override void OnUpdate()
        {
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0)
            {
                look.Look(lookToDirection);
            }

            //�����Ă���Ɠ��͕�����n��
            LookDirection lookDirection = look.GetDirection();
            move.Move(lookDirection, lookToDirection, localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));

            //TODO
            //���͂̊p�x�ω���180�x�t�߂������ꍇsprit�𖳌����A���Ă��ǂ�

            //�Ō�ɏ������ꂽmove�������͂������ꍇ ���� �ړ����͂��Ȃ��ꍇ
            if(!move.isMoving() && lookToDirection.x == 0 && lookToDirection.y == 0)
            {
                sendEvent.Invoke(PlayerEvent.End);
            }
        }

        //�I������Ƃ��Ɏ~�߂�悤�ɂ���
        protected override void OnEnd()
        {
            move.Move(look.GetDirection(), new LookDirection(0, 0), false);
        }
    }
}
