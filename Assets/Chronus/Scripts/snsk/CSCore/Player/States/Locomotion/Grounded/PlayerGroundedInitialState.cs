using FSM.Core;
using Player.Structure;

namespace Player.States.Locomotion.Grounded
{
    public class PlayerGroundedInitialState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        protected override void OnStart()
        {
            LookDirection direction = globalParameterContainer.GetParameter<LookDirection>(PlayerParameterKey.moveInput);

            if (direction.x != 0 || direction.y != 0)
            {
                sendEvent(PlayerEvent.Move);
            }
            else
            {
                sendEvent(PlayerEvent.End);
            }
        }
    }
}
