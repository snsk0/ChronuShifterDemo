using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityView.Player.Behaviours;

namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class SpeedUpFloor : AffectPlayerFloorGimmick {

            [SerializeField]private float baff = 3.0f;
            [SerializeField] private float buffTime = 3.0f;

            bool isExited = true, isWeitTime = false;

            IPlayerVelocityChanger changer = null;
            Collider playerCollider = null;

            protected override void VirtualOnTriggerEnter(Collider other) {
                //���łɊO�֏o�Ă��邩
                if (isExited) {
                    isExited = false;
                    Debug.Log("Reset");

                    //�܂��o�t���؂�Ă��Ȃ�
                    if (isWeitTime) return;

                    //changer & playerCollider �̓o�^
                    if (changer == null) {
                        changer = other.gameObject.GetComponent<IPlayerVelocityChanger>();
                        if (changer != null && playerCollider == null) playerCollider = other;
                    }

                    //�v���C���[�Ȃ�o�t�|��
                    if (other == playerCollider && changer != null) {
                        Debug.Log("SpeedUp");
                        changer.SetVelocityMultiply(baff);
                    }
                }
            }
            protected override void VirtualOnTriggerExit(Collider other) {
                if(other == playerCollider && changer != null) {
                    //�O�ɂ͏o�����o�t�͐؂�Ă��Ȃ�
                      isExited = true; isWeitTime = true;
                    StartCoroutine(ExitPlayer(changer));
                }
            }

            IEnumerator ExitPlayer(IPlayerVelocityChanger changer) {
                float time = 0;
                while (true) {
                    time += Time.deltaTime;
                    if (time >= buffTime) break;

                    //���ɓ�������I��
                    if (!isExited) yield break;

                    yield return null;
                }

                //�o�t���؂��
                isWeitTime = false;

                Debug.Log("End");
                changer.SetVelocityMultiply(1.0f);

            }

        }
    }

}
