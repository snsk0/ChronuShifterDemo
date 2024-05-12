using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityView.Player.Behaviours;


namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class JumpUpFloor : AffectPlayerFloorGimmick {

            [SerializeField] private float baff = 3.0f;

            IPlayerJumpPowerChanger changer;
            Collider playerCollider = null;

            protected override void VirtualOnTriggerEnter(Collider other) {

                //changer & playerCollider �̓o�^
                if (changer == null) {
                    changer = other.gameObject.GetComponent<IPlayerJumpPowerChanger>();
                    if (changer != null && playerCollider == null) playerCollider = other;
                }

                //�v���C���[�Ȃ�o�t�|��
                if (other == playerCollider && changer != null) {
                    Debug.Log("JumpUp");
                    changer.SetJumpMultiply(baff);
                }

            }

            protected override void VirtualOnTriggerExit(Collider other) {
                //�v���C���[�Ȃ�o�t���Ȃ���
                if (other == playerCollider && changer != null) {
                    changer.SetJumpMultiply(1.0f);
                }
            }
        }
    }
}