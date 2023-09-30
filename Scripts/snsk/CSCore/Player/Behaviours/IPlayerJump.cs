namespace Player.Behaviours
{
    public interface IPlayerJump
    {
        public void Jump(float strength);   //ジャンプを行う
        public bool isJumping();            //ジャンプ中かどうか
    }
}
