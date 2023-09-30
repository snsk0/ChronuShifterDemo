using FSM.Core;

using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerFallState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //フィールド
        private IPlayerOnGrounded grounded;
        private IPlayerLock look;
        private IPlayerMove move;


        //初期化
        protected override void OnInitialize()
        {
            grounded = owner.objectContainer.GetObject<IPlayerOnGrounded>();
            look = owner.objectContainer.GetObject<IPlayerLock>();
            move = owner.objectContainer.GetObject<IPlayerMove>();
        }



        //Update
        protected override void OnUpdate()
        {
            //接地判定
            if (grounded.IsOnGrounded())
            {
                sendEvent(PlayerEvent.End);
                return;
            }

            //入力受け取り
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0) look.Look(lookToDirection);             //向きを変える


            //向いていると入力方向を渡す
            LookDirection lookDirection = look.GetDirection();
            move.AirMove(lookDirection, lookToDirection);
        }


        //終了するときに止めるようにする
        protected override void OnEnd()
        {
            move.Move(look.GetDirection(), new LookDirection(0, 0), false);
        }



        //ガード
        protected override bool GuardEvent(PlayerEvent triggerEvent)
        {
            //インタラクトだった場合ガードする
            if (triggerEvent == PlayerEvent.Interact) return false;
            return true;
        }
    }
}
