using System;

using Damagable;


namespace Player.Presenter
{
    public interface IDamagableEventSender
    {
        public IObservable<Damage> onDamageObservable { get; }  //�_���[�W�C�x���g���s
    }
}
