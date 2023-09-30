using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerMove
    {
        public void Move(LookDirection lookDirection, LookDirection moveDirection, bool isSprint);
        public void AirMove(LookDirection lookDirection, LookDirection moveDirection);

        public bool isMoving();   //Move‚Ì“ü—Í‚ªŽc‚Á‚Ä‚¢‚é‚©”»’è‚Å‚«‚é
    }
}
