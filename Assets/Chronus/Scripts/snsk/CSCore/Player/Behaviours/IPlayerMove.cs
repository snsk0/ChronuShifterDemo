using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerMove
    {
        public void Move(LookDirection lookDirection, LookDirection moveDirection, bool isSprint);
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection, bool isSprint);
        public bool isMoving();   //Moveの入力が残っているか判定できる
    }
}
