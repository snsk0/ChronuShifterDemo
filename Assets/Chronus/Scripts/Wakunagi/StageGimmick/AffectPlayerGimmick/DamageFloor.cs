using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damagable;
using Cysharp.Threading.Tasks;
using UnityView.Player.Parameter;


namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class DamageFloor : AffectPlayerFloorGimmick {

            [SerializeField] private float damage = 3.0f;

            IDamagable damagable = null;
            PlayerHealth playerHealth = null;
            Collider playerCollider = null;

            protected override void VirtualOnTriggerEnter(Collider other) {

                //damagable & playerHealth & playerCollider の登録
                if (damagable == null && playerHealth == null) {
                    damagable = other.gameObject.GetComponent<IDamagable>();
                    playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                    if (damagable != null && playerHealth != null && playerCollider == null) playerCollider = other;
                }

                //プレイヤーならダメージ処理
                if (other == playerCollider && damagable != null && playerHealth != null) {
                    Debug.Log("Damage");
                    damagable.Damage(new Damage(damage, 1, 0, false));
                    Debug.Log("HP:" + playerHealth.variableParameterProperty.Value);
                }

            }
        }
    }
}