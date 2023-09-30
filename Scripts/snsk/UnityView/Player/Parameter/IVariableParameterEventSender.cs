using UniRx;


namespace UnityView.Player.Parameter
{
    public interface IVariableParameterEventSender<T>
    {
        public IReadOnlyReactiveProperty<T> variableParameterProperty { get; }
    }
}
