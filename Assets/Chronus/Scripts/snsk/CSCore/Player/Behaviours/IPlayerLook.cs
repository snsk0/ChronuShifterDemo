using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerLook
    {
        public void Look(LookDirection direction);  //w’è‚µ‚½•ûŒü‚ÉŒü‚­
        public LookDirection GetDirection();        //Œ»İŒü‚¢‚Ä‚¢‚é•ûŒü‚ğæ“¾‚·‚é
    }
}
