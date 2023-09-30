namespace Player.Behaviours
{
    public interface IPlayerInteract
    {
        public void Interact(bool isSprint);
        public bool CanInteract();  //Interactできるかどうかのチェック
        public void Abort();        //中断
        public bool isInteracting { get; }
    }
}
