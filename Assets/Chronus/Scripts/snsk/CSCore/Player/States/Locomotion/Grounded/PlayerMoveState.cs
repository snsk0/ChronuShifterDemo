using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Grounded
{
    public class PlayerMoveState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //フィールド
        private IPlayerLook look;
        private IPlayerMove move;

        //初期化
        protected override void OnInitialize()
        {
            look = owner.objectContainer.GetObject<IPlayerLook>();
            move = owner.objectContainer.GetObject<IPlayerMove>();
        }

        //Update処理
        protected override void OnUpdate()
        {
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0)
            {
                look.Look(lookToDirection);
            }

            //向いていると入力方向を渡す
            LookDirection lookDirection = look.GetDirection();
            move.Move(lookDirection, lookToDirection, localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));

            //TODO
            //入力の角度変化が180度付近だった場合spritを無効化、しても良い

            //最後に処理されたmoveが未入力だった場合 かつ 移動入力がない場合
            if(!move.isMoving() && lookToDirection.x == 0 && lookToDirection.y == 0)
            {
                sendEvent.Invoke(PlayerEvent.End);
            }
        }

        //終了するときに止めるようにする
        protected override void OnEnd()
        {
            move.Move(look.GetDirection(), new LookDirection(0, 0), false);
        }
    }
}
