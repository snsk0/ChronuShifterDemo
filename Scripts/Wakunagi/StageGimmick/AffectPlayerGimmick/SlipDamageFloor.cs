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
                //���łɊO�֏o�Ă��邩
                if (isExited) {
                    isExited = false;
                    Debug.Log("Reset");

                    //�܂��o�t���؂�Ă��Ȃ�
                    if (isWeitTime) return;

                    //damagable & playerHealth & playerCollider �̓o�^
                    if (damagable == null && playerHealth == null) {
                        damagable = other.gameObject.GetComponent<IDamagable>();
                        playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                        if (damagable != null && playerHealth != null && playerCollider == null) playerCollider = other;
                    }

                    //�v���C���[�Ȃ�X���b�v�_���[�W�������J�n
                    if (other == playerCollider && damagable != null && playerHealth != null) {
                        Debug.Log("DamageStart");
                        StartCoroutine(ExitPlayer(damagable));
                    }

                }
            }
            protected override void VirtualOnTriggerExit(Collider other) {
                if (other == playerCollider && damagable != null && playerHealth != null) {
                    //�O�ɂ͏o�����o�t�͐؂�Ă��Ȃ�
                    isWeitTime = true;
                    isExited = true;
                }
            }

            IEnumerator ExitPlayer(IDamagable damagable) {
                float endTime = 0,damageTime=0;
                while (true) {

                    //���Ԋu�Ń_���[�W
                    damageTime+= Time.deltaTime;
                    if(damageTime> damadeInterval) {
                        Debug.Log("Damage");
                        damagable.Damage(new Damage(damage, 0, 0, false));
                        damageTime = 0;
                    }

                    //���ɂ���΃��Z�b�g
                    if (!isExited) { endTime = 0; }
                    endTime += Time.deltaTime;

                    if (endTime >= slipTime) break;

                    yield return null;
                }

                //�X���b�v�_���[�W�I��
                isWeitTime = false;
                Debug.Log("End");

            }

        }
    }
}