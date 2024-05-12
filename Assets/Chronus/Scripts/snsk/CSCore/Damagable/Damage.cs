namespace Damagable
{
    public struct Damage
    {
        public Damage(float damage, float knock, float invincibleTick, bool ignoreInvincible)
        {
            this.damage = damage;
            this.knock = knock;
            this.invincibleTick = invincibleTick;
            this.ignoreInvincible = ignoreInvincible;
        }
        public Damage(float damage, float invincibleTick, bool ignoreInvincible) : this(damage, 0, invincibleTick, ignoreInvincible) { }


        public readonly float damage;           //�_���[�W��
        public readonly float knock;            //�m�b�N�l
        public readonly float invincibleTick;   //���G����
        public readonly bool ignoreInvincible;  //���G���Ԃ��ђʂ��邩
    }
}
