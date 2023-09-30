using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerMove
    {
        public void Move(LookDirection lookDirection, LookDirection moveDirection, bool isSprint);
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection);

        public bool isMoving();   //Move�̓��͂��c���Ă��邩����ł���
    }
}
