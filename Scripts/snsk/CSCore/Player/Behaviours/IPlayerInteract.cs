namespace Player.Behaviours
{
    public interface IPlayerInteract
    {
        public void Interact(bool isSprint);
        public bool CanInteract();  //Interact�ł��邩�ǂ����̃`�F�b�N
        public void Abort();        //���f
        public bool isInteracting { get; }
    }
}
