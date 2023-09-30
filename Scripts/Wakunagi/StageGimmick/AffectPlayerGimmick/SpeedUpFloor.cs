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
                //すでに外へ出ているか
                if (isExited) {
                    isExited = false;
                    Debug.Log("Reset");

                    //まだバフが切れていない
                    if (isWeitTime) return;

                    //changer & playerCollider の登録
                    if (changer == null) {
                        changer = other.gameObject.GetComponent<IPlayerVelocityChanger>();
                        if (changer != null && playerCollider == null) playerCollider = other;
                    }

                    //プレイヤーならバフ掛け
                    if (other == playerCollider && changer != null) {
                        Debug.Log("SpeedUp");
                        changer.SetVelocityMultiply(baff);
                    }
                }
            }
            protected override void VirtualOnTriggerExit(Collider other) {
                if(other == playerCollider && changer != null) {
                    //外には出たがバフは切れていない
                      isExited = true; isWeitTime = true;
                    StartCoroutine(ExitPlayer(changer));
                }
            }

            IEnumerator ExitPlayer(IPlayerVelocityChanger changer) {
                float time = 0;
                while (true) {
                    time += Time.deltaTime;
                    if (time >= buffTime) break;

                    //中に入ったら終了
                    if (!isExited) yield break;

                    yield return null;
                }

                //バフが切れる
                isWeitTime = false;

                Debug.Log("End");
                changer.SetVelocityMultiply(1.0f);

            }

        }
    }

}
