using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerItemDropPositionSeracher
    {
        public bool TryGetDropPosition(IPlayerItemObject item, out Position position);
    }
}
