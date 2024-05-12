using Damagable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityView.Player.Parameter;

namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class SlipDamageFloor : AffectPlayerFloorGimmick {

            [SerializeField] private float damage = 1.0f;
            [SerializeField] private float slipTime = 3.0f, damadeInterval = 1.0f;

            bool isExited = true, isWeitTime = false;

            IDamagable damagable = null;
            PlayerHealth playerHealth = null;
            Collider playerCollider = null;

            protected override void VirtualOnTriggerEnter(Collider other) {
                //すでに外へ出ているか
                if (isExited) {
                    isExited = false;
                    Debug.Log("Reset");

                    //まだバフが切れていない
                    if (isWeitTime) return;

                    //damagable & playerHealth & playerCollider の登録
                    if (damagable == null && playerHealth == null) {
                        damagable = other.gameObject.GetComponent<IDamagable>();
                        playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                        if (damagable != null && playerHealth != null && playerCollider == null) playerCollider = other;
                    }

                    //プレイヤーならスリップダメージ処理を開始
                    if (other == playerCollider && damagable != null && playerHealth != null) {
                        Debug.Log("DamageStart");
                        StartCoroutine(ExitPlayer(damagable));
                    }

                }
            }
            protected override void VirtualOnTriggerExit(Collider other) {
                if (other == playerCollider && damagable != null && playerHealth != null) {
                    //外には出たがバフは切れていない
                    isWeitTime = true;
                    isExited = true;
                }
            }

            IEnumerator ExitPlayer(IDamagable damagable) {
                float endTime = 0,damageTime=0;
                while (true) {

                    //一定間隔でダメージ
                    damageTime+= Time.deltaTime;
                    if(damageTime> damadeInterval) {
                        Debug.Log("Damage");
                        damagable.Damage(new Damage(damage, 0, 0, false));
                        damageTime = 0;
                    }

                    //中にいればリセット
                    if (!isExited) { endTime = 0; }
                    endTime += Time.deltaTime;

                    if (endTime >= slipTime) break;

                    yield return null;
                }

                //スリップダメージ終了
                isWeitTime = false;
                Debug.Log("End");

            }

        }
    }
}