using System;

namespace Damagable
{
    public interface IDamagable
    {
        public void Damage(Damage damage);  //ƒ_ƒ[ƒWŠÖ”

        public bool isInvincible { get; }   //–³“GŠÔ‚©‚Ç‚¤‚©
    }
}
