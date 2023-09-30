using System;

namespace Damagable
{
    public interface IDamagable
    {
        public void Damage(Damage damage);  //ダメージ関数

        public bool isInvincible { get; }   //無敵時間かどうか
    }
}
