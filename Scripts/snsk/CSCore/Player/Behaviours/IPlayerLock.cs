using Player.Structure;

namespace Player.Behaviours
{
    public interface IPlayerLock
    {
        public void Look(LookDirection direction);  //�w�肵�������Ɍ���
        public LookDirection GetDirection();        //���݌����Ă���������擾����
    }
}