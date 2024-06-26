using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerLook
    {
        public void Look(LookDirection direction);  //指定した方向に向く
        public LookDirection GetDirection();        //現在向いている方向を取得する
    }
}
