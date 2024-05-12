using FSM.Core;

using Player.Behaviours;

namespace Player.States
{
    public class PlayerDamageReactState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        private IPlayerDamageReaction damageReaction;


        protected override void OnInitialize()
        {
            damageReaction = owner.objectContainer.GetObject<IPlayerDamageReaction>();
        }



        protected override void OnUpdate()
        {
            if (!damageReaction.isDamaging) sendEvent(PlayerEvent.End);
        }
    }
}
