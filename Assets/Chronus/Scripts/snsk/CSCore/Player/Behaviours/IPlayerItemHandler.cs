using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerItemHandler
    {
        public bool HasItem();
        public IPlayerItemObject GetItem();
        public void PickUp(IPlayerItemObject item);
        public void DropOff(Position position, bool isSprint);
        public void Abort();
        public bool IsExecution { get; }
    }
}
