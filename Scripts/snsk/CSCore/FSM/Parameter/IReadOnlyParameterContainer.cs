namespace FSM.Parameter
{
    public interface IReadOnlyParameterContainer
    {
        public T GetParameter<T>(string name);
    }
}
