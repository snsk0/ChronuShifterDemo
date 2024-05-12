using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerJumpState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //フィールド
        private IPlayerJump _jump;
        private IPlayerMove _move;
        private IPlayerLook _look;

        //初期化
        protected override void OnInitialize()
        {
            _jump = owner.objectContainer.GetObject<IPlayerJump>();
            _move = owner.objectContainer.GetObject<IPlayerMove>();
            _look = owner.objectContainer.GetObject<IPlayerLook>();
        }

        protected override void OnStart()
        {
            //ジャンプする
            _jump.Jump(globalParameterContainer.GetParameter<float>(PlayerParameterKey.jumpInput));
        }

        protected override void OnUpdate()
        {
            //ジャンプ中かどうかチェックする
            if (!_jump.isJumping())
            {
                sendEvent(PlayerEvent.Fall);
                return;
            }

            //入力受け取り
            LookDirection lookToDirection = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (lookToDirection.x != 0 || lookToDirection.y != 0)
            {
                _look.Look(lookToDirection);
            }

            //向いていると入力方向を渡す
            LookDirection lookDirection = _look.GetDirection();
            _move.AirMove(lookDirection, lookToDirection, localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));
        }

        //終了するときに止めるようにする
        protected override void OnEnd()
        {
            _move.Move(_look.GetDirection(), new LookDirection(0, 0), false);
        }

        //ガード
        protected override bool GuardEvent(PlayerEvent triggerEvent)
        {
            //インタラクトだった場合ガードする
            if (triggerEvent == PlayerEvent.Interact) 
            { 
                return false; 
            }
            return true;
        }
    }
}
