using FSM.Parameter;

namespace Player
{
    public interface IPlayerParameterContainerAccessable
    {
        public IParameterContainer playerGlobalParameterContainer { get; }
        public IReadOnlyParameterContainer playerLocalParameterContainer { get; }
    }
}
