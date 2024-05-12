using InputProviders.Player;

using Player.Structure;


namespace Player.Presenter
{
    public interface IMoveDirectionConverter
    {
        public LookDirection ConvertMoveInputData(MoveInputData inputData);
    }
}
