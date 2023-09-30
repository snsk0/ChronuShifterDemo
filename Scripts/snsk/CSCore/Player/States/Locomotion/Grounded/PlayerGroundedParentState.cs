using FSM.Core.Hierarchical;

using Player.Behaviours;

namespace Player.States.Locomotion.Grounded
{
    public class PlayerGroundedParentState : ParentStateBase<PlayerFSMOwner, PlayerEvent>
    {
        //フィールド
        private IPlayerOnGrounded grounded;


        //初期化
        protected override void OnInitialize()
        {
            grounded = owner.objectContainer.GetObject<IPlayerOnGrounded>();
        }



        protected override void OnSelfUpdate()
        {
            if (!grounded.IsOnGrounded()) sendEvent(PlayerEvent.Fall);
        }
    }
}
