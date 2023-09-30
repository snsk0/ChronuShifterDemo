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


        public readonly float damage;           //ダメージ量
        public readonly float knock;            //ノック値
        public readonly float invincibleTick;   //無敵時間
        public readonly bool ignoreInvincible;  //無敵時間を貫通するか
    }
}
