using System;

namespace Damagable
{
    public interface IDamagable
    {
        public void Damage(Damage damage);  //�_���[�W�֐�

        public bool isInvincible { get; }   //���G���Ԃ��ǂ���
    }
}
