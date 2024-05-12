using System;

using Player;
using Player.States;
using Player.States.Interact;
using Player.States.Locomotion;
using Player.States.Locomotion.Airborne;
using Player.States.Locomotion.Grounded;
using FSM.Core;


namespace UnityView.Player
{
    public static class PlayerFSMBuilder
    {
        public static Action<FiniteStateMachine<PlayerFSMOwner, PlayerEvent>> builder = (fsm) =>
        {
            //�X�e�[�g�}�V���̏�����
            //�ŏ�K
            PlayerActionState actionState = new PlayerActionState();
            PlayerDamageReactState damageReactState = new PlayerDamageReactState();

            fsm.AddState(actionState);
            fsm.AddState(damageReactState);
            fsm.SetInitialState(actionState);

            fsm.AddTransition(actionState, damageReactState, PlayerEvent.Damage);
            fsm.AddTransition(damageReactState, damageReactState, PlayerEvent.Damage);
            fsm.AddTransition(damageReactState, actionState, PlayerEvent.End);



            //action�w
            PlayerLocomotionParentState locomotionParentState = new PlayerLocomotionParentState();
            PlayerInteractState interactState = new PlayerInteractState();

            actionState.innerFSM.AddState(locomotionParentState);
            actionState.innerFSM.AddState(interactState);
            actionState.innerFSM.SetInitialState(locomotionParentState);

            actionState.innerFSM.AddTransition(locomotionParentState, interactState, PlayerEvent.Interact);
            actionState.innerFSM.AddTransition(interactState, locomotionParentState, PlayerEvent.End);


            //Locomotion�w
            PlayerGroundedParentState groundedParentState = new PlayerGroundedParentState();
            PlayerJumpState jumpState = new PlayerJumpState();
            PlayerFallState fallState = new PlayerFallState();

            locomotionParentState.innerFSM.AddState(groundedParentState);
            locomotionParentState.innerFSM.AddState(jumpState);
            locomotionParentState.innerFSM.AddState(fallState);
            locomotionParentState.innerFSM.SetInitialState(groundedParentState);

            locomotionParentState.innerFSM.AddTransition(groundedParentState, jumpState, PlayerEvent.Jump);
            locomotionParentState.innerFSM.AddTransition(groundedParentState, fallState, PlayerEvent.Fall);
            locomotionParentState.innerFSM.AddTransition(jumpState, fallState, PlayerEvent.Fall);
            locomotionParentState.innerFSM.AddTransition(fallState, groundedParentState, PlayerEvent.End);


            //GroundedParent�w
            PlayerGroundedInitialState groundedInitialState = new PlayerGroundedInitialState();
            PlayerIdleState idleState = new PlayerIdleState();
            PlayerMoveState moveState = new PlayerMoveState();

            groundedParentState.innerFSM.AddState(groundedInitialState);
            groundedParentState.innerFSM.AddState(idleState);
            groundedParentState.innerFSM.AddState(moveState);
            groundedParentState.innerFSM.SetInitialState(groundedInitialState);

            groundedParentState.innerFSM.AddTransition(groundedInitialState, idleState, PlayerEvent.End);
            groundedParentState.innerFSM.AddTransition(groundedInitialState, moveState, PlayerEvent.Move);
            groundedParentState.innerFSM.AddTransition(idleState, moveState, PlayerEvent.Move);
            groundedParentState.innerFSM.AddTransition(moveState, idleState, PlayerEvent.End);
        };
    }
}
