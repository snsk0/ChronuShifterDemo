using FSM.Core;

namespace Player.States.Locomotion.Grounded
{
    public class PlayerIdleState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        protected override void OnStart()
        {
            localParameterContainer.SetSprintLock(true);
        }


        protected override void OnEnd()
        {
            localParameterContainer.SetSprintLock(false);
        }
    }
}
