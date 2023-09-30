using FSM.Core;

using Player.Behaviours;

namespace Player.States.Locomotion.Airborne
{
    public class PlayerJumpState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //フィールド
        private IPlayerJump jump;


        //初期化
        protected override void OnInitialize()
        {
            jump = owner.objectContainer.GetObject<IPlayerJump>();
        }




        protected override void OnStart()
        {
            //ジャンプする
            jump.Jump(globalParameterContainer.GetParameter<float>(PlayerParameterKey.jumpInput));
        }


        protected override void OnUpdate()
        {
            //ジャンプ中かどうかチェックする
            if (!jump.isJumping()) sendEvent(PlayerEvent.Fall);  //落下中を示すイベントを送信
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
