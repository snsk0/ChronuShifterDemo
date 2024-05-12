namespace FSM.Parameter 
{
    public interface IParameterContainer : IReadOnlyParameterContainer
    {
        public bool SetParameter<T>(string name, T parameter); 
    }
}
