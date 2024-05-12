using UnityEngine;
using UniRx;


namespace UnityView.Player.Parameter
{
    public class PlayerHealth : MonoBehaviour, IVariableParameterEventSender<float>
    {
        //�ő�Hp
        [SerializeField] private float maxHealth;



        //����Hp(reactiveProperty)
        private ReactiveProperty<float> currentHealth = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> variableParameterProperty => currentHealth;    //�C�x���g�w�Ǘp



        
        //Hp�_���[�W���\�b�h
        public void Damage(float damage)
        {
            if (damage < 0) return;

            if (currentHealth.Value - damage < 0) currentHealth.Value = 0;
            else currentHealth.Value -= damage;
        }

        //Heal���\�b�h
        public void Heal(float heal)
        {
            if (heal < 0) return;

            if (currentHealth.Value + heal > maxHealth) currentHealth.Value = maxHealth;
            else currentHealth.Value += heal;
        }



        //Destroy���ɔj��
        private void OnDestroy()
        {
            currentHealth.Dispose();
        }
    }
}
