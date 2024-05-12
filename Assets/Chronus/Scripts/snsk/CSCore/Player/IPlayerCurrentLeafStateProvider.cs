using FSM.Core;


namespace Player
{
    public interface IPlayerCurrentLeafStateProvider
    {
        public StateBase<PlayerFSMOwner, PlayerEvent> currentLeafState { get; }
        public bool ContainsTypeCurrentState<TType>();
    }
}
