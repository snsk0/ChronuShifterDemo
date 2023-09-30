using System;

using Damagable;


namespace Player.Presenter
{
    public interface IDamagableEventSender
    {
        public IObservable<Damage> onDamageObservable { get; }  //ダメージイベント発行
    }
}
