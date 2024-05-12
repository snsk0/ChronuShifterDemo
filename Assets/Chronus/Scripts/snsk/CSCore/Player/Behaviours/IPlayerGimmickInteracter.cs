namespace Player.Behaviours
{
    public interface IPlayerGimmickInteracter
    {
        public void Interact(IPlayerGimmickObject gimmickObject);
        public bool IsInteracting { get; }
    }
}
