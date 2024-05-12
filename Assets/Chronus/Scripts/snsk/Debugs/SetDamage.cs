
using UnityEngine;

using Damagable;

using UnityView.Player.Behaviours;

public class SetDamage : MonoBehaviour
{
    [SerializeField] private PlayerDamagable damagable;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            damagable.Damage(new Damage(5, 1, 0, false));
        }
    }
}
