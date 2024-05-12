using UnityEngine;
using UniRx;


namespace UnityView.Player.Parameter
{
    public class PlayerHealth : MonoBehaviour, IVariableParameterEventSender<float>
    {
        //最大Hp
        [SerializeField] private float maxHealth;



        //現在Hp(reactiveProperty)
        private ReactiveProperty<float> currentHealth = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> variableParameterProperty => currentHealth;    //イベント購読用



        
        //Hpダメージメソッド
        public void Damage(float damage)
        {
            if (damage < 0) return;

            if (currentHealth.Value - damage < 0) currentHealth.Value = 0;
            else currentHealth.Value -= damage;
        }

        //Healメソッド
        public void Heal(float heal)
        {
            if (heal < 0) return;

            if (currentHealth.Value + heal > maxHealth) currentHealth.Value = maxHealth;
            else currentHealth.Value += heal;
        }



        //Destroy時に破棄
        private void OnDestroy()
        {
            currentHealth.Dispose();
        }
    }
}
